using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;

public class intimate_game_tutorial : MonoBehaviour
{
    face_emo_edit_intimate face_emo_edit_inti_script;

    GameObject Pet;
    Animator anim;
    SkinnedMeshRenderer face_renderer;

    private float time_remain;
    private bool start_flag;

    public GameObject gamestart_Button;
    public Slider slider_time;
    public TextMeshProUGUI text_emo;
    public TextMeshProUGUI text_last;
    public TextMeshProUGUI text_instruct;

    public GameObject panel_instruct;
    public GameObject panel_fail;
    public GameObject gameDonePrefab;
    public GameObject face_panel;

    int cnt_succes;
    int cnt_fail;
    int emo_idx;

    float time_limit;
    bool correct;
    List<string> emo_list = new List<string>();
    public List<Texture2D> emo_picture_list = new List<Texture2D>();
    public RawImage emo_picture_panel;

    int tutorial_step;
    public Text tutorial_msg;
    public GameObject tutorial_start_bt;
    public GameObject tutorial_next_bt;
    int step1_cnt;
    int step2_cnt;
    int step3_cnt;
    bool excute_once;
    public GameObject face_panel_target;
    public RawImage emo_picture_panel_target;
    public GameObject emo_Bt_set_step2;
    bool skip;
    int step2_rand_idx;

    care_effect care_effect_script;
    bgm_player bgm_player_script;
    Logger logger_script;

    // Start is called before the first frame update
    void Start()
    {
        emo_idx = 0;
        face_panel.SetActive(false);
        gameDonePrefab.SetActive(false);
        panel_fail.SetActive(false);
        text_last.gameObject.SetActive(false);
        emo_picture_panel.gameObject.SetActive(false);

        emo_list.Add("neutral");
        emo_list.Add("happy");
        emo_list.Add("surprised");
        emo_list.Add("angry");

        time_limit = 0;
        face_emo_edit_inti_script = GameObject.Find("facialexpression").GetComponent<face_emo_edit_intimate>();
        Pet = GameObject.Find("pomeLV0" + PlayerPrefs.GetInt("Level_pet").ToString());
        anim = Pet.GetComponent<Animator>();
        face_renderer = Pet.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
        set_difficulty();

        face_panel_target.SetActive(false);
        emo_picture_panel_target.gameObject.SetActive(false);
        tutorial_next_bt.SetActive(false);
        emo_Bt_set_step2.SetActive(false);
        skip = true;

        care_effect_script = GameObject.Find("care_effect_player").GetComponent<care_effect>();
        bgm_player_script = GameObject.Find("Audio player").GetComponent<bgm_player>();
        logger_script = GameObject.Find("logger_gb").GetComponent<Logger>();

        StartCoroutine(bgm_player_script.excute_sound("53", 1f));
        logger_script.logger_master.insert_data("친해지기 게임 튜토리얼 시작");

    }

