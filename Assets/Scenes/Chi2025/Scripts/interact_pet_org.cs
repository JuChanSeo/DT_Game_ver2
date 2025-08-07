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

public class interact_pet_org : MonoBehaviour
{
    public InferenceController_G infer_G_script;

    public Animator anim;

    private float lastTouchTime;
    private const float doubleTouchDelay = 0.5f;
    int touch_cnt;
    int touch_cnt_pet;

    //public Vector2 center = Vector2.zero;
    //public bool one_flag;
    //public GameObject gamestart_Button;
    //public Slider slider_time;
    //public TextMeshProUGUI text_gesture;
    //public TextMeshProUGUI text_STT;


    public GameObject speech_bubble;
    public GameObject virtual_hand;
    public GameObject Gauge_BG_red;
    public Image Gauge_red;
    public GameObject Gauge_BG_blue;
    public Image Gauge_blue;
    public GameObject copyed_ball_showup;
    public Rigidbody ball;
    public GameObject pet_skinned;
    public TextMeshProUGUI text_emo;
    public TextMeshProUGUI text_ges;
    //public GameObject ground;
    Rigidbody copyed_ball;
    Rigidbody rgbody;
    Texture2D texture;
    float jump_action_interval;
    int prev;
    int cur;
    bool flag_speech;

    VoiceController voiceController_script;
    Vector3 org_pos;
    Vector3 start_pos;
    Vector3 goal_position;
    Vector3 org_position;
    Vector3 ball_goal_pos;
    Vector3 org_pet_rot;
    Vector3 org_pet_pos;

    SkinnedMeshRenderer face_renderer;

    bool walk_pet;
    bool walk_pet_idle;

    bool track_flag;
    bool return_to_org;

    bool bool_ball_play;//볼 play 할 경우에만 true, 아니면 false
    bool ball_move_flag;

    public bool face_emo;

    // Start is called before the first frame update
    void Start()
    {
        prev = 0;
        cur = 0;
        anim = gameObject.GetComponent<Animator>();
        text_emo.gameObject.SetActive(false);
        text_ges.gameObject.SetActive(false);

        SpeechToText.Instance.onResultCallback = OnFinalSpeechResult;
        voiceController_script = GameObject.Find("VoiceController").GetComponent<VoiceController>();
        Gauge_BG_red.SetActive(false);
        Gauge_BG_blue.SetActive(false);

        //copyed_ball_showup.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.2f;
        //copyed_ball_showup.transform.position = Camera.main.transform.position + Vector3.forward*0.3f + Vector3.down * 0.3f;
        //Debug.Log("Camera.main.transform.position: " + Camera.main.transform.position);
        //copyed_ball_showup.transform.position = Camera.main.transform.position;
        copyed_ball_showup.SetActive(false);

        org_pos = gameObject.transform.position;
        start_pos = org_pos + 2f*Vector3.forward;

        face_renderer = pet_skinned.GetComponent<SkinnedMeshRenderer>();

        infer_G_script = GameObject.Find("InferenceManager").GetComponent<InferenceController_G>();
        //anim.Play("Walk_ahead");
    }

