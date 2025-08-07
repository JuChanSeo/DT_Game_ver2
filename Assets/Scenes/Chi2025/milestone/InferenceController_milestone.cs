using System.Linq;
using UnityEngine;
using CJM.BBox2DToolkit;
using CJM.DeepLearningImageProcessor;
using System.Collections.Generic;
using CJM.BarracudaInference.YOLOX;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class InferenceController_milestone : MonoBehaviour
{
    #region Fields

    // Components
    [Header("Components")]
    [SerializeField, Tooltip("Responsible for image preprocessing")]
    private ImageProcessor imageProcessor;
    [SerializeField, Tooltip("Executes YOLOX model for object detection")]
    private YOLOXObjectDetector modelRunner;
    [SerializeField, Tooltip("Manages user interface updates")]
    private UIController uiController;
    [SerializeField, Tooltip("Visualizes detected object bounding boxes")]
    private BoundingBox2DVisualizer boundingBoxVisualizer;
    [SerializeField, Tooltip("Renders the input image on a screen")]
    private UnityEngine.UI.RawImage screenRenderer;

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


    private float time_remain;
    private bool start_flag;

    public bool fist_flag;
    public Slider slider_time;
    float time_limit;

    public List<Sprite> list_sprite = new List<Sprite>();
    List<string> list_instruct = new List<string>()
    { "call",
    "dislike",
    "fist",
    "four",
    "ok",
    "one",
    "palm",
    "peace",
    "rock",
    "stop",
    "three",
    "two_up"};
    public Image display_image;

    int cur_ans_idx;
    int cnt_answer;

    public List<int> cnt_ans__ = new List<int>();
    public List<string> ground_truth__ = new List<string>();
    public List<string> answer__ = new List<string>();

    public TextMeshProUGUI ges_instruct_text;
    public TextMeshProUGUI time_remain_text;
    public TextMeshProUGUI cnt_ans_text;
    public TextMeshProUGUI Pnum_text;

    bool true_or_false;

    string cur_ges;

    #endregion

    #region MonoBehaviour Methods
    private void Start()
    {
        time_limit = PlayerPrefs.GetFloat("milestone_time_lim");
        time_remain_text.gameObject.SetActive(false);
    
    }

    /// <summary>
    /// Update the InferenceController every frame, processing the input image and updating the UI and bounding boxes.
    /// </summary>
    private void Update()
    {
        slider_time.value = time_remain / time_limit;
        time_remain_text.text = $"{(int)time_remain + (int)1}초 안에 사진의 손모양을 따라해주세요";
        Pnum_text.text = "P" + PlayerPrefs.GetInt("milestone_pnum").ToString();

        if (start_flag && time_remain > 0)
            time_remain -= Time.deltaTime;
        else if (start_flag)
        {
            cnt_answer++;
            cnt_ans__.Add(cnt_answer);
            cnt_ans_text.text = "갯수: " + cnt_answer.ToString();

            start_flag = false;
            time_remain = 0;
            if (true_or_false)
            {
                Debug.Log("성공!");
                answer__.Add(list_instruct[cur_ans_idx]);
            }
            else
            {
                Debug.Log("실패!");
                answer__.Add(cur_ges);
            }

            //if(cnt_answer == 5)
            //{
            //    JsonSave();
            //}

            time_remain_text.gameObject.SetActive(false);
            Invoke("game_start_button_click", 3f);
        }

        // Check if all required components are valid
        //Debug.Log("check1");
        if (!AreComponentsValid()) return;

        //Debug.Log("check2");
        // Get the input image and dimensions
        var imageTexture = screenRenderer.texture;
        //Debug.Log("check2-1");
        //Debug.Log(imageTexture.width + "\t" + imageTexture.height);
        var imageDims = new Vector2Int(imageTexture.width, imageTexture.height);
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


        if (bboxInfoArray.Length != 0)
        {
            UpdateBoundingBoxes(inputDims);
            ////uiController.UpdateUI(bboxInfoArray.Length);
            boundingBoxVisualizer.UpdateBoundingBoxVisualizations(bboxInfoArray);
            cur_ges = bboxInfoArray[0].label;
            if (bboxInfoArray[0].label == list_instruct[cur_ans_idx])
            {
                true_or_false = true;
            }
        }

        ////Debug.Log(bboxInfoArray[0].label + "\t"+ bboxInfoArray[0].bbox.x0 + "\t" + bboxInfoArray[0].bbox.y0
        ////          + "\t" + bboxInfoArray[0].bbox.width + "\t" + bboxInfoArray[0].bbox.height);
        //// Update bounding boxes and user interface
        ////Debug.Log(inputDims);
        //UpdateBoundingBoxes(inputDims);
        //uiController.UpdateUI(bboxInfoArray.Length);
        //boundingBoxVisualizer.UpdateBoundingBoxVisualizations(bboxInfoArray);
        
    }

    #endregion

    public void game_start_button_click()
    {
        start_flag = true;
        cur_ans_idx = UnityEngine.Random.Range(0, 12);
        ges_instruct_text.text = list_instruct[cur_ans_idx];
        display_image.sprite = list_sprite[cur_ans_idx];
        ground_truth__.Add(ges_instruct_text.text);
        time_remain = time_limit;
        start_flag = true;
        time_remain_text.gameObject.SetActive(true);
        true_or_false = false;
        if (GameObject.Find("Button_Start") != null) GameObject.Find("Button_Start").SetActive(false);
    }

    public void JsonSave()
    {
        SaveData_g saveData = new SaveData_g();

        saveData.name = Pnum_text.text;

        for (int i = 0; i < cnt_ans__.Count; i++)
        {
            saveData.cnt_ans.Add(cnt_ans__[i]);
            saveData.ground_truth.Add(ground_truth__[i]);
            saveData.answer.Add(answer__[i]);
        }

        string json = JsonUtility.ToJson(saveData, true);
        string path = saveData.name;
        string fileN = saveData.name + "_" + saveData.Type + ".json";

        if (!new DirectoryInfo(path).Exists) new DirectoryInfo(path).Create();

        if (!File.Exists(Path.Combine(path, fileN))) File.WriteAllText(Path.Combine(path, fileN), json);
        else
        {
            string newN = FileUploadName(path, fileN);
            File.WriteAllText(Path.Combine(path, newN), json);
        }
    }

    public string FileUploadName(string dirPath, string fileN)
    {
        string fileName = fileN;

        if (fileN.Length > 0)
        {
            int indexOfDot = fileName.LastIndexOf(".");
            string strName = fileName.Substring(0, indexOfDot);
            string strExt = fileName.Substring(indexOfDot);

            bool bExist = true;
            int fileCount = 0;

            string dirMapPath = string.Empty;

            while (bExist)
            {
                dirMapPath = dirPath;
                string pathCombine = System.IO.Path.Combine(dirMapPath, fileName);

                if (System.IO.File.Exists(pathCombine))
                {
                    fileCount++;
                    fileName = strName + "(" + fileCount + ")" + strExt;
                }
                else
                {
                    bExist = false;
                }
            }
        }

        return fileName;

    }

    public void back_bt_clicked()
    {
        JsonSave();
        SceneManager.LoadScene("milestone_main");
    }

    private void OnApplicationQuit()
    {
        JsonSave();
    }
    #region Private Methods

    /// <summary>
    /// Check if all required components are assigned and valid.
    /// </summary>
    /// <returns>True if all components are valid, false otherwise</returns>
    private bool AreComponentsValid()
    {
        if (imageProcessor == null || modelRunner == null || uiController == null || boundingBoxVisualizer == null)
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
        //// Check if the screen is mirrored
        //mirrorScreen = screenRenderer.transform.localScale.z == -1;

        // Get the screen dimensions
        //Vector2 screenDims = new Vector2(screenRenderer.transform.localScale.x, screenRenderer.transform.localScale.y);
        //Debug.Log("screenDims: " + screenDims);
        Vector2 screenDims = new Vector2(1280, 720);

        // Scale and position the bounding boxes based on the input and screen dimensions
        //for (int i = 0; i < bboxInfoArray.Length; i++)
        //{
        //    bboxInfoArray[i].bbox = BBox2DUtility.ScaleBoundingBox(bboxInfoArray[i].bbox, inputDims, screenDims, offset, mirrorScreen);
        //    Debug.Log(bboxInfoArray[i].label + "\t" + bboxInfoArray[i].bbox.x0 + "\t" + bboxInfoArray[i].bbox.y0
        //  + "\t" + bboxInfoArray[i].bbox.width + "\t" + bboxInfoArray[i].bbox.height);

        //}
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