    // Update is called once per frame
    void Update()
    {
        //slider_time.value = time_remain / time_limit;
        //if (start_flag)
        //{
        //    if (time_remain > 0) time_remain -= Time.deltaTime;

        //    if (time_remain > 0 && !correct)
        //    {
        //        var idx = 0;
        //        if (text_emo.text == "neutral") idx = 0;
        //        else if (text_emo.text == "happy") idx = 1;
        //        else if (text_emo.text == "surprised") idx = 2;
        //        else if (text_emo.text == "angry") idx = 3;
        //        else if (text_emo.text == "sad") idx = 4;

        //        face_panel.SetActive(true);
        //        emo_picture_panel.texture = emo_picture_list[idx];

        //    }
        //    //시간이 지나기 전에 정답을 맞췄다면 시간을 초기화 하고 넘긴다
        //    else if (time_remain > 0 && correct)
        //    {
        //        start_flag = false;
        //        time_remain = 0;
        //        cnt_succes++;
        //        text_instruct.text = "잘 하셨어요!  맞춘 갯수: " + cnt_succes + "/5";

        //        Invoke("restart", 2f);
        //        Invoke("game_start_button_click", 5f);
        //    }

        //    //시간이 다 지날 때 까지 정답을 맞추지 못했다, 초기화
        //    else if (time_remain < 0 && !correct)
        //    {
        //        start_flag = false;
        //        time_remain = 0;
        //        cnt_fail++;
        //        text_instruct.text = "다시 해볼까요?";

        //        Invoke("restart", 2f);
        //        Invoke("game_start_button_click", 5f);
        //    }
        //}

        ////약 2초동안은 check function 실행하지 않고 표정을 따라하게 한다.
        //if (time_remain < time_limit - 2f) check_fn();
        if (skip) return;

        if (tutorial_step == 1)
        {
            //Debug.Log("check12");
            var idx = 0;
            if (text_emo.text == "neutral") idx = 0;
            else if (text_emo.text == "happy") idx = 1;
            else if (text_emo.text == "surprised") idx = 2;
            else if (text_emo.text == "angry") idx = 3;
            else if (text_emo.text == "sad") idx = 4;

            emo_picture_panel.texture = emo_picture_list[idx];

            if (step1_cnt == 0 || step1_cnt == 4)
            {
                if (excute_once)
                {
                    emo_picture_panel_target.texture = emo_picture_list[1];
                    tutorial_msg.text = "화면 오른쪽에 보이는 그림에 따라 웃는표정을 지어주세요!";
                    bgm_player_script.excute_narration("55");
                    excute_once = false;
                }
                if (text_emo.text == "happy")
                {
                    logger_script.logger_master.insert_data("step1: 웃는 표정 따라하기 성공");
                    care_effect_script.sound_correct();
                    tutorial_msg.text = "잘 하셨어요!";
                    skip = true;
                    Invoke("tutorial_step1_delay_func", 2f);
                }
            }
            else if (step1_cnt == 1 || step1_cnt == 5)
            {
                if (excute_once)
                {
                    emo_picture_panel_target.texture = emo_picture_list[0];
                    tutorial_msg.text = "화면 오른쪽에 보이는 그림에 따라 무표정을 지어주세요!";
                    bgm_player_script.excute_narration("57");
                    excute_once = false;
                }
                if (text_emo.text == "neutral")
                {
                    logger_script.logger_master.insert_data("step1: 무표정 따라하기 성공");
                    care_effect_script.sound_correct();
                    tutorial_msg.text = "잘 하셨어요!";
                    skip = true;
                    Invoke("tutorial_step1_delay_func", 2f);
                }
            }
            else if (step1_cnt == 2 || step1_cnt == 6)
            {
                if (excute_once)
                {
                    emo_picture_panel_target.texture = emo_picture_list[2];
                    tutorial_msg.text = "화면 오른쪽에 보이는 그림에 따라 놀란 표정을 지어주세요!";
                    bgm_player_script.excute_narration("58");
                    excute_once = false;
                }
                if (text_emo.text == "surprised")
                {
                    logger_script.logger_master.insert_data("step1: 놀란 표정 따라하기 성공");
                    care_effect_script.sound_correct();
                    tutorial_msg.text = "잘 하셨어요!";
                    skip = true;
                    Invoke("tutorial_step1_delay_func", 2f);
                }
            }
            else if (step1_cnt == 3 || step1_cnt == 7)
            {
                if (excute_once)
                {
                    emo_picture_panel_target.texture = emo_picture_list[3];
                    tutorial_msg.text = "화면 오른쪽에 보이는 그림에 따라 화난 표정을 지어주세요!";
                    bgm_player_script.excute_narration("56");
                    excute_once = false;
                }
                if (text_emo.text == "angry")
                {
                    logger_script.logger_master.insert_data("step1: 화난 표정 따라하기 성공");
                    care_effect_script.sound_correct();
                    tutorial_msg.text = "잘 하셨어요!";
                    skip = true;
                    Invoke("tutorial_step1_delay_func", 2f);
                }
            }


        }
        else if (tutorial_step == 2)
        {
            if (excute_once)
            {
                Debug.Log("tutorial_step2 루프 실행횟수: " + step2_cnt);
                step2_rand_idx = MakeRandomNumbers(4)[0];
                setemotion_nodefault(emo_list[step2_rand_idx]);
                tutorial_msg.text = "강아지의 표정과 일치하는 표정을 골라서 터치해주세요!";
                if(step2_cnt < 1) bgm_player_script.excute_narration("60");
                excute_once = false;

                GameObject.Find("Button_neutral").transform.GetComponent<Button>().interactable = true;
                GameObject.Find("Button_happy").transform.GetComponent<Button>().interactable = true;
                GameObject.Find("Button_surprised").transform.GetComponent<Button>().interactable = true;
                GameObject.Find("Button_angry").transform.GetComponent<Button>().interactable = true;
            }
            //나머지 부분은 버튼 클릭 함수에 포함
        }
        else if (tutorial_step == 3)
        {
            var idx = 0;
            if (text_emo.text == "neutral") idx = 0;
            else if (text_emo.text == "happy") idx = 1;
            else if (text_emo.text == "surprised") idx = 2;
            else if (text_emo.text == "angry") idx = 3;
            else if (text_emo.text == "sad") idx = 4;

            if(!face_panel.activeSelf && step3_cnt != 10) face_panel.SetActive(true);
            emo_picture_panel.texture = emo_picture_list[idx];

            if(excute_once)
            {
                step2_rand_idx = MakeRandomNumbers(4)[0];
                setemotion_nodefault(emo_list[step2_rand_idx]);
                excute_once = false;
            }
            //정답을 맞췄다면 시간을 초기화 하고 넘긴다
            if (text_emo.text == emo_list[step2_rand_idx])
            {
                //start_flag = false;
                //time_remain = 0;
                step3_cnt++;
                skip = true;
                Invoke("tutorial_step3_delay_func", 2f);

                //text_instruct.text = "잘 하셨어요!  맞춘 갯수: " + cnt_succes + "/5";
                
                tutorial_msg.text = "잘 하셨어요!  남은 횟수: " + (10 - step3_cnt).ToString();
                logger_script.logger_master.insert_data("step3 강아지표정 따라하기 성공. 남은 횟수: " + (10 - step3_cnt).ToString());

                //Invoke("restart", 2f);
                //Invoke("game_start_button_click", 5f);
            }
        }
    }

