using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class Contents1 : MonoBehaviour
{
    public GameObject contents1_panel;
    public bool c1_flag;
    public bool c1_ongoing;
    float prev_angle;
    float prev_angle_frame;//속도 계산용
    public List<GameObject> foods = new List<GameObject>();
    List<GameObject> food_selected = new List<GameObject>();
    float[] speed_mv_avg = new float[15];
    int cnt_moving_average;
    public GameObject net;
    Petctrl petctrl_script;
    Logger logger_script;
    RaycastHit hit;
    Vector2 Center_device;
    int cnt_corr;
    float time_remain;
    public TMP_Text time_remain_text;
    //public GameObject bt_face;
    public GameObject bt_picture;
    public GameObject bt_set;
    bgm_player bgm_player_;
    Player_statu player;
    random_play random_play_script;
    int min_statu;

    int[] shuffle_idx_mixed;
    int[] shuffle_idx_org;

    int cnt_false;
    bool success_flag;
    int[] level_per_corr = new int[] { 0, 3, 4, 5 };
    int level;
    // Start is called before the first frame update
    void Start()
    {
        contents1_panel.transform.position = new Vector3(1194, 834, 0);
        Debug.Log(contents1_panel.transform.position);

        petctrl_script = GameObject.Find("Scripts").GetComponent<Petctrl>();
        logger_script = GameObject.Find("Scripts").GetComponent<Logger>();
        bgm_player_ = GameObject.Find("Audio player").GetComponent<bgm_player>();
        player = GameObject.Find("player_statu").GetComponent<Player_statu>();
        random_play_script = GameObject.Find("Scripts").GetComponent<random_play>();
        level = player.Level_hungry;
        //level = 1;
        time_remain = 0;
        cnt_corr = 0;
        Center_device = new Vector2(Screen.width / 2f, Screen.height / 2f);
        cnt_moving_average = 0;
        cnt_false = 0;
        prev_angle = 0f;
        prev_angle_frame = 0f;

        contents1_panel.SetActive(false);
        net.SetActive(false);
        min_statu = player.choose_higlight(); //enegry:0, fatigue:1, cleanliness:2, intimity:3
        bt_set.transform.GetChild(0).transform.position = bt_set.transform.GetChild(min_statu + 1).transform.position
                                                        + Vector3.left * 100;
        success_flag = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(time_remain > 0)
        {
            time_remain -= Time.deltaTime;
            time_remain_text.text = "음식의 순서를 기억해주세요!\n남은시간:" + ((int)time_remain % 60).ToString();
            if(time_remain <= 0)
            {
                time_remain_text.text = "";
            }
        }


        //if(c1_flag)
        //{
        //    var cur_angle = Camera.main.transform.eulerAngles.x;
        //    var catching_obj_pos = Camera.main.ScreenToWorldPoint(new Vector3(Center_device.x, Center_device.y, 0.2f));
        //    net.transform.position = catching_obj_pos;

        //    if (cur_angle > 0 && cur_angle < 90)
        //    {
        //        float cur_speed = Mathf.Abs(cur_angle - prev_angle_frame);
        //        float filtered_speed = filter_speed(cur_speed);

        //        //Debug.Log(Camera.main.transform.eulerAngles.x + "\t" + filtered_speed);
        //        if(filtered_speed < 2.5f)
        //        {
        //            //Debug.Log("가마니");
        //            return;
        //        }

        //        if (cur_angle - prev_angle < 0) // 들어 올릴 때
        //        {
        //            //net.transform.eulerAngles = new Vector3(net.transform.eulerAngles.x, net.transform.eulerAngles.y, 45f);
        //            var ray = Camera.main.ScreenPointToRay(Center_device);
        //            Physics.Raycast(ray, out hit, float.PositiveInfinity);
        //            if(hit.transform.CompareTag("food"))
        //            {
        //                Debug.Log(hit.transform.name);
        //                if(hit.transform.name == food_selected[cnt_corr].transform.name)
        //                {
        //                    petctrl_script.set_text_speechBubble("정답입니다!");
        //                    Invoke("clear_text", 2f);
        //                    food_selected[cnt_corr].SetActive(false);
        //                    cnt_corr += 1;
        //                    cnt_false = 0;

        //                    if(level == 1)
        //                    {
        //                        if (cnt_corr == 3)
        //                        {
        //                            //re-initialize
        //                            if(success_flag == true)
        //                            player.change_statu(0.02f,-0.02f, 0, 0.01f);
        //                            petctrl_script.set_text_speechBubble("음식을 모두\n다 골랐습니다!");
        //                            petctrl_script.heart_effect_true();
        //                            bgm_player_.getitem_sound_excute();
        //                            petctrl_script.pet_reaction_hungry_true();
        //                            choose_and_show_random_food();
        //                            net.SetActive(false);
        //                            Invoke("re_init", 10f);
        //                        }
        //                        else
        //                        {
        //                            bgm_player_.success_sound_excute();
        //                        }
        //                    }
        //                    else if(level == 2)
        //                    {
        //                        if (cnt_corr == 4)
        //                        {
        //                            //re-initialize
        //                            player.change_statu(0.04f, -0.04f, 0, 0.02f);
        //                            petctrl_script.set_text_speechBubble("음식을 모두\n다 골랐습니다!");
        //                            petctrl_script.heart_effect_true();
        //                            bgm_player_.getitem_sound_excute();
        //                            petctrl_script.pet_reaction_hungry_true();
        //                            choose_and_show_random_food();
        //                            net.SetActive(false);
        //                            Invoke("re_init", 10f);
        //                        }
        //                        else
        //                        {
        //                            bgm_player_.success_sound_excute();
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (cnt_corr == 5)
        //                        {
        //                            //re-initialize
        //                            player.change_statu(0.06f, -0.06f, 0, 0.03f);
        //                            petctrl_script.set_text_speechBubble("음식을 모두\n다 골랐습니다!");
        //                            petctrl_script.heart_effect_true();
        //                            bgm_player_.getitem_sound_excute();
        //                            petctrl_script.pet_reaction_hungry_true();
        //                            choose_and_show_random_food();
        //                            net.SetActive(false);
        //                            Invoke("re_init", 10f);
        //                        }
        //                        {
        //                            bgm_player_.success_sound_excute();
        //                        }
        //                    }
        //                }
        //                else//다른 음식을 고른 경우 
        //                {
        //                    //time_remain_text.text = "다른 음식을 골라주세요";
        //                    //Invoke("clear_text", 2f);
        //                    cnt_false++;
        //                    if(cnt_false == 5)
        //                    {
        //                        success_flag = false;
        //                        food_selected[cnt_corr].SetActive(false);
        //                        cnt_corr += 1;
        //                        if(cnt_corr == level_per_corr[level])
        //                        {
        //                            Invoke("re_init", 2f);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        petctrl_script.set_text_speechBubble("다시 골라볼까요?");
        //                        bgm_player_.fail_sound_excute();
        //                        petctrl_script.pet_reaction_false();
        //                    }
        //                }
        //            }
        //        }
        //        else// 
        //        {
        //            net.transform.eulerAngles = new Vector3(net.transform.eulerAngles.x, net.transform.eulerAngles.y, 15f);
        //        }
        //        prev_angle_frame = cur_angle;
        //        if (Mathf.Abs(cur_angle - prev_angle) > 5f)
        //        {
        //            prev_angle = cur_angle;
        //        }
        //    }
        //    //if (cur_acce - prev_acce < 0)
        //    //{
        //    //    tilting_fw = true;
        //    //    Debug.Log("숙이고 있는중");
        //    //}
        //    //else
        //    //{
        //    //    tilting_fw = false;
        //    //    Debug.Log("숙이기 반대");
        //    //}
        //    //if (Mathf.Abs(cur_acce - prev_acce) < 0.05f)
        //    //    prev_acce = cur_acce;
        //    //prev_acce_frame = cur_acce;
        //}

        if (c1_flag && Input.touchCount > 0)
        {
            //Debug.Log("logging");
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                //Debug.Log("asdf");
                var ray = Camera.main.ScreenPointToRay(touch.position);
                Physics.Raycast(ray, out var hit, float.PositiveInfinity);

                if (hit.transform.parent.transform.name == "foods")
                {
                    //Debug.Log(hit.transform.name);
                    check_the_answer(hit.transform.name);
                }
                else
                {

                }
            }
        }
    }

    public void choose_and_show_random_food()
    {
        GameObject random_food = Instantiate(foods[Random.Range(0, foods.Count)]);
        //random_food.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        random_food.transform.position = petctrl_script.spawnedObject.transform.GetChild(3).transform.position
                                        + 0.016f*Vector3.up;
        random_food.SetActive(true);
        Destroy(random_food.gameObject, 10f);

    }
        
    public void rewatch_bt_clicked()
    {
        logger_script.logger_master.insert_data("다시 보기 버튼 클릭");
        c1_flag = false;
        contents1_panel.SetActive(false);
        //처음 정답 화면 보여주기
        for (int i = 0; i < level+2; i++)
        {
            //randomly choose food from foods gameobject
            if (level == 1)
            {
                food_selected[i].transform.position = Camera.main.transform.position
                                                + 0.7f * Camera.main.transform.forward
                                                + (-0.2f + i * 0.2f) * Vector3.right;
                food_selected[i].SetActive(true);
            }
            if (level == 2)
            {
                food_selected[i].transform.position = Camera.main.transform.position
                                   + 0.7f * Camera.main.transform.forward
                                   + (-0.3f + i * 0.2f) * Vector3.right;
                food_selected[i].SetActive(true);
            }
            if (level == 3)
            {
                food_selected[i].transform.position = Camera.main.transform.position
                                  + 0.7f * Camera.main.transform.forward
                                  + (-0.4f + i * 0.2f) * Vector3.right;
                food_selected[i].SetActive(true);
            }
        }

        time_remain = 10f;
        Invoke("reshow", 10f);

    }

    void reshow()
    {
        //현재 화면에 나와있는 음식들 없애기
        for (int i = 0; i < level + 2; i++)
        {
            food_selected[i].SetActive(false);
        }


        //다시 원래 화면 보여주기
        for (int j = 0; j < food_selected.Count; j++)
        {
            if (level == 1)
            {
                food_selected[shuffle_idx_mixed[j]].transform.position = Camera.main.transform.position
                                                                + 0.7f * Camera.main.transform.forward
                                                                + Vector3.right * (-0.2f + j * 0.2f);
                Debug.Log($"j:{j} \t cnt_corr:{cnt_corr}");
                if (j >= cnt_corr) food_selected[j].SetActive(true);
            }
            if (level == 2)
            {
                food_selected[shuffle_idx_mixed[j]].transform.position = Camera.main.transform.position
                                                                + 0.7f * Camera.main.transform.forward
                                                                + Vector3.right * (-0.3f + j * 0.2f);
                if (j >= cnt_corr) food_selected[j].SetActive(true);
            }
            if (level == 3)
            {
                food_selected[shuffle_idx_mixed[j]].transform.position = Camera.main.transform.position
                                                                + 0.7f * Camera.main.transform.forward
                                                                + Vector3.right * (-0.4f + j * 0.2f);
                if (j >= cnt_corr) food_selected[j].SetActive(true);
            }
        }
        c1_flag = true;
        contents1_panel.SetActive(true);
    }

    public void hungry_bt_click()
    {
        logger_script.logger_master.insert_data("음식 맞추기 게임 시작");
        c1_ongoing = true;
        int num_food;
        num_food = level + 2; //1단계면 3개, 2단계면 4개, 3단계면 5개
        //bt_face.SetActive(false);
        bt_picture.SetActive(false);
        bt_set.SetActive(false);
        petctrl_script.not_move_pet = true;

        var shuffle_idx_org = MakeRandomNumbers(foods.Count); ////food_selected
        for (int i = 0; i < num_food;/*레벨에 따라 3또는4또는5*/ i++)
        {
            //randomly choose food from foods gameobject
            food_selected.Add(foods[shuffle_idx_org[i]]);
            Debug.Log($"level:{level}\t"+food_selected[i].name);
            if(level == 1)
            {
                food_selected[i].transform.position = Camera.main.transform.position
                                                + 0.7f * Camera.main.transform.forward
                                                + (-0.2f + i * 0.2f) * Vector3.right;
                food_selected[i].SetActive(true);
            }
            if(level == 2)
            {
                food_selected[i].transform.position = Camera.main.transform.position
                                   + 0.7f * Camera.main.transform.forward
                                   + (-0.3f + i * 0.2f) * Vector3.right;
                food_selected[i].SetActive(true);
            }
            if(level == 3)
            {
                food_selected[i].transform.position = Camera.main.transform.position
                                  + 0.7f * Camera.main.transform.forward
                                  + (-0.4f + i * 0.2f) * Vector3.right;
                food_selected[i].SetActive(true);
            }
        }
        Invoke("change_to_shuffled", 10f);
        time_remain = 10f;
        //foods[0].transform.position = Camera.main.transform.position + 0.3f*Vector3.forward;

        //bowl will be located infront of pet, attached to the ground
        //
        //bowl.transform.position = petctrl_script.spawnedObject.transform.position
        //                          + petctrl_script.spawnedObject.transform.forward*0.1f;
        //Debug.Log(petctrl_script.spawnedObject.transform.position + "\t"
        //          + petctrl_script.spawnedObject.transform.forward
        //          + "\t" + bowl.transform.position);

        //pop-up panel create(3d)
        //pop-up panel will be located in front of camera
        //Lv.1: 3 foods, Lv.2: 4foods, Lv3: 5foods.
        //firstly, the sequence of foods should be saved.
    }

    //showing mode, selecting mode 등으로 구분 필요


    void change_to_shuffled()
    {
        //if (net.activeSelf == false) net.SetActive(true);
        //for (int i = 0; i < food_selected.Count;)
        //{
        //    food_selected[i].SetActive(false);
        //}
        shuffle_idx_mixed = MakeRandomNumbers(food_selected.Count); //food_selected_shuffled

        for (int j = 0; j < food_selected.Count; j++)
        {
            if(level == 1)
            {
                food_selected[shuffle_idx_mixed[j]].transform.position = Camera.main.transform.position
                                                                + 0.7f * Camera.main.transform.forward
                                                                + Vector3.right * (-0.2f + j * 0.2f);
            }
            if (level == 2)
            {
                food_selected[shuffle_idx_mixed[j]].transform.position = Camera.main.transform.position
                                                                + 0.7f * Camera.main.transform.forward
                                                                + Vector3.right * (-0.3f + j * 0.2f);
            }
            if (level == 3)
            {
                food_selected[shuffle_idx_mixed[j]].transform.position = Camera.main.transform.position
                                                                + 0.7f * Camera.main.transform.forward
                                                                + Vector3.right * (-0.4f + j * 0.2f);
            }

        }
        c1_flag = true;
        contents1_panel.SetActive(true);

    }

    public void re_init()
    {
        logger_script.logger_master.insert_data("음식 순서 맞추기 게임 종료");
        //bt_face.SetActive(true);
        time_remain_text.text = "";
        bt_picture.SetActive(true);
        bt_set.SetActive(true);
        contents1_panel.SetActive(false);
        prev_angle = 0f;
        cnt_moving_average = 0;
        petctrl_script.not_move_pet = false;
        food_selected.Clear();
        c1_flag = false;
        cnt_corr = 0;
        cnt_false = 0;
        success_flag = true;
        random_play_script.time_remain = random_play_script.time_max;
        random_play_script.game_start_flag = true;
        random_play_script.time_remain_text_wBG.SetActive(true);

        for (int i=0; i<foods.Count; i++)
        {
            foods[i].SetActive(false);
        }

        min_statu = player.choose_higlight(); //enegry:0, fatigue:1, cleanliness:2, intimity:3
        bt_set.transform.GetChild(0).transform.position = bt_set.transform.GetChild(min_statu + 1).transform.position
                                                        + Vector3.left * 100;
        c1_ongoing = false;

    }

    public void check_the_answer(string Name)
    {
        if (time_remain > 0) return;

        //GameObject clickedobj = EventSystem.current.currentSelectedGameObject;
        GameObject clickedobj = GameObject.Find(Name);
        string clicked_foods_name = Name;

        if (level == 1)
        {
            string answer_food_name = food_selected[cnt_corr].name;
            if (clicked_foods_name == answer_food_name)
            {
                cnt_corr += 1;
                cnt_false = 0;
                clickedobj.SetActive(false);
                if (cnt_corr == 3)
                {
                    if(success_flag)
                    {
                        logger_script.logger_master.insert_data("음식 순서 맞추기 게임 성공");
                        player.change_statu(0.02f, -0.02f, 0, 0.01f);
                        PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin")+3);
                        time_remain_text.text = "축하드립니다 모두 맞추셨습니다!";
                        bgm_player_.getitem_sound_excute();
                        petctrl_script.set_text_speechBubble("음식을 모두\n다 골랐습니다!");
                        petctrl_script.pet_reaction_hungry_true();
                        choose_and_show_random_food();
                        Invoke("re_init", 10f);
                    }
                    else
                    {
                        petctrl_script.set_text_speechBubble("다음에 다시\n도전해볼까요?");
                        Invoke("re_init", 5f);
                    }
                }
                else
                {
                    if(success_flag)
                    {
                        logger_script.logger_master.insert_data($"맞춘 음식 갯수:{cnt_corr}/3");
                        petctrl_script.set_text_speechBubble("잘 맞추셨어요!");
                        bgm_player_.success_sound_excute();
                    }
                }
            }
            else
            {
                //Debug.Log("check111");
                cnt_false++;
                if (cnt_false == 5)
                {
                    logger_script.logger_master.insert_data("음식 순서 맞추기 게임 실패");
                    petctrl_script.set_text_speechBubble("다음 음식을\n골라볼까요?");
                    cnt_false = 0;
                    success_flag = false;
                    food_selected[cnt_corr].SetActive(false);
                    cnt_corr += 1;
                    if (cnt_corr == level_per_corr[level])
                    {
                        Invoke("re_init", 1f);
                    }
                }
                else
                {
                    logger_script.logger_master.insert_data($"음식 순서 맞추기 오답 횟수: {cnt_false}");
                    petctrl_script.set_text_speechBubble("다시 골라볼까요?");
                    bgm_player_.fail_sound_excute();
                    petctrl_script.pet_reaction_false();
                    Invoke("clear_text", 1f);
                }
            }
        }
        else if (level == 2)
        {
            string answer_food_name = food_selected[cnt_corr].name;
            if (clicked_foods_name == answer_food_name)
            {
                cnt_corr += 1;
                cnt_false = 0;
                clickedobj.SetActive(false);
                if (cnt_corr == 4)
                {
                    if (success_flag)
                    {
                        logger_script.logger_master.insert_data("음식 순서 맞추기 게임 성공");
                        player.change_statu(0.04f, -0.04f, 0, 0.02f);
                        PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") + 4);
                        time_remain_text.text = "축하드립니다 모두 맞추셨습니다!";
                        bgm_player_.getitem_sound_excute();
                        petctrl_script.set_text_speechBubble("음식을 모두\n다 골랐습니다!");
                        petctrl_script.pet_reaction_hungry_true();
                        choose_and_show_random_food();
                        Invoke("re_init", 10f);
                    }
                    else
                    {
                        petctrl_script.set_text_speechBubble("다음에 다시\n도전해볼까요?");
                        Invoke("re_init", 5f);
                    }
                }
                else
                {
                    if (success_flag)
                    {
                        logger_script.logger_master.insert_data($"맞춘 음식 갯수:{cnt_corr}/4");
                        petctrl_script.set_text_speechBubble("잘 맞추셨어요!");
                        bgm_player_.success_sound_excute();
                    }
                }
            }
            else
            {
                //Debug.Log("check111");
                cnt_false++;
                if (cnt_false == 5)
                {
                    logger_script.logger_master.insert_data("음식 순서 맞추기 게임 실패");
                    petctrl_script.set_text_speechBubble("다음 음식을\n골라볼까요?");
                    cnt_false = 0;
                    success_flag = false;
                    food_selected[cnt_corr].SetActive(false);
                    cnt_corr += 1;
                    if (cnt_corr == level_per_corr[level])
                    {
                        Invoke("re_init", 1f);
                    }
                }
                else
                {
                    logger_script.logger_master.insert_data($"음식 순서 맞추기 오답 횟수: {cnt_false}");
                    petctrl_script.set_text_speechBubble("다시 골라볼까요?");
                    bgm_player_.fail_sound_excute();
                    petctrl_script.pet_reaction_false();
                    Invoke("clear_text", 1f);
                }
            }
        }
        else
        {
            string answer_food_name = food_selected[cnt_corr].name;
            if (clicked_foods_name == answer_food_name)
            {
                cnt_corr += 1;
                cnt_false = 0;
                clickedobj.SetActive(false);
                if (cnt_corr == 5)
                {
                    if (success_flag)
                    {
                        player.change_statu(0.06f, -0.06f, 0, 0.03f);
                        PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") + 5);
                        time_remain_text.text = "축하드립니다 모두 맞추셨습니다!";
                        bgm_player_.getitem_sound_excute();
                        petctrl_script.set_text_speechBubble("음식을 모두\n다 골랐습니다!");
                        petctrl_script.pet_reaction_hungry_true();
                        choose_and_show_random_food();
                        Invoke("re_init", 10f);
                    }
                    else
                    {
                        petctrl_script.set_text_speechBubble("다음에 다시\n도전해볼까요?");
                        Invoke("re_init", 5f);
                    }
                }
                else
                {
                    if (success_flag)
                    {
                        petctrl_script.set_text_speechBubble("잘 맞추셨어요!");
                        bgm_player_.success_sound_excute();
                    }
                }
            }
            else
            {
                //Debug.Log("check111");
                cnt_false++;
                if (cnt_false == 5)
                {
                    petctrl_script.set_text_speechBubble("다음 음식을\n골라볼까요?");
                    cnt_false = 0;
                    success_flag = false;
                    food_selected[cnt_corr].SetActive(false);
                    cnt_corr += 1;
                    if (cnt_corr == level_per_corr[level])
                    {
                        Invoke("re_init", 1f);
                    }
                }
                else
                {
                    petctrl_script.set_text_speechBubble("다시 골라볼까요?");
                    bgm_player_.fail_sound_excute();
                    petctrl_script.pet_reaction_false();
                    Invoke("clear_text", 1f);
                }
            }
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

    private float filter_speed(float cur_speed)
    {
        float filtered_speed;
        if (cnt_moving_average < speed_mv_avg.Length)
        {
            filtered_speed = 0;
            speed_mv_avg[cnt_moving_average] = cur_speed;
            cnt_moving_average += 1;
            for (int i = 0; i < cnt_moving_average; i++)
            {
                filtered_speed += speed_mv_avg[i];
            }
            filtered_speed /= cnt_moving_average;
        }
        else
        {
            speed_mv_avg = speed_mv_avg.Skip(1).ToArray(); // 맨 앞 삭제
            speed_mv_avg = speed_mv_avg.Append(cur_speed).ToArray(); // 맨 끝 추가

            filtered_speed = 0;
            for (int i = 0; i < speed_mv_avg.Length; i++)
            {
                filtered_speed += speed_mv_avg[i];
            }
            filtered_speed /= speed_mv_avg.Length;

        }

        return filtered_speed;
    }
    void clear_text()
    {
        time_remain_text.text = "";
    }
}
