using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextSpeech;
using TMPro;
using UnityEngine.SceneManagement;

public class touch_interact_tutorial : MonoBehaviour
{
    private Animator anim;
    public GameObject[] pets_copy;
    float jump_action_interval;
    GameObject pet;
    InferenceController_edit inf_edit_script;
    face_emo_edit face_emo_edit_script;
    VoiceController voiceController_script;
    public GameObject text_instruct_panel;
    public GameObject GameEnd_panel;
    public TextMeshProUGUI text_instruct;
    public TextMeshProUGUI text_last;

    bool vocie_or_ges;// false일 경우 제스처 이용, true일 경우 보이스 이용
    bool jump_or_not;
    bool check_per_interval;//jump or not if문이 인터벌당 한번만 돌도록 해주는 값

    bool start;
    bool excute_finish;
    float interval;
    GameManager gameManager_script;
    shooting_obstacle shooting_obs_script;

    public GameObject Button_gamestart;
    public GameObject gb_agilityGame;

    int tutorial_step;
    public UnityEngine.UI.Text tutorial_msg;
    public GameObject tutorial_start_bt;
    public GameObject tutorial_next_bt;
    public GameObject tutorial_end_panel;
    public TextMeshProUGUI text_gesture;
    fly_randmoving fly_Randmoving_script;
    bool excute_once;
    public GameObject one_ges;

    int step2_cnt;
    bool step2_bool;

    int step4_cnt;
    bool step4_bool;

    int step6_cnt;
    bool step6_bool;

    main_eff main_eff_script;
    care_effect care_effect_script;
    bgm_player bgm_player_script;
    Logger logger_script;

    // Start is called before the first frame update
    void Start()
    {
        //text_instruct.gameObject.SetActive(false);
        text_instruct_panel.SetActive(false);
        interval = 8f;
        pet = pets_copy[PlayerPrefs.GetInt("Level_pet")];
        jump_action_interval = 4f;
        anim = pet.GetComponent<Animator>();

        gameManager_script = GameObject.Find("agility_game").GetComponent<GameManager>();
        shooting_obs_script = GameObject.Find("agility_game").GetComponent<shooting_obstacle>();
        inf_edit_script = GameObject.Find("InferenceManager").GetComponent<InferenceController_edit>();
        face_emo_edit_script = GameObject.Find("facialexpression").GetComponent<face_emo_edit>();
        SpeechToText.Instance.onResultCallback = OnFinalSpeechResult;
        voiceController_script = GameObject.Find("VoiceController").GetComponent<VoiceController>();

        face_emo_edit_script.bool_fmodel = false;
        inf_edit_script.excute_ges_recog = true;
        check_per_interval = true;

        tutorial_end_panel.SetActive(false);
        tutorial_next_bt.SetActive(false);
        one_ges.SetActive(false);

        main_eff_script = GameObject.Find("main_effect_player").GetComponent<main_eff>();
        care_effect_script = GameObject.Find("care_effect_player").GetComponent<care_effect>();
        bgm_player_script = GameObject.Find("Audio player").GetComponent<bgm_player>();
        logger_script = GameObject.Find("logger_gb").GetComponent<Logger>();

        StartCoroutine(bgm_player_script.excute_sound("67", 1f));
    }


    // Update is called once per frame
    void Update()
    {

        //if (jump_action_interval > 5f && check_per_interval)
        //{
        //    check_per_interval = false;
        //    if (!vocie_or_ges)
        //    {
        //        text_instruct.text = "손을 이용하세요!";
        //        face_emo_edit_script.bool_fmodel = true;
        //        vocie_or_ges = !vocie_or_ges;
        //    }
        //    else
        //    {
        //        text_instruct.text = "목소리를 이용하세요!";
        //        start_listening();
        //        vocie_or_ges = !vocie_or_ges;
        //    }
        //}

        //bt_jump_click_ges();

        //if(gameManager_script.cnt_succes == 5)
        //{
        //    if(!excute_finish)
        //    {
        //        excute_finish = true;
        //        PlayerPrefs.SetFloat("exp", PlayerPrefs.GetFloat("exp") + 0.03f);
        //        PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") + 2);
        //        text_last.gameObject.SetActive(true);
        //        text_last.text = "잘 하셨어요!\n 보상은 다음과 같습니다";
        //        pet.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //        anim.Play("Sit");
        //        Invoke("load_AR_scene", 4f);
        //        return;
        //    }
        //}

        //if (gameManager_script.cnt_fail == 10)
        //{
        //    if (!excute_finish)
        //    {
        //        excute_finish = true;
        //        //실패 문구 보여주기
        //        TextMeshProUGUI text_fail = GameObject.Find("Text_fail").GetComponent<TextMeshProUGUI>();
        //        text_fail.text = "다음 기회에 다시 도전해봐요!";
        //        Invoke("load_AR_scene", 4f);
        //        return;
        //    }
        //}

        if(tutorial_step == 2)
        {
            if(!step2_bool)
            {
                step2();
            }
        }
        else if(tutorial_step == 4)
        {
            if(!step4_bool)
            {
                step4();
            }
        }
        else if(tutorial_step == 6)
        {
            if(!step6_bool)
            {
                step6();
            }
        }
    }