    public void game_next_bt_clicked()
    {
        if (tutorial_step == 1)
        {
            tutorial_step++;
            emo_Bt_set_step2.SetActive(true);
            step2_rand_idx = MakeRandomNumbers(4)[0];
            setemotion_nodefault(emo_list[step2_rand_idx]);

            tutorial_msg.text = "강아지의 표정을 보고, 강아지의 표정과 일치하는 표정을 터치해주세요";
        }
        else if (tutorial_step == 2)
        {
            tutorial_step++;
            face_panel.SetActive(true);
            emo_picture_panel.gameObject.SetActive(true);
            excute_once = true;

            tutorial_msg.text = "이제는 강아지의 표정을 보고, 강아지의 표정을 따라 지어볼게요!";
            bgm_player_script.excute_narration("61");

        }

        tutorial_next_bt.SetActive(false);
    }

    void skip_false()
    {
        skip = false;
    }

    void tutorial_step1_delay_func()
    {
        Invoke("skip_false", 2f);
        step1_cnt++;
        excute_once = true;

        if (step1_cnt == 8)
        {
            //tutorial step1 종료
            face_panel.SetActive(false);
            face_panel_target.SetActive(false);
            emo_picture_panel.gameObject.SetActive(false);
            emo_picture_panel_target.gameObject.SetActive(false);
            tutorial_next_bt.SetActive(true);
        }
    }

