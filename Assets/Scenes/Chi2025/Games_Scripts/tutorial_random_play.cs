using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class tutorial_random_play : MonoBehaviour
{
    public bool game_start_flag;
    public float time_remain;
    public float time_max = 15f;
    public TextMeshProUGUI time_remain_text;
    public GameObject time_remain_text_wBG;
    public GameObject inven_panel;
    public GameObject help_panel;

    Tutorial_Contents1 t_contents1_script;
    Tutorial_Contents2 t_contents2_script;
    Tutorial_Contents3 t_contents3_script;
    Tutorial_Contents4 t_contents4_script;
    int zero_three;

    // Start is called before the first frame update
    // 언제 game_start_flag = true가 되나??
    // 1. 시작할때
    // 2. 다른게임이 끝나고 나서 re_init 할때


    void enable_t_r_text()
    {
        time_remain_text_wBG.SetActive(true);
    }

    void Start()
    {
        time_remain_text_wBG.SetActive(false);
        Invoke("enable_t_r_text", 8f);
        time_remain = time_max;
        game_start_flag = true;
        t_contents1_script = GameObject.Find("Scripts_tutorial").GetComponent<Tutorial_Contents1>();
        t_contents2_script = GameObject.Find("Scripts_tutorial").GetComponent<Tutorial_Contents2>();
        t_contents3_script = GameObject.Find("Scripts_tutorial").GetComponent<Tutorial_Contents3>();
        t_contents4_script = GameObject.Find("Scripts_tutorial").GetComponent<Tutorial_Contents4>();
    }

    // Update is called once per frame
    void Update()
    {
        if (t_contents1_script.cnt_next_bt_clicked != 0 || t_contents2_script.cnt_next_bt_clicked != 0 ||
            t_contents3_script.cnt_next_bath_bt_clicked != 0 || t_contents4_script.cnt_next_bt_clicked != 0)
        {
            time_remain = -.1f;
            time_remain_text_wBG.SetActive(false);
            //Debug.Log("random_play 패스");
            return;
        }


        //game_start_flag가 true가 되면 한번 실행.
        //game_start_flag가 true가 되는 순간은 게임이 끝났을때인데, 그 기능은 그냥 time_remain만 time_max로 바꿔주면 끝나는 일이다
        //게임이 실행되는 순간은 다른 게임이 끝났을때가 아니라, time_remain이 0이 되어야 할 때 이므로,
        //그러므로, 게임을 끝났을 때는 time_remain을 0으로만 초기화 시켜주고, time_remain이 0이 될 때 게임을 실행 시켜 준다. ㅇ

        if (time_remain > 0)
        {
            if (time_remain_text.enabled == true)
            {
                if (zero_three == 0)
                {
                    time_remain_text.text = ((int)time_remain % 60).ToString() + "초 후 먹이주기 게임이\n시작 됩니다.";
                }
                if (zero_three == 1)
                {
                    time_remain_text.text = ((int)time_remain % 60).ToString() + "초 후 재우기 게임이\n시작 됩니다.";
                }
                if (zero_three == 2)
                {
                    time_remain_text.text = ((int)time_remain % 60).ToString() + "초 후 목욕하기 게임이\n시작 됩니다.";
                }
                if (zero_three == 3)
                {
                    time_remain_text.text = ((int)time_remain % 60).ToString() + "초 후 놀아주기 게임이\n시작 됩니다.";
                }
            }
            //인벤토리, 도움말 창 등을 볼 때는 시간 카운팅x
            if (inven_panel.activeSelf == false && help_panel.activeSelf == false)
            {
                time_remain -= Time.deltaTime;
            }
            //Debug.Log(time_remain.ToString()); 
        }
        else
        {
            time_remain_text_wBG.SetActive(false);
            if (game_start_flag == true)
            {
                game_start_flag = false;
                when_game_start_flag_is_true();
                Debug.Log("랜덤 게임 시작");
            }
        }



    }

    public void when_game_start_flag_is_true()
    {
        if (t_contents1_script.cnt_next_bt_clicked != 0 || t_contents2_script.cnt_next_bt_clicked != 0 ||
            t_contents3_script.cnt_next_bath_bt_clicked != 0 || t_contents4_script.cnt_next_bt_clicked != 0)
        {
            return;
        }

        if (zero_three == 0)
        {
            t_contents1_script.hungry_next_bt_clicked();
        }
        if (zero_three == 1)
        {
            t_contents2_script.sleep_next_bt_clicked();
        }
        if (zero_three == 2)
        {
            t_contents3_script.bath_next_bt_clicked();
        }
        if (zero_three == 3)
        {
            t_contents4_script.intimity_next_bt_clicked();
        }

        zero_three += 1;
        if (zero_three == 4) zero_three = 0;

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