    public void load_AR_scene()
    {
        logger_script.logger_master.insert_data("산책하기 튜토리얼 종료, 본 게임으로 넘어감");
        SceneManager.LoadScene("10_AR_interaction_MZ");
    }

    void step2()
    {
        if (face_emo_edit_script.bool_fmodel && inf_edit_script.text_ges.text == "one")
        {
            logger_script.logger_master.insert_data("tutorial_손동작 이용하여 강아지 점프 시키기. 성공횟수: " + step4_cnt.ToString());
            jump_or_not = true;
            just_jump();
            one_ges.SetActive(false);

            care_effect_script.sound_correct();
            step2_cnt++;
            step2_bool = true;

            if (step2_cnt < 5)
            {
                tutorial_msg.text = "잘 하셨어요! 남은 횟수:" + (5-step2_cnt).ToString();
                face_emo_edit_script.bool_fmodel = false;
                inf_edit_script.excute_ges_recog = false;
                Invoke("step2_false", 3f);
            }
            else
            {
                face_emo_edit_script.bool_fmodel = false;
                inf_edit_script.excute_ges_recog = false;
                tutorial_msg.text = "잘 하셨어요! 남은 횟수: " + step2_cnt.ToString();
                Invoke("game_next_bt_clicked",5f);
            }
        }
    }

    void step2_false()
    {
        tutorial_msg.text = "그림과 같이 손 모양을 만들어서 강아지가 뛰어보도록 할까요?";
        face_emo_edit_script.bool_fmodel = true;
        inf_edit_script.excute_ges_recog = true;
        text_gesture.text = "";
        Invoke("step2_delay_false", 2f);
    }

    void step2_delay_false()
    {
        one_ges.SetActive(true);
        step2_bool = false;

    }

    void step4()
    {
        start_listening();
    }

    void step4_false()
    {
        tutorial_msg.text = "강아지에게 '뛰어!'라고 해볼까요?";
        step4_bool = false;
    }

    void step6()
    {
        if(step6_cnt % 2 == 0)
        {
            Debug.Log("손으로!");
            if (face_emo_edit_script.bool_fmodel && inf_edit_script.text_ges.text == "one")
            {
                jump_or_not = true;
                just_jump();
                logger_script.logger_master.insert_data("tutorial_손과 목소리 함께 이용하여 강아지 점프 시키기. 성공횟수: " + step6_cnt.ToString());

                step6_cnt++;
                step6_bool = true;

                if (step6_cnt < 9)
                {
                    tutorial_msg.text = "잘 하셨어요! 남은 횟수: " + (9-step6_cnt).ToString();
                    face_emo_edit_script.bool_fmodel = false;
                    inf_edit_script.excute_ges_recog = false;
                    Invoke("step6_false", 3f);
                }
                else
                {
                    face_emo_edit_script.bool_fmodel = false;
                    inf_edit_script.excute_ges_recog = false;
                    tutorial_msg.text = "잘 하셨어요! 남은 횟수: " + (9-step6_cnt).ToString();
                    Invoke("game_next_bt_clicked", 5f);
                }
            }

        }
        else
        {
            Debug.Log("목소리로!");
            start_listening();

        }
    }

    void step6_false()
    {
        if (step6_cnt % 2 == 0)
        {
            tutorial_msg.text = "손을 이용해서 강아지를 뛰게 해주세요!";
            face_emo_edit_script.bool_fmodel = true;
            inf_edit_script.excute_ges_recog = true;
            text_gesture.text = "";
            Invoke("step6_delay_false", 2f);
        }
        else
        {
            tutorial_msg.text = "목소리를 이용해서 강아지를 뛰게 해주세요!";
            step6_bool = false;
        }
    }

