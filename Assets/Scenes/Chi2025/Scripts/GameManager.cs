using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gamestart_Button;
    public Slider slider_time;
    public shooting_obstacle shooting_script;

    public int cnt_succes;
    public int cnt_fail;
    private float time_remain;
    private float time_remain_text;
    private bool start_flag;
    private int idx_answer;
    public GameObject pet;
    public GameObject pet_skinned;
    public GameObject speech_bubble; 
    public TextMeshProUGUI text_time;
    public TextMeshProUGUI text_succes;
    public TextMeshProUGUI text_fail;

    float show_debug_interval;
    public bool succes_or_fail;

    SkinnedMeshRenderer face_renderer;

    Logger logger_script;
    //InferenceController_G infControl__script;

    // Start is called before the first frame update
    void Start()
    {
        speech_bubble.SetActive(false);
        if(text_time != null) text_time.text = "";
        cnt_succes = 0;
        cnt_fail = 0;
        //infControl__script = GameObject.Find("InferenceManager").GetComponent<InferenceController_G>();
        shooting_script = GameObject.Find("agility_game").GetComponent<shooting_obstacle>();
        face_renderer = pet_skinned.GetComponent<SkinnedMeshRenderer>();

        logger_script = GameObject.Find("logger_gb").GetComponent<Logger>();
    }


    // Update is called once per frame
    void Update()
    {
        if (!SceneManager.GetActiveScene().name.Contains("agility")) return;
        if (show_debug_interval < shooting_script.time_cube_create)
        {
            show_debug_interval += Time.deltaTime;
        }
        else
        {
            //show_game_log();
            show_debug_interval = 0;
        }

        if (start_flag)
        {
            if(slider_time != null) slider_time.value = time_remain / 30f;
            if (time_remain > 0)
                time_remain -= Time.deltaTime;
        }

        //if (time_remain_text > 0)
        //{
        //    time_remain_text -= Time.deltaTime;
        //    text_time.text = ((int)time_remain_text % 60).ToString() + "초 안으로 말해주세요";
        //}

        //if (infControl__script.one_flag)
        //{
        //    Debug.Log("fist_flag_agility");
        //    anim.Play("002_Ball_Jump");
        //}
    }

    public void game_start_button_click()
    {
        time_remain = 30f;
        text_time.text = "";
        if (start_flag == false) start_flag = true;
        if (gamestart_Button.activeSelf == true) gamestart_Button.SetActive(false);
    }

    public void show_game_log()
    {
        if (SceneManager.GetActiveScene().name == "base_interaction") return;

        if (text_succes != null) text_succes.text = "성공: " + cnt_succes.ToString() + "/5";
        if (text_fail != null) text_fail.text = "실패: " + cnt_fail.ToString() + "/5";
        Debug.Log("cnt_succes: " + cnt_succes.ToString() + "\tcnt_fail: " + cnt_fail.ToString());
        if(!SceneManager.GetActiveScene().name.Contains("tutorial"))
            logger_script.logger_master.insert_data("현재 성공횟수: " + cnt_succes.ToString() + "\t현재 실패횟수: " + cnt_fail.ToString());


        //if (succes_or_fail)
        //{
        //    //setemotion("happy");
        //    //anim.Play("Dance");
        //    set_text_speechBubble("잘 하셨어요!");
        //    //heart_effect_true();
        //}
        //else
        //{
        //    //anim.Play("Disappoint");
        //    set_text_speechBubble("다시 해볼까요?");
        //}

    }

    public void set_text_speechBubble(string message)
    {
        speech_bubble.SetActive(true);
        if (speech_bubble.gameObject.activeSelf == true)
        {
            //Vector3 screenPos = Camera.main.WorldToScreenPoint(spawnedObject.transform.position + 0.5f * Vector3.up
            //													+ 0.3f * Vector3.right);
            //speech_bubble.transform.position = pet.transform.position + 0.25f * Vector3.up
            //                                  + 0.1f * Vector3.right + 0.1f * Vector3.back;
            //speech_bubble.transform.position = pet.transform.position + 0.7f * Vector3.up
            //          + 0.3f * Vector3.left + 0.1f * Vector3.back;


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

}
