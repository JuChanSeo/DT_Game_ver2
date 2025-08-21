using System.Linq;
using UnityEngine;
using CJM.BBox2DToolkit;
using CJM.DeepLearningImageProcessor;
using System.Collections.Generic;
using CJM.BarracudaInference.YOLOX;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class InferenceController_edit_sleep : MonoBehaviour
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
    private RawImage screenRenderer;

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

    GameObject pet;
    public Vector2 center = Vector2.zero;
    public bool excute_ges_recog;

    public GameObject gameDonepanel;
    public TextMeshProUGUI text_last;

    public GameObject panel_dist;
    public TextMeshProUGUI text_dist;

    public GameObject panel_fail;
    public TextMeshProUGUI text_fail;

    public bool fist_flag;
    public GameObject fly;
    public GameObject gamestart_Button;
    public Slider slider_time;
    public GameObject fly_catcher;

    public GameObject sleepInfoPanel;

    private int cnt_catch;
    private int cnt_fail;
    private float time_remain;
    private bool start_flag;

    Animator anim;
    SkinnedMeshRenderer face_renderer;

    public QuestManager_daily questM_daily_script;
    public QuestManager_weekly questM_weekly_script;

    float time_limit;
    bool model_excute;
    //face_emo_edit face_emo_edit_script;


    care_effect care_effect_script;
    bgm_player bgm_player_script;
    Logger logger_script;

    #endregion

    #region MonoBehaviour Methods
    private void Start()
    {
        //face_emo_edit_script = GameObject.Find("facialexpression").GetComponent<face_emo_edit>();
        pet = GameObject.Find("pomeLV0" + PlayerPrefs.GetInt("Level_pet").ToString());
        if (panel_dist != null) panel_dist.SetActive(false);
        if (panel_fail != null) panel_fail.SetActive(false);
        if (gameDonepanel != null) gameDonepanel.SetActive(false);
        text_last.gameObject.SetActive(false);
        fly.SetActive(false);
        fly_catcher.SetActive(false);
        cnt_catch = 0;
        cnt_fail = 0;

        if (sleepInfoPanel != null) sleepInfoPanel.SetActive(false);

        anim = pet.GetComponent<Animator>();
        face_renderer = pet.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
        anim.Play("Lie_down");
        face_renderer.SetBlendShapeWeight(0, 100);
        questM_daily_script = GameObject.Find("Quest_Manager").GetComponent<QuestManager_daily>();
        questM_weekly_script = GameObject.Find("Quest_Manager").GetComponent<QuestManager_weekly>();

        care_effect_script = GameObject.Find("care_effect_player").GetComponent<care_effect>();
        bgm_player_script = GameObject.Find("Audio player").GetComponent<bgm_player>();
        logger_script = GameObject.Find("logger_gb").GetComponent<Logger>();

        StartCoroutine(bgm_player_script.excute_sound("30", 1f));
        logger_script.logger_master.insert_data("잠자기 게임 본게임 시작");

        //face_emo_edit_script.bool_fmodel = false;
    }

    /// <summary>
    /// Update the InferenceController every frame, processing the input image and updating the UI and bounding boxes.
    /// </summary>
    private void Update()
    {
        slider_time.value = time_remain / time_limit;
        if (start_flag)
        {
            if (time_remain > 0)
                time_remain -= Time.deltaTime;
            else //제한 시간이 초과되면 fail의 갯수가 증가하고 5초후에 다시 파리잡기를 실행한다
            {
                start_flag = false;
                time_remain = 0;
                fly.SetActive(false);
                fly_catcher.SetActive(false);
                cnt_fail += 1;
                Invoke("game_start_button_click", 5f);
                //face_emo_edit_script.bool_fmodel = false;
                model_excute = false;
            }


        }

        //if (time_remain <= 0)// 파리를 못 잡고 시간이 다 지나간 경우
        //{
        //    if (start_flag)
        //    {
        //        time_remain = 0;
        //        fly.SetActive(false);
        //        Invoke("game_start_button_click", 5f);
        //    }
        //    return;
        //}
        if (!AreComponentsValid()) return;

        if (!model_excute) return;
        //var imageTexture = screenRenderer.material.mainTexture;
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

        //Debug.Log(bboxInfoArray[0].label + "\t"+ bboxInfoArray[0].bbox.x0 + "\t" + bboxInfoArray[0].bbox.y0
        //          + "\t" + bboxInfoArray[0].bbox.width + "\t" + bboxInfoArray[0].bbox.height);
        // Update bounding boxes and user interface
        //Debug.Log(inputDims);
        //UpdateBoundingBoxes(inputDims);
        //boundingBoxVisualizer.UpdateBoundingBoxVisualizations(bboxInfoArray);

        if (bboxInfoArray.Length != 0)
        {
            UpdateBoundingBoxes(inputDims);
            ////uiController.UpdateUI(bboxInfoArray.Length);
            boundingBoxVisualizer.UpdateBoundingBoxVisualizations(bboxInfoArray);

            if (bboxInfoArray[0].label == "fist" && fist_flag == false)
            {
                //compare the region
                fist_flag = true;
                bgm_player_script.fly_catch_sound_excute();
                logger_script.logger_master.insert_data("파리잡기 시도");
                compare_region();

            }
            else if (bboxInfoArray[0].label != "fist")
            {
                fist_flag = false;
            }
        }
        else
        {
            //text_dist.text = "";
        }


    }

    #endregion

    public void load_AR_scene()
    {
        SceneManager.LoadScene("10_AR_interaction_MZ");
    }


    void set_difficulty()
    {
        if (PlayerPrefs.GetInt("Level_slep") == 1)
        {
            time_limit = 15f;
            fly.GetComponent<fly_randmoving>().speed_fly = 12f;
        }
        if (PlayerPrefs.GetInt("Level_slep") == 2)
        {
            time_limit = 14f;
            fly.GetComponent<fly_randmoving>().speed_fly = 16f;
        }
        if (PlayerPrefs.GetInt("Level_slep") == 3)
        {
            time_limit = 13f;
            fly.GetComponent<fly_randmoving>().speed_fly = 20f;
        }



        Debug.Log("time_limt= " + time_limit.ToString());
    }

    public void game_start_button_click()
    {
        if (cnt_fail == 5)
        {
            //실패 문구 보여주기
            logger_script.logger_master.insert_data("파리잡기 게임 실패. 게임 종료");
            if (panel_fail != null) panel_fail.SetActive(true);
            //TextMeshProUGUI text_fail = GameObject.Find("Text_fail").GetComponent<TextMeshProUGUI>();
            text_fail.text = "다음 기회에 다시 도전해봐요!";
            Invoke("load_AR_scene", 4f);
            return;
        }

        if (cnt_catch == 5)
        {
            logger_script.logger_master.insert_data("파리잡기 게임 성공. 게임 종료");
            care_effect_script.sound_reward_popup();
            if (gameDonepanel != null) gameDonepanel.SetActive(true);
            questM_daily_script.sleep_plus();
            questM_weekly_script.caregame_plus("sleep");
            text_last.gameObject.SetActive(true);
            text_last.text = "잘 하셨어요!\n 보상은 다음과 같습니다";
            PlayerPrefs.SetFloat("exp", PlayerPrefs.GetFloat("exp") + 0.02f);
            PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") + 2);
            PlayerPrefs.SetFloat("fatigue", PlayerPrefs.GetFloat("fatigue") + 0.01f);
            Invoke("load_AR_scene", 4f);
            return;
        }

        //face_emo_edit_script.bool_fmodel = true;
        set_difficulty();
        bgm_player_script.excute_narration("45");
        model_excute = true;
        time_remain = time_limit;
        if (start_flag == false) start_flag = true;
        if (gamestart_Button.activeSelf == true) gamestart_Button.SetActive(false);
        if (fly.activeSelf == false) fly.SetActive(true);
        if (fly_catcher.activeSelf == false) fly_catcher.SetActive(true);
        if (sleepInfoPanel != null) sleepInfoPanel.SetActive(true);

    }

    #region Private Methods

    private void compare_region()
    {
        Vector2 loc_fist = center;
        Vector2 loc_fly = fly.transform.position;
        var dist = Vector2.Distance(loc_fist, loc_fly);
        //Debug.Log("loc_fist: " + loc_fist + "\tloc_fly: " + loc_fly + "\t" + dist);

        if (dist < 300f)
        {
            start_flag = false;
            time_remain = 0;
            cnt_catch += 1;
            fly_catched();
            fly_catcher.SetActive(false);
            if (panel_dist != null) panel_dist.SetActive(true);
            text_dist.text = "잘 하셨어요! 맞춘 횟수: " + cnt_catch.ToString() + "/5";
            logger_script.logger_master.insert_data("파리잡기 성공! 남은 횟수: " + (5-cnt_catch).ToString());
            Invoke("game_start_button_click", 5f);
            //face_emo_edit_script.bool_fmodel = false;
            model_excute = false;
        }
        else
        {
            logger_script.logger_master.insert_data("파리잡기 실패. 남은 횟수: " + (5 - cnt_catch).ToString());
        }
    }

    void fly_catched()
    {
        if (!fly.transform.GetChild(0).gameObject.activeSelf) fly.transform.GetChild(0).gameObject.SetActive(true);
        if (fly.GetComponent<fly_randmoving>().enabled) fly.GetComponent<fly_randmoving>().enabled = false;
        Invoke("fly_deactivate", 2f);
    }

    void fly_deactivate()
    {
        if (!fly.GetComponent<fly_randmoving>().enabled) fly.GetComponent<fly_randmoving>().enabled = true;
        if (fly.transform.GetChild(0).gameObject.activeSelf) fly.transform.GetChild(0).gameObject.SetActive(false);
        fly.SetActive(false);
    }

    /// <summary>
    /// Check if all required components are assigned and valid.
    /// </summary>
    /// <returns>True if all components are valid, false otherwise</returns>
    private bool AreComponentsValid()
    {
        if (imageProcessor == null || modelRunner == null  || boundingBoxVisualizer == null)
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
        //mirrorScreen = screenRenderer.transform.localScale.z == -1;

        // Get the screen dimensions
        //Vector2 screenDims = new Vector2(screenRenderer.transform.localScale.x, screenRenderer.transform.localScale.y);
        //Debug.Log("screenDims: " + screenDims);
        Vector2 screenDims = new Vector2(1280, 720);

        // Scale and position the bounding boxes based on the input and screen dimensions
        for (int i = 0; i < bboxInfoArray.Length; i++)
        {
            bboxInfoArray[i].bbox = BBox2DUtility.ScaleBoundingBox(bboxInfoArray[i].bbox, inputDims, screenDims, offset, mirrorScreen);
          //  Debug.Log(bboxInfoArray[i].label + "\t" + bboxInfoArray[i].bbox.x0 + "\t" + bboxInfoArray[i].bbox.y0
          //+ "\t" + bboxInfoArray[i].bbox.width + "\t" + bboxInfoArray[i].bbox.height);
            center.x = /*2388 - */(bboxInfoArray[i].bbox.x0 + bboxInfoArray[i].bbox.width / 2);
            center.y = bboxInfoArray[i].bbox.y0 - bboxInfoArray[i].bbox.height / 2;
            fly_catcher.transform.position = new Vector3(center.x, center.y);

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
