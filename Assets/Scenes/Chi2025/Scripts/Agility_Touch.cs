using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Agility_Touch : MonoBehaviour
{
    public GameObject gamestart_Button;
    public Slider slider_time;

    private int cnt_succes;
    private int cnt_fail;
    private float time_remain;
    private float time_remain_text;
    private bool start_flag;
    private int idx_answer;
    private Animator anim;
    public GameObject pet;
    public TextMeshProUGUI text_time;

    InferenceController_G infControl__script;

    // Start is called before the first frame update
    void Start()
    {
        text_time.text = "";
        cnt_succes = 0;
        cnt_fail = 0;
        infControl__script = GameObject.Find("InferenceManager").GetComponent<InferenceController_G>();
    }

    // Update is called once per frame
    void Update()
    {
        slider_time.value = time_remain / 30f;
        if (start_flag)
        {
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
}
