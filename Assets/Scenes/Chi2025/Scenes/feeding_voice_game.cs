using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextSpeech;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class feeding_voice_game : MonoBehaviour
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


    Vector3 loc0 = new Vector3(496, 632, 0);
    Vector3 loc1 = new Vector3(496, 443, 0);
    Vector3 loc2 = new Vector3(496, 254, 0);
    List<Vector3> location_3 = new List<Vector3>();

    //public Animator anim_Lv1;
    Animator anim;

    public QuestManager_daily questM_daily_script;
    public QuestManager_weekly questM_weekly_script;

    float time_limit = 1f;
    int cnt_answer;


    care_effect care_effect_script;
    bgm_player bgm_player_script;
    Logger logger_script;


    // Start is called before the first frame update
    void Start()
    {
        text_last.gameObject.SetActive(false);
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
            list_command[k].SetActive(false);
        }

        cnt_answer = 0;
        //anim.Play("Walk_ahead");
        questM_daily_script = GameObject.Find("Quest_Manager").GetComponent<QuestManager_daily>();
        questM_weekly_script = GameObject.Find("Quest_Manager").GetComponent<QuestManager_weekly>();

        care_effect_script = GameObject.Find("care_effect_player").GetComponent<care_effect>();
        bgm_player_script = GameObject.Find("Audio player").GetComponent<bgm_player>();
        logger_script = GameObject.Find("logger_gb").GetComponent<Logger>();

        StartCoroutine(bgm_player_script.excute_sound("30", 1f));
    }

    // Update is called once per frame
    void Update()
    {
        slider_time.value = time_remain / time_limit;
        if (start_flag)
        {
            if (time_remain > 0)
                time_remain -= Time.deltaTime;
            else
            {
                //Debug.Log("5초가 지났네용?");
                start_flag = false;
                //stop_listening();
                time_remain = 0;
                for(int k=0; k<list_command.Count; k++)
                {
                    list_command[k].SetActive(false);
                }
            }


        }
    }

    void set_difficulty()
    {
        if (PlayerPrefs.GetInt("Level_hung") == 1) time_limit = 10f;
        if (PlayerPrefs.GetInt("Level_hung") == 2) time_limit = 8f;
        if (PlayerPrefs.GetInt("Level_hung") == 3) time_limit = 6f;

        Debug.Log("time_limt= " + time_limit.ToString());
    }

    void select_instruction()
    {
        shuffled_idx = MakeRandomNumbers(list_instruct.Count);
        answer_idx = shuffled_idx[0];
        //answer_idx = 0;

        var shuffled_loc_idx = MakeRandomNumbers(3);
        for(int j = 0; j < shuffled_loc_idx.Length; j++)
        {
            list_command[shuffled_idx[j]].transform.position = location_3[shuffled_loc_idx[j]];
            list_command[shuffled_idx[j]].SetActive(true);
        }
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

        if (answer_idx == 0)
        {
            if (result.Contains("멍") || result.Contains("멍") || result.Contains("일") || result.Contains("루") || result.Contains("와"))
            {
                //Pet.transform.GetChild(3).transform.gameObject.SetActive(true);
                //Pet.transform.GetChild(4).transform.gameObject.SetActive(true);
                anim.Play("Walk_Idle");
                Invoke("disapper_reaction", 3f);
                Invoke("game_start_button_click", time_limit);
                cnt_answer++;
                resultPrefab.SetActive(true);
                result_text.text = "잘 하셨어요! 맞춘 갯수: " + cnt_answer.ToString() + "/5";
                instruct_panel.SetActive(false);
                care_effect_script.sound_correct();
                logger_script.logger_master.insert_data("'멍멍앙 일루와' 명령 성공");
            }
            else
            {
                care_effect_script.sound_false2();
                cnt_fail += 1;
                start_flag = false;
                time_remain = 0;
                for (int k = 0; k < list_command.Count; k++)
                {
                    list_command[k].SetActive(false);
                }
                instruct_panel.SetActive(false);
                Invoke("game_start_button_click", time_limit);
                logger_script.logger_master.insert_data("'멍멍앙 일루와' 명령 실패");
            }
        }
        else if (answer_idx == 1)
        {
            if (result.Contains("한") || result.Contains("바") || result.Contains("퀴") || result.Contains("돌") || result.Contains("아"))
            {
                //Pet.transform.GetChild(3).transform.gameObject.SetActive(true);
                //Pet.transform.GetChild(4).transform.gameObject.SetActive(true);
                anim.Play("Rotation");
                Invoke("disapper_reaction", 3f);
                Invoke("game_start_button_click", time_limit);
                cnt_answer++;
                resultPrefab.SetActive(true);
                result_text.text = "잘 하셨어요! 맞춘 갯수: " + cnt_answer.ToString() + "/5";
                instruct_panel.SetActive(false);
                care_effect_script.sound_correct();
                logger_script.logger_master.insert_data("'한 바퀴 돌아' 명령 성공");
            }
            else
            {
                care_effect_script.sound_false2();
                cnt_fail += 1;
                start_flag = false;
                time_remain = 0;
                for (int k = 0; k < list_command.Count; k++)
                {
                    list_command[k].SetActive(false);
                }
                Invoke("game_start_button_click", time_limit);
                instruct_panel.SetActive(false);
                logger_script.logger_master.insert_data("'한 바퀴 돌아 명령' 실패");
            }
        }
        else if (answer_idx == 2)
        {
            if (result.Contains("뛰") || result.Contains("띠") || result.Contains("어"))
            {
                //Pet.transform.GetChild(3).transform.gameObject.SetActive(true);
                //Pet.transform.GetChild(4).transform.gameObject.SetActive(true);
                anim.Play("Jump_NoWalk");
                Invoke("disapper_reaction", 3f);
                Invoke("game_start_button_click", time_limit);
                cnt_answer++;
                resultPrefab.SetActive(true);
                result_text.text = "잘 하셨어요! 맞춘 갯수: " + cnt_answer.ToString() + "/5";
                instruct_panel.SetActive(false);
                care_effect_script.sound_correct();
                logger_script.logger_master.insert_data("'뛰어' 명령 성공");
            }
            else
            {
                care_effect_script.sound_false2();
                cnt_fail += 1;
                start_flag = false;
                time_remain = 0;
                for (int k = 0; k < list_command.Count; k++)
                {
                    list_command[k].SetActive(false);
                }
                instruct_panel.SetActive(false);
                Invoke("game_start_button_click", time_limit);
                logger_script.logger_master.insert_data("'뛰어' 명령 실패");
            }
        }
        else if (answer_idx == 3)
        {
            if (result.Contains("누") || result.Contains("느") || result.Contains("어") || result.Contains("워"))
            {
                //Pet.transform.GetChild(3).transform.gameObject.SetActive(true);
                //Pet.transform.GetChild(4).transform.gameObject.SetActive(true);
                anim.Play("Idle_Blend_LieOnBack");
                Invoke("disapper_reaction", 3f);
                Invoke("game_start_button_click", time_limit);
                cnt_answer++;
                resultPrefab.SetActive(true);
                result_text.text = "잘 하셨어요! 맞춘 갯수: " + cnt_answer.ToString() + "/5";
                instruct_panel.SetActive(false);
                care_effect_script.sound_correct();
                logger_script.logger_master.insert_data("'누워' 명령 성공");
            }
            else
            {
                care_effect_script.sound_false2();
                cnt_fail += 1;
                start_flag = false;
                time_remain = 0;
                for (int k = 0; k < list_command.Count; k++)
                {
                    list_command[k].SetActive(false);
                }
                instruct_panel.SetActive(false);
                Invoke("game_start_button_click", time_limit);
                logger_script.logger_master.insert_data("'누워' 명령 실패");
            }
        }
        else if (answer_idx == 4)
        {
            if (result.Contains("애") || result.Contains("교") || result.Contains("부") || result.Contains("려"))
            {
                //Pet.transform.GetChild(3).transform.gameObject.SetActive(true);
                //Pet.transform.GetChild(4).transform.gameObject.SetActive(true);
                anim.Play("Dance");
                Invoke("disapper_reaction", 3f);
                Invoke("game_start_button_click", time_limit);
                cnt_answer++;
                resultPrefab.SetActive(true);
                result_text.text = "잘 하셨어요! 맞춘 갯수: " + cnt_answer.ToString() + "/5";
                instruct_panel.SetActive(false);
                care_effect_script.sound_correct();
                logger_script.logger_master.insert_data("'애교부려' 명령 성공");
            }
            else
            {
                care_effect_script.sound_false2();
                cnt_fail += 1;
                start_flag = false;
                time_remain = 0;
                for (int k = 0; k < list_command.Count; k++)
                {
                    list_command[k].SetActive(false);
                }
                Invoke("game_start_button_click", time_limit);
                instruct_panel.SetActive(false);
                logger_script.logger_master.insert_data("'애교부려' 명령 실패");
            }
        }

        //if(cnt_answer==2)
        //{
        //    Invoke("appear_reaction", 3.5f);
        //}
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

    public void game_start_button_click()
    {
        if (cnt_fail == 5)
        {
            //실패 문구 보여주기
            logger_script.logger_master.insert_data("먹이주기 게임 실패. 게임 종료");
            //gameend_panel.SetActive(true);
            TextMeshProUGUI text_fail = GameObject.Find("Text_fail").GetComponent<TextMeshProUGUI>();
            text_fail.text = "다음 기회에 다시 도전해봐요!";
            Invoke("load_AR_scene", 4f);
            return;
        }

        if (cnt_answer == 5)
        {
            logger_script.logger_master.insert_data("먹이주기 게임 성공. 게임 종료");
            speechPanel.SetActive(false);
            Invoke("gameend_true", 4f);
            PlayerPrefs.SetFloat("exp", PlayerPrefs.GetFloat("exp") + 0.02f);
            PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") + 2);
            PlayerPrefs.SetFloat("energy", PlayerPrefs.GetFloat("energy") + 0.01f);
            text_last.gameObject.SetActive(true);
            text_last.text = "잘 하셨어요!\n 보상은 다음과 같습니다";
            anim.Play("Eat_loop");
            care_effect_script.sound_dog_eating0();
            Pet.transform.GetChild(4).gameObject.SetActive(true);
            questM_daily_script.feed_plus();
            questM_weekly_script.caregame_plus("feed");
            Invoke("load_AR_scene", 8f);
            return;
        }
        set_difficulty();
        select_instruction();
        instruct_panel.SetActive(true);
        instruct_text.text = list_instruct[answer_idx];
        time_remain = time_limit;
        if (start_flag == false) start_flag = true;
        if (gamestart_Button.activeSelf == true) gamestart_Button.SetActive(false);
        speechPanel.SetActive(true);

        start_listening();
        logger_script.logger_master.insert_data("먹이주기 게임 본게임 시작");

    }

    void gameend_true()
    {
        care_effect_script.sound_reward_popup();
        gameend_panel.SetActive(true);
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
