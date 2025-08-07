using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Video;
using UnityEngine.UI;

public class Contents2 : MonoBehaviour
{
    Petctrl petctrl_script;
    Logger logger_script;
    random_play random_play_script;

    public GameObject reshow_bt;
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
    public bool c2_ongoing;

    bgm_player bgm_player_;
    Player_statu player;
    int level;
    int cnt_false;
    bool success_flag;
    int[] level_per_corr = new int[] { 0, 3, 4, 5 };

    // Start is called before the first frame update
    void Start()
    {

        petctrl_script = GameObject.Find("Scripts").GetComponent<Petctrl>();
        bgm_player_ = GameObject.Find("Audio player").GetComponent<bgm_player>();
        player = GameObject.Find("player_statu").GetComponent<Player_statu>();
        logger_script = GameObject.Find("Scripts").GetComponent<Logger>();
        random_play_script = GameObject.Find("Scripts").GetComponent<random_play>();

        list_answer_set = new List<List<string>>{
            new List<string> {"dot1", "dot2", "dot5", "dot7"},//4-1
            new List<string> {"dot2", "dot5", "dot8", "dot7"},//4-2
            new List<string> {"dot2", "dot5", "dot4", "dot7"},//4-3
            new List<string> {"dot3", "dot5", "dot4", "dot7"},//4-4
            new List<string> {"dot1", "dot4", "dot5", "dot9"},//4-5
            new List<string> {"dot3", "dot2", "dot5", "dot8"},//4-6
            new List<string> {"dot1", "dot4", "dot8", "dot9"},//4-7
            new List<string> {"dot7", "do4t", "dot2", "dot3"},//4-8
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
        reshow_bt.SetActive(false);
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
        level = player.Level_sleep;
        c2_flag = false;
        rand_idx = 0;
        level = player.Level_sleep;

        success_flag = true;
    }

    // Update is called once per frame
    void Update()
    {
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
                            cnt_false = 0;
                            Debug.Log("맞습니다-1");
                            cnt_answer += 1;
                            bgm_player_.success_sound_excute();
                            choose_answer_randomly();
                            //패턴을 한번 더 그려주세요!! 라는 문구 띄우기
                        }
                        else if (cnt_answer == 1)
                        {
                            if(success_flag)
                            {
                                Debug.Log("맞습니다-2");
                                if(level == 1)
                                {
                                    player.change_statu(0, -0.03f, 0, 0);
                                    PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") + 3);
                                }
                                else if(level == 2)
                                {
                                    PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") + 4);
                                    player.change_statu(0, -0.04f, 0, 0);
                                }
                                else if(level ==3)
                                {
                                    player.change_statu(0, -0.05f, 0, 0);
                                    PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") + 5);
                                }
                                logger_script.logger_master.insert_data("잠자기 게임 성공");
                                content2_panel.SetActive(false);
                                linegenerator.SetActive(false);
                                petctrl_script.set_text_speechBubble("패턴 맞추기에\n성공하였습니다!");
                                bgm_player_.getitem_sound_excute();
                                petctrl_script.heart_effect_true();
                                petctrl_script.pet_reaction_sleep();
                                cnt_answer = 0;
                                Invoke("sleep_bt_reset", 10f);
                            }
                            else
                            {
                                logger_script.logger_master.insert_data("잠자기 게임 실패, 5번 이상 오답");
                                petctrl_script.set_text_speechBubble("다음에 다시\n도전해볼까요?");
                                petctrl_script.pet_reaction_false();
                                content2_panel.SetActive(false);
                                linegenerator.SetActive(false);
                                Invoke("sleep_bt_reset", 5f);
                            }
                        }

                    }
                   else
                    {
                        //Debug.Log("check111");
                        cnt_false++;
                        if (cnt_false == 5)
                        {
                            cnt_false = 0;
                            success_flag = false;
                            if (cnt_answer == 1)
                            {
                                logger_script.logger_master.insert_data("잠자기 게임 실패, 5번 이상 오답");
                                petctrl_script.set_text_speechBubble("다음에 다시\n도전해볼까요?");
                                content2_panel.SetActive(false);
                                linegenerator.SetActive(false);
                                Invoke("sleep_bt_reset", 5f);
                                return;
                            }
                            petctrl_script.set_text_speechBubble("다음으로\n넘어가볼까요?");
                            choose_answer_randomly();
                            cnt_answer += 1;
                        }
                        else
                        {
                            logger_script.logger_master.insert_data($"오답 입니다. 오답 횟수: {cnt_false}");
                            petctrl_script.set_text_speechBubble("다시 그려볼까요?");
                            bgm_player_.fail_sound_excute();
                            petctrl_script.pet_reaction_false();
                        }

                    }
                    Debug.Log(string.Join(" ", current_sequence));
                    current_sequence = new string[]{ };
                    //현재 sequence 초기화 


                    break;
            }
        }
    }

    public void reshow_bt_clicked()
    {
        logger_script.logger_master.insert_data("패턴 다시보기 버튼 클릭");
        content2_panel.SetActive(false);

        //처음 정답 화면 보여주기
        if (answer_vid_screen != null && video != null)
        {
            Debug.Log("prepard_vid 실행");
            StartCoroutine(Preparevid());
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

        Invoke("null_video_screen", 10f);

    }

    void null_video_screen()
    {
        answer_vid_screen.texture = null;
        answer_vid_screen.enabled = false;
        if(c2_flag == true)    content2_panel.SetActive(true);
        reshow_bt.SetActive(true);
    }

    void choose_answer_randomly()
    {
        if(level == 1)
        {
            rand_idx = MakeRandomNumbers(0, 20)[0];
        }
        else if(level == 2)
        {
            rand_idx = MakeRandomNumbers(20, 40)[0];
        }
        else if(level == 3)
        {
            rand_idx = MakeRandomNumbers(40, 60)[0];
        }
        current_answer = list_answer_set[rand_idx].ToArray();

        video.url = Application.streamingAssetsPath + "/" + list_video_set[rand_idx];

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
            if(Vector2.Distance(touch_pos, items.Value) < 50f)
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
        logger_script.logger_master.insert_data("잠자기 게임 시작");
        c2_ongoing = true;
        c2_flag = true;
        petctrl_script.not_move_pet = true;
        linegenerator.SetActive(true);
        //bt_face.SetActive(false);
        bt_picture.SetActive(false);
        bt_set.SetActive(false);
        choose_answer_randomly();
    }

    public void sleep_bt_reset()
    {
        logger_script.logger_master.insert_data("잠자기 게임 종료");
        c2_flag = false;
        petctrl_script.not_move_pet = false;
        random_play_script.time_remain = random_play_script.time_max;
        random_play_script.game_start_flag = true;
        random_play_script.time_remain_text_wBG.SetActive(true);


        //bt_face.SetActive(true);
        bt_picture.SetActive(true);
        bt_set.SetActive(true);

        cnt_answer = 0;
        answer_vid_screen.texture = null;
        answer_vid_screen.enabled = false;

        success_flag = true;
        cnt_false = 0;

        int min_statu;
        min_statu = player.choose_higlight(); //enegry:0, fatigue:1, cleanliness:2, intimity:3
        bt_set.transform.GetChild(0).transform.position = bt_set.transform.GetChild(min_statu + 1).transform.position
                                                        + Vector3.left * 100;
        c2_ongoing = false;

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
