using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class random_play : MonoBehaviour
{
    Player_statu player_statu_script;
    Petctrl petctrl_script;
    Logger logger_script;
    public bool game_start_flag;
    public float time_remain;
    public float time_max = 15f;
    public TextMeshProUGUI time_remain_text;
    public GameObject time_remain_text_wBG;
    bool initial_false;
    public GameObject inven_panel;
    public GameObject ques_panel;

    Contents1 contents1_script;
    Contents2 contents2_script;
    Contents3 contents3_script;
    Contents4 contents4_script;
    // Start is called before the first frame update
    // 언제 game_start_flag = true가 되나??
    // 1. 시작할때
    // 2. 다른게임이 끝나고 나서 re_init 할때

    void enable_t_r_text ()
    {
        time_remain_text_wBG.SetActive(true);
    }

    void Start()
    {
        time_remain_text_wBG.SetActive(false);
        Invoke("enable_t_r_text", 8f);
        time_remain = time_max;
        game_start_flag = true;
        player_statu_script = GameObject.Find("player_statu").GetComponent<Player_statu>();
        petctrl_script = GameObject.Find("Scripts").GetComponent<Petctrl>();
        logger_script = GameObject.Find("Scripts").GetComponent<Logger>();
        contents1_script = GameObject.Find("Scripts").GetComponent<Contents1>();
        contents2_script = GameObject.Find("Scripts").GetComponent<Contents2>();
        contents3_script = GameObject.Find("Scripts").GetComponent<Contents3>();
        contents4_script = GameObject.Find("Scripts").GetComponent<Contents4>();
    }

    // Update is called once per frame
    void Update()
    {
        if (contents1_script.c1_ongoing == true || contents2_script.c2_ongoing == true ||
            contents3_script.c3_ongoing == true || contents4_script.c4_ongoing == true)
        {
            time_remain = -.1f;
            time_remain_text_wBG.SetActive(false);
            //Debug.Log("random_play 패스");
            return;
        }

        //game_start_flag가 true가 되면 한번 실행.
        //game_start_flag가 true가 되는 순간은 게임이 끝났을때인데, 그 기능은 그냥 time_remain만 time_max로 바꿔주면 끝나는 일이다
        //게임이 실행되는 순간은 다른 게임이 끝났을때가 아니라, time_remain이 0이 되어야 할 때 이므로,
        //그러므로, 게임을 끝났을 때는 time_remain을 0으로만 초기화 시켜주고, time_remain이 0이 될 때 게임을 실행 시켜 준다.

        if (time_remain > 0)
        {
            if (time_remain_text.enabled == true)
                time_remain_text.text = ((int)time_remain % 60).ToString() + "초 후 돌보기 게임이\n시작 됩니다.";

            //인벤토리, 도움말 창 등을 볼 때는 시간 카운팅x
            if(inven_panel.activeSelf == false && ques_panel.activeSelf == false)
            {
                time_remain -= Time.deltaTime;
            }
            //Debug.Log(time_remain.ToString()); 
        }
        else
        {
            time_remain_text_wBG.SetActive(true);
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
        //다른 게임이 진행중이라면 새 게임을 시작하지 않는다.
        if (contents1_script.c1_ongoing == true || contents2_script.c2_ongoing == true ||
            contents3_script.c3_ongoing == true || contents4_script.c4_ongoing == true)
        {
            //Debug.Log("random_play 패스");
            return;
        }
        else
        {
            Debug.Log("random_play 실행\t" +
                contents1_script.c1_ongoing.ToString() + ("\t") +
                contents2_script.c2_ongoing.ToString() + ("\t") +
                contents3_script.c3_ongoing.ToString() + ("\t") +
                contents4_script.c4_ongoing.ToString() + ("\t"));
        }

        var zero_three = MakeRandomNumbers(4)[0];

        if(zero_three == 0)
        {
            contents1_script.hungry_bt_click();
        }
        if (zero_three == 1)
        {
            contents2_script.sleep_bt_clicked();
        }
        if (zero_three == 2)
        {
            contents3_script.bath_button_clicked();
        }
        if (zero_three == 3)
        {
            contents4_script.play_bt_clicked();
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