    void step6_delay_false()
    {
        step6_bool = false;
    }


    void just_jump()
    {
        //8초에 한 번씩 실행. obstacle이 생성되어 다시 날라오는 시간이 8초. 
        //Invoke("just_jump", 8f);
        if (jump_or_not)
        {
            main_eff_script.sound_jump_0();
            anim.Play("Jump_NoWalk");
            velo_up();
            Invoke("velo_down", .7f);
            Invoke("velo_0", 1.4f);
        }
        jump_action_interval = 0;
        inf_edit_script.face_texture.texture = null;
        inf_edit_script.text_ges.text = "";
        //jump_or_not = false;
        check_per_interval = true;
        face_emo_edit_script.bool_fmodel = false;
        jump_or_not = false;
        //Invoke("just_jump", 4f);

    }

    public void game_start_bt_clicked()
    {
        //gb_agilityGame.GetComponent<shooting_obstacle>().create_and_destroy_cube();
        //start = true;
        //text_instruct_panel.SetActive(true);
        //text_instruct.gameObject.SetActive(true);
        ////장애물 발사도 시작
        ////강아지 앞으로 걷기 시작
        //anim.Play("Walk_ahead");
        //pet.GetComponent<Rigidbody>().velocity = new Vector3(0.5f, 0, 0);
        //Invoke("just_jump", 4f);

        tutorial_start_bt.SetActive(false);
        tutorial_next_bt.SetActive(true);
        tutorial_msg.text = "산책하기 게임은 산책중인 강아지 앞으로\n다가오는 장애물을 강아지가 뛰어서 피하는 게임이에요!";
        StartCoroutine(bgm_player_script.excute_sound("68", 1f));
    }

    public void game_next_bt_clicked()
    {
        if(tutorial_step == 0)
        {
            StartCoroutine(bgm_player_script.excute_sound("69", 0));
            main_eff_script.sound_button1();
            logger_script.logger_master.insert_data("산책하기 튜토리얼 다음으로 버튼 클릭(step0)");
            tutorial_msg.text = "먼저 손 동작을 이용해서 강아지를 뛰게 해볼게요!";
            tutorial_step++;
        }
        else if(tutorial_step == 1)
        {
            StartCoroutine(bgm_player_script.excute_sound("70", 0));
            tutorial_next_bt.SetActive(false);
            one_ges.SetActive(true);
            tutorial_msg.text = "화면 중앙에 보이는 손모양을 따라하면 강아지가\n제자리에서 뛰어요! 한번 해볼까요?";
            face_emo_edit_script.bool_fmodel = true;
            inf_edit_script.excute_ges_recog = true;
            tutorial_step++;
        }
        else if(tutorial_step == 2)
        {
            StartCoroutine(bgm_player_script.excute_sound("71", 0));
            tutorial_next_bt.SetActive(true);
            tutorial_msg.text = "이번에는 목소리를 이용해서 강아지를 뛰게 해볼게요!";
            tutorial_step++;
        }
        else if(tutorial_step == 3)
        {
            StartCoroutine(bgm_player_script.excute_sound("72", 0));
            main_eff_script.sound_button1();
            logger_script.logger_master.insert_data("산책하기 튜토리얼 다음으로 버튼 클릭(step3)");
            tutorial_next_bt.SetActive(false);
            tutorial_msg.text = "강아지에게 '뛰어!'라고 해볼까요?";
            tutorial_step++;
        }
        else if(tutorial_step == 4)
        {
            StartCoroutine(bgm_player_script.excute_sound("73", 0));
            tutorial_next_bt.SetActive(true);
            tutorial_msg.text = "이번에는 번갈아 가면서 강아지에게 명령 해볼까요?";
            tutorial_step++;
        }
        else if(tutorial_step == 5)
        {
            StartCoroutine(bgm_player_script.excute_sound("74", 0));
            main_eff_script.sound_button1();
            logger_script.logger_master.insert_data("산책하기 튜토리얼 다음으로 버튼 클릭(step5)");
            tutorial_next_bt.SetActive(false);
            tutorial_msg.text = "손을 이용해서 강아지를 뛰게 해주세요!";
            tutorial_step++;

            face_emo_edit_script.bool_fmodel = true;
            inf_edit_script.excute_ges_recog = true;
            text_gesture.text = "";
        }
        else if(tutorial_step == 6)
        {
            Button_gamestart.SetActive(false);
            tutorial_end_panel.SetActive(true);
            Invoke("load_main_Scene", 5f);
        }
    }

