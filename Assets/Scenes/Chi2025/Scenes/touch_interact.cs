using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextSpeech;
using TMPro;
using UnityEngine.SceneManagement;

public class touch_interact : MonoBehaviour
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

        main_eff_script = GameObject.Find("main_effect_player").GetComponent<main_eff>();
        care_effect_script = GameObject.Find("care_effect_player").GetComponent<care_effect>();
        bgm_player_script = GameObject.Find("Audio player").GetComponent<bgm_player>();
        logger_script = GameObject.Find("logger_gb").GetComponent<Logger>();
    }


    // Update is called once per frame
    void Update()
    {
        if (!start) return;

        if (jump_action_interval < interval) jump_action_interval += Time.deltaTime;

        if (jump_action_interval > 5f && check_per_interval)
        {
            check_per_interval = false;
            if (!vocie_or_ges)
            {
                logger_script.logger_master.insert_data("손동작 이용해서 장애물 넘기 시도");
                text_instruct.text = "손을 이용하세요!";
                face_emo_edit_script.bool_fmodel = true;
                vocie_or_ges = !vocie_or_ges;
            }
            else
            {
                logger_script.logger_master.insert_data("목소리 이용해서 장애물 넘기 시도");
                text_instruct.text = "목소리를 이용하세요!";
                start_listening();
                vocie_or_ges = !vocie_or_ges;
            }
        }

        bt_jump_click_ges();

        if(gameManager_script.cnt_succes == 5)
        {
            if(!excute_finish)
            {
                logger_script.logger_master.insert_data("산책하기 게임 종료. 게임 성공");
                excute_finish = true;
                PlayerPrefs.SetFloat("exp", PlayerPrefs.GetFloat("exp") + 0.03f);
                PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") + 2);
                text_last.gameObject.SetActive(true);
                text_last.text = "잘 하셨어요!\n 보상은 다음과 같습니다";
                pet.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                anim.Play("Sit");
                Invoke("load_AR_scene", 4f);
                return;
            }
        }

        if (gameManager_script.cnt_fail == 10)
        {
            if (!excute_finish)
            {
                logger_script.logger_master.insert_data("산책하기 게임 종료. 게임 실패!");
                excute_finish = true;
                //실패 문구 보여주기
                TextMeshProUGUI text_fail = GameObject.Find("Text_fail").GetComponent<TextMeshProUGUI>();
                text_fail.text = "다음 기회에 다시 도전해봐요!";
                Invoke("load_AR_scene", 4f);
                return;
            }
        }
    }

    public void load_AR_scene()
    {
        SceneManager.LoadScene("10_AR_interaction_MZ");
    }

    void just_jump()
    {
        //8초에 한 번씩 실행. obstacle이 생성되어 다시 날라오는 시간이 8초. 
        Invoke("just_jump", 8f);
        if (jump_or_not)
        {
            main_eff_script.sound_jump_0();
            anim.Play("Jump");
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
        gb_agilityGame.GetComponent<shooting_obstacle>().create_and_destroy_cube();
        start = true;
        text_instruct_panel.SetActive(true);
        text_instruct.gameObject.SetActive(true);
        //장애물 발사도 시작
        //강아지 앞으로 걷기 시작
        anim.Play("Walk_ahead");
        pet.GetComponent<Rigidbody>().linearVelocity = new Vector3(0.5f, 0, 0);
        Invoke("just_jump", 4f);
        Button_gamestart.SetActive(false);
    }

    public void bt_jump_click_ges()
    {
        if (face_emo_edit_script.bool_fmodel && inf_edit_script.text_ges.text == "one")
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
        pet.GetComponent<Rigidbody>().linearVelocity = 2f * Vector3.up + new Vector3(0.5f, 0, 0);
    }
    void velo_down()
    {
        pet.GetComponent<Rigidbody>().linearVelocity = 2f * Vector3.down + new Vector3(0.5f, 0, 0);
    }
    void velo_0()
    {
        pet.GetComponent<Rigidbody>().linearVelocity = new Vector3(0.5f, 0, 0);
    }

    public void start_listening()
    {
        voiceController_script.StartListening();
        Invoke("stop_listening", 2f);
    }

    public void stop_listening()
    {
        voiceController_script.StoptListening();
        //jump_action_interval = 0;
        //inf_edit_script.face_texture.texture = null;
        //inf_edit_script.text_ges.text = "";
        ////jump_or_not = false;
        //check_per_interval = true;
        //face_emo_edit_script.bool_fmodel = false;
    }
    void OnFinalSpeechResult(string result)
    {
        Debug.Log("FinalSpeech 실행\t" + result);

        if (result.Contains("뛰") || result.Contains("띠") || result.Contains("어")
            || result.Contains("점") || result.Contains("프"))
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
}
