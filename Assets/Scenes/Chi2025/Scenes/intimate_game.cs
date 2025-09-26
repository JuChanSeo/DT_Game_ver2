using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class intimate_game : MonoBehaviour
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

    public QuestManager_daily questM_daily_script;
    public QuestManager_weekly questM_weekly_script;

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

        time_limit = 1f;
        face_emo_edit_inti_script = GameObject.Find("facialexpression").GetComponent<face_emo_edit_intimate>();
        Pet = GameObject.Find("pomeLV0" + PlayerPrefs.GetInt("Level_pet").ToString());
        anim = Pet.GetComponent<Animator>();
        face_renderer = Pet.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
        questM_daily_script = GameObject.Find("Quest_Manager").GetComponent<QuestManager_daily>();
        questM_weekly_script = GameObject.Find("Quest_Manager").GetComponent<QuestManager_weekly>();

        care_effect_script = GameObject.Find("care_effect_player").GetComponent<care_effect>();
        bgm_player_script = GameObject.Find("Audio player").GetComponent<bgm_player>();
        logger_script = GameObject.Find("logger_gb").GetComponent<Logger>();

        StartCoroutine(bgm_player_script.excute_sound("63", 1f));
        logger_script.logger_master.insert_data("친해지기 게임 본게임 시작!");
    }

    // Update is called once per frame
    void Update()
    {
        slider_time.value = time_remain / time_limit;
        if (start_flag)
        {
            if(time_remain > 0) time_remain -= Time.deltaTime;

            if (time_remain > 0 && !correct)
            {
                var idx = 0;
                if (text_emo.text == "neutral") idx = 0;
                else if (text_emo.text == "happy") idx = 1;
                else if (text_emo.text == "surprised") idx = 2;
                else if (text_emo.text == "angry") idx = 3;
                else if (text_emo.text == "sad") idx = 4;


                face_panel.SetActive(true);
                emo_picture_panel.texture = emo_picture_list[idx];

            }
            //시간이 지나기 전에 정답을 맞췄다면 시간을 초기화 하고 넘긴다
            else if (time_remain > 0 && correct)
            {
                care_effect_script.sound_correct();
                start_flag = false;
                time_remain = 0;
                cnt_succes++;
                text_instruct.text = "잘 하셨어요!  맞춘 갯수: " + cnt_succes + "/5";
                logger_script.logger_master.insert_data("표정 맞추기 성공. 성공 횟수: " + cnt_succes);


                Invoke("restart", 2f);
                Invoke("game_start_button_click", 5f);
            }

            //시간이 다 지날 때 까지 정답을 맞추지 못했다, 초기화
            else if (time_remain < 0 && !correct)
            {
                care_effect_script.sound_false2();
                start_flag = false;
                time_remain = 0;
                cnt_fail++;
                logger_script.logger_master.insert_data("시간초과. 표정 맞추기 실패. 실패 횟수: " + cnt_fail);
                text_instruct.text = "다시 해볼까요?";

                Invoke("restart", 2f);
                Invoke("game_start_button_click", 5f);
            }
        }

        //약 2초동안은 check function 실행하지 않고 표정을 따라하게 한다.
        if(time_remain < time_limit - 2f) check_fn();
    }

    void restart()
    {
        emo_picture_panel.gameObject.SetActive(false);
        face_panel.SetActive(false);
        setemotion_default();
    }

    void set_difficulty()
    {
        if (PlayerPrefs.GetInt("Level_inti") == 1) time_limit = 7f;
        if (PlayerPrefs.GetInt("Level_inti") == 2) time_limit = 6f;
        if (PlayerPrefs.GetInt("Level_inti") == 3) time_limit = 5f;

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

    }

    public void load_AR_scene()
    {
        SceneManager.LoadScene("10_AR_interaction_MZ");
    }

    public void game_start_button_click()
    {

        if(cnt_fail == 5)
        {
            //실패 문구 보여주기
            logger_script.logger_master.insert_data("친해지기 게임 실패!");
            face_panel.SetActive(false);
            panel_instruct.SetActive(false);
            panel_fail.SetActive(true);
            //TextMeshProUGUI text_fail = GameObject.Find("Text_fail").GetComponent<TextMeshProUGUI>();
            //text_fail.text = "다음 기회에 다시 도전해봐요!";
            Invoke("load_AR_scene", 4f);
            return;
        }

        if (cnt_succes == 5)
        {
            logger_script.logger_master.insert_data("친해지기 게임 성공!");
            bgm_player_script.excute_narration("80");
            care_effect_script.sound_reward_popup();
            face_panel.SetActive(false);
            gameDonePrefab.SetActive(true);
            panel_instruct.SetActive(false);
            PlayerPrefs.SetFloat("exp", PlayerPrefs.GetFloat("exp") + 0.02f);
            PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") + 2);
            PlayerPrefs.SetFloat("intimity", PlayerPrefs.GetFloat("intimity") + 0.01f);
            text_last.gameObject.SetActive(true);
            text_last.text = "잘 하셨어요!\n 보상은 다음과 같습니다";
            text_instruct.text = "";
            questM_daily_script.intimate_plus();
            questM_weekly_script.caregame_plus("intimate");
            Invoke("load_AR_scene", 4f);
            return;
        }

        set_difficulty();
        if (!panel_instruct.activeSelf) panel_instruct.SetActive(true);
        face_panel.SetActive(true);
        emo_picture_panel.gameObject.SetActive(true);
        face_emo_edit_inti_script.excute_emo_model = true;
        time_remain = time_limit;
        if (start_flag == false) start_flag = true;
        if (gamestart_Button.activeSelf == true) gamestart_Button.SetActive(false);
        emo_idx = MakeRandomNumbers(4)[0];
        setemotion_nodefault(emo_list[emo_idx]);
        correct = false;
        text_instruct.text = "강아지의 표정을 따라해보세요";
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
