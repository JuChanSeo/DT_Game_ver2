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
using TMPro;
using TextSpeech;

public class voice_only : MonoBehaviour
{
    #region Fields



    public Vector2 center = Vector2.zero;
    public bool one_flag;
    public GameObject gamestart_Button;
    public Slider slider_time;
    public TextMeshProUGUI text_gesture;
    public TextMeshProUGUI text_STT;


    private Animator anim;
    public GameObject pet;
    Texture2D texture;
    float jump_action_interval;
    int prev;
    int cur;
    bool flag_speech;

    VoiceController voiceController_script;
    shooting_obstacle shooting_obstacle_script;

    #endregion

    #region MonoBehaviour Methods

    private void Start()
    {
        prev = 0;
        cur = 0;
        SpeechToText.Instance.onResultCallback = OnFinalSpeechResult;
        anim = pet.GetComponent<Animator>();
        voiceController_script = GameObject.Find("VoiceController").GetComponent<VoiceController>();
        shooting_obstacle_script = GameObject.Find("agility_game").GetComponent<shooting_obstacle>();
    }

    /// <summary>
    /// Update the InferenceController every frame, processing the input image and updating the UI and bounding boxes.
    /// </summary>
    private void Update()
    {
        if (jump_action_interval < 7f) jump_action_interval += Time.deltaTime;

        cur = ((int)shooting_obstacle_script.currTime % 60);

        if (prev > cur)
        {
            Debug.Log("prev: " + prev.ToString() + "\tcur:" + cur.ToString());
            //instruction이 필요하다면 여기서 reset
        }

        if (cur == 2 && prev == 1)
        {
            start_listening();
        }
        prev = cur;


    }


    unsafe void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
    {
        {
            cur = ((int)shooting_obstacle_script.currTime % 60);
            if (cur == 2 && prev == 1)
            {
                start_listening();
            }
            prev = cur;

        }
    }

    void velo_up()
    {
        pet.GetComponent<Rigidbody>().linearVelocity = 0.7f * Vector3.up;
    }
    void velo_down()
    {
        pet.GetComponent<Rigidbody>().linearVelocity = 0.7f * Vector3.down;
    }
    void velo_0()
    {
        pet.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
    }

    void text_STT_reset()
    {
        text_STT.text = "";
    }
    public void start_listening()
    {
        //idx_answer = MakeRandomNumbers(5)[0];
         text_STT.text = "지금 말하세요!";
        Invoke("text_STT_reset", 1f);
        voiceController_script.StartListening();
        Invoke("stop_listening", 2.5f);

    }

    public void stop_listening()
    {
        voiceController_script.StoptListening();
        //text_instruct.text = "";
        //text_time.text = "";
        //check_answer();
    }

    void OnFinalSpeechResult(string result)
    {
        Debug.Log("FinalSpeechResult 실행");
        text_STT.text = result;
        check_answer(result);

    }

    void check_answer(string res)
    {
        if (res.Contains("뛰") || res.Contains("띠"))
        {
            anim.Play("003_Ball_Jump+Catch");
            velo_up();
            Invoke("velo_down", .5f);
            Invoke("velo_0", 1f);
        }
        else
        {
        }
    }
    #endregion

}
