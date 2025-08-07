using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contents4 : MonoBehaviour
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
    public bool c4_ongoing;
    int touch_cnt;
    int touch_cnt_pet;

    //public GameObject bt_face;
    public GameObject bt_picture;
    public GameObject bt_set;
    Rigidbody rgbody;
    Vector3 goal_position;
    Vector3 org_position;
    bgm_player bgm_player_;
    Player_statu player;
    Logger logger_script;
    random_play random_play_script;

    bool track_flag;
    bool return_to_org;

    // Start is called before the first frame update
    void Start()
    {
        content4_panel.transform.position = new Vector3(1194, 834, 0);

        content4_panel.SetActive(false);
        copyed_ball_showup.SetActive(false);
        goal_position = Vector3.zero;
        touch_cnt = 0;
        touch_cnt_pet = 0;
        lastTouchTime = Time.time;
        petctrl_script = GameObject.Find("Scripts").GetComponent<Petctrl>();
        logger_script = GameObject.Find("Scripts").GetComponent<Logger>();
        bgm_player_ = GameObject.Find("Audio player").GetComponent<bgm_player>();
        player = GameObject.Find("player_statu").GetComponent<Player_statu>();
        random_play_script = GameObject.Find("Scripts").GetComponent<random_play>();

        //Debug.Log("Ball name: " + ball.transform.name);
    }

    // Update is called once per frame
    void Update()
    {
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
                            logger_script.logger_master.insert_data("강아지 점프 애니메이션 실행");
                            anim.Play("002_Ball_Jump");
                            bgm_player_.jump_sound_excute();
                        }
                    }
                    break;

                case TouchPhase.Moved:
                    if (hit.transform.name.StartsWith("Pome"))
                    {
                        ++touch_cnt_pet;
                        Debug.Log(anim.GetCurrentAnimatorStateInfo(0));
                        if (touch_cnt_pet > 50 && anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                        {
                            logger_script.logger_master.insert_data("강아지 애교부리기 애니메이션 실행");
                            anim.Play("067_Idle_Blend_LieOnBack");
                            petctrl_script.heart_effect_true();
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
                            logger_script.logger_master.insert_data("강아지 놀아주기용 공 던지기 실행");
                            copyed_ball = Instantiate(ball, ray.origin, Quaternion.identity);
                            var rigidbody = copyed_ball.GetComponent<Rigidbody>();
                            rigidbody.linearVelocity = ray.direction * 3;
                            Invoke("set_ball_velocity_0", 5f);
                            copyed_ball_showup.SetActive(false);
                            content4_panel.SetActive(false);

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


    public void ballplay_bt_clicked()
    {
        logger_script.logger_master.insert_data("강아지 공던지기 버튼 클릭");
        content4_panel.SetActive(false);
        copyed_ball_showup.SetActive(true);

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

    public void play_bt_clicked()
    {
        logger_script.logger_master.insert_data("게임 시작. 놀아주기 게임 시작 버튼 클릭");
        c4_ongoing = true;
        content4_panel.SetActive(true);
        bt_picture.SetActive(false);
        bt_set.SetActive(false);

        petctrl_script.not_move_pet = true;
        c4_flag = true;
    }

    public void re_init()
    {
        logger_script.logger_master.insert_data("게임 종. 놀아주기 게임 종료 버튼 클릭");
        if (copyed_ball != null)
        {
            Destroy(copyed_ball);
            copyed_ball = null;
        }
        track_flag = false;
        copyed_ball_showup.SetActive(false);
        bt_picture.SetActive(true);
        bt_set.SetActive(true);

        petctrl_script.not_move_pet = false;
        return_to_org = false;
        c4_flag = false;
        random_play_script.time_remain = random_play_script.time_max;
        random_play_script.game_start_flag = true;
        random_play_script.time_remain_text_wBG.SetActive(true);

        int min_statu;
        min_statu = player.choose_higlight(); //enegry:0, fatigue:1, cleanliness:2, intimity:3
        bt_set.transform.GetChild(0).transform.position = bt_set.transform.GetChild(min_statu + 1).transform.position
                                                        + Vector3.left * 100;

        Vector3 dir = Camera.main.transform.position - petctrl_script.spawnedObject.transform.position;
        dir.y = 0;
        Quaternion rot = Quaternion.LookRotation(dir.normalized);
        petctrl_script.spawnedObject.transform.rotation = rot;
        petctrl_script.spawnedObject.transform.GetChild(0).transform.rotation = rot;
        petctrl_script.spawnedObject.transform.GetChild(0).transform.localPosition = Vector3.zero;

        c4_ongoing = false;

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
                    petctrl_script.set_text_speechBubble("공 가져오기에]\n성공했어요!");
                    player.change_statu(-0.01f, 0.03f, -0.04f, 0.03f);
                    PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") + 5);
                    re_init();
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