    // Update is called once per frame
    void Update()
    {
        //if (!walk_pet)
        //{
        //    gameObject.transform.position = start_pos;
        //    walk_pet = true;
        //    Debug.Log("character 이동 완료");
        //}

        ////Debug.Log(org_pos +"\t" + start_pos + "\t" + Vector3.Distance(transform.position, org_pos)); 
        //if (Vector3.Distance(transform.position, org_pos) > 0.05f)
        //{
        //    Debug.Log("Character is moving");
        //    transform.position = Vector3.MoveTowards(transform.position, org_pos, Time.deltaTime);
        //}
        //else
        //{
        //    if (!walk_pet_idle)
        //    {
        //        walk_pet_idle = true;
        //        anim.Play("Idle");
        //    }

        //}
        //ground.transform.position = transform.position;
        //ground.transform.rotation = transform.rotation;
        //Debug.Log(transform.forward + "\t" + transform.right + "\t" + transform.up);
        if (copyed_ball_showup.activeSelf == false) copyed_ball_showup.transform.position =
                Camera.main.transform.position + Camera.main.transform.forward * 0.4f;

        if(bool_ball_play)
        {
            var cent = infer_G_script.center;
            Debug.Log(cent);
            var ray1 = Camera.main.ScreenPointToRay(cent);
            if (infer_G_script.text_gesture.text == "fist")
            {
                //Debug.Log("fist");
                if (!Gauge_BG_blue.activeSelf) Gauge_BG_blue.SetActive(true);
                if (touch_cnt < 30) ++touch_cnt;
                copyed_ball_showup.transform.position =
                                Camera.main.ScreenToWorldPoint(new Vector3(cent.x, cent.y, 0.4f));
                Gauge_BG_blue.transform.position = Camera.main.WorldToScreenPoint(copyed_ball_showup.transform.position) + Vector3.up * 300f;
                float fillA_Value_blue = (float)touch_cnt / 30f;
                Gauge_blue.fillAmount = fillA_Value_blue;
                return;
            }
            else if(infer_G_script.text_gesture.text == "palm")
            {
                //Debug.Log("palm");
                if (touch_cnt == 30)
                {
                    infer_G_script.excute_ges_recog = false;
                    Gauge_BG_blue.SetActive(false);
                    Gauge_blue.fillAmount = 0;
                    touch_cnt = 0;
                    if (copyed_ball == null && copyed_ball_showup.activeSelf == true)
                    {
                        org_pet_rot = transform.eulerAngles;
                        org_pet_pos = transform.position;
                        copyed_ball = Instantiate(ball, ray1.origin, Quaternion.identity);
                        ball_move_flag = true;
                        ball_goal_pos = transform.position
                                                + UnityEngine.Random.Range(-0.5f, 0.5f) * transform.right - UnityEngine.Random.Range(0.5f, 1.5f) * transform.forward;
                        //ball_goal_pos = transform.position
                        //                        + UnityEngine.Random.Range(-2f, 2f) * transform.right - UnityEngine.Random.Range(1f, 5f) * transform.forward;
                        copyed_ball_showup.SetActive(false);
                    }

                }
            }
        }


        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            var ray = Camera.main.ScreenPointToRay(touch.position);
            Physics.Raycast(ray, out var hit, float.PositiveInfinity);

            switch(touch.phase)
            {
                case TouchPhase.Began:
                    touch_cnt = 0;
                    touch_cnt_pet = 0;

                    Debug.Log("Touch begin");
                    if (Time.time - lastTouchTime < doubleTouchDelay) // 더블터치 판정
                    {
                        if (hit.transform.name.StartsWith("pomeLV05"))
                        {
                            //anim.Play("002_Ball_Jump");
                            //bgm_player_.jump_sound_excute();
                        }
                    }


                    Gauge_red.fillAmount = 0;
                    break;

                case TouchPhase.Moved:
                    if (hit.transform.name.StartsWith("pomeLV05") && copyed_ball_showup.activeSelf == false)
                    {
                        if(virtual_hand.activeSelf != true) virtual_hand.SetActive(true);
                        if(Gauge_BG_red.activeSelf != true) Gauge_BG_red.SetActive(true);
                        Gauge_BG_red.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position) + Vector3.up * 450f;

                        ++touch_cnt_pet;
                        //Debug.Log(touch_cnt_pet + "\t" + ((float)touch_cnt_pet / 150f) + "\t" + Gauge.name);
                        float fillA_Value = (float)touch_cnt_pet / 70f;
                        Gauge_red.fillAmount = fillA_Value;
                        //Debug.Log(anim.GetCurrentAnimatorStateInfo(0));
                        if (touch_cnt_pet > 70 && anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                        {
                            virtual_hand.SetActive(false);
                            Gauge_BG_red.SetActive(false);
                            Gauge_red.fillAmount = 0;
                            anim.Play("Idle_Blend_LieOnBack");
                            heart_effect_true();
                        }
                        //Invoke("Set_Pome_Idle", 4f);
                    }
                    else
                    {
                        //++touch_cnt;
                        //Debug.Log("펫은 안 만지는중");
                        //{
                        //    copyed_ball_showup.transform.position =
                        //        Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0.4f));
                        //}
                    }
                    break;

                case TouchPhase.Ended:

                    Debug.Log("Touch End");

                    lastTouchTime = Time.time;

                    virtual_hand.SetActive(false);
                    Gauge_BG_red.SetActive(false);
                    Gauge_red.fillAmount = 0;
                    Gauge_BG_blue.SetActive(false);
                    Gauge_blue.fillAmount = 0;

                    break;

            }
        }
        ball_move();
        if (goal_position != Vector3.zero)
            move_to_point(goal_position);
    }

    public void test_fn()
    {
        org_pet_rot = transform.eulerAngles;
        ball_goal_pos = transform.position
                        + UnityEngine.Random.Range(-0.5f, 0.5f) * transform.right - UnityEngine.Random.Range(0.5f, 1.5f) * transform.forward;

        GameObject.Find("test_gb").transform.position = ball_goal_pos;
    }

    public void test_fn2()
    {
        Debug.Log("test_fn2 실행" + "\t");
        ball_move_flag = true;
    }

    public void move_to_point(Vector3 hitpoint)
    {
        if (track_flag)
        {
            //일정거리 이상 가깝지 않으면 MoveToward
            if (Vector3.Distance(transform.position, goal_position) > 0.03f)
            {
                //Debug.Log(Vector3.Distance(spawnedObject.transform.position, hitpoint));
                //spawnedObject.transform.position = Vector3.MoveTowards(spawnedObject.transform.position,
                //													  hitpoint /*- 0.5f * Vector3.up*/,
                //													  0.01f);

                Vector3 dir = hitpoint - transform.position;
                //dir.y = 0f;
                Quaternion rot = Quaternion.LookRotation(dir.normalized);


                transform.position = Vector3.MoveTowards(transform.position,
                                                                      hitpoint,
                                                                      0.01f);
                transform.rotation = rot;

            }
            //일정거리 이상 가까워지면 track_flag는 false로, 공을 주운 후에 원래 자리로 back
            else
            {
                track_flag = false;

                //org Pos로 돌아가기 위한 준비
                if (return_to_org)
                {
                    //공을 줍는듯한 reaction
                    anim.Play("Neck_Dn");

                    Invoke("show_ball_on_mouth", 1f);

                    //return_to_org 값을 false로
                    return_to_org = false;
                    //3 초 후에 다시 돌아올 수 있도록 goal_position 값을 org_position으로 변경
                    Invoke("back_to_org", 3f);
                }
                //org position으로 도착하면 입에 물고 있는 공 없애기, 카메라 쪽으로 몸 방향 돌리고 말풍선 실행 
                else
                {
                    transform.GetChild(6).gameObject.SetActive(false);//입에 물고있는 공 없애기
                    transform.eulerAngles = org_pet_rot;
                    transform.position = org_pet_pos;
                    Debug.Log("transform_rotation 종료: " + transform.eulerAngles + "\t" + org_pet_rot);
                    set_text_speechBubble("공 가져오기에\n성공했어요!");
                    anim.Play("Idle");
                    setemotion("happy");
                    heart_effect_true();
                    track_flag = false;
                    //transform.GetComponent<move_pet>().invoke_walk_after_3sec();
                    //ground.SetActive(false);
                }
            }

        }
    }

    public void ball_move()
    {
        //var te_copyed_ball = GameObject.Find("test_gb").transform;
        //copyed_ball = GameObject.Find("test_gb").gameObject.transform.GetComponent<Rigidbody>();
        if (ball_move_flag)
        {
            //발사된 공이 의도된 위치에 도착한다면
            if(Vector3.Distance(copyed_ball.position, ball_goal_pos) < 0.01f)
            {
                copyed_ball.position = ball_goal_pos;
                track_flag = true;
                return_to_org = true;
                org_position = transform.position;
                goal_position = copyed_ball.transform.position;
                ball_move_flag = false;
                bool_ball_play = false;
                text_ges.gameObject.SetActive(false);
                anim.Play("Walk_ahead");
                
            }
            //발사된 공이 의도된 위치에 도착할 때 까지 공이 움직인다.
            else
            {
                copyed_ball.position = Vector3.MoveTowards(copyed_ball.position, ball_goal_pos, 0.02f);
            }
        }
    }

    void show_ball_on_mouth()
    {
        Destroy(copyed_ball.gameObject);
        copyed_ball = null;
        transform.GetChild(6).gameObject.SetActive(true);

    }

    void back_to_org()
    {
        goal_position = org_position;
        track_flag = true;
        anim.Play("Walk_ahead");

    }

    public void set_ball_velocity_0()
    {
        //if (copyed_ball.transform.position.y < -5f)
        //{
        //    Destroy(copyed_ball.gameObject);
        //    copyed_ball = null;
        //    return;
        //}

        Debug.Log("set_ball_velocity_0 실행");
        bool_ball_play = false;
        if (copyed_ball != null && copyed_ball.gameObject.activeSelf == true)
        {
            rgbody = copyed_ball.GetComponent<Rigidbody>();
            rgbody.isKinematic = true;
            rgbody.useGravity = false;
            rgbody.linearVelocity = Vector3.zero;
            goal_position = copyed_ball.transform.position;
            if (copyed_ball.transform.position.y > -5f)
            {
                //set_to_kinematic();
                track_flag = true;
                return_to_org = true;
                org_position = transform.position;
            }
        }
        //anim = petctrl_script.spawnedObject.transform.GetChild(0).GetComponent<Animator>();
        //anim.Play("Walk");
    }

    //private void set_to_kinematic()
    //{
    //    Rigidbody rg_body = transform.GetChild(0).GetComponent<Rigidbody>();
    //    rg_body.isKinematic = true;
    //    rg_body.useGravity = false;
    //    //Debug.Log("Kinematic mode");
    //}

    public void face_bt_click()
    {
        face_emo = !face_emo;

        if(face_emo)//face_emo == true이면 표정 따라하기 실행
        {
            text_emo.gameObject.SetActive(true);
            Invoke("face_bt_click", 10f);
        }
        else
        {
            text_emo.gameObject.SetActive(false);
            //transform.GetComponent<move_pet>().invoke_walk_after_3sec();
        }
    }

    public void feed_bt_click()
    {
        gameObject.transform.GetChild(4).gameObject.SetActive(true);
        set_text_speechBubble("기다려! 명령 \n내려주세요");
        start_listening();
        Debug.Log("feed버튼 클릭!");
    }

    public void command_hand_bt_click()
    {
        set_text_speechBubble("손! 명령 \n내려주세요");
        start_listening();
    }

    public void ballplay_bt_clicked()
    {
        if (!infer_G_script.excute_ges_recog) infer_G_script.excute_ges_recog = true;
        text_ges.gameObject.SetActive(true);
        bool_ball_play = true;
        copyed_ball_showup.SetActive(true);
    }

    void get_down_dog()
    {
        anim.Play("Idle_Blend_Get_Down");
    }

    void eat_anim()
    {
        anim.Play("Eat");
    }

    void instruct_eat()
    {
        set_text_speechBubble("먹어! 명령 \n내려주세요");
        start_listening();
    }

    void disappear_food()
    {
        gameObject.transform.GetChild(4).gameObject.SetActive(false);
        heart_effect_true();
        setemotion("smile");
    }

    void text_STT_reset()
    {
        //text_STT.text = "";
    }
    public void start_listening()
    {
        //idx_answer = MakeRandomNumbers(5)[0];
        //text_STT.text = "지금 말하세요!";
        voiceController_script.StartListening();
        Invoke("stop_listening", 3f);//listening 시작 후 3초 안에 명령을 해야 함

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
        //text_STT.text = result;
        if(result.Contains("기") || result.Contains("다") || result.Contains("려"))
        {
            get_down_dog();
            Invoke("instruct_eat", 2.5f);
        }
        else if(result.Contains("먹") || result.Contains("어"))
        {
            anim.Play("Get_Down_Blend_Idle");
            Invoke("eat_anim", 1.5f);
            Invoke("disappear_food", 3f);
            //transform.GetComponent<move_pet>().invoke_walk_after_3sec();
        }
        else if(result.Contains("손"))
        {
            anim.Play("Raise_hand");
            //transform.GetComponent<move_pet>().invoke_walk_after_3sec();
        }
        else// 제대로 인식하지 못했을 경우
        {
            set_text_speechBubble("다시 한번 명령해볼까요?");
        }

        
    }

    public void set_text_speechBubble(string message)
    {
        speech_bubble.SetActive(true);
        if (speech_bubble.gameObject.activeSelf == true)
        {
            //Vector3 screenPos = Camera.main.WorldToScreenPoint(spawnedObject.transform.position + 0.5f * Vector3.up
            //													+ 0.3f * Vector3.right);
            speech_bubble.transform.position = gameObject.transform.position + 0.4f * Vector3.up
                                              + 0.1f * Vector3.right + 0.1f * Vector3.back;
            //speech_bubble.transform.position = gameObject.transform.position + 0.7f * Vector3.up
            //                      + 0.3f * Vector3.left + 0.1f * Vector3.back;

        }

        TMP_Text txt_bubble = speech_bubble.transform.GetChild(0).transform.GetComponent<TMP_Text>();
        //Debug.Log(speechbubble.transform.GetChild(0).transform.name);
        txt_bubble.text = message;
        Invoke("init_destroy_speechBubble", 3f);

    }

    void init_destroy_speechBubble()
    {
        TMP_Text txt_bubble = speech_bubble.transform.GetChild(0).transform.GetComponent<TMP_Text>();
        txt_bubble.text = "";
        speech_bubble.SetActive(false);
    }

    public void setemotion_nodefault(string emo)
    {
        if (emo == "normal")
        {
            for (int i = 0; i < 7; i++)
            {
                face_renderer.SetBlendShapeWeight(i, 0);
            }
            return;
        }

        //0:blink, 1:bark, 2:smile, 3:angry, 4:sad, 5:happy, 6:suprise
        int emo_label;
        emo_label = 0;
        if (emo == "blink") emo_label = 0;
        else if (emo == "bark") emo_label = 1;
        else if (emo == "smile") emo_label = 2;
        else if (emo == "angry") emo_label = 3;
        else if (emo == "sad") emo_label = 4;
        else if (emo == "happy") emo_label = 5;
        else if (emo == "suprise") emo_label = 6;

        for (int i = 0; i < 7; i++)
        {
            if (i == emo_label)
            {
                face_renderer.SetBlendShapeWeight(i, 100);
            }
            else
            {
                face_renderer.SetBlendShapeWeight(i, 0);
            }
        }

    }

    public void setemotion(string emo)
    {
        //0:blink, 1:bark, 2:smile, 3:angry, 4:sad, 5:happy, 6:suprise
        int emo_label;
        if (emo == "blink") emo_label = 0;
        else if (emo == "bark") emo_label = 1;
        else if (emo == "smile") emo_label = 2;
        else if (emo == "angry") emo_label = 3;
        else if (emo == "sad") emo_label = 4;
        else if (emo == "happy") emo_label = 5;
        else emo_label = 6;

        face_renderer.SetBlendShapeWeight(emo_label, 100);
        Invoke("setemotion_default", 2f);
    }

    void setemotion_default()
    {
        for (int i = 0; i < 7; i++)
        {
            face_renderer.SetBlendShapeWeight(i, 0);
        }

    }

    public void heart_effect_true()
    {
        transform.GetChild(3).transform.gameObject.SetActive(true);
        Invoke("heart_effect_false", 3f);
    }

    void heart_effect_false()
    {
        transform.GetChild(3).transform.gameObject.SetActive(false);
    }

}
