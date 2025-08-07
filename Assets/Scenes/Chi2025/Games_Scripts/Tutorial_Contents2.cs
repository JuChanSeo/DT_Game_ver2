using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;

public class Tutorial_Contents2 : MonoBehaviour
{
    Petctrl petctrl_script;
    Player_statu player_statu_script;


    public GameObject linegenerator;
    public GameObject content2_panel;
    //public GameObject bt_face;
    public GameObject bt_picture;
    public GameObject bt_set;

    public RawImage answer_vid_screen;
    //public List<VideoPlayer> list_video_set = new List<VideoPlayer>();
    //public GameObject list_video_set;
    public VideoPlayer video;

    public List<List<string>> list_answer_set = new List<List<string>>();
    List<string> list_video_set = new List<string>();
    string[] current_answer;
    string[] current_sequence = new string[] { };
    public Transform[] dot_transform;
    Dictionary<string, Vector2> dot_vec2 = new Dictionary<string, Vector2>();
    int cnt_answer;
    int rand_idx;
    public bool c2_flag;
    bgm_player bgm_player_;
    Logger logger_script;
    Player_statu player;
    drawing_pattern drawing_pattern_script;
    tutorial_random_play tutorial_random_play_script;

    int level;

    public int cnt_next_bt_clicked;
    public GameObject tutorial_panel;
    public GameObject tutorial_bt;
    public TMP_Text tutorial_msg;
    public GameObject drawing_cursor;
    public TextMeshProUGUI time_text;
    float time;
    bool execute_next_bt;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("tutorial2 dot pos:\t" + content2_panel.transform.GetChild(0).position + "\t" + content2_panel.transform.GetChild(4).position
            + "\t" + content2_panel.transform.GetChild(8).position);
        petctrl_script = GameObject.Find("Scripts_tutorial").GetComponent<Petctrl>();
        bgm_player_ = GameObject.Find("Audio player").GetComponent<bgm_player>();
        player = GameObject.Find("player_statu").GetComponent<Player_statu>();
        drawing_pattern_script = GameObject.Find("Scripts_tutorial").GetComponent<drawing_pattern>();
        tutorial_random_play_script = GameObject.Find("Scripts_tutorial").GetComponent<tutorial_random_play>();
        list_answer_set = new List<List<string>>{
            new List<string> {"dot1", "dot2", "dot5", "dot7"},//4-1
            new List<string> {"dot2", "dot5", "dot8", "dot7"},//4-2
            new List<string> {"dot2", "dot5", "dot4", "dot7"},//4-3
            new List<string> {"dot3", "dot5", "dot4", "dot7"},//4-4
            new List<string> {"dot1", "dot4", "dot5", "dot9"},//4-5
            new List<string> {"dot3", "dot2", "dot5", "dot8"},//4-6
            new List<string> {"dot1", "dot4", "dot8", "dot9"},//4-7
            new List<string> {"dot7", "dot4", "dot2", "dot3"},//4-8
            new List<string> {"dot2", "dot4", "dot8", "dot6"},//4-9
            new List<string> {"dot2", "dot4", "dot8", "dot9"},//4-10
            new List<string> {"dot1", "dot2", "dot3", "dot5", "dot7"},//5-1
            new List<string> {"dot1", "dot4", "dot5", "dot8", "dot9"},//5-2
            new List<string> {"dot7", "dot8", "dot5", "dot6", "dot3"},//5-3
            new List<string> {"dot3", "dot6", "dot5", "dot4", "dot7"},//5-4
            new List<string> {"dot1", "dot4", "dot5", "dot6", "dot9"},//5-5
            new List<string> {"dot1", "dot2", "dot6", "dot8", "dot7"},//5-6
            new List<string> {"dot1", "dot4", "dot8", "dot6", "dot3"},//5-7
            new List<string> {"dot1", "dot2", "dot3", "dot6", "dot9"},//5-8
            new List<string> {"dot2", "dot4", "dot8", "dot6", "dot5"},//5-9
            new List<string> {"dot3", "dot2", "dot5", "dot8", "dot7"},//5-10
            new List<string> {"dot1", "dot5", "dot3", "dot6", "dot8", "dot9"},//6-1
            new List<string> {"dot1", "dot4", "dot7", "dot8", "dot6", "dot3"},//6-2
            new List<string> {"dot1", "dot2", "dot5", "dot7", "dot8", "dot9"},//6-3
            new List<string> {"dot3", "dot5", "dot6", "dot9", "dot8", "dot7"},//6-4
            new List<string> {"dot1", "dot5", "dot6", "dot9", "dot8", "dot7"},//6-5
            new List<string> {"dot2", "dot4", "dot5", "dot6", "dot8", "dot9"},//6-6
            new List<string> {"dot1", "dot4", "dot8", "dot9", "dot6", "dot3"},//6-7
            new List<string> {"dot1", "dot5", "dot7", "dot8", "dot6", "dot3"},//6-8
            new List<string> {"dot3", "dot6", "dot5", "dot9", "dot8", "dot7"},//6-9
            new List<string> {"dot7", "dot4", "dot1", "dot2", "dot5", "dot8", "dot9"},//7-1
            new List<string> {"dot1", "dot2", "dot3", "dot5", "dot7", "dot8", "dot9"},//7-2
            new List<string> {"dot7", "dot4", "dot5", "dot2", "dot3", "dot6", "dot9"},//7-3
            new List<string> {"dot3", "dot2", "dot5", "dot4", "dot7", "dot8", "dot9"},//7-4
            new List<string> {"dot1", "dot2", "dot5", "dot6", "dot9", "dot8", "dot7"},//7-5
            new List<string> {"dot3", "dot2", "dot1", "dot4", "dot7", "dot8", "dot9"},//7-6
            new List<string> {"dot1", "dot4", "dot7", "dot8", "dot9", "dot6", "dot3"},//7-7
            new List<string> {"dot7", "dot4", "dot5", "dot2", "dot3", "dot6", "dot9 "},//7-8
            new List<string> {"dot3", "dot2", "dot5", "dot6", "dot9", "dot8", "dot7"},//7-9
            new List<string> {"dot1", "dot2", "dot5", "dot8", "dot9", "dot6", "dot3"},//7-10
            new List<string> {"dot2", "dot4", "dot5", "dot6", "dot9", "dot8", "dot7"},//7-11
            new List<string> {"dot1", "dot2", "dot3", "dot6", "dot5", "dot4", "dot7", "dot8", "dot9"},//9-1
            new List<string> {"dot3", "dot2", "dot1", "dot4", "dot5", "dot6", "dot9", "dot8", "dot7"},//9-2
            new List<string> {"dot7", "dot4", "dot1", "dot2", "dot5", "dot8", "dot9", "dot6", "dot3"},//9-3
            new List<string> {"dot1", "dot4", "dot7", "dot8", "dot5", "dot2", "dot3", "dot6", "dot9"},//9-4
            new List<string> {"dot4", "dot1", "dot2", "dot6", "dot9", "dot8", "dot7", "dot5", "dot3"},//9-5
            new List<string> {"dot1", "dot5", "dot9", "dot6", "dot3", "dot2", "dot4", "dot7", "dot8"},//9-6
            new List<string> {"dot7", "dot8", "dot5", "dot4", "dot1", "dot2", "dot3", "dot6", "dot9"},//9-7
            new List<string> {"dot3", "dot2", "dot1", "dot4", "dot7", "dot8", "dot5", "dot6", "dot9"},//9-8
            new List<string> {"dot3", "dot2", "dot5", "dot6", "dot9", "dot8", "dot7", "dot4", "dot1"},//9-9
            new List<string> {"dot1", "dot4", "dot5", "dot2", "dot3", "dot6", "dot9", "dot8", "dot7"},//9-10
            new List<string> {"dot5", "dot4", "dot1", "dot2", "dot3", "dot6", "dot9", "dot8", "dot7"},//9-11
            new List<string> {"dot7", "dot4", "dot1", "dot2", "dot3", "dot6", "dot9", "dot8", "dot5"},//9-12
            new List<string> {"dot1", "dot4", "dot2", "dot6", "dot9", "dot8", "dot7", "dot5", "dot3"},//9-13
            new List<string> {"dot1", "dot4", "dot2", "dot7", "dot5", "dot3", "dot8", "dot6", "dot9"},//9-14
            new List<string> {"dot1", "dot4", "dot2", "dot5", "dot3", "dot6", "dot7", "dot8", "dot9"},//9-15
            new List<string> {"dot1", "dot4", "dot7", "dot2", "dot5", "dot8", "dot3", "dot6", "dot9"},//9-16
            new List<string> {"dot1", "dot5", "dot2", "dot3", "dot4", "dot7", "dot8", "dot6", "dot9"},//9-17
            new List<string> {"dot1", "dot2", "dot3", "dot4", "dot5", "dot6", "dot7", "dot8", "dot9"},//9-18
            new List<string> {"dot1", "dot2", "dot3", "dot6", "dot9", "dot8", "dot7", "dot4", "dot5"},//9-19
            new List<string> {"dot1", "dot2", "dot3", "dot6", "dot9", "dot8", "dot7", "dot5", "dot4"},//9-20
        };
        list_video_set = new List<string>()
        {
            "4p_0.mp4","4p_1.mp4","4p_2.mp4","4p_3.mp4","4p_4.mp4","4p_5.mp4","4p_6.mp4","4p_7.mp4","4p_8.mp4","4p_9.mp4",
            "5p_0.mp4","5p_1.mp4","5p_2.mp4","5p_3.mp4","5p_4.mp4","5p_5.mp4","5p_6.mp4","5p_7.mp4","5p_8.mp4","5p_9.mp4",
            "6p_0.mp4","6p_1.mp4","6p_2.mp4","6p_3.mp4","6p_4.mp4","6p_5.mp4","6p_6.mp4","6p_7.mp4","6p_8.mp4","7p_0.mp4",
            "7p_1.mp4","7p_2.mp4","7p_3.mp4","7p_4.mp4","7p_5.mp4","7p_6.mp4","7p_7.mp4","7p_8.mp4","7p_9.mp4","7p_10.mp4",
            "9p_0.mp4","9p_1.mp4","9p_2.mp4","9p_3.mp4","9p_4.mp4","9p_5.mp4","9p_6.mp4","9p_7.mp4","9p_8.mp4","9p_9.mp4",
            "9p_10.mp4","9p_11.mp4","9p_12.mp4","9p_13.mp4","9p_14.mp4","9p_15.mp4","9p_16.mp4","9p_17.mp4","9p_18.mp4","9p_19.mp4",
        };

