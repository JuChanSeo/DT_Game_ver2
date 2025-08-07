using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class cylinderTouchHandler_tutorial : MonoBehaviour
{
    //GameObject Pet;
    Animator anim;
    SkinnedMeshRenderer face_renderer;

    private float time_remain;
    private bool start_flag;

    public GameObject gamestart_Button;
    //public Slider slider_time;
    public GameObject dirtys;

    public GameObject gameDonePanel;
    //public TMPro.TextMeshProUGUI text_last;

    //public GameObject fail_panel;
    public GameObject info_panel;

    public int c_0;
    public int c_1;
    public int c_2;
    public int c_3;
    public int c_4;
    public int c_5;
    public int c_6;
    public int c_7;
    public int c_8;
    public int c_9;
    public int c_10;

    int cnt_succes;
    int cnt_fail;
    float time_limit;
    int cnt_dirtys = 0;

    public ParticleSystem particlePrefab;

    public Text tutorial_msg;
    public GameObject tutorial_start_Bt;
    public GameObject tutorial_next_Bt;
    int tutorial_step;

    care_effect care_effect_script;
    bgm_player bgm_player_script;
    Logger logger_script;

    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.SetInt("Level_pet", 2);
        info_panel.SetActive(false);
        //fail_panel.SetActive(false);
        gameDonePanel.SetActive(false);
        //text_last.gameObject.SetActive(false);
        //Pet = GameObject.Find("pomeLV0" + PlayerPrefs.GetInt("Level_pet").ToString());
        //dirtys = Pet.transform.GetChild(7).gameObject;
        dirtys.SetActive(false);
        //var gba = GameObject.Find("SpikeBall").transform.GetChild(0);
        //gba.GetComponent<Renderer>().material.color = Random.ColorHSV();
        //anim = Pet.GetComponent<Animator>();
        //face_renderer = Pet.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
        time_limit = 10f;
        //set_difficulty();

        tutorial_step = 0;
        tutorial_next_Bt.SetActive(false);
        dirtys.SetActive(false);

        care_effect_script = GameObject.Find("care_effect_player").GetComponent<care_effect>();
        bgm_player_script = GameObject.Find("Audio player").GetComponent<bgm_player>();
        logger_script = GameObject.Find("logger_gb").GetComponent<Logger>();

        //bgm_player_script.excute_narration("46");
        StartCoroutine(bgm_player_script.excute_sound("46", 1f));
        logger_script.logger_master.insert_data("목욕하기 게임 튜토리얼 시작");

    }

    // Update is called once per frame

    void Update()
    {
        //slider_time.value = time_remain / time_limit;
        //if (start_flag)
        //{
        //    if (time_remain > 0)
        //        time_remain -= Time.deltaTime;
        //    else
        //    {
        //        start_flag = false;
        //        dirtys.SetActive(false);
        //        time_remain = 0;
        //        cnt_succes = 0;
        //        cnt_fail += 1;
        //        info_panel.SetActive(false);
        //        Invoke("game_start_button_click", 5f);
        //    }


        //}


        if (Input.GetMouseButton(0))
        {
            var mousepos = Input.mousePosition;
            //Debug.Log("mousepos: " + mousepos);
            // 터치 위치를 레이로 변환
            Ray ray = Camera.main.ScreenPointToRay(mousepos);
            RaycastHit hit;

            // 레이캐스트 수행
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("터치된 객체: " + hit.collider.gameObject.name);

                // 히트된 객체가 "Touchable" 태그를 가지고 있는지 확인
                if (hit.collider != null && hit.collider.gameObject.tag == "cylinder")
                {
                    // 터치된 객체에 대한 액션 수행
                    if (hit.collider.gameObject.name == "SpikeBall")
                    {
                        c_0 += 1;
                        Debug.Log("터치된 객체: " + hit.collider.gameObject.name + "\t횟수: " + c_0.ToString());
                    }
                    if (hit.collider.gameObject.name == "SpikeBall (1)")
                    {
                        c_1 += 1;
                        Debug.Log("터치된 객체: " + hit.collider.gameObject.name + "\t횟수: " + c_1.ToString());
                    }
                    if (hit.collider.gameObject.name == "SpikeBall (2)")
                    {
                        c_2 += 1;
                        Debug.Log("터치된 객체: " + hit.collider.gameObject.name + "\t횟수: " + c_2.ToString());
                    }
                    if (hit.collider.gameObject.name == "SpikeBall (3)")
                    {
                        c_3 += 1;
                        Debug.Log("터치된 객체: " + hit.collider.gameObject.name + "\t횟수: " + c_3.ToString());
                    }
                    if (hit.collider.gameObject.name == "SpikeBall (4)")
                    {
                        c_4 += 1;
                        Debug.Log("터치된 객체: " + hit.collider.gameObject.name + "\t횟수: " + c_4.ToString());
                    }
                    if (hit.collider.gameObject.name == "SpikeBall (5)")
                    {
                        c_5 += 1;
                        Debug.Log("터치된 객체: " + hit.collider.gameObject.name + "\t횟수: " + c_5.ToString());
                    }
                    if (hit.collider.gameObject.name == "SpikeBall (6)")
                    {
                        c_6 += 1;
                        Debug.Log("터치된 객체: " + hit.collider.gameObject.name + "\t횟수: " + c_6.ToString());
                    }
                    if (hit.collider.gameObject.name == "SpikeBall (7)")
                    {
                        c_7 += 1;
                        Debug.Log("터치된 객체: " + hit.collider.gameObject.name + "\t횟수: " + c_7.ToString());
                    }
                    if (hit.collider.gameObject.name == "SpikeBall (8)")
                    {
                        c_8 += 1;
                        Debug.Log("터치된 객체: " + hit.collider.gameObject.name + "\t횟수: " + c_8.ToString());
                    }
                    if (hit.collider.gameObject.name == "SpikeBall (9)")
                    {
                        c_9 += 1;
                        Debug.Log("터치된 객체: " + hit.collider.gameObject.name + "\t횟수: " + c_9.ToString());
                    }
                    if (hit.collider.gameObject.name == "SpikeBall (10)")
                    {
                        c_10 += 1;
                        Debug.Log("터치된 객체: " + hit.collider.gameObject.name + "\t횟수: " + c_10.ToString());
                    }

                    // 여기에 원하는 동작 추가, 예: 색상 변경, 애니메이션 트리거 등
                    hit.collider.gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color = Random.ColorHSV();
                }
            }
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            var ray = Camera.main.ScreenPointToRay(touch.position);
            Physics.Raycast(ray, out var hit, float.PositiveInfinity);

            switch(touch.phase)
            {
                case TouchPhase.Began:
                    break;
                case TouchPhase.Moved:
                    Debug.Log("터치된 객체: " + hit.collider.gameObject.name);
                    // 히트된 객체가 "Touchable" 태그를 가지고 있는지 확인
                    if (hit.collider != null && hit.collider.gameObject.tag == "cylinder")
                    {
                        care_effect_script.soap_and_bubble();
                        // 터치된 객체에 대한 액션 수행
                        if (hit.collider.gameObject.name == "SpikeBall")
                        {
                            c_0 += 1;
                            Debug.Log("터치된 객체: " + hit.collider.gameObject.name + "\t횟수: " + c_0.ToString());
                        }
                        if (hit.collider.gameObject.name == "SpikeBall (1)")
                        {
                            c_1 += 1;
                            Debug.Log("터치된 객체: " + hit.collider.gameObject.name + "\t횟수: " + c_1.ToString());
                        }
                        if (hit.collider.gameObject.name == "SpikeBall (2)")
                        {
                            c_2 += 1;
                            Debug.Log("터치된 객체: " + hit.collider.gameObject.name + "\t횟수: " + c_2.ToString());
                        }
                        if (hit.collider.gameObject.name == "SpikeBall (3)")
                        {
                            c_3 += 1;
                            Debug.Log("터치된 객체: " + hit.collider.gameObject.name + "\t횟수: " + c_3.ToString());
                        }
                        if (hit.collider.gameObject.name == "SpikeBall (4)")
                        {
                            c_4 += 1;
                            Debug.Log("터치된 객체: " + hit.collider.gameObject.name + "\t횟수: " + c_4.ToString());
                        }
                        if (hit.collider.gameObject.name == "SpikeBall (5)")
                        {
                            c_5 += 1;
                            Debug.Log("터치된 객체: " + hit.collider.gameObject.name + "\t횟수: " + c_5.ToString());
                        }
                        if (hit.collider.gameObject.name == "SpikeBall (6)")
                        {
                            c_6 += 1;
                            Debug.Log("터치된 객체: " + hit.collider.gameObject.name + "\t횟수: " + c_6.ToString());
                        }
                        if (hit.collider.gameObject.name == "SpikeBall (7)")
                        {
                            c_7 += 1;
                            Debug.Log("터치된 객체: " + hit.collider.gameObject.name + "\t횟수: " + c_7.ToString());
                        }
                        if (hit.collider.gameObject.name == "SpikeBall (8)")
                        {
                            c_8 += 1;
                            Debug.Log("터치된 객체: " + hit.collider.gameObject.name + "\t횟수: " + c_8.ToString());
                        }
                        if (hit.collider.gameObject.name == "SpikeBall (9)")
                        {
                            c_9 += 1;
                            Debug.Log("터치된 객체: " + hit.collider.gameObject.name + "\t횟수: " + c_9.ToString());
                        }
                        if (hit.collider.gameObject.name == "SpikeBall (10)")
                        {
                            c_10 += 1;
                            Debug.Log("터치된 객체: " + hit.collider.gameObject.name + "\t횟수: " + c_10.ToString());
                        }
                        // 여기에 원하는 동작 추가, 예: 색상 변경, 애니메이션 트리거 등
                        hit.collider.gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color = Random.ColorHSV();
                    }
                    break;
                case TouchPhase.Ended:
                    break;
            }
            //// 터치가 방금 시작되었는지 확인
            //if (touch.phase == TouchPhase.Began)
            //{
            //    // 레이캐스트 수행
                
            //    Debug.Log("터치된 객체: " + hit.collider.gameObject.name);

            //    // 히트된 객체가 "Touchable" 태그를 가지고 있는지 확인
            //    if (hit.collider != null && hit.collider.gameObject.tag == "cylinder")
            //    {
            //        // 터치된 객체에 대한 액션 수행
            //        if(hit.collider.gameObject.name == "SpikeBall")
            //        {
            //            c_0 += 1;
            //            Debug.Log("터치된 객체: " + hit.collider.gameObject.name + "\t횟수: " + c_0.ToString());
            //        }
            //        if (hit.collider.gameObject.name == "SpikeBall (1)")
            //        {
            //            c_1 += 1;
            //            Debug.Log("터치된 객체: " + hit.collider.gameObject.name + "\t횟수: " + c_1.ToString());
            //        }
            //        if (hit.collider.gameObject.name == "SpikeBall (2)")
            //        {
            //            c_2 += 1;
            //            Debug.Log("터치된 객체: " + hit.collider.gameObject.name + "\t횟수: " + c_2.ToString());
            //        }
            //        if (hit.collider.gameObject.name == "SpikeBall (3)")
            //        {
            //            c_3 += 1;
            //            Debug.Log("터치된 객체: " + hit.collider.gameObject.name + "\t횟수: " + c_3.ToString());
            //        }
            //        if (hit.collider.gameObject.name == "SpikeBall (4)")
            //        {
            //            c_4 += 1;
            //            Debug.Log("터치된 객체: " + hit.collider.gameObject.name + "\t횟수: " + c_4.ToString());
            //        }
            //        if (hit.collider.gameObject.name == "SpikeBall (5)")
            //        {
            //            c_5 += 1;
            //            Debug.Log("터치된 객체: " + hit.collider.gameObject.name + "\t횟수: " + c_5.ToString());
            //        }
            //        if (hit.collider.gameObject.name == "SpikeBall (6)")
            //        {
            //            c_6 += 1;
            //            Debug.Log("터치된 객체: " + hit.collider.gameObject.name + "\t횟수: " + c_6.ToString());
            //        }

            //        // 여기에 원하는 동작 추가, 예: 색상 변경, 애니메이션 트리거 등
            //        hit.collider.gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color = Random.ColorHSV();
            //    }
                
            //}
        }

        if (tutorial_step > 1) check_answer();

    }

    void set_difficulty()
    {
        if (PlayerPrefs.GetInt("Level_bath") == 1)
        {
            cnt_dirtys = 7;
        }
        if (PlayerPrefs.GetInt("Level_bath") == 2)
        {
            cnt_dirtys = 9;
        }
        if (PlayerPrefs.GetInt("Level_bath") == 3)
        {
            cnt_dirtys = 11;
        }
    }

    public void load_AR_scene()
    {
        SceneManager.LoadScene("10_AR_interaction_MZ");
    }

    void goTo_mainGame()
    {
        SceneManager.LoadScene("22_Virtual__washing");
    }

    public void game_start_button_click()
    {
        //if (cnt_fail == 3)
        //{
        //    //실패 문구 보여주기
        //    info_panel.SetActive(false);
        //    //fail_panel.SetActive(true);
        //    TextMeshProUGUI text_fail = GameObject.Find("Text_fail").GetComponent<TextMeshProUGUI>();
        //    text_fail.text = "다음 기회에 다시 도전해봐요!";
        //    Invoke("load_AR_scene", 4f);
        //    return;
        //}

        //time_remain = time_limit;
        //if (start_flag == false) start_flag = true; 
        if (!info_panel.activeSelf) info_panel.SetActive(true);
        if (tutorial_start_Bt.activeSelf) tutorial_start_Bt.SetActive(false);
        //if (gamestart_Button.activeSelf == true) gamestart_Button.SetActive(false);
        if (dirtys.activeSelf == false) dirtys.SetActive(true);

        if(tutorial_step == 0)
        {
            tutorial_step = 1;
            cnt_dirtys = 1;
            for (int i = 0; i < cnt_dirtys; i++)
            {
                dirtys.transform.GetChild(i).gameObject.SetActive(true);
            }
            bgm_player_script.excute_narration("47");
            logger_script.logger_master.insert_data("손으로 바이러스 문질러보기 시작");
            tutorial_msg.text = "화면에 나타난 먼지를 손가락으로 문질러 보세요.\n문지를 때 마다 색상이 변하는게 확인 되시나요? \n확인 되시면 다음 버튼을 눌러 주세요";
            //tutorial_next_Bt.SetActive(true);
            Invoke("show_next_bt", 10f);
        }




        //for (int i = 0; i < dirtys.transform.childCount-1; i++)
        //{
        //    dirtys.transform.GetChild(i).gameObject.SetActive(true);
        //    //dirtys.transform.GetChild(i).transform.GetChild(0).GetComponent<Renderer>().material.color = Color.white;
        //}
    }

    public void game_next_bt_clicked()
    {
        if(tutorial_step == 1)
        {
            logger_script.logger_master.insert_data("손으로 바이러스 문질러서 지워보기 시작");
            bgm_player_script.excute_narration("48");
            tutorial_msg.text = "이젠 화면에 나타난 먼지를 손으로 문질러서 지워볼게요!";
            c_0 = 0;
            for (int i = 0; i < cnt_dirtys; i++)
            {
                dirtys.transform.GetChild(i).gameObject.SetActive(true);
                dirtys.transform.GetChild(i).gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.white;
            }
            tutorial_step++;
            Invoke("show_next_bt", 5f);
        }
        else if(tutorial_step == 2)
        {
            cnt_succes = 0;
            bgm_player_script.excute_narration("49");
            logger_script.logger_master.insert_data("바이러스 여러개 문질러서 지워보기 시작");
            tutorial_msg.text = "이번에는 먼지 여러개를 한꺼번에 문질러서 지워볼게요!";
            cnt_dirtys = 3;
            for (int i = 0; i < cnt_dirtys; i++)
            {
                dirtys.transform.GetChild(i).gameObject.SetActive(true);
            }
            tutorial_step++;
        }

        tutorial_next_Bt.SetActive(false);
    }

    void show_next_bt()
    {
        Debug.Log("show_next_bt 실행");
        tutorial_next_Bt.SetActive(true);
    }

    void check_answer()
    {

        if (c_0 > 10)
        {
            logger_script.logger_master.insert_data("0번째 먼지 사라짐");
            cnt_succes += 1;
            var gb = GameObject.Find("SpikeBall");
            c_0 = 0;
            gb.SetActive(false);
            TriggerParticleEffect(gb.transform.position);
        }

        if (c_1 > 10)
        {
            logger_script.logger_master.insert_data("1번째 먼지 사라짐");
            cnt_succes += 1;
            var gb = GameObject.Find("SpikeBall (1)");
            c_1 = 0;
            gb.SetActive(false);
            TriggerParticleEffect(gb.transform.position);
        }

        if (c_2 > 10)
        {
            logger_script.logger_master.insert_data("2번째 먼지 사라짐");
            cnt_succes += 1;
            var gb = GameObject.Find("SpikeBall (2)");
            c_2 = 0;
            gb.SetActive(false);
            TriggerParticleEffect(gb.transform.position);
        }

        if (c_3 > 10)
        {
            logger_script.logger_master.insert_data("3번째 먼지 사라짐");
            cnt_succes += 1;
            var gb = GameObject.Find("SpikeBall (3)");
            c_3 = 0;
            gb.SetActive(false);
            TriggerParticleEffect(gb.transform.position);
        }

        if (c_4 > 10)
        {
            logger_script.logger_master.insert_data("4번째 먼지 사라짐");
            cnt_succes += 1;
            var gb = GameObject.Find("SpikeBall (4)");
            c_4 = 0;
            gb.SetActive(false);
            TriggerParticleEffect(gb.transform.position);
        }

        if (c_5 > 10)
        {
            logger_script.logger_master.insert_data("5번째 먼지 사라짐");
            cnt_succes += 1;
            var gb = GameObject.Find("SpikeBall (5)");
            c_5 = 0;
            gb.SetActive(false);
            TriggerParticleEffect(gb.transform.position);
        }

        if (c_6 > 10)
        {
            logger_script.logger_master.insert_data("6번째 먼지 사라짐");
            cnt_succes += 1;
            var gb = GameObject.Find("SpikeBall (6)");
            c_6 = 0;
            gb.SetActive(false);
            TriggerParticleEffect(gb.transform.position);
        }

        if (c_7 > 10)
        {
            logger_script.logger_master.insert_data("7번째 먼지 사라짐");
            cnt_succes += 1;
            var gb = GameObject.Find("SpikeBall (7)");
            c_7 = 0;
            gb.SetActive(false);
            TriggerParticleEffect(gb.transform.position);
        }
        if (c_8 > 10)
        {
            logger_script.logger_master.insert_data("8번째 먼지 사라짐");
            cnt_succes += 1;
            var gb = GameObject.Find("SpikeBall (8)");
            c_8 = 0;
            gb.SetActive(false);
            TriggerParticleEffect(gb.transform.position);
        }
        if (c_9 > 10)
        {
            logger_script.logger_master.insert_data("9번째 먼지 사라짐");
            cnt_succes += 1;
            var gb = GameObject.Find("SpikeBall (9)");
            c_9 = 0;
            gb.SetActive(false);
            TriggerParticleEffect(gb.transform.position);
        }
        if (c_10 > 10)
        {
            logger_script.logger_master.insert_data("10번째 먼지 사라짐");
            cnt_succes += 1;
            var gb = GameObject.Find("SpikeBall (10)");
            c_10 = 0;
            gb.SetActive(false);
            TriggerParticleEffect(gb.transform.position);
        }

        if (cnt_succes == cnt_dirtys)
        {
            info_panel.SetActive(false);
            Debug.Log(cnt_succes + "\t" + cnt_dirtys);
            //cnt_succes = 0;
            //start_flag = false;
            //dirtys.SetActive(false);
            //time_remain = 0;
            //Invoke("game_start_button_click", 5f);

            //Pet.transform.GetChild(8).gameObject.SetActive(true);
            //face_renderer.SetBlendShapeWeight(5, 100);

            //Invoke("set_face_default", 5f);
            //Invoke("heart_effect_deactivate", 3f);

            //Invoke("ShowGameResult", 3.1f);

            //Invoke("load_AR_scene", 7.1f);
            if(tutorial_step == 3)
            {
                tutorial_step = 0;
                logger_script.logger_master.insert_data("목욕하기 게임 튜토리얼 종료");
                Invoke("ShowGameResult", 3.1f);
            }
        }
    }

    // 결과 표시 함수
    void ShowGameResult()
    {
        bgm_player_script.excute_narration("50");
        info_panel.SetActive(false);
        gameDonePanel.SetActive(true);
        gamestart_Button.SetActive(false);
        Invoke("goTo_mainGame", 7f);
        //text_last.gameObject.SetActive(true);
        //text_last.text = "잘 하셨어요!\n 보상은 다음과 같습니다";
    }

    // 파티클 시스템 트리거 함수
    void TriggerParticleEffect(Vector3 position)
    {
        if (particlePrefab != null)
        {
            // 위치에 파티클 시스템 생성
            var particleInstance = Instantiate(particlePrefab, position, Quaternion.identity);
            particleInstance.Play();

            // 파티클 시스템 자동 제거
            Destroy(particleInstance.gameObject, particleInstance.main.duration);
        }
        else
        {
            Debug.LogError("ParticlePrefab이 Inspector에서 설정되지 않았습니다.");
        }
    }

    void set_face_default()
    {
        for(int i=0; i<7; i++)
        {
            face_renderer.SetBlendShapeWeight(i, 0);
        }
    }

    //void heart_effect_deactivate()
    //{
    //    Pet.transform.GetChild(3).gameObject.SetActive(false);
    //}

}