    void tutorial_step2_delay_func()
    {
        Invoke("skip_false", 2f);
        excute_once = true;

        if(step2_cnt == 10)
        {
            logger_script.logger_master.insert_data("강아지 표정 맞추기 완료");
            excute_once = false;
            setemotion_nodefault(emo_list[0]);
            emo_Bt_set_step2.SetActive(false);
            tutorial_next_bt.SetActive(true);
        }
    }

    public void tutorial_step2_bt_click_function()
    {
        Debug.Log("실행횟수 확인");
        GameObject clickedobj = EventSystem.current.currentSelectedGameObject;

        Debug.Log(clickedobj.name + "          " + emo_list[step2_rand_idx]);

        if (clickedobj.name.Split("_")[1] == emo_list[step2_rand_idx])
        {
            care_effect_script.sound_correct();
            //버튼들 비활성화
            GameObject.Find("Button_neutral").transform.GetComponent<Button>().interactable = false;
            GameObject.Find("Button_happy").transform.GetComponent<Button>().interactable = false;
            GameObject.Find("Button_surprised").transform.GetComponent<Button>().interactable = false;
            GameObject.Find("Button_angry").transform.GetComponent<Button>().interactable = false;
            step2_cnt++;
            care_effect_script.sound_correct();
            tutorial_msg.text = "잘 하셨어요! 남은 횟수: " + (10 - step2_cnt).ToString();
            logger_script.logger_master.insert_data("강아지 표정 맞추기 성공! 남은횟수: " + (10 - step2_cnt).ToString());
            skip = true;
            Invoke("tutorial_step2_delay_func", 2f);
        }
        else
        {
            care_effect_script.sound_false2();
            logger_script.logger_master.insert_data("강아지 표정 맞추기 실패");
            Debug.Log("clickedobj.name.Split()[1] = " + clickedobj.name.Split("_")[1] +
                "\temo_list[step2_rand_idx] = " + emo_list[step2_rand_idx]);
            tutorial_msg.text = "다시 골라볼까요?";
        }
         
    }

    void tutorial_step3_delay_func()
    {
        Invoke("skip_false", 2f);
        excute_once = true;
        tutorial_msg.text = "이제는 강아지의 표정을 보고, 강아지의 표정을 따라 지어볼게요!";

        if (step3_cnt == 10)
        {
            logger_script.logger_master.insert_data("step3 강아지표정 따라하기 완료!, 친해지기 튜토리얼 종료");
            skip = true;
            excute_once = false;
            setemotion_nodefault(emo_list[0]);
            //tutorial_next_bt.SetActive(true);
            face_panel.SetActive(false);
            emo_picture_panel.gameObject.SetActive(false);
            tutorial_msg.text = "";
            gameDonePrefab.SetActive(true);
            gamestart_Button.SetActive(false);
            text_last.gameObject.SetActive(true);
            Invoke("goToMainGame", 5f);
            bgm_player_script.excute_narration("62");
        }
        else if(step3_cnt < 10)
        {

        }
    }

    void restart()
    {
        emo_picture_panel.gameObject.SetActive(false);
        face_panel.SetActive(false);
        setemotion_default();
    }

    void set_difficulty()
    {
        if (PlayerPrefs.GetInt("Level_inti") == 1) time_limit = 5f;
        if (PlayerPrefs.GetInt("Level_inti") == 2) time_limit = 4f;
        if (PlayerPrefs.GetInt("Level_inti") == 3) time_limit = 3f;

        Debug.Log("time_limt= " + time_limit.ToString());
    }

