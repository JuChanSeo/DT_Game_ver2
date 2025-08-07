using System.Linq;
using UnityEngine;
using CJM.BBox2DToolkit;
using CJM.DeepLearningImageProcessor;
using System.Collections.Generic;
using CJM.BarracudaInference.YOLOX;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class InferenceController_edit_sleep_tutorial : MonoBehaviour
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

    //public GameObject gameDonepanel;
    //public TextMeshProUGUI text_last;

    public GameObject panel_dist;
    public TextMeshProUGUI text_dist;

    public GameObject panel_fail;
    public TextMeshProUGUI text_fail;

    public bool fist_flag;
    public GameObject fly;
    public GameObject gamestart_Button;
    public Slider slider_time;
    public GameObject fly_catcher;

    //public GameObject sleepInfoPanel;

    private int cnt_catch;
    private int cnt_fail;
    private float time_remain;
    private bool start_flag;

    Animator anim;
    SkinnedMeshRenderer face_renderer;

    float time_limit;
    bool model_excute;
    //face_emo_edit face_emo_edit_script;

    int tutorial_step;
    int fist_and_palm;
    public GameObject fig_fist;
    public GameObject fig_palm;
    public UnityEngine.UI.Text tutorial_msg;
    public GameObject tutorial_start_bt;
    public GameObject tutorial_next_bt;
    public GameObject tutorial_end_panel;
    fly_randmoving fly_Randmoving_script;
    bool excute_once;

    care_effect care_effect_script;
    bgm_player bgm_player_script;
    Logger logger_script;

    bool narration_bool;
    #endregion

    #region MonoBehaviour Methods
    private void Start()
    {
        //face_emo_edit_script = GameObject.Find("facialexpression").GetComponent<face_emo_edit>();
        pet = GameObject.Find("pomeLV0" + PlayerPrefs.GetInt("Level_pet").ToString());
        if(panel_dist != null) panel_dist.SetActive(false);
        if(panel_fail != null) panel_fail.SetActive(false);
        //if(gameDonepanel != null) gameDonepanel.SetActive(false);
        //text_last.gameObject.SetActive(false);
        fly.SetActive(false);
        fly_catcher.SetActive(false);
        cnt_catch = 0;
        cnt_fail = 0;

        //if(sleepInfoPanel != null) sleepInfoPanel.SetActive(false);

        anim = pet.GetComponent<Animator>();
        face_renderer = pet.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
        anim.Play("Lie_down");
        face_renderer.SetBlendShapeWeight(0, 100);
        //set_difficulty();
        fist_and_palm = 0;
        tutorial_step = 0;

        fig_palm.SetActive(false);
        fig_fist.SetActive(false);
        //face_emo_edit_script.bool_fmodel = false;
        tutorial_end_panel.SetActive(false);

        care_effect_script = GameObject.Find("care_effect_player").GetComponent<care_effect>();
        bgm_player_script = GameObject.Find("Audio player").GetComponent<bgm_player>();
        logger_script = GameObject.Find("logger_gb").GetComponent<Logger>();

        StartCoroutine(bgm_player_script.excute_sound("37", 1f));
        logger_script.logger_master.insert_data("잠자기 게임 튜토리얼 시작");

    }

    /// <summary>
    /// Update the InferenceController every frame, processing the input image and updating the UI and bounding boxes.
    /// </summary>
    private void Update()
    {
        //slider_time.value = time_remain / time_limit;
        //if (start_flag)
        //{
        //    if (time_remain > 0)
        //        time_remain -= Time.deltaTime;
        //    else //제한 시간이 초과되면 fail의 갯수가 증가하고 5초후에 다시 파리잡기를 실행한다
        //    {
        //        start_flag = false;
        //        time_remain = 0;
        //        fly.SetActive(false);
        //        fly_catcher.SetActive(false);
        //        cnt_fail += 1;
        //        Invoke("game_start_button_click", 5f);
        //        //face_emo_edit_script.bool_fmodel = false;
        //        model_excute = false;
        //    }


        //}

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

            //Debug.Log("check_1");
            Debug.Log("현재 스탭: " + tutorial_step.ToString());

            if (tutorial_step == 1)
            {
                //Debug.Log("check_2");
                //손 펴보기, 손 주먹쥐기를 반복 -> fist, palm

                //fist와 palm을 번갈아 가면서
                string res_pos;
                if (fist_and_palm % 2 == 0)
                {
                    //Debug.Log("check 1-0");
                    tutorial_msg.text = "그림과 같이 테블릿 앞에서 손바닥을 모두 펴주세요";
                    fig_fist.SetActive(false);
                    fig_palm.SetActive(true);
                    res_pos = "palm";
                }
                else
                {
                    //Debug.Log("check 1-1");
                    tutorial_msg.text = "그림과 같이 테블릿 앞에서 주먹을 쥐어 주세요";
                    fig_palm.SetActive(false);
                    fig_fist.SetActive(true);
                    res_pos = "fist";
                }


                //현재 손동작이 res_pos와 맞으면 성공
                if(bboxInfoArray[0].label == res_pos)
                {
                    model_excute = false;
                    //잘했다 텍스트 켰다가 끄기
                    if (!panel_dist.activeSelf) panel_dist.SetActive(true);
                    Invoke("panel_dist_active_false", 3f);
                    fist_and_palm++;
                    if (fist_and_palm % 2 == 0)
                    {
                        logger_script.logger_master.insert_data("주먹쥐기 성공!");
                        if(fist_and_palm < 2)bgm_player_script.excute_narration("38");
                    }
                    else
                    {
                        logger_script.logger_master.insert_data("손바닥 펴기 성공!");
                        if (fist_and_palm < 2) bgm_player_script.excute_narration("39");
                    }
                }


                //10번 맞추면 다음 튜토리얼 스탭으로 넘어감
                if (fist_and_palm >= 10)
                {
                    logger_script.logger_master.insert_data("손바닥 쥐고 펴기 튜토리얼 완료. 파리채 움직이기 튜토리얼 시작");
                    fig_palm.SetActive(false);
                    fig_fist.SetActive(false);
                    tutorial_step++;
                }
            }
            else if(tutorial_step == 2)
            {
                // 손을 편 상태에서 파리채 움직여보기
                if(!excute_once)
                {
                    if (!fly_catcher.activeSelf) fly_catcher.SetActive(true);
                    bgm_player_script.excute_narration("40");
                    tutorial_msg.text = "손바닥을 편 상태에서 파리채를\n좌우, 위아래로 움직여 보세요";
                    Invoke("show_tutorial_next_bt", 10f);
                    excute_once = true;
                }
            }
            else if(tutorial_step == 3)
            {
                // 정지해 있는 파리를 향해 주먹을 폈다가 쥐면서 잡기: 5회 반복
                if (fly.GetComponent<fly_randmoving>().enabled) fly.GetComponent<fly_randmoving>().enabled = false;
                if (!fly.activeSelf)
                {
                    fly.SetActive(true);
                    fly.transform.position = new Vector2(Random.Range(800, 1500), Random.Range(500, 1000));
                }
                if (!fly_catcher.activeSelf) fly_catcher.SetActive(true);

                if (bboxInfoArray[0].label == "fist" && fist_flag == false)
                {
                    //compare the region
                    fist_flag = true;
                    bgm_player_script.fly_catch_sound_excute();
                    compare_region();
                }
                else if (bboxInfoArray[0].label != "fist")
                {
                    fist_flag = false;
                }
            }
            else if(tutorial_step == 4)
            {
                // 느리게 움직이는 파리를 잡기
                if (!fly.GetComponent<fly_randmoving>().enabled) fly.GetComponent<fly_randmoving>().enabled = true;
                if (!fly.activeSelf) fly.SetActive(true);
                if (!fly_catcher.activeSelf) fly_catcher.SetActive(true);

                if (bboxInfoArray[0].label == "fist" && fist_flag == false)
                {
                    //compare the region
                    fist_flag = true;
                    bgm_player_script.fly_catch_sound_excute();
                    logger_script.logger_master.insert_data("주먹쥐어 파리잡기 시도");
                    compare_region();
                }
                else if (bboxInfoArray[0].label != "fist")
                {
                    fist_flag = false;
                }
            }

            //if (bboxInfoArray[0].label == "fist" && fist_flag == false)
            //{
            //    //compare the region
            //    fist_flag = true;
            //    compare_region();

            //}
            //else if (bboxInfoArray[0].label != "fist")
            //{
            //    fist_flag = false;
            //}
        }
        else
        {
            //text_dist.text = "";
        }


    }

    #endregion

    void show_tutorial_next_bt()
    {
        if (!tutorial_next_bt.activeSelf) tutorial_next_bt.SetActive(true);

        if(tutorial_step == 2)
        {
            tutorial_msg.text = "파리채의 움직임에 익숙해졌다면 다음 버튼을 눌러주세요";
        }
    }


    void panel_dist_active_false()
    {
        if (panel_dist.activeSelf) panel_dist.SetActive(false);
        model_excute = true;

    }

    public void load_AR_scene()
    {
        SceneManager.LoadScene("10_AR_interaction_MZ");
    }

    void Goto_mainGame()
    {
        SceneManager.LoadScene("21_Virtual__sleeping");
    }

    void set_difficulty()
    {
        time_limit = 15f;
        fly.GetComponent<fly_randmoving>().speed_fly = 20f;

        Debug.Log("time_limt= " + time_limit.ToString());
    }

    public void game_start_button_click()
    {
        //if(cnt_fail == 5)
        //{
        //    //실패 문구 보여주기
        //    if(panel_fail != null) panel_fail.SetActive(true);
        //    //TextMeshProUGUI text_fail = GameObject.Find("Text_fail").GetComponent<TextMeshProUGUI>();
        //    text_fail.text = "다음 기회에 다시 도전해봐요!";
        //    Invoke("load_AR_scene",4f);
        //    return;
        //}

        //if (cnt_catch == 5)
        //{
        //    if(gameDonepanel != null) gameDonepanel.SetActive(true);
        //    text_last.gameObject.SetActive(true);
        //    text_last.text = "잘 하셨어요!\n 보상은 다음과 같습니다";
        //    PlayerPrefs.SetFloat("exp", PlayerPrefs.GetFloat("exp") + 0.03f);
        //    PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") + 2);
        //    Invoke("load_AR_scene", 4f);
        //    return;
        //}

        //face_emo_edit_script.bool_fmodel = true;
        if (tutorial_step == 0)
        {
            bgm_player_script.excute_narration("38");
            tutorial_msg.text = "그림과 같이 테블릿 앞에서 손바닥을 모두 펴주세요";
            fig_fist.SetActive(false);
            fig_palm.SetActive(true);
            tutorial_step = 1;
            narration_bool = true;
        }

        model_excute = true;
        //time_remain = time_limit;
        //if (start_flag == false) start_flag = true;
        if (tutorial_start_bt.activeSelf == true) tutorial_start_bt.SetActive(false);
        //if (fly.activeSelf == false) fly.SetActive(true);
        //if (fly_catcher.activeSelf == false) fly_catcher.SetActive(true);
        //if(sleepInfoPanel != null) sleepInfoPanel.SetActive(true);

    }

    public void tutorial_bt_clicked()
    {
        if(tutorial_step == 2)
        {
            logger_script.logger_master.insert_data("파리채 움직이기 종료. 멈춰있는 파리 잡기 튜토리얼 시작.");
            tutorial_step++;
            bgm_player_script.excute_narration("42");
            tutorial_msg.text = "멈춰있는 파리를 잡아주세요. \n손바닥을 펴서 파리채를 움직이고, \n파리 근처로 파리채를 가져가서 주먹을 쥐면 파리가 잡혀요!";
            if (tutorial_next_bt.activeSelf == true) tutorial_next_bt.SetActive(false);
        }

        //if (tutorial_step == 3)
        //{
        //    tutorial_step++;
        //    if (tutorial_next_bt.activeSelf == true) tutorial_next_bt.SetActive(false);
        //}

    }

    #region Private Methods

    private void compare_region()
    {
        Vector2 loc_fist = center;
        Vector2 loc_fly = fly.transform.position;
        var dist = Vector2.Distance(loc_fist, loc_fly);
        //Debug.Log("loc_fist: " + loc_fist + "\tloc_fly: " + loc_fly + "\t" + dist);

        //파리를 잡으면 게임을 잠시 정지한 후 5초 후에 다시 시작한다.
        if (dist < 300f)
        {
            //start_flag = false;
            //time_remain = 0;
            logger_script.logger_master.insert_data("파리잡기 성공!");
            cnt_catch += 1;
            fly_catched();
            fly_catcher.SetActive(false);
            if(panel_dist != null) panel_dist.SetActive(true);
            Invoke("panel_dist_active_false", 3f);
            //text_dist.text = "잘 하셨어요! 맞춘 횟수: " + cnt_catch.ToString() + "/5";
            Invoke("game_start_button_click", 3f);
            //face_emo_edit_script.bool_fmodel = false;
            model_excute = false;
            if(cnt_catch == 5)
            {
                cnt_catch = 0;
                tutorial_step++;
                if(tutorial_step == 4)
                {
                    logger_script.logger_master.insert_data("멈춰있는 파리잡기 종료. 날아다니는 파리잡기 튜토리얼 시작.");
                    bgm_player_script.excute_narration("43");
                    tutorial_msg.text = "날아다니는 파리를 잡아볼까요?";
                }

                if(tutorial_step == 5)
                {
                    logger_script.logger_master.insert_data("파리잡기 튜토리얼 종료.");
                    tutorial_msg.text = "";
                    gamestart_Button.SetActive(false);
                    bgm_player_script.excute_narration("44");
                    tutorial_end_panel.SetActive(true);
                    Invoke("Goto_mainGame", 5f);
                }
            }
        }
        else
        {
            
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
