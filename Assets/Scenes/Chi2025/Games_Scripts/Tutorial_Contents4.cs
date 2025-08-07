using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Video;
using UnityEngine.UI;

public class Tutorial_Contents4 : MonoBehaviour
{
    Rigidbody copyed_ball;
    public GameObject content4_panel;
    public GameObject copyed_ball_showup;
    public Rigidbody ball;
    private float lastTouchTime;
    private const float doubleTouchDelay = 0.5f;
    private Animator anim;
    Petctrl petctrl_script;
    public bool c4_flag;
    int touch_cnt;
    int touch_cnt_pet;

    //public GameObject bt_face;
    public GameObject bt_picture ;
    public GameObject bt_set;
    Rigidbody rgbody;
    Vector3 goal_position;
    Vector3 org_position;
    bgm_player bgm_player_;
    Player_statu player;
    Logger logger_script;
    tutorial_random_play tutorial_random_play_script;
    public VideoPlayer video;
    public RawImage vid_screen;

    bool track_flag;
    bool return_to_org;

    public int cnt_next_bt_clicked;
    public GameObject tutorial_panel;
    public GameObject tutorial_bt;
    public TMP_Text tutorial_msg;

    public TextMeshProUGUI time_text;
    float time;
    bool execute_next_bt;

    // Start is called before the first frame update
    void Start()
    {
        content4_panel.SetActive(false);
        copyed_ball_showup.SetActive(false);
        goal_position = Vector3.zero;
        touch_cnt = 0;
        touch_cnt_pet = 0;
        lastTouchTime = Time.time;
        petctrl_script = GameObject.Find("Scripts_tutorial").GetComponent<Petctrl>();
        bgm_player_ = GameObject.Find("Audio player").GetComponent<bgm_player>();
        player = GameObject.Find("player_statu").GetComponent<Player_statu>();
        tutorial_random_play_script = GameObject.Find("Scripts_tutorial").GetComponent<tutorial_random_play>();

        //Debug.Log("Ball name: " + ball.transform.name);

        time = 15;
        cnt_next_bt_clicked = 0;
        tutorial_panel.SetActive(false);
        tutorial_bt.SetActive(false);
        vid_screen.enabled = false;

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
                if (cnt_next_bt_clicked == 1)
                {
                    Debug.Log("logging:\tcnt_next_bt_clicked: " + cnt_next_bt_clicked);
                    execute_next_bt = false;
                    intimity_next_bt_clicked();
                }
            }
        }

        if (!c4_flag) return;
        if (content4_panel.activeSelf == false && touch_cnt == 0)
        {
            copyed_ball_showup.transform.position =
                Camera.main.transform.position + Camera.main.transform.forward * 0.4f;
        }


        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            var ray = Camera.main.ScreenPointToRay(touch.position);
            Physics.Raycast(ray, out var hit, float.PositiveInfinity);
            anim = petctrl_script.spawnedObject.transform.GetChild(0).GetComponent<Animator>();

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touch_cnt = 0;
                    touch_cnt_pet = 0;

                    Debug.Log("Touch begin");
                    if (Time.time - lastTouchTime < doubleTouchDelay) // 더블터치 판정
                    {
                        if (hit.transform.name.StartsWith("Pome"))
                        {
                            anim.Play("002_Ball_Jump");
                            bgm_player_.jump_sound_excute();
                            intimity_next_bt_clicked();
                        }
                    }
                    break;

                case TouchPhase.Moved:
                    //Debug.Log("Touch moving " + hit.transform.name + "\t" + touch_cnt_pet);
                    if (hit.transform.name.StartsWith("Pome"))
                    {
                        ++touch_cnt_pet;
                        if (touch_cnt_pet > 50 && !anim.GetCurrentAnimatorStateInfo(0).IsName("067_Idle_Blend_LieOnBack"))
                        {
                            anim.Play("067_Idle_Blend_LieOnBack");
                            petctrl_script.heart_effect_true();
                            intimity_next_bt_clicked();
                        }

                        //Invoke("Set_Pome_Idle", 4f);
                    }
                    else
                    {
                        ++touch_cnt;
                        if (content4_panel.activeSelf == false)
                        {
                            copyed_ball_showup.transform.position =
                                Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0.4f));
                        }
                    }
                    
                    break;

                case TouchPhase.Ended:
                    Debug.Log("Touch End");
                    if (touch_cnt > 10)
                    {
                        //if (player.intimity < 0.3f)
                        //{
                        //    petctrl_script.set_text_speechBubble("친밀도를 채운 후 \n공놀이를 하고싶어요");
                        //    return;
                        //}
                        touch_cnt = 0;
                        if (copyed_ball == null && copyed_ball_showup.activeSelf == true)
                        {
                            copyed_ball = Instantiate(ball, ray.origin, Quaternion.identity);
                            var rigidbody = copyed_ball.GetComponent<Rigidbody>();
                            rigidbody.linearVelocity = ray.direction * 3;
                            Invoke("set_ball_velocity_0", 5f);
                            intimity_next_bt_clicked();
                            copyed_ball_showup.SetActive(false);
                            content4_panel.SetActive(false);
                            //Invoke("set_ball_active_false", 15f);
                        }
                        //else
                        //{
                        //    if (copyed_ball.activeSelf == false)
                        //    {
                        //        copyed_ball.SetActive(true);
                        //        copyed_ball.transform.position = ray.origin;
                        //        rgbody = copyed_ball.GetComponent<Rigidbody>();
                        //        rgbody.velocity = ray.direction * 10f;
                        //        //Invoke("set_ball_active_false", 15f);
                        //    }
                        //}

                    }
                    lastTouchTime = Time.time;
                    break;
            }
        }

        //if(copyed_ball != null && copyed_ball.activeSelf == true)
        //{
        //    ` = copyed_ball.GetComponent<Rigidbody>();
        //    Debug.Log(rgbody.velocity + copyed_ball.transform.position + "\t" + goal_position);
        //}

        if (goal_position != Vector3.zero)
            move_to_point(goal_position);
    }

    protected IEnumerator Preparevid()
    {
        video.Prepare();

        while (!video.isPrepared)
        {

            yield return new WaitForSeconds(0.5f);
        }


        vid_screen.enabled = true;
        vid_screen.texture = video.texture;
        video.Play();

    }

    void null_video_screen()
    {
        vid_screen.texture = null;
        vid_screen.enabled = false;
    }

    public void intimity_next_bt_clicked()
    {
        if(cnt_next_bt_clicked == 0)
        {
            bt_picture.SetActive(false);
            bt_set.SetActive(false);
            petctrl_script.not_move_pet = true;
            tutorial_panel.SetActive(true);
            tutorial_bt.SetActive(true);
            tutorial_msg.text = "강아지와 놀아볼까요?";
            cnt_next_bt_clicked++;
            if (time_text.gameObject.activeSelf != true) time_text.gameObject.SetActive(true);
            time = 0;
            execute_next_bt = true;
            logger_script.logger_master.insert_data("연습하기 - 놀아주기게임 연습 시작.");
        }
        else if(cnt_next_bt_clicked == 1)
        {
            tutorial_bt.SetActive(false);
            play_bt_clicked();
            tutorial_msg.text = "강아지를 쓰다듬어주세요!\n강아지가 좋아할거에요!";
            cnt_next_bt_clicked++;
            time_text.gameObject.SetActive(false);
            video.url = Application.streamingAssetsPath + "/" + "int_1.mp4";
            if (vid_screen != null && video != null)
            {
                Debug.Log("prepard_vid 실행");
                StartCoroutine(Preparevid());
            }
            logger_script.logger_master.insert_data("연습하기 - 놀아주기게임 강아지 쓰다듬기 시작");
        }
        else if (cnt_next_bt_clicked == 2)
        {
            tutorial_msg.text = "강아지가 이렇게나 좋아하네요!";
            cnt_next_bt_clicked++;
            Invoke("intimity_next_bt_clicked", 7f);
            null_video_screen();
            logger_script.logger_master.insert_data("연습하기 - 놀아주기게임 강아지 쓰다듬기 완료");
        }
        else if (cnt_next_bt_clicked == 3)
        {
            tutorial_msg.text = "강이지를 빠르게 두번 터치해주세요!";
            cnt_next_bt_clicked++;
            video.url = Application.streamingAssetsPath + "/" + "int_2.mp4";
            if (vid_screen != null && video != null)
            {
                Debug.Log("prepard_vid 실행");
                StartCoroutine(Preparevid());
            }
            logger_script.logger_master.insert_data("연습하기 - 놀아주기게임 강아지 점프하기 시작");
        }
        else if (cnt_next_bt_clicked == 4)
        {
            tutorial_msg.text = "강아지를 빠르게 두번 터치하면\n제자리에서 점프를 하네요!";
            cnt_next_bt_clicked++;
            Invoke("intimity_next_bt_clicked", 5f);
            null_video_screen();
            logger_script.logger_master.insert_data("연습하기 - 놀아주기게임 강아지 점프하기 완료");
        }
        else if (cnt_next_bt_clicked == 5)
        {
            content4_panel.SetActive(true);
            tutorial_msg.text = "공놀이를 해볼까요?\n공던지고 놀기 버튼을 눌러주세요!";
            cnt_next_bt_clicked++;
            logger_script.logger_master.insert_data("연습하기 - 놀아주기게임 강아지 공놀이 시작");
        }

        else if (cnt_next_bt_clicked == 6)
        {
            tutorial_msg.text = "공을 한번 던져볼까요?\n공을 던지고 싶은 곳에 끌어다 놓아주세요!";
            cnt_next_bt_clicked++;
            video.url = Application.streamingAssetsPath + "/" + "int_3.mp4";
            if (vid_screen != null && video != null)
            {
                Debug.Log("prepard_vid 실행");
                StartCoroutine(Preparevid());
            }
            logger_script.logger_master.insert_data("연습하기 - 놀아주기게임 강아지 공놀이- 공 던지기");
        }
        else if (cnt_next_bt_clicked == 7)
        {
            tutorial_msg.text = "강아지가 공을 주우러 갈거에요!";
            Invoke("intimity_next_bt_clicked", 5f);
            cnt_next_bt_clicked++;
            null_video_screen();
            logger_script.logger_master.insert_data("연습하기 - 놀아주기게임 강아지 공놀이- 공 주워오기 시작");
        }
        else if (cnt_next_bt_clicked == 8)
        {
            tutorial_msg.text = "강아지가 공을 주우러 가네요!";
            Invoke("intimity_next_bt_clicked", 7.5f);
            cnt_next_bt_clicked++;
            logger_script.logger_master.insert_data("연습하기 - 놀아주기게임 강아지 공놀이- 공 주워오기 완료");
        }
        else if (cnt_next_bt_clicked == 9)
        {
            tutorial_bt.SetActive(false);
            tutorial_msg.text = "강아지가 공을 주워왔어요!";
            Invoke("re_init", 10f);
        }
        logger_script.logger_master.insert_data("연습하기 - 놀아주기게임 연습 종료.");
    }

    void set_ball_velocity_0()
    {
        if (copyed_ball.transform.position.y < -5f)
        {
            Destroy(copyed_ball.gameObject);
            copyed_ball = null;
            return;
        }

        Debug.Log("set_ball_velocity_0 실행");
        if (copyed_ball != null && copyed_ball.gameObject.activeSelf == true)
        {
            rgbody = copyed_ball.GetComponent<Rigidbody>();
            rgbody.isKinematic = true;
            rgbody.useGravity = false;
            rgbody.linearVelocity = Vector3.zero;
            goal_position = copyed_ball.transform.position;
            if (copyed_ball.transform.position.y > -5f)
            {
                set_to_kinematic();
                track_flag = true;
                return_to_org = true;
                org_position = petctrl_script.spawnedObject.transform.position;
            }
        }
        anim = petctrl_script.spawnedObject.transform.GetChild(0).GetComponent<Animator>();
        anim.Play("Walk");
    }

    //void set_ball_active_false()
    //{
    //    if(copyed_ball != null && copyed_ball.activeSelf == true)
    //    {
    //        copyed_ball.SetActive(false);
    //    }

    //}

    public void ballplay_bt_clicked()
    {
        content4_panel.SetActive(false);
        copyed_ball_showup.SetActive(true);

    }
    public void play_bt_clicked()
    {
        //copyed_ball_showup.SetActive(true);
        //bt_picture.SetActive(false);
        //bt_set.SetActive(false);

        petctrl_script.not_move_pet = true;
        c4_flag = true;
    }

    public void re_init()
    {
        if (copyed_ball != null)
        {
            Destroy(copyed_ball);
            copyed_ball = null;
        }
        track_flag = false;
        copyed_ball_showup.SetActive(false);
        bt_picture.SetActive(true);
        bt_set.SetActive(true);
        tutorial_panel.SetActive(false);

        petctrl_script.not_move_pet = false;
        return_to_org = false;
        c4_flag = false;
        tutorial_random_play_script.time_remain = tutorial_random_play_script.time_max;
        tutorial_random_play_script.game_start_flag = true;
        tutorial_random_play_script.time_remain_text_wBG.SetActive(true);



        Vector3 dir = Camera.main.transform.position - petctrl_script.spawnedObject.transform.position;
        dir.y = 0;
        Quaternion rot = Quaternion.LookRotation(dir.normalized);
        petctrl_script.spawnedObject.transform.rotation = rot;
        petctrl_script.spawnedObject.transform.GetChild(0).transform.rotation = rot;
        petctrl_script.spawnedObject.transform.GetChild(0).transform.localPosition = Vector3.zero;

        cnt_next_bt_clicked = 0;
    }

    void back_to_org()
    {
        goal_position = org_position;
        track_flag = true;
        anim = petctrl_script.spawnedObject.transform.GetChild(0).GetComponent<Animator>();
        anim.Play("Walk");

    }

    public void move_to_point(Vector3 hitpoint)
    {
        if (track_flag)
        {
            if (Vector2.Distance(new Vector2(petctrl_script.spawnedObject.transform.position.x,
                                             petctrl_script.spawnedObject.transform.position.z),
                                     new Vector2(hitpoint.x, hitpoint.z)) > 0.2f)
            {
                //Debug.Log(Vector3.Distance(spawnedObject.transform.position, hitpoint));
                //spawnedObject.transform.position = Vector3.MoveTowards(spawnedObject.transform.position,
                //													  hitpoint /*- 0.5f * Vector3.up*/,
                //													  0.01f);

                Vector3 dir = hitpoint - petctrl_script.spawnedObject.transform.position;
                dir.y = 0f;
                Quaternion rot = Quaternion.LookRotation(dir.normalized);


                petctrl_script.spawnedObject.transform.position = Vector3.MoveTowards(petctrl_script.spawnedObject.transform.position,
                                                                      hitpoint,
                                                                      0.01f);
                petctrl_script.spawnedObject.transform.rotation = rot;
                petctrl_script.spawnedObject.transform.GetChild(0).transform.rotation = rot;
                petctrl_script.spawnedObject.transform.GetChild(0).transform.localPosition = Vector3.zero;

                //Vector3 dir = hitpoint - spawnedObject.transform.position;
                //dir.y = 0f;
                //Quaternion rot = Quaternion.LookRotation(dir.normalized);
                //spawnedObject.transform.rotation = rot;

            }
            else
            {
                track_flag = false;

                if (return_to_org)
                {
                    //공을 줍는듯한 reaction
                    anim = petctrl_script.spawnedObject.transform.GetChild(0).GetComponent<Animator>();
                    anim.Play("310_Stroll_Take_half");

                    Invoke("show_ball_on_mouth", 1f);

                    //return_to_org 값을 false로
                    return_to_org = false;
                    //몇 초 후에 다시 돌아올 수 있도록 goal_position 값을 org_position으로 변경
                    Invoke("back_to_org", 3f);
                }
                else
                {
                    petctrl_script.spawnedObject.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.SetActive(false);
                    set_to_gravity();
                    Vector3 dir = Camera.main.transform.position - petctrl_script.spawnedObject.transform.position;
                    dir.y = 0f;
                    Quaternion rot = Quaternion.LookRotation(dir.normalized);
                    petctrl_script.spawnedObject.transform.rotation = rot;
                    petctrl_script.spawnedObject.transform.GetChild(0).transform.rotation = rot;
                    petctrl_script.spawnedObject.transform.GetChild(0).transform.localPosition = Vector3.zero;
                    petctrl_script.pet_reaction_true();
                    //re_init();
                }

                //set_to_gravity();
                //Vector3 dir = Camera.main.transform.position - petctrl_script.spawnedObject.transform.position;
                //dir.y = 0f;
                //Quaternion rot = Quaternion.LookRotation(dir.normalized);
                //petctrl_script.spawnedObject.transform.rotation = rot;
                //petctrl_script.spawnedObject.transform.GetChild(0).transform.rotation = rot;
                //petctrl_script.spawnedObject.transform.GetChild(0).transform.localPosition = Vector3.zero;
                //Debug.Log("Idle animation excute");
                //anim = petctrl_script.spawnedObject.transform.GetChild(0).GetComponent<Animator>();
                //anim.Play("Idle");
            }

        }
    }

    void show_ball_on_mouth()
    {
        Destroy(copyed_ball.gameObject);
        copyed_ball = null;
        petctrl_script.spawnedObject.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.SetActive(true);

    }


    private void set_to_kinematic()
    {
        Rigidbody rg_body = petctrl_script.spawnedObject.transform.GetChild(0).GetComponent<Rigidbody>();
        rg_body.isKinematic = true;
        rg_body.useGravity = false;
        //Debug.Log("Kinematic mode");
    }

    private void set_to_gravity()
    {
        Rigidbody rg_body = petctrl_script.spawnedObject.transform.GetChild(0).GetComponent<Rigidbody>();
        rg_body.useGravity = true;
        rg_body.isKinematic = false;
        //Debug.Log("Gravity mode");
    }
}