        answer_vid_screen.enabled = false;
        linegenerator.SetActive(false);
        content2_panel.SetActive(false);
        // Vector2.Distance(touch_pos, dot_transform[0].position)
        dot_vec2.Add("dot1", dot_transform[0].position);
        dot_vec2.Add("dot2", dot_transform[1].position);
        dot_vec2.Add("dot3", dot_transform[2].position);
        dot_vec2.Add("dot4", dot_transform[3].position);
        dot_vec2.Add("dot5", dot_transform[4].position);
        dot_vec2.Add("dot6", dot_transform[5].position);
        dot_vec2.Add("dot7", dot_transform[6].position);
        dot_vec2.Add("dot8", dot_transform[7].position);
        dot_vec2.Add("dot9", dot_transform[8].position);
        cnt_answer = 0;
        c2_flag = false;
        rand_idx = 0;


        time = 15;
        level = 1;
        cnt_next_bt_clicked = 0;
        tutorial_panel.SetActive(false);
        tutorial_bt.SetActive(false);

        logger_script = GameObject.Find("Scripts_tutorial").GetComponent<Logger>();
    }

    // Update is called once per frame
    void Update()
    {
        time_text.text = ((int)(15 - (time % 60))).ToString();
        if (time < 15f) //게임이 진행중일 때만 시간을 계산하려고 c1_flag &&를 추가했었는데 하면 안 될듯...
        {
            time += Time.deltaTime;
        }
        else
        {
            if (execute_next_bt)
            {
                if (cnt_next_bt_clicked == 1 || cnt_next_bt_clicked == 2)
                {
                    Debug.Log("logging:\tcnt_next_bt_clicked: " + cnt_next_bt_clicked);
                    execute_next_bt = false;
                    sleep_next_bt_clicked();
                }
            }
        }

        if (!c2_flag) return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:

                    break;

                case TouchPhase.Moved:
                    //순서대로 설정되어있는 배열을 제대로 지났는지 확인한다.
                    var touch_pos = touch.position;
                    store_crossed(touch_pos);
                    break;

                case TouchPhase.Ended:
                    GameObject lineobj = GameObject.Find("UI LineRenderer(Clone)");
                    Destroy(lineobj);
                    if (current_sequence.Length < 3)
                    {
                        break;
                    }
                    if (Enumerable.SequenceEqual(current_sequence, current_answer))
                    {
                        if (cnt_answer == 0)
                        {
                            Debug.Log("맞습니다!!");
                            sleep_next_bt_clicked();
                            bgm_player_.success_sound_excute();
                            content2_panel.SetActive(false);
                            linegenerator.SetActive(false);
                            petctrl_script.heart_effect_true();
                            petctrl_script.pet_reaction_sleep();
                            Invoke("sleep_bt_reset", 10f);
                            //패턴을 한번 더 그려주세요!! 라는 문구 띄우기
                        }
                    }
                    else
                    {
                        bgm_player_.fail_sound_excute();
                        petctrl_script.pet_reaction_false();
                    }
                    Debug.Log(string.Join(" ", current_sequence));
                    current_sequence = new string[] { };
                    //현재 sequence 초기화 


                    break;
            }
        }
    }

    public void sleep_next_bt_clicked()
    {
        Debug.Log("cnt_next_bt_clicked(sleep): " + cnt_next_bt_clicked);
        if (cnt_next_bt_clicked == 0)
        {
            bt_picture.SetActive(false);
            bt_set.SetActive(false);
            petctrl_script.not_move_pet = true;
            tutorial_bt.SetActive(true);
            tutorial_msg.text = "강아지를 한번 재워볼까요?";
            tutorial_panel.SetActive(true);
            cnt_next_bt_clicked++;
            if (time_text.gameObject.activeSelf != true) time_text.gameObject.SetActive(true);
            time = 0;
            execute_next_bt = true;
            logger_script.logger_master.insert_data("연습하기 - 재우기 게임 연습 시작");
        }
        else if (cnt_next_bt_clicked == 1)
        {
            tutorial_msg.text = "화면에 보이는 패턴을 기억해주세요!";
            sleep_bt_clicked();
            cnt_next_bt_clicked++;
            time = 0;
            execute_next_bt = true;
            logger_script.logger_master.insert_data("연습하기 - 재우기 게임 패턴 암기 시작");
        }
        else if (cnt_next_bt_clicked == 2)
        {
            tutorial_msg.text = "화면에 손을 붙인 상태에서\n손 모양을 따라 한번에 그려주세요!";
            drawing_pattern_script.alloc_pattern(rand_idx);
            null_video_screen();
            cnt_next_bt_clicked++;
            time_text.gameObject.SetActive(false);
            logger_script.logger_master.insert_data("연습하기 - 재우기 게임 가이드 따라 패턴 따라 그리기");
        }
        else if (cnt_next_bt_clicked == 3)
        {
            tutorial_bt.SetActive(false);
            tutorial_msg.text = "패턴 그리기에 성공했어요!";
            drawing_cursor.SetActive(false);
            Invoke("sleep_bt_reset", 10f);
            logger_script.logger_master.insert_data("연습하기 - 재우기 게임 가이드 따라 패턴 그리기 성공. 재우기 게임 연습 종료.");
        }
    }

    protected IEnumerator Preparevid()
    {
        video.Prepare();


        while (!video.isPrepared)
        {

            yield return new WaitForSeconds(0.5f);
        }


        answer_vid_screen.enabled = true;
        answer_vid_screen.texture = video.texture;
        video.Play();


    }

    public void null_video_screen()
    {
        answer_vid_screen.texture = null;
        answer_vid_screen.enabled = false;
        if (c2_flag == true) content2_panel.SetActive(true);

    }

    void choose_answer_randomly()
    {
        if (level == 1)
        {
            rand_idx = MakeRandomNumbers(0, 10)[0];
        }

        //rand_idx = MakeRandomNumbers(0, list_video_set.Count)[0];
        current_answer = list_answer_set[rand_idx].ToArray();
        video.url = Application.streamingAssetsPath + "/" + list_video_set[rand_idx];

        Debug.Log(level + "\t" + rand_idx + "\t" + current_answer +"\t" + list_video_set[rand_idx]);


        Debug.Log("current_answer: " + string.Join(", ", current_answer));

        if (answer_vid_screen != null && video != null)
        {
            Debug.Log("prepard_vid 실행");
            StartCoroutine(Preparevid());
        }

    }

    void store_crossed(Vector2 touch_pos)
    {
        //Debug.Log(touch_pos.ToString() + "\t" + dot_transform[0].position.ToString() + "\t" +  Vector2.Distance(touch_pos, dot_transform[0].position));
        foreach (KeyValuePair<string, Vector2> items in dot_vec2)
        {
            if (Vector2.Distance(touch_pos, items.Value) < 50f)
            {
                //Debug.Log(items.Key);
                //없으면 추가시켜 준다.
                if (Array.Find(current_sequence, element => element == items.Key) != items.Key)
                {
                    //Debug.Log(Array.Find(current_sequence, element => element == items.Key) + "\t" + items.Key + " 추가");
                    current_sequence = current_sequence.Append(items.Key).ToArray();
                }
            }
        }

    }

    public void sleep_bt_clicked()
    {
        c2_flag = true;
        linegenerator.SetActive(true);
        //bt_face.SetActive(false);
        choose_answer_randomly();
    }

    public void sleep_bt_reset()
    {
        c2_flag = false;
        petctrl_script.not_move_pet = false;
        //bt_face.SetActive(true);
        bt_picture.SetActive(true);
        bt_set.SetActive(true);

        tutorial_random_play_script.time_remain = tutorial_random_play_script.time_max;
        tutorial_random_play_script.game_start_flag = true;
        tutorial_random_play_script.time_remain_text_wBG.SetActive(true);

        cnt_answer = 0;
        answer_vid_screen.texture = null;
        answer_vid_screen.enabled = false;


        tutorial_panel.SetActive(false);
        cnt_next_bt_clicked = 0;

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
