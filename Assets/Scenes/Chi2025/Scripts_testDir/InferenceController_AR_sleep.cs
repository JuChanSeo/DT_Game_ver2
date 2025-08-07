using System;
using System.Linq;
using UnityEngine;
using CJM.BBox2DToolkit;
using CJM.DeepLearningImageProcessor;
using System.Collections.Generic;
using CJM.BarracudaInference.YOLOX;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.SceneManagement;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.DnnModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.ImgcodecsModule;
using OpenCVForUnity.ObjdetectModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.UnityUtils.Helper;
using TMPro;

public class InferenceController_AR_sleep : MonoBehaviour
{
    #region Fields

    // Components
    [Header("Components")]
    [SerializeField, Tooltip("Responsible for image preprocessing")]
    private ImageProcessor imageProcessor;
    [SerializeField, Tooltip("Executes YOLOX model for object detection")]
    private YOLOXObjectDetector modelRunner;
    //[SerializeField, Tooltip("Manages user interface updates")]
    //private UIController uiController;
    [SerializeField, Tooltip("Visualizes detected object bounding boxes")]
    private BoundingBox2DVisualizer boundingBoxVisualizer;
    [SerializeField, Tooltip("Renders the input image on a screen")]
    private MeshRenderer screenRenderer;

    [Header("Data Processing")]
    [Tooltip("The target dimensions for the processed image")]
    [SerializeField] private int targetDim = 224;
    [Tooltip("Flag to use compute shaders for processing input images.")]
    [SerializeField] private bool useComputeShaders = false;
    [Tooltip("Flag to normalize input images before passing them to the model.")]
    [SerializeField] private bool normalizeInput = false;

    // Output processing settings
    [Header("Output Processing")]
    [SerializeField, Tooltip("Flag to enable/disable async GPU readback for model output")]
    private bool useAsyncGPUReadback = false;
    [SerializeField, Tooltip("Minimum confidence score for an object proposal to be considered"), Range(0, 1)]
    private float confidenceThreshold = 0.5f;
    [SerializeField, Tooltip("Threshold for Non-Maximum Suppression (NMS)"), Range(0, 1)]
    private float nmsThreshold = 0.45f;

    // Runtime variables
    private BBox2DInfo[] bboxInfoArray; // Array to store bounding box information
    private bool mirrorScreen = false; // Flag to check if the screen is mirrored
    private Vector2Int offset; // Offset used when cropping the input image

    #endregion

    #region MonoBehaviour Methods
    [SerializeField]
    [Tooltip("The ARCameraManager which will produce frame events.")]
    ARCameraManager m_CameraManager;
    XRCpuImage.Transformation m_Transformation = XRCpuImage.Transformation.MirrorX;
    //public RawImage canvas_rawImg;
    public TextMeshProUGUI text_gesture;
    public TextMeshProUGUI text_dist;
    public Vector2 center = Vector2.zero;
    public bool fist_flag;
    public GameObject fly;
    public GameObject gamestart_Button;
    public Slider slider_time;

    private int cnt_catch;
    private int cnt_fail;
    private float time_remain;
    private bool start_flag;

    Texture2D texture;
    Mat bgrMat;

    /// <summary>
    /// Update the InferenceController every frame, processing the input image and updating the UI and bounding boxes.
    /// </summary>

    private void Start()
    {
        fly.SetActive(false);
        cnt_catch = 0;
        cnt_fail = 0;
        m_CameraManager.frameReceived += OnCameraFrameReceived;
    }

    private void Update()
    {
        slider_time.value = time_remain / 15f;
        if(start_flag)
        {
            if(time_remain > 0)
                time_remain -= Time.deltaTime;
        }
        Debug.Log(time_remain);
    }