    void load_main_Scene()
    {
        SceneManager.LoadScene("24_Virtual_agility");
    }

    public void bt_jump_click_ges()
    {
        if (face_emo_edit_script.bool_fmodel && text_gesture.text == "one")
        {
            jump_or_not = true;
        }

        //jump_action_interval = 0;
        //inf_edit_script.face_texture.texture = null;
        //inf_edit_script.text_ges.text = "";
        ////jump_or_not = false;
        //check_per_interval = true;
        //face_emo_edit_script.bool_fmodel = false;

    }

    //public void bt_jump_click()
    //{
    //    //한번 실행되면 최소 몇 초 이후에 다시 실행될 수 있게
    //    if (jump_action_interval > interval-1f && jump_or_not)
    //    {
    //        //interval = Random.Range(5.0f, 10.0f);
    //        //Debug.Log("다음 인터벌: "+interval.ToString());
    //        anim.Play("Jump");
    //        velo_up();
    //        Invoke("velo_down", .7f);
    //        Invoke("velo_0", 1.4f);
    //    }

    //    jump_action_interval = 0;
    //    inf_edit_script.face_texture.texture = null;
    //    inf_edit_script.text_ges.text = "";
    //    jump_or_not = false;
    //    check_per_interval = true;
    //    face_emo_edit_script.bool_fmodel = false;
    //}

    void velo_up()
    {
        pet.GetComponent<Rigidbody>().linearVelocity = 2f * Vector3.up + 0*new Vector3(0.2f, 0, 0);
    }
    void velo_down()
    {
        pet.GetComponent<Rigidbody>().linearVelocity = 2f * Vector3.down + 0*new Vector3(0.2f, 0, 0);
    }
    void velo_0()
    {
        pet.GetComponent<Rigidbody>().linearVelocity = 0*new Vector3(0.5f, 0, 0);
    }

    public void start_listening()
    {
        Debug.Log("start_listening 시작");
        voiceController_script.StartListening();
        Invoke("stop_listening", 5f);

        if (tutorial_step == 4) step4_bool = true;
        else if (tutorial_step == 6) step6_bool = true;
    }

    public void stop_listening()
    {
        voiceController_script.StoptListening();
    }
    void OnFinalSpeechResult(string result)
    {
        Debug.Log("FinalSpeech 실행\t" + result);

        if (result.Contains("뛰") || result.Contains("띠") || result.Contains("어")
            || result.Contains("점") || result.Contains("프"))
        {
            jump_or_not = true;
            just_jump();
            care_effect_script.sound_correct();
            if (tutorial_step == 4)
            {
                step4_cnt++;
                logger_script.logger_master.insert_data("tutorial_목소리 이용하여 강아지 점프 시키기. 성공횟수: " + step4_cnt.ToString());
                if (step4_cnt < 5)
                {
                    tutorial_msg.text = "잘 하셨어요! 남은 횟수:" + (5 - step4_cnt).ToString();
                    Invoke("step4_false", 5f);
                }
                else
                {
                    tutorial_msg.text = "잘 하셨어요!";
                    Invoke("game_next_bt_clicked", 5f);
                }
            }
            else if (tutorial_step == 6)
            {
                step6_cnt++;
                logger_script.logger_master.insert_data("tutorial_손과 목소리 함께 이용하여 강아지 점프 시키기. 성공횟수: " + step6_cnt.ToString());
                if (step6_cnt < 9)
                {
                    tutorial_msg.text = "잘 하셨어요! 남은 횟수: " + (9-step6_cnt).ToString();
                    Invoke("step6_false", 5f);
                }
            }
        }
        else
        {
            logger_script.logger_master.insert_data("tutorial_손과 목소리 함께 이용하여 강아지 점프 시키기 실패.");
            care_effect_script.sound_false2();
            tutorial_msg.text = "다시 해볼까요?";
            if (tutorial_step == 4) Invoke("step4_false", 5f);
            else if (tutorial_step == 6) Invoke("step6_false", 5f);
        }

        //jump_action_interval = 0;
        //inf_edit_script.face_texture.texture = null;
        //inf_edit_script.text_ges.text = "";
        ////jump_or_not = false;
        //check_per_interval = true;
        //face_emo_edit_script.bool_fmodel = false;

    }
}