    void check_fn()
    {
        //text_emo와 정답(emo_list[emo_idx]]이 일치하면 모델 실행 코스트를 줄이기  위해 모델 실행을 멈춘다
        if (text_emo.text == emo_list[emo_idx])
        {
            correct = true;
            emo_picture_panel.texture = emo_picture_list[emo_idx];
            face_emo_edit_inti_script.excute_emo_model = false;
        }
        //정답이 "angry"일 경우 에는 text_emo가 sad나 disgust여도 정답 처리 한다 
        else if(emo_list[emo_idx] == "angry")
        {
            if(text_emo.text == "sad" || text_emo.text =="disgust")
            {
                correct = true;
                emo_picture_panel.texture = emo_picture_list[emo_idx];
                face_emo_edit_inti_script.excute_emo_model = false;
            }
        }
        else
        {
            Debug.Log(text_emo.text + "\t" + emo_list[emo_idx]);
        }

        if(cnt_succes == 5)
        {
            tutorial_step++;
        }

    }

    public void load_AR_scene()
    {
        SceneManager.LoadScene("10_AR_interaction");
    }

    void goToMainGame()
    {
        SceneManager.LoadScene("23_Virtual_intimating");
    }

    public void game_start_button_click()
    {

        //if(cnt_fail == 5)
        //{
        //    //실패 문구 보여주기
        //    face_panel.SetActive(false);
        //    panel_instruct.SetActive(false);
        //    panel_fail.SetActive(true);
        //    //TextMeshProUGUI text_fail = GameObject.Find("Text_fail").GetComponent<TextMeshProUGUI>();
        //    //text_fail.text = "다음 기회에 다시 도전해봐요!";
        //    Invoke("load_AR_scene", 4f);
        //    return;
        //}

        //if (cnt_succes == 5)
        //{
        //    face_panel.SetActive(false);
        //    gameDonePrefab.SetActive(true);
        //    panel_instruct.SetActive(false);
        //    PlayerPrefs.SetFloat("exp", PlayerPrefs.GetFloat("exp") + 0.03f);
        //    PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") + 2);
        //    text_last.gameObject.SetActive(true);
        //    text_last.text = "잘 하셨어요!\n 보상은 다음과 같습니다";
        //    text_instruct.text = "";
        //    Invoke("load_AR_scene", 4f);
        //    return;
        //}

        //emo_picture_panel.gameObject.SetActive(true);
        tutorial_step++;
        Invoke("skip_false", 2f);
        face_panel.SetActive(true);
        face_panel_target.SetActive(true);
        emo_picture_panel.gameObject.SetActive(true);
        emo_picture_panel_target.gameObject.SetActive(true);
        
        face_emo_edit_inti_script.excute_emo_model = true;
        //time_remain = time_limit;
        //if (start_flag == false) start_flag = true;
        if (tutorial_start_bt.activeSelf == true) tutorial_start_bt.SetActive(false);
        excute_once = true;
        tutorial_msg.text = "화면 오른쪽에 보이는 그림에 따라 웃는표정을 지어주세요!";

        //emo_idx = MakeRandomNumbers(4)[0];
        //setemotion_nodefault(emo_list[emo_idx]);
        //correct = false;
        //text_instruct.text = "";
    }

    public void setemotion_nodefault(string emo)
    {
        Debug.Log(emo);
        if (emo == "neutral")
        {
            for (int i = 0; i < 7; i++)
            {
                face_renderer.SetBlendShapeWeight(i, 0);
            }
            return;
        }

        //0:blink, 1:bark, 2:smile, 3:angry, 4:sad, 5:happy, 6:surprise
        int emo_label;
        emo_label = 0;
        if (emo == "blink") emo_label = 0;
        else if (emo == "bark") emo_label = 1;
        else if (emo == "smile") emo_label = 2;
        else if (emo == "angry") emo_label = 3;
        else if (emo == "sad") emo_label = 4;
        else if (emo == "happy") emo_label = 5;
        else if (emo == "surprised") emo_label = 6;

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
        //0:blink, 1:bark, 2:smile, 3:angry, 4:sad, 5:happy, 6:surprise
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
}
