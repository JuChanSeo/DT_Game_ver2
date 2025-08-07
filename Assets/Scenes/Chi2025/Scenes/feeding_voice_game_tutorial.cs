using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextSpeech;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class feeding_voice_game_tutorial : MonoBehaviour
{
    VoiceController voiceController_script;

    GameObject Pet;

    public GameObject gamestart_Button;
    public GameObject instruct_panel;
    public TextMeshProUGUI instruct_text;
    public GameObject resultPrefab; ///
    public TextMeshProUGUI result_text;
    public Slider slider_time;
    public GameObject gameend_panel;///
    public TextMeshProUGUI text_last;

    public GameObject speechPanel; ///

    private float time_remain;
    private bool start_flag;

    int cnt_succes;
    int cnt_fail;
    int answer_idx;
    int[] shuffled_idx;

    public List<GameObject> list_command;
    List<string> list_command_select_answer;

    List<string> list_instruct = new List<string>()
    { "강아지를 불러주세요",
      "강아지가 한바퀴 돌게 해주세요",
      "강아지가 제자리에서 점프하게 해주세요",
      "강아지가 눕게 해주세요",
      "강아지가 애교부리게 해주세요"};


    Vector3 loc0 = new Vector3(496, 862, 0);
    Vector3 loc1 = new Vector3(496, 673, 0);
    Vector3 loc2 = new Vector3(496, 484, 0);
    List<Vector3> location_3 = new List<Vector3>();

    //public Animator anim_Lv1;
    Animator anim;

    public QuestManager_daily questM_daily_script;
    public QuestManager_weekly questM_weekly_script;

    float time_limit;
    int cnt_answer;

    int tutorial_step;
    public Text tutorial_msg;
    public GameObject tutorial_start_bt;
    public GameObject tutorial_next_bt;
    public TextMeshProUGUI text_remain_time;
    public GameObject step3_instruct_panel;
    public TextMeshProUGUI step3_instruct_panel_text;
    int step1_cnt;
    int step2_cnt;
    int step3_cnt;
    bool excute_once;
    bool skip;
    int step2_rand_idx;

    care_effect care_effect_script;
    bgm_player bgm_player_script;
    Logger logger_script;

    // Start is called before the first frame update
    void Start()
    {
        instruct_panel.SetActive(false);
        resultPrefab.SetActive(false);
        gameend_panel.SetActive(false);
        Pet = GameObject.Find("pomeLV0" + PlayerPrefs.GetInt("Level_pet").ToString());
        anim = Pet.GetComponent<Animator>();
        SpeechToText.Instance.onResultCallback = OnFinalSpeechResult;
        voiceController_script = GameObject.Find("VoiceController").GetComponent<VoiceController>();

        speechPanel.SetActive(false);

        location_3.Add(loc0);
        location_3.Add(loc1);
        location_3.Add(loc2);

        for (int k = 0; k < list_command.Count; k++)
        {
            list_command[k].gameObject.transform.GetChild(1).gameObject.SetActive(false);
            list_command[k].SetActive(false);
        }

        cnt_answer = 0;
        //anim.Play("Walk_ahead");
        questM_daily_script = GameObject.Find("Quest_Manager").GetComponent<QuestManager_daily>();
        questM_weekly_script = GameObject.Find("Quest_Manager").GetComponent<QuestManager_weekly>();
        //set_difficulty();
        time_limit = 10f;

        tutorial_next_bt.SetActive(false);
        gameend_panel.SetActive(false);

        care_effect_script = GameObject.Find("care_effect_player").GetComponent<care_effect>();
        bgm_player_script = GameObject.Find("Audio player").GetComponent<bgm_player>();
        logger_script = GameObject.Find("logger_gb").GetComponent<Logger>();

        StartCoroutine(bgm_player_script.excute_sound("10", 1f));
        StartCoroutine(bgm_player_script.excute_sound("11", 7f));
        logger_script.logger_master.insert_data("먹이주기 게임 튜토리얼 시작");

    }

    // Update is called once per frame
    void Update()
    {
        slider_time.value = time_remain / time_limit;
        if (start_flag)
        {
            if (time_remain > 0)
            {
                time_remain -= Time.deltaTime;
                text_remain_time.text = "남은 시간: " + ((int)time_remain).ToString();
            }
            else
            {
                //Debug.Log("5초가 지났네용?");
                start_flag = false;
                time_remain = 0;
            }


        }
    }

    void set_difficulty()
    {
        if (PlayerPrefs.GetInt("Level_hung") == 1) time_limit = 5f;
        if (PlayerPrefs.GetInt("Level_hung") == 2) time_limit = 4f;
        if (PlayerPrefs.GetInt("Level_hung") == 3) time_limit = 3f;

        Debug.Log("time_limt= " + time_limit.ToString());
    }

    void select_instruction()
    {
        //1부터 5까지의 숫자중 하나를 랜덤으로 뽑고, 그 숫자에 해당하는 인덱스 위치에 있는 instruct를 정답으로 정한다.
        shuffled_idx = MakeRandomNumbers(list_instruct.Count);
        answer_idx = shuffled_idx[0];
        //answer_idx = 0;

        //shuffle_idx에서 0번째로 answer_idx가 고정되어 있음으로, shuffled idx를 한번 더 섞어서 list_command에 넣어준다.
        var shuffled_loc_idx = MakeRandomNumbers(3);
        for(int j = 0; j < shuffled_loc_idx.Length; j++)
        {
            list_command[shuffled_idx[j]].transform.position = location_3[shuffled_loc_idx[j]];
            list_command[shuffled_idx[j]].SetActive(true);
            list_command[shuffled_idx[j]].gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }

        step3_instruct_panel_text.text = list_instruct[answer_idx];
        //StartCoroutine(bgm_player_script.excute_sound("27", 0));
        tutorial_msg.text = "위에 보이는 명령 문장을 읽고\n왼쪽의 명령어중 하나를 말해주세요!";
    }

    public void test_fn()
    {
        Debug.Log("check1\t" + Pet.name);
        anim.Play("Walk_ahead");
        Debug.Log("check12");
        Invoke("disapper_reaction", 3f);
        Debug.Log("check123");
        Invoke("game_start_button_click", time_limit);
        Debug.Log("check1234");
        resultPrefab.SetActive(true);
        result_text.text = "잘 하셨어요!";
        Debug.Log("check12345");
        cnt_answer++;
        Debug.Log("check123456");
        instruct_panel.SetActive(false);
        Debug.Log("check1234567");
    }

    public void start_listening()
    {
        voiceController_script.StartListening();
        Invoke("stop_listening", time_limit);
    }

    public void stop_listening()
    {
        //Debug.Log("check1\t" + Pet.name);
        //anim = Pet.GetComponent<Animator>();
        //anim.Play("Walk_ahead");
        //Debug.Log("check12");
        //Invoke("disapper_reaction", 3f);
        //Debug.Log("check123");
        //Invoke("game_start_button_click", time_limit);
        //Debug.Log("check1234");
        //result_text.text = "잘 하셨어요!";
        //Debug.Log("check12345");
        //cnt_answer++;
        //Debug.Log("check123456");
        //instruct_panel.SetActive(false);
        //Debug.Log("check1234567");

        voiceController_script.StoptListening();
    }

    void OnFinalSpeechResult(string result)
    {
        Debug.Log("FinalSpeechResult 실행");
        Debug.Log("결과: " + result + "\tanswer_idx: " + answer_idx.ToString());

        //if (answer_idx == 0)
        //{
        //    if (true || result.Contains("멍") || result.Contains("멍") || result.Contains("일") || result.Contains("루") || result.Contains("와"))
        //    {
        //        //Pet.transform.GetChild(3).transform.gameObject.SetActive(true);
        //        //Pet.transform.GetChild(4).transform.gameObject.SetActive(true);
        //        anim.Play("Walk_Idle");
        //        Invoke("disapper_reaction", 3f);
        //        Invoke("game_start_button_click", time_limit);
        //        cnt_answer++;
        //        resultPrefab.SetActive(true);
        //        result_text.text = "잘 하셨어요! 맞춘 갯수: " + cnt_answer.ToString() + "/5";
        //        instruct_panel.SetActive(false);

        //    }
        //    else
        //    {
        //        cnt_fail += 1;
        //        start_flag = false;
        //        time_remain = 0;
        //        for (int k = 0; k < list_command.Count; k++)
        //        {
        //            list_command[k].SetActive(false);
        //        }
        //        instruct_panel.SetActive(false);
        //        Invoke("game_start_button_click", time_limit);
        //    }
        //}
        //else if (answer_idx == 1)
        //{
        //    if (result.Contains("한") || result.Contains("바") || result.Contains("퀴") || result.Contains("돌") || result.Contains("아"))
        //    {
        //        //Pet.transform.GetChild(3).transform.gameObject.SetActive(true);
        //        //Pet.transform.GetChild(4).transform.gameObject.SetActive(true);
        //        anim.Play("Rotation");
        //        Invoke("disapper_reaction", 3f);
        //        Invoke("game_start_button_click", time_limit);
        //        cnt_answer++;
        //        resultPrefab.SetActive(true);
        //        result_text.text = "잘 하셨어요! 맞춘 갯수: " + cnt_answer.ToString() + "/5";
        //        instruct_panel.SetActive(false);
        //    }
        //    else
        //    {
        //        cnt_fail += 1;
        //        start_flag = false;
        //        time_remain = 0;
        //        for (int k = 0; k < list_command.Count; k++)
        //        {
        //            list_command[k].SetActive(false);
        //        }
        //        Invoke("game_start_button_click", time_limit);
        //        instruct_panel.SetActive(false);
        //    }
        //}
        //else if (answer_idx == 2)
        //{
        //    if (result.Contains("뛰") || result.Contains("띠") || result.Contains("어"))
        //    {
        //        //Pet.transform.GetChild(3).transform.gameObject.SetActive(true);
        //        //Pet.transform.GetChild(4).transform.gameObject.SetActive(true);
        //        anim.Play("Jump_NoWalk");
        //        Invoke("disapper_reaction", 3f);
        //        Invoke("game_start_button_click", time_limit);
        //        cnt_answer++;
        //        resultPrefab.SetActive(true);
        //        result_text.text = "잘 하셨어요! 맞춘 갯수: " + cnt_answer.ToString() + "/5";
        //        instruct_panel.SetActive(false);
        //    }
        //    else
        //    {
        //        cnt_fail += 1;
        //        start_flag = false;
        //        time_remain = 0;
        //        for (int k = 0; k < list_command.Count; k++)
        //        {
        //            list_command[k].SetActive(false);
        //        }
        //        instruct_panel.SetActive(false);
        //        Invoke("game_start_button_click", time_limit);
        //    }
        //}
        //else if (answer_idx == 3)
        //{
        //    if (result.Contains("누") || result.Contains("느") || result.Contains("어") || result.Contains("워"))
        //    {
        //        //Pet.transform.GetChild(3).transform.gameObject.SetActive(true);
        //        //Pet.transform.GetChild(4).transform.gameObject.SetActive(true);
        //        anim.Play("Idle_Blend_LieOnBack");
        //        Invoke("disapper_reaction", 3f);
        //        Invoke("game_start_button_click", time_limit);
        //        cnt_answer++;
        //        resultPrefab.SetActive(true);
        //        result_text.text = "잘 하셨어요! 맞춘 갯수: " + cnt_answer.ToString() + "/5";
        //        instruct_panel.SetActive(false);
        //    }
        //    else
        //    {
        //        cnt_fail += 1;
        //        start_flag = false;
        //        time_remain = 0;
        //        for (int k = 0; k < list_command.Count; k++)
        //        {
        //            list_command[k].SetActive(false);
        //        }
        //        instruct_panel.SetActive(false);
        //        Invoke("game_start_button_click", time_limit);
        //    }
        //}
        //else if (answer_idx == 4)
        //{
        //    if (result.Contains("애") || result.Contains("교") || result.Contains("부") || result.Contains("려"))
        //    {
        //        //Pet.transform.GetChild(3).transform.gameObject.SetActive(true);
        //        //Pet.transform.GetChild(4).transform.gameObject.SetActive(true);
        //        anim.Play("Dance");
        //        Invoke("disapper_reaction", 3f);
        //        Invoke("game_start_button_click", time_limit);
        //        cnt_answer++;
        //        resultPrefab.SetActive(true);
        //        result_text.text = "잘 하셨어요! 맞춘 갯수: " + cnt_answer.ToString() + "/5";
        //        instruct_panel.SetActive(false);
        //    }
        //    else
        //    {
        //        cnt_fail += 1;
        //        start_flag = false;
        //        time_remain = 0;
        //        for (int k = 0; k < list_command.Count; k++)
        //        {
        //            list_command[k].SetActive(false);
        //        }
        //        Invoke("game_start_button_click", time_limit);
        //        instruct_panel.SetActive(false);
        //    }
        //}

        if(tutorial_step == 1)
        {
            if(step1_cnt == 0)
            {
                if (result.Contains("안") || result.Contains("녕") || result.Contains("하") || result.Contains("세") || result.Contains("요"))
                {
                    logger_script.logger_master.insert_data("step1: '안녕하세요' 따라하기 성공");
                    care_effect_script.sound_correct();
                    step1_cnt++;
                    tutorial_msg.text = "잘 하셨어요! 남은 횟수: " + (5-step1_cnt).ToString();
                    result_text.text = result;
                    Invoke("change_instruct", 3f);
                    Invoke("time_reset", 3f);
                    Invoke("start_listening", 3f);
                    Invoke("disable_res_text", 3f);
                }
                else
                {
                    //다시 실행;
                    logger_script.logger_master.insert_data("step1: '안녕하세요' 따라하기 실패");
                    care_effect_script.sound_false2();
                    tutorial_msg.text = "다시 한번 따라서 말 해볼까요?";
                    Invoke("time_reset", 3f);
                    Invoke("start_listening", 3f);
                }
            }
            else if(step1_cnt == 1)
            {
                if (result.Contains("반") || result.Contains("갑") || result.Contains("습") || result.Contains("니") || result.Contains("다"))
                {
                    logger_script.logger_master.insert_data("step1: '반갑습니다' 따라하기 성공");
                    care_effect_script.sound_correct();
                    step1_cnt++;
                    tutorial_msg.text = "잘 하셨어요! 남은 횟수: " + (5 - step1_cnt).ToString();
                    result_text.text = result;
                    Invoke("change_instruct", 3f);
                    Invoke("time_reset", 3f);
                    Invoke("start_listening", 3f);
                    Invoke("disable_res_text", 3f);
                }
                else
                {
                    logger_script.logger_master.insert_data("step1: '반갑습니다' 따라하기 실패");
                    care_effect_script.sound_false2();
                    tutorial_msg.text = "다시 한번 따라서 말 해볼까요?";
                    Invoke("time_reset", 3f);
                    Invoke("start_listening", 3f);
                }
            }
            else if (step1_cnt == 2)
            {
                if (result.Contains("제") || result.Contains("이") || result.Contains("름") || result.Contains("은") || result.Contains("홍") ||
                    result.Contains("길") || result.Contains("동") || result.Contains("입") || result.Contains("니") || result.Contains("다"))
                {
                    logger_script.logger_master.insert_data("step1: '제 이름은 홍길동 입니다' 따라하기 성공");
                    care_effect_script.sound_correct();
                    step1_cnt++;
                    tutorial_msg.text = "잘 하셨어요! 남은 횟수: " + (5 - step1_cnt).ToString();
                    result_text.text = result;
                    Invoke("change_instruct", 3f);
                    Invoke("time_reset", 3f);
                    Invoke("start_listening", 3f);
                    Invoke("disable_res_text", 3f);
                    
                }
                else
                {
                    logger_script.logger_master.insert_data("step1: '제 이름은 홍길동 입니다' 따라하기 실패");
                    care_effect_script.sound_false2();
                    tutorial_msg.text = "다시 한번 따라서 말 해볼까요?";
                    Invoke("time_reset", 3f);
                    Invoke("start_listening", 3f);
                }
            }
            else if (step1_cnt == 3)
            {
                if (result.Contains("수") || result.Contains("고") || result.Contains("하") || result.Contains("세") || result.Contains("요"))
                {
                    logger_script.logger_master.insert_data("step1: '수고하세요' 따라하기 성공");
                    care_effect_script.sound_correct();
                    step1_cnt++;
                    tutorial_msg.text = "잘 하셨어요! 남은 횟수: " + (5 - step1_cnt).ToString();
                    result_text.text = result;
                    Invoke("change_instruct", 3f);
                    Invoke("time_reset", 3f);
                    Invoke("start_listening", 3f);
                    Invoke("disable_res_text", 3f);
                }
                else
                {
                    logger_script.logger_master.insert_data("step1: '수고하세요' 따라하기 실패");
                    care_effect_script.sound_false2();
                    tutorial_msg.text = "다시 한번 따라서 말 해볼까요?";
                    Invoke("time_reset", 3f);
                    Invoke("start_listening", 3f);
                }
            }
            else if (step1_cnt == 4)
            {
                if (result.Contains("여") || result.Contains("기") || result.Contains("로") || result.Contains("와") ||
                    result.Contains("주") || result.Contains("세") || result.Contains("요"))
                {
                    logger_script.logger_master.insert_data("step1: '여기로 와주세요' 따라하기 성공");
                    care_effect_script.sound_correct();
                    step1_cnt++;
                    StartCoroutine(bgm_player_script.excute_sound("18", 0));
                    tutorial_msg.text = "잘 하셨어요! 이제는 강아지에게 명령하여 개인기를 시켜볼까요?";
                    result_text.text = result;
                    Invoke("disable_res_text", 3f);

                    //instruct_panel.SetActive(true);
                    //resultPrefab.SetActive(true);
                    tutorial_next_bt.SetActive(true);
                }
                else
                {
                    logger_script.logger_master.insert_data("step1: '여기로 와주세요' 따라하기 실패");
                    care_effect_script.sound_false2();
                    tutorial_msg.text = "다시 한번 따라서 말 해볼까요?";
                    Invoke("time_reset", 3f);
                    Invoke("start_listening", 3f);
                }
            }
        }

        if (tutorial_step == 2)
        {
            if (step2_cnt == 0 || step2_cnt == 5)
            {
                if (result.Contains("멍") || result.Contains("멍") || result.Contains("아") || result.Contains("일") || result.Contains("루") || result.Contains("와"))
                {
                    logger_script.logger_master.insert_data("step2: '멍멍아 일루와' 따라하기 성공");
                    anim.Play("Walk_Idle");
                    Invoke("disapper_reaction", 3f);

                    care_effect_script.sound_correct();
                    step2_cnt++;
                    tutorial_msg.text = "잘 하셨어요! 남은 횟수: " + (10 - step2_cnt).ToString();
                    result_text.text = result;

                    Invoke("change_instruct", 3f);
                    Invoke("time_reset", 3f);
                    Invoke("start_listening", 3f);
                    Invoke("disable_res_text", 3f);
                }
                else
                {
                    logger_script.logger_master.insert_data("step2: '멍멍아 일루와' 따라하기 실패");
                    care_effect_script.sound_false2();
                    tutorial_msg.text = "다시 한번 따라서 말 해볼까요?";
                    Invoke("time_reset", 3f);
                    Invoke("start_listening", 3f);
                }
            }
            else if (step2_cnt == 1 || step2_cnt == 6)
            {
                if (result.Contains("한") || result.Contains("바") || result.Contains("퀴") || result.Contains("돌") || result.Contains("아"))
                {
                    logger_script.logger_master.insert_data("step2: '한 바퀴 돌아' 따라하기 성공");
                    anim.Play("Rotation");
                    Invoke("disapper_reaction", 3f);

                    care_effect_script.sound_correct();
                    step2_cnt++;
                    tutorial_msg.text = "잘 하셨어요! 남은 횟수: " + (10 - step2_cnt).ToString();
                    result_text.text = result;

                    Invoke("change_instruct", 3f);
                    Invoke("time_reset", 3f);
                    Invoke("start_listening", 3f);
                    Invoke("disable_res_text", 3f);
                }
                else
                {
                    logger_script.logger_master.insert_data("step2: '한 바퀴 돌아' 따라하기 실패");
                    care_effect_script.sound_false2();
                    tutorial_msg.text = "다시 한번 따라서 말 해볼까요?";
                    Invoke("time_reset", 3f);
                    Invoke("start_listening", 3f);
                }
            }
            else if (step2_cnt == 2 || step2_cnt == 7)
            {
                if (result.Contains("뛰") || result.Contains("띠") || result.Contains("어"))
                {
                    logger_script.logger_master.insert_data("step2: '뛰어' 따라하기 성공");
                    anim.Play("Jump_NoWalk");
                    Invoke("disapper_reaction", 3f);

                    care_effect_script.sound_correct();
                    step2_cnt++;
                    tutorial_msg.text = "잘 하셨어요! 남은 횟수: " + (10 - step2_cnt).ToString();
                    result_text.text = result;

                    Invoke("change_instruct", 3f);
                    Invoke("time_reset", 3f);
                    Invoke("start_listening", 3f);
                    Invoke("disable_res_text", 3f);
                }
                else
                {
                    logger_script.logger_master.insert_data("step2: '뛰어' 따라하기 실패");
                    care_effect_script.sound_false2();
                    tutorial_msg.text = "다시 한번 따라서 말 해볼까요?";
                    Invoke("time_reset", 3f);
                    Invoke("start_listening", 3f);
                }
            }
            else if (step2_cnt == 3 || step2_cnt == 8)
            {
                if (result.Contains("누") || result.Contains("어") || result.Contains("워"))
                {
                    logger_script.logger_master.insert_data("step2: '누워' 따라하기 성공");
                    anim.Play("Idle_Blend_LieOnBack");
                    Invoke("disapper_reaction", 3f);

                    care_effect_script.sound_correct();
                    step2_cnt++;
                    tutorial_msg.text = "잘 하셨어요! 남은 횟수: " + (10 - step2_cnt).ToString();
                    result_text.text = result;

                    Invoke("change_instruct", 3f);
                    Invoke("time_reset", 3f);
                    Invoke("start_listening", 3f);
                    Invoke("disable_res_text", 3f);
                }
                else
                {
                    logger_script.logger_master.insert_data("step2: '누워' 따라하기 실패");
                    care_effect_script.sound_false2();
                    tutorial_msg.text = "다시 한번 따라서 말 해볼까요?";
                    Invoke("time_reset", 3f);
                    Invoke("start_listening", 3f);
                }
            }
            else if (step2_cnt == 4 || step2_cnt == 9)
            {
                if (result.Contains("애") || result.Contains("교") || result.Contains("부") || result.Contains("려"))
                {
                    logger_script.logger_master.insert_data("step2: '애교부려' 따라하기 성공");
                    anim.Play("Dance");
                    Invoke("disapper_reaction", 3f);

                    care_effect_script.sound_correct();
                    step2_cnt++;
                    result_text.text = result;

                    if (step2_cnt == 10)
                    {
                        StartCoroutine(bgm_player_script.excute_sound("26", 0));
                        tutorial_msg.text = "잘 하셨어요! 다시 한 번 강아지에게 명령을 내려볼까요?";
                        Invoke("disable_res_text", 3f);
                        tutorial_next_bt.SetActive(true);
                        instruct_panel.SetActive(false);
                        resultPrefab.SetActive(false);
                    }
                    else
                    {
                        tutorial_msg.text = "잘 하셨어요! 남은 횟수: " + (10 - step2_cnt).ToString();
                        Invoke("change_instruct", 3f);
                        Invoke("time_reset", 3f);
                        Invoke("start_listening", 3f);
                        Invoke("disable_res_text", 3f);
                    }
                }
                else
                {
                    logger_script.logger_master.insert_data("step2: '애교부려' 따라하기 실패");
                    care_effect_script.sound_false2();
                    tutorial_msg.text = "다시 한번 따라서 말 해볼까요?";
                    Invoke("time_reset", 3f);
                    Invoke("start_listening", 3f);
                }
            }
        }

        if (tutorial_step == 3)
        {
            if (answer_idx == 0)
            {
                if (result.Contains("멍") || result.Contains("멍") || result.Contains("일") || result.Contains("루") || result.Contains("와"))
                {
                    care_effect_script.sound_correct();
                    step3_cnt++;
                    for (int k = 0; k < list_command.Count; k++)
                    {
                        list_command[k].SetActive(false);
                    }
                    if (step3_cnt == 10)
                    {
                        logger_script.logger_master.insert_data("먹이주기 튜토리얼 종료!");
                        StartCoroutine(bgm_player_script.excute_sound("29", 0));
                        gameend_panel.SetActive(true);
                        gamestart_Button.SetActive(false);
                        Invoke("goToMainGame", 7f);
                        anim.Play("Walk_Idle");
                        Invoke("disapper_reaction", 3f);
                        tutorial_msg.text = "";
                        step3_instruct_panel_text.text = "";
                    }
                    else
                    {
                        anim.Play("Walk_Idle");
                        Invoke("disapper_reaction", 3f);
                        tutorial_msg.text = "잘 하셨어요! 남은 횟수: " + (10 - step3_cnt).ToString();
                        logger_script.logger_master.insert_data("step3: '멍멍아 일루와' 성공");
                        Invoke("time_reset", 3f);
                        Invoke("start_listening", 3f);
                        Invoke("select_instruction", 3f);
                    }
                }
                else
                {
                    logger_script.logger_master.insert_data("step3: '멍멍아 일루와' 실패");
                    care_effect_script.sound_false2();
                    StartCoroutine(bgm_player_script.excute_sound("28", 0));
                    tutorial_msg.text = "다시 한번 지시문을 읽고, 왼쪽의 보기들 중 골라서 말 해볼까요?";
                    list_command[answer_idx].gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    Invoke("time_reset", 3f);
                    Invoke("start_listening", 3f);
                }
            }
            else if (answer_idx == 1)
            {
                if (result.Contains("한") || result.Contains("바") || result.Contains("퀴") || result.Contains("돌") || result.Contains("아"))
                {
                    care_effect_script.sound_correct();
                    step3_cnt++;
                    for (int k = 0; k < list_command.Count; k++)
                    {
                        list_command[k].SetActive(false);
                    }

                    if (step3_cnt == 10)
                    {
                        logger_script.logger_master.insert_data("먹이주기 튜토리얼 종료!");
                        StartCoroutine(bgm_player_script.excute_sound("29", 0));
                        gameend_panel.SetActive(true);
                        gamestart_Button.SetActive(false);
                        Invoke("goToMainGame", 7f);
                        anim.Play("Rotation");
                        Invoke("disapper_reaction", 3f);
                        tutorial_msg.text = "";
                        step3_instruct_panel_text.text = "";
                    }
                    else
                    {
                        anim.Play("Rotation");
                        Invoke("disapper_reaction", 3f);
                        tutorial_msg.text = "잘 하셨어요! 남은 횟수: " + (10 - step3_cnt).ToString();
                        logger_script.logger_master.insert_data("step3: '한바퀴 돌아' 성공");
                        Invoke("time_reset", 3f);
                        Invoke("start_listening", 3f);
                        Invoke("select_instruction", 3f);
                    }
                }
                else
                {
                    care_effect_script.sound_false2();
                    StartCoroutine(bgm_player_script.excute_sound("28", 0));
                    logger_script.logger_master.insert_data("step3: '한바퀴 돌아' 실패");
                    list_command[answer_idx].gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    tutorial_msg.text = "다시 한번 지시문을 읽고, 왼쪽의 보기들 중 골라서 말 해볼까요?";
                    Invoke("time_reset", 3f);
                    Invoke("start_listening", 3f);
                }
            }
            else if (answer_idx == 2)
            {
                if (result.Contains("뛰") || result.Contains("띠") || result.Contains("어"))
                {
                    care_effect_script.sound_correct();
                    step3_cnt++;
                    for (int k = 0; k < list_command.Count; k++)
                    {
                        list_command[k].SetActive(false);
                    }

                    if (step3_cnt == 10)
                    {
                        logger_script.logger_master.insert_data("먹이주기 튜토리얼 종료!");
                        StartCoroutine(bgm_player_script.excute_sound("29", 0));
                        gameend_panel.SetActive(true);
                        gamestart_Button.SetActive(false);
                        Invoke("goToMainGame", 7f);
                        anim.Play("Jump_NoWalk");
                        Invoke("disapper_reaction", 3f);
                        tutorial_msg.text = "";
                        step3_instruct_panel_text.text = "";
                    }
                    else
                    {
                        anim.Play("Jump_NoWalk");
                        Invoke("disapper_reaction", 3f);
                        tutorial_msg.text = "잘 하셨어요! 남은 횟수: " + (10 - step3_cnt).ToString();
                        logger_script.logger_master.insert_data("step3: '뛰어' 성공");
                        Invoke("time_reset", 3f);
                        Invoke("start_listening", 3f);
                        Invoke("select_instruction", 3f);
                    }
                }
                else
                {
                    care_effect_script.sound_false2();
                    StartCoroutine(bgm_player_script.excute_sound("28", 0));
                    logger_script.logger_master.insert_data("step3: '뛰어' 실패");
                    list_command[answer_idx].gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    tutorial_msg.text = "다시 한번 지시문을 읽고, 왼쪽의 보기들 중 골라서 말 해볼까요?";
                    Invoke("time_reset", 3f);
                    Invoke("start_listening", 3f);
                }
            }
            else if (answer_idx == 3)
            {
                if (result.Contains("누") || result.Contains("너") || result.Contains("어") || result.Contains("워"))
                {
                    care_effect_script.sound_correct();
                    step3_cnt++;
                    for (int k = 0; k < list_command.Count; k++)
                    {
                        list_command[k].SetActive(false);
                    }

                    if (step3_cnt == 10)
                    {
                        logger_script.logger_master.insert_data("먹이주기 튜토리얼 종료!");
                        StartCoroutine(bgm_player_script.excute_sound("29", 0));
                        gameend_panel.SetActive(true);
                        gamestart_Button.SetActive(false);
                        Invoke("goToMainGame", 7f);
                        anim.Play("Idle_Blend_LieOnBack");
                        Invoke("disapper_reaction", 3f);
                        tutorial_msg.text = "";
                        step3_instruct_panel_text.text = "";
                    }
                    else
                    {
                        anim.Play("Idle_Blend_LieOnBack");
                        Invoke("disapper_reaction", 3f);
                        tutorial_msg.text = "잘 하셨어요! 남은 횟수: " + (10 - step3_cnt).ToString();
                        logger_script.logger_master.insert_data("step3: '누워' 성공");
                        Invoke("time_reset", 3f);
                        Invoke("start_listening", 3f);
                        Invoke("select_instruction", 3f);
                    }
                }
                else
                {
                    care_effect_script.sound_false2();
                    StartCoroutine(bgm_player_script.excute_sound("28", 0));
                    list_command[answer_idx].gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    tutorial_msg.text = "다시 한번 지시문을 읽고, 왼쪽의 보기들 중 골라서 말 해볼까요?";
                    logger_script.logger_master.insert_data("step3: '누워' 실패");
                    Invoke("time_reset", 3f);
                    Invoke("start_listening", 3f);
                }
            }
            else if (answer_idx == 4)
            {
                care_effect_script.sound_correct();
                step3_cnt++;
                if (result.Contains("애") || result.Contains("교") || result.Contains("부") || result.Contains("려"))
                {
                    for (int k = 0; k < list_command.Count; k++)
                    {
                        list_command[k].SetActive(false);
                    }

                    if (step3_cnt == 10)
                    {
                        logger_script.logger_master.insert_data("먹이주기 튜토리얼 종료!");
                        StartCoroutine(bgm_player_script.excute_sound("29", 0));
                        gameend_panel.SetActive(true);
                        gamestart_Button.SetActive(false);
                        Invoke("goToMainGame", 7f);
                        anim.Play("Dance");
                        Invoke("disapper_reaction", 3f);
                        tutorial_msg.text = "";
                        step3_instruct_panel_text.text = "";
                    }
                    else
                    {
                        anim.Play("Dance");
                        Invoke("disapper_reaction", 3f);
                        tutorial_msg.text = "잘 하셨어요! 남은 횟수: " + (10 - step3_cnt).ToString();
                        logger_script.logger_master.insert_data("step3: '애교부려' 성공");
                        Invoke("time_reset", 3f);
                        Invoke("start_listening", 3f);
                        Invoke("select_instruction", 3f);
                    }
                }
                else
                {
                    care_effect_script.sound_false2();
                    StartCoroutine(bgm_player_script.excute_sound("28", 0));
                    list_command[answer_idx].gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    logger_script.logger_master.insert_data("step3: '애교부려' 실패");
                    tutorial_msg.text = "다시 한번 지시문을 읽고, 왼쪽의 보기들 중 골라서 말 해볼까요?";
                    Invoke("time_reset", 3f);
                    Invoke("start_listening", 3f);
                }
            }

        }

        //if(cnt_answer==2)
        //{
        //    Invoke("appear_reaction", 3.5f);
        //}
    }

    public void tutorial_next_bt_clicked()
    {
        tutorial_step++;

        if(tutorial_step == 2)
        {
            tutorial_msg.text = "화면에 보이는 문구를 읽어서 강아지를 움직여볼까요?";
            instruct_text.text = "";
            Invoke("change_instruct", 3f);
            Invoke("time_reset", 3f);
            Invoke("start_listening", 3f);
        }
        else if(tutorial_step == 3)
        {
            StartCoroutine(bgm_player_script.excute_sound("27", 0));
            tutorial_msg.text = "위에 보이는 명령 문장을 읽고\n왼쪽의 명령어중 하나를 말해주세요!";
            step3_instruct_panel.SetActive(true);
            step3_instruct_panel_text.text = "멍멍아 일루와!";
            //time_limit = 3f;
            Invoke("select_instruction", 3f);
            Invoke("time_reset", 3f);
            Invoke("start_listening", 3f);

        }

        if (tutorial_next_bt.activeSelf) tutorial_next_bt.SetActive(false);
    }

    void change_instruct()
    {
        if(tutorial_step == 1)
        {
            StartCoroutine(bgm_player_script.excute_sound("12", 0));
            tutorial_msg.text = "위에 보이는 문장을 따라 읽어볼까요?";
            if (step1_cnt == 1)
            {
                instruct_text.text = "반갑습니다";
            }
            else if (step1_cnt == 2)
            {
                instruct_text.text = "제 이름은 홍길동 입니다";
            }
            else if (step1_cnt == 3)
            {
                instruct_text.text = "수고하세요";
            }
            else if (step1_cnt == 4)
            {
                instruct_text.text = "여기로 와주세요";
            }
        }
        else if (tutorial_step == 2)
        {
            StartCoroutine(bgm_player_script.excute_sound("20", 0));
            tutorial_msg.text = "화면에 보이는 문구를 읽어서 강아지를 움직여볼까요?";
            if (step2_cnt == 0 || step2_cnt == 5)
            {
                instruct_text.text = "멍멍아 일루와!";
            }
            else if (step2_cnt == 1 || step2_cnt == 6)
            {
                instruct_text.text = "한 바퀴 돌아!";
            }
            else if (step2_cnt == 2 || step2_cnt == 7)
            {
                instruct_text.text = "뛰어!";
            }
            else if (step2_cnt == 3 || step2_cnt == 8)
            {
                instruct_text.text = "누워!";
            }
            else if (step2_cnt == 4 || step2_cnt == 9)
            {
                instruct_text.text = "애교부려!";
            }
        }


    }

    void time_reset()
    {
        time_remain = time_limit;
        if (start_flag == false) start_flag = true;
    }

    void disable_res_text()
    {
        result_text.text = "";
    }
    void appear_reaction()
    {
        Pet.transform.GetChild(3).transform.gameObject.SetActive(true);
        Pet.transform.GetChild(4).transform.gameObject.SetActive(true);
        anim.Play("Eat");
    }

    void disapper_reaction()
    {
        Pet.transform.GetChild(3).transform.gameObject.SetActive(false);
        Pet.transform.GetChild(4).transform.gameObject.SetActive(false);
        result_text.text = "";
    }


    public static int[] MakeRandomNumbers(int maxValue, int randomSeed = 0)
    {
        return MakeRandomNumbers(0, maxValue, randomSeed);
    }
    public static int[] MakeRandomNumbers(int minValue, int maxValue, int randomSeed = 0)
    {
        if (randomSeed == 0)
            randomSeed = (int)System.DateTime.Now.Ticks;

        List<int> values = new List<int>();
        for (int v = minValue; v < maxValue; v++)
        {
            values.Add(v);
        }

        int[] result = new int[maxValue - minValue];
        System.Random random = new System.Random(Seed: randomSeed);
        int i = 0;
        while (values.Count > 0)
        {
            int randomValue = values[random.Next(0, values.Count)];
            result[i++] = randomValue;

            if (!values.Remove(randomValue))
            {
                // Exception
                break;
            }
        }

        return result;
    }

    public void load_AR_scene()
    {
        SceneManager.LoadScene("10_AR_interaction_MZ");
    }

    void goToMainGame()
    {
        SceneManager.LoadScene("20_Virtual__feeding");
    }

    public void game_start_button_click()
    {
        //if (cnt_fail == 5)
        //{
        //    //실패 문구 보여주기
        //    gameend_panel.SetActive(true);
        //    TextMeshProUGUI text_fail = GameObject.Find("Text_fail").GetComponent<TextMeshProUGUI>();
        //    text_fail.text = "다음 기회에 다시 도전해봐요!";
        //    Invoke("load_AR_scene", 4f);
        //    return;
        //}

        //if (cnt_answer == 5)
        //{
        //    gameend_panel.SetActive(true);
        //    PlayerPrefs.SetFloat("exp", PlayerPrefs.GetFloat("exp") + 0.03f);
        //    PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") + 2);
        //    text_last.gameObject.SetActive(true);
        //    text_last.text = "잘 하셨어요!\n 보상은 다음과 같습니다";
        //    anim.Play("Eat_loop");
        //    Pet.transform.GetChild(4).gameObject.SetActive(true);
        //    questM_daily_script.feed_plus();
        //    questM_weekly_script.caregame_plus("feed");
        //    Invoke("load_AR_scene", 4f);
        //    return;
        //}

        tutorial_step++;
        StartCoroutine(bgm_player_script.excute_sound("12", 0));
        tutorial_msg.text = "위에 보이는 문장을 따라서 읽어볼까요?";
        //select_instruction();
        instruct_panel.SetActive(true);
        resultPrefab.SetActive(true);
        //instruct_text.text = list_instruct[answer_idx];
        instruct_text.text = "안녕하세요";
        time_remain = time_limit;
        if (start_flag == false) start_flag = true;
        //if (gamestart_Button.activeSelf == true) gamestart_Button.SetActive(false);
        if (tutorial_start_bt.activeSelf == true) tutorial_start_bt.SetActive(false);
        //speechPanel.SetActive(true);

        start_listening();

    }

    //public void set_text_speechBubble(string message)
    //{
    //    speech_bubble.SetActive(true);
    //    if (speech_bubble.gameObject.activeSelf == true)
    //    {
    //        //Vector3 screenPos = Camera.main.WorldToScreenPoint(spawnedObject.transform.position + 0.5f * Vector3.up
    //        //													+ 0.3f * Vector3.right);
    //        //speech_bubble.transform.position = gameObject.transform.position + 0.4f * Vector3.up
    //        //                                  + 0.1f * Vector3.right + 0.1f * Vector3.back;
    //        speech_bubble.transform.position = gameObject.transform.position + 0.7f * Vector3.up
    //                              + 0.3f * Vector3.left + 0.1f * Vector3.back;

    //    }

    //    TMP_Text txt_bubble = speech_bubble.transform.GetChild(0).transform.GetComponent<TMP_Text>();
    //    //Debug.Log(speechbubble.transform.GetChild(0).transform.name);
    //    txt_bubble.text = message;
    //    Invoke("init_destroy_speechBubble", 3f);

    //}
}