    unsafe void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
    {
        {
            if (!m_CameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
            {
                return;
            }
            const TextureFormat format = TextureFormat.RGBA32;

            if (texture == null || texture.width != image.width || texture.height != image.height)
                texture = new Texture2D(image.width, image.height, format, false);

            var conversionParams = new XRCpuImage.ConversionParams(image, format, m_Transformation);

            // Texture2D allows us write directly to the raw texture data
            // This allows us to do the conversion in-place without making any copies.
            var rawTextureData = texture.GetRawTextureData<byte>();
            try
            {
                image.Convert(conversionParams, new IntPtr(rawTextureData.GetUnsafePtr()), rawTextureData.Length);
            }
            finally
            {
                // We must dispose of the XRCpuImage after we're finished
                // with it to avoid leaking native resources.
                image.Dispose();
            }

            texture.Apply();

            // Check if all required components are valid
            //Debug.Log("check1");
            if (!AreComponentsValid()) return;
            if (time_remain <= 0)// 파리를 못 잡고 시간이 다 지나간 경우
            {
                if (start_flag)
                {
                    time_remain = 0;
                    fly.SetActive(false);
                    Invoke("game_start_button_click", 5f);
                }
                return;
            }
            //Debug.Log("check2");
            // Get the input image and dimensions
            var imageTexture = texture;
            //Debug.Log(imageTexture.width + "\t" + imageTexture.height);
            //Debug.Log("check2-1");
            var imageDims = new Vector2Int(imageTexture.width, imageTexture.height);
            //Debug.Log(imageDims);
            //Debug.Log(imageDims);
            //Debug.Log("check2-2");
            var inputDims = imageProcessor.CalculateInputDims(imageDims, targetDim);
            //Debug.Log("check2-3");

            //Debug.Log("check3");
            // Calculate source and input dimensions for model input
            var sourceDims = inputDims;
            inputDims = modelRunner.CropInputDims(inputDims);
            
            //Debug.Log("check4");
            // Prepare and process the input texture
            RenderTexture inputTexture = PrepareInputTexture(inputDims);
            ProcessInputImage(inputTexture, imageTexture, sourceDims, inputDims);

            //Debug.Log("check5");
            // Get the model output and process the detected objects
            float[] outputArray = GetModelOutput(inputTexture, useAsyncGPUReadback);
            bboxInfoArray = modelRunner.ProcessOutput(outputArray, confidenceThreshold, nmsThreshold);

            //Debug.Log(bboxInfoArray[0].label);
            if(bboxInfoArray.Length != 0)
            {
                text_gesture.text = bboxInfoArray[0].label;

                UpdateBoundingBoxes(inputDims);
                ////uiController.UpdateUI(bboxInfoArray.Length);
                boundingBoxVisualizer.UpdateBoundingBoxVisualizations(bboxInfoArray);

                if (bboxInfoArray[0].label == "fist" && fist_flag == false)
                {
                    //compare the region
                    fist_flag = true;
                    compare_region();

                }
                else if(bboxInfoArray[0].label != "fist")
                {
                    fist_flag = false;
                }
            }
            else
            {
                text_gesture.text = "";
                text_dist.text = "";
            }

        }
    }

    public void game_start_button_click()
    {
        time_remain = 15f;
        if (start_flag == false) start_flag = true;
        if(gamestart_Button.activeSelf == true) gamestart_Button.SetActive(false);
        if(fly.activeSelf == false) fly.SetActive(true);
    }

    #endregion





    #region Private Methods

    private void compare_region()
    {
        Vector2 loc_fist = center;
        Vector2 loc_fly = fly.transform.position;
        var dist = Vector2.Distance(loc_fist, loc_fly);

        Debug.Log("loc_fist: "+ loc_fist + "\tloc_fly: " + loc_fly + "\t" + dist);

        if(dist < 300f)
        {
            time_remain = 0;
            cnt_catch += 1;
            fly.SetActive(false);
            Invoke("game_start_button_click", 5f);
        }
        else
        {
            cnt_fail += 1;
        }
        text_dist.text = "cnt_catch: " + cnt_catch.ToString() + "\tcnt_fail: " + cnt_fail.ToString();

    }

    /// <summary>
    /// Check if all required components are assigned and valid.
    /// </summary>
    /// <returns>True if all components are valid, false otherwise</returns>
    private bool AreComponentsValid()
    {
        if (imageProcessor == null || modelRunner == null)
        {
            Debug.LogError("InferenceController requires ImageProcessor, ModelRunner, and InferenceUI components.");
            return false;
        }
        return true;
    }

    /// <summary>
    /// Prepare a temporary RenderTexture with the given input dimensions.
    /// </summary>
    /// <param name="inputDims">The input dimensions for the RenderTexture</param>
    /// <returns>A temporary RenderTexture with the specified input dimensions</returns>
    private RenderTexture PrepareInputTexture(Vector2Int inputDims)
    {
        return RenderTexture.GetTemporary(inputDims.x, inputDims.y, 0, RenderTextureFormat.ARGBHalf);
    }

    /// <summary>
    /// Process the input image and apply necessary transformations.
    /// </summary>
    /// <param name="inputTexture">The input RenderTexture to process</param>
    /// <param name="imageTexture">The source image texture</param>
    /// <param name="sourceDims">The source image dimensions</param>
    /// <param name="inputDims">The input dimensions for processing</param>
    private void ProcessInputImage(RenderTexture inputTexture, Texture imageTexture, Vector2Int sourceDims, Vector2Int inputDims)
    {
        // Calculate the offset for cropping the input image
        offset = (sourceDims - inputDims) / 2;

        // Create a temporary render texture to store the cropped image
        RenderTexture sourceTexture = RenderTexture.GetTemporary(sourceDims.x, sourceDims.y, 0, RenderTextureFormat.ARGBHalf);
        Graphics.Blit(imageTexture, sourceTexture);

        // Crop and normalize the input image using Compute Shaders or fallback to Shader processing
        if (SystemInfo.supportsComputeShaders && useComputeShaders)
        {
            imageProcessor.CropImageComputeShader(sourceTexture, inputTexture, offset, inputDims);
            if (normalizeInput) imageProcessor.ProcessImageComputeShader(inputTexture, "NormalizeImage");
        }
        else
        {
            ProcessImageShader(sourceTexture, inputTexture, sourceDims, inputDims);
        }

        // Release the temporary render texture
        RenderTexture.ReleaseTemporary(sourceTexture);
    }

    /// <summary>
    /// Process the input image using Shaders when Compute Shaders are not supported.
    /// </summary>
    /// <param name="sourceTexture">The source image RenderTexture</param>
    /// <param name="inputTexture">The input RenderTexture to process</param>
    /// <param name="sourceDims">The source image dimensions</param>
    /// <param name="inputDims">The input dimensions for processing</param>
    private void ProcessImageShader(RenderTexture sourceTexture, RenderTexture inputTexture, Vector2Int sourceDims, Vector2Int inputDims)
    {
        // Calculate the scaled offset and size for cropping the input image
        Vector2 scaledOffset = offset / (Vector2)sourceDims;
        Vector2 scaledSize = inputDims / (Vector2)sourceDims;

        // Create offset and size arrays for the Shader
        float[] offsetArray = new float[] { scaledOffset.x, scaledOffset.y };
        float[] sizeArray = new float[] { scaledSize.x, scaledSize.y };

        // Crop and normalize the input image using Shaders
        imageProcessor.CropImageShader(sourceTexture, inputTexture, offsetArray, sizeArray);
        if (normalizeInput) imageProcessor.ProcessImageShader(inputTexture);
    }

    /// <summary>
    /// Get the model output either using async GPU readback or by copying the output to an array.
    /// </summary>
    /// <param name="inputTexture">The processed input RenderTexture</param>
    /// <param name="useAsyncReadback">Flag to indicate if async GPU readback should be used</param>
    /// <returns>An array of float values representing the model output</returns>
    private float[] GetModelOutput(RenderTexture inputTexture, bool useAsyncReadback)
    {
        // Run the model with the processed input texture
        modelRunner.ExecuteModel(inputTexture);
        RenderTexture.ReleaseTemporary(inputTexture);

        // Get the model output using async GPU readback or by copying the output to an array
        if (useAsyncReadback)
        {
            return modelRunner.CopyOutputWithAsyncReadback();
        }
        else
        {
            return modelRunner.CopyOutputToArray();
        }
    }

    /// <summary>
    /// Update the bounding boxes based on the input dimensions and screen dimensions.
    /// </summary>
    /// <param name="inputDims">The input dimensions for processing</param>
    private void UpdateBoundingBoxes(Vector2Int inputDims)
    {
        // Check if the screen is mirrored
        mirrorScreen = screenRenderer.transform.localScale.z == -1;

        // Get the screen dimensions
        //Vector2 screenDims = new Vector2(screenRenderer.transform.localScale.x, screenRenderer.transform.localScale.y);
        //Debug.Log("screenDims: " + screenDims);
        Vector2 screenDims = new Vector2(texture.width, texture.height);

        // Scale and position the bounding boxes based on the input and screen dimensions
        for (int i = 0; i < bboxInfoArray.Length; i++)
        {
            bboxInfoArray[i].bbox = BBox2DUtility.ScaleBoundingBox(bboxInfoArray[i].bbox, inputDims, screenDims, offset, mirrorScreen);
            //Debug.Log(bboxInfoArray[i].label + "\t" + bboxInfoArray[i].bbox.x0 + "\t" + bboxInfoArray[i].bbox.y0
            //+ "\t" + bboxInfoArray[i].bbox.width + "\t" + bboxInfoArray[i].bbox.height);
            center.x = bboxInfoArray[i].bbox.x0 + bboxInfoArray[i].bbox.width/2;
            center.y = bboxInfoArray[i].bbox.y0 - bboxInfoArray[i].bbox.height/2;
        }
    }


    #endregion

    #region Public Methods

    /// <summary>
    /// Update the confidence threshold for object detection.
    /// </summary>
    /// <param name="value">The new confidence threshold value</param>
    public void UpdateConfidenceThreshold(float value)
    {
        confidenceThreshold = value;
    }

    #endregion
}
