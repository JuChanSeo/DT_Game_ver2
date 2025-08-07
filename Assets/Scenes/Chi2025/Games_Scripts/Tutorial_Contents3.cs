using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tutorial_Contents3 : MonoBehaviour
{
    public GameObject content3_panel;
    //public GameObject bt_face;
    public GameObject bt_picture;
    public GameObject bt_set;
    public List<Sprite> bottles_sprite = new List<Sprite>();
    public List<GameObject> bottle_3d = new List<GameObject>();
    public List<Image> bottles = new List<Image>();
    public TMP_Text time_remain_text;
    public GameObject debug_prefab;
    bool flag_set_empty;
    //Vector3[] spreaded_points;
    List<Vector3> spreaded_points = new List<Vector3>();
    List<Vector3> extracted_points = new List<Vector3>();

    Petctrl petctrl_script;
    bgm_player bgm_player_;
    Player_statu player;
    Logger logger_script;
    tutorial_random_play tutorial_random_play_script;
    float time_remain;
    int level;//나중에 다른데에서 부터 받아온다
    int cnt_answer;
    string[] answer_seq_color = new string[5];//bottle_red, bottle_blue, ...
    Dictionary<string, int> color_to_num = new Dictionary<string, int>();
    Dictionary<string, string> color_en_to_kr = new Dictionary<string, string>();
    public bool c3_flag;

    public int cnt_next_bath_bt_clicked;
    int[] shuffled_idx;
    public GameObject tutorial_panel;
    public GameObject tutorial_bt;
    public TMP_Text tutorial_msg;
    public GameObject arrow_3d;
    public GameObject touch_highlight;

    public TextMeshProUGUI time_text;
    float time;
    bool execute_next_bt;

    // Start is called before the first frame update
    void Start()
    {
        time = 15;
        color_to_num.Add("red", 1);
        color_to_num.Add("orange", 2);
        color_to_num.Add("yellow", 3);
        color_to_num.Add("green", 4);
        color_to_num.Add("blue", 5);
        color_en_to_kr.Add("red", "빨강");
        color_en_to_kr.Add("orange", "주황");
        color_en_to_kr.Add("yellow", "노랑");
        color_en_to_kr.Add("green", "초록");
        color_en_to_kr.Add("blue", "파랑");

        //System.Range r1 = 0..4;
        for (int i = 0; i < 4; i++)
        {
            bottle_3d[i].SetActive(false);
        }
        cnt_answer = 0;
        petctrl_script = GameObject.Find("Scripts_tutorial").GetComponent<Petctrl>();
        bgm_player_ = GameObject.Find("Audio player").GetComponent<bgm_player>();
        player = GameObject.Find("player_statu").GetComponent<Player_statu>();
        tutorial_random_play_script = GameObject.Find("Scripts_tutorial").GetComponent<tutorial_random_play>();
        time_remain = 0;
        content3_panel.SetActive(false);
        level = 1;

        cnt_next_bath_bt_clicked = 0;
        tutorial_panel.SetActive(false);
        tutorial_bt.SetActive(false);
        arrow_3d.SetActive(false);
        touch_highlight.SetActive(false);

        logger_script = GameObject.Find("Scripts_tutorial").GetComponent<Logger>();
    }

    // Update is called once per frame
    void Update()
    {
        time_text.text = ((int)(15 - (time % 60))).ToString();
        if (time < 15f)
        {
            time += Time.deltaTime;
        }
        else
        {
            if (execute_next_bt)
            {
                if (cnt_next_bath_bt_clicked == 1 || cnt_next_bath_bt_clicked == 2)
                {
                    Debug.Log("logging:\tcnt_next_bt_clicked: " + cnt_next_bath_bt_clicked);
                    execute_next_bt = false;
                    bath_next_bt_clicked();
                }
            }
        }

        if (!c3_flag) return;

        if (touch_highlight.activeSelf == true)
        {
            string answer_c = answer_seq_color[cnt_answer].Split("_")[1];
            touch_highlight.transform.position = Camera.main.WorldToScreenPoint(bottle_3d[color_to_num[answer_c] - 1].transform.position);
        }


        if (Input.touchCount > 0)
        {
            //Debug.Log("logging");
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                var ray = Camera.main.ScreenPointToRay(touch.position);
                Physics.Raycast(ray, out var hit, float.PositiveInfinity);

                if (hit.transform.name.StartsWith("bottle"))
                {
                    Debug.Log(hit.transform.name);
                    check_the_answer(hit.transform.name);
                }
                else
                {

                }
            }
        }
    }

    public void bath_next_bt_clicked()
    {
        Debug.Log("cnt_next_bath_bt_clicked: " + cnt_next_bath_bt_clicked);
        if (cnt_next_bath_bt_clicked == 0)
        {
            bt_picture.SetActive(false);
            bt_set.SetActive(false);
            petctrl_script.not_move_pet = true;
            tutorial_panel.SetActive(true);
            tutorial_bt.SetActive(true);
            tutorial_msg.text = "강아지를 씻겨볼까요?";
            cnt_next_bath_bt_clicked++;
            if (time_text.gameObject.activeSelf != true) time_text.gameObject.SetActive(true);
            time = 0;
            execute_next_bt = true;
        }
        else if (cnt_next_bath_bt_clicked == 1)
        {
            bath_button_clicked();
            cnt_next_bath_bt_clicked++;
            time = 0;
            execute_next_bt = true;
            logger_script.logger_master.insert_data("연습하기 - 목욕하기 게임 연습 시작.");
        }
        else if (cnt_next_bath_bt_clicked == 2)
        {
            arrow_3d.SetActive(true);
            touch_highlight.SetActive(true);
            spread_bottle_3d();
            reset_to_empty();
            tutorial_bt.SetActive(false);
            tutorial_msg.text = "외운 순서대로 첫 번째 샴푸병을 골라주세요!";
            cnt_next_bath_bt_clicked++;
            time_text.gameObject.SetActive(false);
            logger_script.logger_master.insert_data("연습하기 - 목욕하기 게임 첫 번째 샴푸병 선택");
        }
        else if (cnt_next_bath_bt_clicked == 3)
        {
            string answer_c = answer_seq_color[1].Split("_")[1];
            arrow_3d.transform.position = bottle_3d[color_to_num[answer_c] - 1].transform.position + 0.2f * Vector3.up;
            tutorial_msg.text = "정답이에요!\n두 번째 샴푸병을 골라주세요!";
            cnt_next_bath_bt_clicked++;
            logger_script.logger_master.insert_data("연습하기 - 목욕하기 게임 두 번째 샴푸병 선택");
        }
        else if (cnt_next_bath_bt_clicked == 4)
        {
            string answer_c = answer_seq_color[2].Split("_")[1];
            arrow_3d.transform.position = bottle_3d[color_to_num[answer_c] - 1].transform.position + 0.2f * Vector3.up;
            tutorial_msg.text = "정답이에요!\n마지막 샴푸병을 골라주세요!";
            cnt_next_bath_bt_clicked++;
            logger_script.logger_master.insert_data("연습하기 - 목욕하기 게임 세 번째 샴푸병 선택");
        }
        else if (cnt_next_bath_bt_clicked == 5)
        {
            arrow_3d.SetActive(false);
            touch_highlight.SetActive(false);
            tutorial_msg.text = "잘 하셨어요! 샴푸병을 모두 다 골랐어요!";
            Invoke("re_init", 10f);
            logger_script.logger_master.insert_data("연습하기 - 목욕하기 게임 샴푸병 선택 완료. 목욕하기 게임 연습 종료.");
        }
    }
    public void bath_button_clicked()
    {
        c3_flag = true;

        content3_panel.SetActive(true);

        content3_panel.transform.GetChild(0).gameObject.SetActive(true);
        content3_panel.transform.GetChild(1).gameObject.SetActive(true);
        content3_panel.transform.GetChild(2).gameObject.SetActive(true);
        content3_panel.transform.GetChild(3).gameObject.SetActive(true);
        content3_panel.transform.GetChild(4).gameObject.SetActive(true);



        if (level == 1)
        {
            content3_panel.transform.GetChild(0).gameObject.SetActive(false);
            content3_panel.transform.GetChild(4).gameObject.SetActive(false);
        }
        else if (level == 2)
        {
            content3_panel.transform.GetChild(0).gameObject.SetActive(false);
            bottles[1].transform.position = new Vector3(906f - 240f, 929.22f, 0);
            bottles[2].transform.position = new Vector3(1253f - 240f, 929.22f, 0);
            bottles[3].transform.position = new Vector3(1601f - 240f, 929.22f, 0);
            bottles[4].transform.position = new Vector3(1948f - 240f, 929.22f, 0);

        }
        show_the_answer();

    }

    void show_the_answer()
    {
        var shuffled_idx = MakeRandomNumbers(1, 6);
        if (level == 1)
        {
            bottles[1].sprite = bottles_sprite[shuffled_idx[0]];
            bottles[2].sprite = bottles_sprite[shuffled_idx[1]];
            bottles[3].sprite = bottles_sprite[shuffled_idx[2]];
            answer_seq_color[0] = bottles[1].sprite.name;
            answer_seq_color[1] = bottles[2].sprite.name;
            answer_seq_color[2] = bottles[3].sprite.name;
        }
        else if (level == 2)
        {
            bottles[1].sprite = bottles_sprite[shuffled_idx[0]];
            bottles[2].sprite = bottles_sprite[shuffled_idx[1]];
            bottles[3].sprite = bottles_sprite[shuffled_idx[2]];
            bottles[4].sprite = bottles_sprite[shuffled_idx[3]];
            answer_seq_color[0] = bottles[1].sprite.name;
            answer_seq_color[1] = bottles[2].sprite.name;
            answer_seq_color[2] = bottles[3].sprite.name;
            answer_seq_color[3] = bottles[4].sprite.name;
        }
        else //levle==3
        {
            bottles[0].sprite = bottles_sprite[shuffled_idx[0]];
            bottles[1].sprite = bottles_sprite[shuffled_idx[1]];
            bottles[2].sprite = bottles_sprite[shuffled_idx[2]];
            bottles[3].sprite = bottles_sprite[shuffled_idx[3]];
            bottles[4].sprite = bottles_sprite[shuffled_idx[4]];
            answer_seq_color[0] = bottles[0].sprite.name;
            answer_seq_color[1] = bottles[1].sprite.name;
            answer_seq_color[2] = bottles[2].sprite.name;
            answer_seq_color[3] = bottles[3].sprite.name;
            answer_seq_color[4] = bottles[4].sprite.name;

        }
        tutorial_msg.text = $"왼쪽부터 {color_en_to_kr[answer_seq_color[0].Split("_")[1]]}, " +
                                   $"{color_en_to_kr[answer_seq_color[1].Split("_")[1]]}, " +
                                   $"{color_en_to_kr[answer_seq_color[2].Split("_")[1]]}, " +
                                   "\n순서대로 샴푸병 순서를 외워주세요!";
    }

    void reset_to_empty()
    {
        if (!c3_flag) return;
        //bottles[0].sprite = bottles_sprite[0];
        //bottles[1].sprite = bottles_sprite[0];
        //bottles[2].sprite = bottles_sprite[0];
        //bottles[3].sprite = bottles_sprite[0];
        //bottles[4].sprite = bottles_sprite[0];
        bottles[0].gameObject.SetActive(false);
        bottles[1].gameObject.SetActive(false);
        bottles[2].gameObject.SetActive(false);
        bottles[3].gameObject.SetActive(false);
        bottles[4].gameObject.SetActive(false);

    }

    void spread_bottle_3d()
    {

        for (int i = 0; i < 500; i++)
        {
            int range_x = Random.Range(0, Screen.width);
            int range_y = Random.Range(0, (int)(Screen.height / 2));
            var ray = Camera.main.ScreenPointToRay(new Vector2(range_x, range_y));
            Physics.Raycast(ray, out var hit, float.PositiveInfinity);
            //Debug.Log(range_x + "\t" + range_y + "\t" + hit.point.x + "\t" + hit.point.y);
            //Instantiate(debug_prefab, hit.point, Quaternion.identity);
            if (hit.point.y > -2f && hit.point.y < 0)
            {
                spreaded_points.Add(hit.point);
            }

        }

        Debug.Log("spread done, spreaded_points.Count: " + spreaded_points.Count);
        int level_per_num_bottles = level + 2;
        shuffled_idx = MakeRandomNumbers(0, spreaded_points.Count);


        //for (int i = 0; i < level_per_num_bottles; i++)
        //{
        //    string answer_color = answer_seq_color[i].Split("_")[1];
        //    //Debug.Log("color_to_num[answer_color] - 1: " + (color_to_num[answer_color] - 1));
        //    //Debug.Log(spreaded_points[rand_idx].x + "\t" + spreaded_points[rand_idx].y + "\t" + spreaded_points[rand_idx].z);

        //    int rand_idx = shuffled_idx[i];
        //    bottle_3d[color_to_num[answer_color] - 1].transform.position
        //            = spreaded_points[rand_idx] + 0.2f * Vector3.up;
        //    extracted_points.Add(spreaded_points[rand_idx]);
        //    bottle_3d[color_to_num[answer_color] - 1].SetActive(true);
        //    Debug.Log(answer_color + " 위치: " + bottle_3d[color_to_num[answer_color] - 1].transform.position);
        //}

        while (extracted_points.Count < level_per_num_bottles)
        {
            int cnt_pt_check = 0;
            bool add_point_flag = false;
            var i = extracted_points.Count;
            string answer_color = answer_seq_color[i].Split("_")[1];

            int rand_idx = MakeRandomNumbers(0, spreaded_points.Count)[0];
            for(int j = 0; j < extracted_points.Count; j++)
            {
                if (Vector3.Distance(spreaded_points[rand_idx], extracted_points[j]) > 0.5f
                    && Vector3.Distance(spreaded_points[rand_idx], extracted_points[j]) < 1f)
                {
                    cnt_pt_check++;
                }

                if(cnt_pt_check == extracted_points.Count)
                {
                    add_point_flag = true;
                }
            }

            if (i == 0) add_point_flag = true;
            if(add_point_flag)
            {
                bottle_3d[color_to_num[answer_color] - 1].transform.position
                        = spreaded_points[rand_idx] + 0.2f * Vector3.up;
                extracted_points.Add(spreaded_points[rand_idx]);
                bottle_3d[color_to_num[answer_color] - 1].SetActive(true);
                Debug.Log(answer_color + " 위치: " + bottle_3d[color_to_num[answer_color] - 1].transform.position);
            }
        }

        Debug.Log("extractedpoints.Count: " + extracted_points.Count);
        Debug.Log(Vector3.Distance(extracted_points[0], extracted_points[1])
                  + "\t" + Vector3.Distance(extracted_points[0], extracted_points[2])
                  + "\t" + Vector3.Distance(extracted_points[1], extracted_points[2]));
        string answer_c = answer_seq_color[0].Split("_")[1];
        arrow_3d.transform.position = bottle_3d[color_to_num[answer_c] - 1].transform.position + 0.2f * Vector3.up;
        //세 개의 포인트를 선택한다
        // 세개의 포인트 선정 기: y값(높낮이)가 (-0.5, 0)인 포인트들로만 선택한다
        // 화면상의 여러 부분에 ray를 쏘아서 포인트들을 막 저장한다 -> game_mode script의 extract_point와 비슷하게 구현하면 될 듯

        //세 개의 포인트 보다 살짝 위 쪽에 게임오브젝트(물병)을 위치시킨다.
    }

    public void check_the_answer(string Name)
    {
        if (time_remain > 0) return;

        //GameObject clickedobj = EventSystem.current.currentSelectedGameObject;
        GameObject clickedobj = GameObject.Find(Name + "_prefab");
        string clicked_bt_name = Name.Split("_")[1];

        if (level == 1)
        {

            string sprite_name = answer_seq_color[cnt_answer].Split("_")[1];
            if (clicked_bt_name == sprite_name)
            {
                bath_next_bt_clicked();
                cnt_answer += 1;
                time_remain_text.text = "정답입니다!";
                clickedobj.SetActive(false);
                bottles[cnt_answer].sprite = bottles_sprite[color_to_num[clicked_bt_name]];//정답을 맞추면 원래 색깔대로 바뀐다
                Invoke("clear_text", 2f);
                if (cnt_answer == 3)
                {
                    time_remain_text.text = "축하드립니다 모두 맞추셨습니다!";
                    petctrl_script.shower_effect_true();
                    bgm_player_.getitem_sound_excute();
                    //petctrl_script.pet_reaction_true();
                    Invoke("re_init", 10f);
                }
                else
                {
                    bgm_player_.success_sound_excute();
                }
            }
            else
            {
                //Debug.Log("check111");
                time_remain_text.text = "다시 골라볼까요?";
                petctrl_script.pet_reaction_false();
                bgm_player_.fail_sound_excute();
                Invoke("clear_text", 2f);
            }
        }
    }

    public void re_init()
    {
        c3_flag = false;
        time_remain_text.text = "";
        petctrl_script.not_move_pet = false;
        content3_panel.SetActive(false);
        petctrl_script.not_move_pet = false;
        cnt_answer = 0;
        //bt_face.SetActive(true);
        bt_picture.SetActive(true);
        bt_set.SetActive(true);
        tutorial_random_play_script.time_remain = tutorial_random_play_script.time_max;
        tutorial_random_play_script.game_start_flag = true;
        tutorial_random_play_script.time_remain_text_wBG.SetActive(true);

        for (int i = 0; i < 4; i++)
        {
            bottle_3d[i].SetActive(false);
        }
        tutorial_panel.SetActive(false);

        extracted_points.Clear();
        spreaded_points.Clear();
        cnt_next_bath_bt_clicked = 0;

    }


    //public static int[] MakeRandomNumbers(int maxValue, int randomSeed = 0)
    //{
    //    return MakeRandomNumbers(0, maxValue, randomSeed);
    //}
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

    void clear_text()
    {
        time_remain_text.text = "";
    }
}