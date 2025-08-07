using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


namespace UnityEngine.XR.ARFoundation
{
    public class Tutorial_pictureGame : MonoBehaviour
    {
        //Material[] mat = new Material[2];
        //Material[] rgb_mat = new Material[3];
        public bool flag_scan_mode;
        bool flag_game_mode;
        bool is_check_pos_true;
        bool pet_move_flag;
        bool do_nothing;
        bool m_bPause;
        bool check_pos_clicked;
        bool go_to_below;
        int cnt_founded;
        Petctrl petctrl_script;
        vid_control vid_ctrl_script;
        //public GameObject scanning_panel;
        public GameObject gamming_panel;
        public GameObject video_screen;
        public GameObject take_picture_bt;
        public GameObject shoot_bt;
        public GameObject gamming_take_picture_bt;
        public GameObject gamming_shoot_bt;
        public GameObject guide_circle_orange;
        public GameObject start_game_bt;
        public GameObject start_game_bt_no;
        public GameObject Arrow_L;
        public GameObject Arrow_R;
        //public Image guide_image;
        public List<Sprite> guide_img_set = new List<Sprite>();
        public bool flag_game_started;


        ARMeshManager Meshing;
        //MeshRenderer occ_mat;
        //MeshRenderer white_mat;
        //MeshRenderer red_mat;
        //MeshRenderer green_mat;
        //MeshRenderer blue_mat;

        //MeshFilter occ_prefab;
        //MeshFilter white_prefab;
        List<Vector3> extracted_points = new List<Vector3>();
        List<Vector3> list_points = new List<Vector3>();
        List<GameObject> Gb_points = new List<GameObject>();
        Vector3 touched_mesh_pose;
        //Vector3 oldPosition; //속도 계산 위한 이전 프레임 포인트 저장

        float time_remaining;
        public float min_y = 0;
        public Image timer_radial_image;
        public Image gamming_timer_radial_image;
        public TMP_Text Panel_text;
        public GameObject item_meal;
        public TMP_Text time_text;
        public TMP_Text item_text;

        Vector2 Center_device;
        Vector2 touch_pos;
        RaycastHit hit;
        private Animator anim;
        private bool track_flag;

        int local_success = 0;
        int global_success = 0;
        int game_local_success = 0;
        int game_global_success = 0;

        int cnt_next_bt_clicked;
        public GameObject tutorial_panel;
        public GameObject tutorial_bt;
        public TMP_Text tutorial_msg;

        // Start is called before the first frame update
        void Start()
        {
            Center_device = new Vector2(Screen.width / 2f, Screen.height / 2f);
            petctrl_script = GameObject.Find("Main Camera").GetComponent<Petctrl>();
            vid_ctrl_script = GameObject.Find("Scripts").GetComponent<vid_control>();
            //scanning_panel.SetActive(false);
            gamming_panel.SetActive(false);
            start_game_bt.SetActive(false);
            start_game_bt_no.SetActive(false);
            Arrow_L.SetActive(false);
            Arrow_R.SetActive(false);
            //Meshing = GameObject.Find("Meshing").GetComponent<ARMeshManager>();
            //occ_prefab = GameObject.Find("OcclusionMeshPrefab").GetComponent<MeshFilter>();
            //white_prefab = GameObject.Find("white_mesh_prefab").GetComponent<MeshFilter>();
            //occ_mat = GameObject.Find("OcclusionMeshPrefab").GetComponent<MeshRenderer>();
            //white_mat = GameObject.Find("white_mesh_prefab").GetComponent<MeshRenderer>();
            //red_mat = GameObject.Find("red_mesh_prefab").GetComponent<MeshRenderer>();
            //green_mat = GameObject.Find("green_mesh_prefab").GetComponent<MeshRenderer>();
            //blue_mat = GameObject.Find("blue_mesh_prefab").GetComponent<MeshRenderer>();
            //mat[0] = occ_mat.material;
            //mat[1] = white_mat.material;
            //rgb_mat[0] = red_mat.material;
            //rgb_mat[1] = green_mat.material;
            //rgb_mat[2] = blue_mat.material;
            touched_mesh_pose = Vector3.zero;
            cnt_founded = 0;
            //oldPosition = Vector3.zero;
            time_text.enabled = false;

            cnt_next_bt_clicked = 0;
        }

        // Update is called once per frame
        void Update()
        {
            if(petctrl_script.blink_circle.activeSelf == true)
            {
                var ray_blink = Camera.main.ScreenPointToRay(Center_device);
                Physics.Raycast(ray_blink, out var hit_blink, float.PositiveInfinity);

                if (hit_blink.transform.name.StartsWith("Mesh"))
                    petctrl_script.blink_circle.transform.position = hit_blink.point;
            }
            

            if (flag_scan_mode)
            {
                scan_mode();
            }

            if (flag_game_mode)
            {
                gaming_mode();

            }
        }

        public void picture_next_bt_clicked()
        {
            Debug.Log("cnt_next_bt_clicked: " + cnt_next_bt_clicked);
            if (cnt_next_bt_clicked == 0)
            {
                tutorial_bt.SetActive(false);
                tutorial_msg.text = "동영상을 보고 방을 둘러보는 방법을 배워볼까요?";
                vid_ctrl_script.excute_vid();
                cnt_next_bt_clicked++;
            }

            else if (cnt_next_bt_clicked == 1)
            {
                tutorial_msg.text = "강아지와 함께 방 안을 구경해볼까요?";
                cnt_next_bt_clicked++;
                start_scan();
            }

            else if (cnt_next_bt_clicked == 2)
            {
                tutorial_msg.text = "이제 강아지를 불러올 수 있어요. 한번 불러와 볼까요?";
                flag_scan_mode = false;
                guide_circle_orange.SetActive(false);
                petctrl_script.Load_pet_bt.SetActive(true);
                petctrl_script.blink_circle.SetActive(true);
                Arrow_L.SetActive(false);
                Arrow_R.SetActive(false);
                time_text.text = "";

                cnt_next_bt_clicked++;
            }

            else if (cnt_next_bt_clicked == 3)
            {
                tutorial_msg.text = "강아지를 불러왔어요!";
                petctrl_script.blink_circle.SetActive(false);
                petctrl_script.SetPosition();

                var Load_pet_bt = GameObject.Find("Load_pet");
                Load_pet_bt.SetActive(false);
                Invoke("picture_next_bt_clicked", 5f);
                time_text.text = "";

                cnt_next_bt_clicked++;
            }

            else if (cnt_next_bt_clicked == 4)
            {
                tutorial_msg.text = "강아지를 따라 이동해볼까요?";
                Invoke("picture_next_bt_clicked", 7f);
                start_game();
                cnt_next_bt_clicked++;
            }

            else if (cnt_next_bt_clicked == 5)
            {
                tutorial_msg.text = "강아지가 보이도록 사진을 찍어볼까요?";
                tutorial_bt.SetActive(true);
                cnt_next_bt_clicked++;
            }

            else if (cnt_next_bt_clicked == 6)
            {
                tutorial_bt.SetActive(false);
                tutorial_msg.text = "준비가 되면 자동으로 사진이 찍혀요!";
                Invoke("check_pos", 5f);
                //Invoke("picture_next_bt_clicked", 10f);
                cnt_next_bt_clicked++;
                Debug.Log("check1");
            }

            else if (cnt_next_bt_clicked == 7)
            {
                tutorial_bt.SetActive(true);
                tutorial_msg.text = "다시 한 번 강아지를 따라가 볼까요?";
                cnt_next_bt_clicked++;
                Debug.Log("check2");
            }

            else if (cnt_next_bt_clicked == 8)
            {
                tutorial_bt.SetActive(false);
                tutorial_msg.text = "준비가 되면 자동으로 사진이 찍혀요!";
                Invoke("check_pos", 5f);
                //Invoke("picture_next_bt_clicked", 10f);
                cnt_next_bt_clicked++;
                Debug.Log("check3");
            }

            else if (cnt_next_bt_clicked == 9)
            {
                tutorial_msg.text = "잘 하셨어요! 강아지와 조금 더 친해진 것 같지 않나요?";
                //check_pos();
                Invoke("load_tutorial_page", 9f);
                cnt_next_bt_clicked = 0;
                flag_game_mode = false;
            }


        }

        void load_tutorial_page ()
        {
            SceneManager.LoadScene("Tutorial");
        }

        public void print_trackable()
        {
            GameObject trackables = GameObject.Find("Trackables");
            var childs = trackables.transform.childCount;

            for (int i = 0; i < childs; i++)
            {
                var childs_name = trackables.transform.GetChild(i).transform.name;
                Debug.Log(childs_name);
            }
        }

        public void scan_mode()
        {
            ////Meshing.meshPrefab = white_prefab; 스캐닝 효과를 어떻게 줄지??
            ////스캐닝이 완료되면 종료 버튼을 눌러주세요 클릭-> 종료버튼을 생성해주, 스캐닝이 완료되면 스캐닝을 종료하고 마커를 남겨 놓는다  
            ////스캐닝 하면서 마커를 남겨놓는 방식 혹은 자동으로 스캐닝 포인트들을 생성해준다
            ////scanning이 완료되면, 바닥에 marker가 표시되게 할 수 있나?  
            //var Center_device = new Vector2(Screen.width / 2f, Screen.height / 2f);
            //var ray = Camera.main.ScreenPointToRay(Center_device);
            //var hasHit = Physics.Raycast(ray, out var hit, float.PositiveInfinity);
            //if(hasHit)
            //{
            //    //point들을 저장한다.
            //    //var origin = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //    //origin.transform.localScale = Vector3.one * 0.05f;
            //    //origin.transform.position = hit.point;
            //    list_points.Add(hit.point);
            //    //Debug.Log("length of list_points: " + list_points.Count);

            //}
            ////mode가 끝날 때는 list_points로부터 7개의 point들을 고른다.


            //만약 시간내에 take picture 버튼을 누른다면 local sucess += 1
            //local success가 3번 이상이면 global_sucess += 1
            //global success는 촬영한 물체의 갯수로 생각하면 된다.

            if (time_remaining < 0)
            {
                flag_scan_mode = false;
                guide_circle_orange.SetActive(false);
                petctrl_script.Load_pet_bt.SetActive(true);
                petctrl_script.blink_circle.SetActive(true);

                //Meshing.enabled = false;
                //petctrl_script.SetPosition();
                Arrow_L.SetActive(false);
                Arrow_R.SetActive(false);
                time_text.text = "";
                picture_next_bt_clicked();
                return;
            }
            time_remaining -= Time.deltaTime;
            time_text.text = ((int)time_remaining % 60).ToString() +
                             "초 후에 게임이 시작됩니다." +
                             "\n강아지에게 방을 구경시켜 주세요!" +
                             "\n\n\n\n\n\n\n\n\n\n강아지가 둘러볼 수 있도록 화면을 좌우로 움직여 볼까요?";
            var ray = Camera.main.ScreenPointToRay(Center_device);
            var hasHit = Physics.Raycast(ray, out hit, float.PositiveInfinity);
            //var currentPoisition = hit.point;
            //var dis = (currentPoisition - oldPosition);
            //var distance = Mathf.Sqrt(Mathf.Pow(dis.x, 2) + Mathf.Pow(dis.y, 2) + Mathf.Pow(dis.z, 2));
            //var velocity = distance / Time.deltaTime;
            //oldPosition = currentPoisition;
            //Debug.Log("velocity: " + velocity);


            if (hasHit && hit.transform.name.StartsWith("Mesh"))//
            {
                if (min_y > hit.point.y) min_y = hit.point.y;
                if (time_remaining < 10)
                {
                    var diff = Mathf.Abs((hit.point.y - min_y) / min_y);
                    //Debug.Log(hit.transform.name + "\tdiff: " + diff
                    //          + "\thit.point.y: " + hit.point.y
                    //          + "\tmin_y: " + min_y);

                    if (diff < 0.05)
                    {
                        list_points.Add(hit.point);
                    }
                }
                guide_circle_orange.transform.position = hit.point + Vector3.up * 0.1f;
                guide_circle_orange.transform.eulerAngles = new Vector3(0f, Camera.main.transform.eulerAngles.y, 0f);
                //MeshRenderer prefab_mat = hit.transform.GetComponent<MeshRenderer>();
                //Debug.Log(prefab_mat.material);
                //MeshFilter collider_mat = hit.transform.GetComponent<MeshFilter>();
                //MeshRenderer renderer_mat = collider_mat.GetComponent<MeshRenderer>();
                //Debug.Log(collider_mat + "\t" + collider_mat.name + "\t" + collider_mat.GetInstanceID() + "\t"
                //        + collider_mat.mesh);
            }

            //if (time_remaining > 0)
            //{
            //    //Debug.Log("Scanning mode 실행중");
            //    time_remaining -= Time.deltaTime;
            //    timer_radial_image.fillAmount = time_remaining / 10f;
            //}
            //else
            //{
            //    //take picture 버튼을 누른 후 10초가 경과하면 다시 shoot_bt를 꺼주고 take_picture bt을 켜준다
            //    take_picture_bt.SetActive(true);
            //    shoot_bt.SetActive(false);
            //    timer_radial_image.fillAmount = 1f;
            //}


        }

        public void Pause()
        {
            if (m_bPause == false)
            {
                m_bPause = true;
            }
            else
            {
                m_bPause = false;
            }
        }

        void extract_points()
        {
            int len_points = list_points.Count;
            Debug.Log("len of list_points: " + len_points);
            for (int i = 0; i < 10; i++)
            {
                int index_point = (len_points / 9) * i;
                if (index_point == list_points.Count) index_point -= 1;
                extracted_points.Add(list_points[index_point]);//실제 사용은 1부터 8까지
                Debug.Log("index: " + i.ToString() + "\t     point:" +
                           extracted_points[i].x + ",\t" + extracted_points[i].y + ",\t" + extracted_points[i].z);
                //var origin = GameObject.CreatePrimitive(PrimitiveType.Cube);
                //origin.transform.localScale = Vector3.one * 0.2f;
                //origin.transform.position = list_points[index_point];
                //Gb_points.Add(origin);

            }
            System.Range range = 0..5;
            var shuffled_idx = MakeRandomNumbers(9)[range];

            for (int i = 0; i < shuffled_idx.Length; i++)
            {
                var item_created = Instantiate(item_meal, extracted_points[shuffled_idx[i]] + 0.1f * Vector3.up, hit.transform.rotation);
                var gb_name = "item_" + shuffled_idx[i].ToString();
                Debug.Log(gb_name);
                item_created.transform.name = gb_name;
                Gb_points.Add(item_created);
            }
            //list_points.Clear();
            extracted_points.Clear();

        }

        public void gaming_mode()
        {
            /*
            Vector3 cur_target_pos = extracted_points[cnt_founded];
            Vector3 next_target_pos = extracted_points[cnt_founded + 1];

            if (Input.touchCount > 0)//펫이 화면에 존재할 경우, 화면을 터치하면 그곳으로 이동시켜준다. 그러기 위해 이동할 목표 위치를 업데이트 시켜준다. 
            {
                Debug.Log("Pet is in the Scene, screen touched");
                Touch touch2 = Input.GetTouch(0);
                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    track_flag_changed = true;
                    MeshRenderer render = Gb_points[cnt_founded + 1].GetComponent<MeshRenderer>();
                    render.material = rgb_mat[0];//change to red
                    petctrl_script.track_flag = true;
                    var ray = Camera.main.ScreenPointToRay(touch2.position);
                    var hasHit = Physics.Raycast(ray, out var hit, float.PositiveInfinity);
                    Debug.Log("tracking point 저장: " + hit.point);
                    if (hasHit) touched_mesh_pose = hit.point;
                }
            }

            if (touched_mesh_pose != Vector3.zero) //Vector3.zero는 초기값.
            {
                petctrl_script.move_to_point(touched_mesh_pose);
            }

            Debug.Log(petctrl_script.track_flag + "\t" + track_flag_changed);


            //track_flag가 false가 되었을 때 한번만 실행시키는 작업을 해주는 함수가 하나 필요하다
            //track_flag가 false가 되었다는 것은 object가 손으로 찍은 지점과 비슷한 곳에 있다는 이야기이다.
            if (petctrl_script.track_flag == false && track_flag_changed)
            {
                //터치한 곳으로 펫이 이동했을 때, 해당 위치와 next_target_pose위치가 오차범위 이내에 있는지 확인해야 한다. 
                track_flag_changed = false;
                Debug.Log("Pet과 target pos 사이의 거리: " +
                           Vector3.Distance(petctrl_script.spawnedObject.transform.position, Gb_points[cnt_founded + 1].transform.position));
                if (Vector3.Distance(petctrl_script.spawnedObject.transform.position, Gb_points[cnt_founded + 1].transform.position) < 0.2f)
                {
                    //pet이 target object 근처로 이동했으므로 Target object의 색을 초록으로 바꿔준 후 cnt_founded를 올려준다
                    Debug.Log("색상을 초록으로 바꿔 줍니다. 현재 찾은 개수: " + (cnt_founded + 1).ToString());
                    MeshRenderer render = Gb_points[cnt_founded + 1].GetComponent<MeshRenderer>();
                    render.material = rgb_mat[1];// change to green
                    cnt_founded += 1;
                }
                else
                {
                    //pet의 위치가 target pos와 더 가까워져야 하므로 다시 트레킹 모드로 전환시켜줘야한다.
                    petctrl_script.track_flag = true;
                    Debug.Log("물체를 찾아가주세요");
                }
            }
            */

            petctrl_script.spawnedObject.transform.eulerAngles = new Vector3(0f, petctrl_script.spawnedObject.transform.eulerAngles.y, 0f);
            petctrl_script.spawnedObject.transform.GetChild(0).transform.eulerAngles = new Vector3(0f, petctrl_script.spawnedObject.transform.GetChild(0).transform.eulerAngles.y, 0f);
            if (petctrl_script.spawnedObject.transform.GetChild(0).transform.position.y < -5f)
            {
                petctrl_script.spawnedObject.transform.GetChild(0).transform.position =
                    new Vector3(petctrl_script.spawnedObject.transform.GetChild(0).transform.position.x,
                                0, petctrl_script.spawnedObject.transform.GetChild(0).transform.position.z);
            }

            //Debug.Log("Pome Angle: " + petctrl_script.spawnedObject.transform.eulerAngles
            //        + "\tPome_inva Angle: " + petctrl_script.spawnedObject.transform.GetChild(0).transform.eulerAngles
            //        +"\tPome_position: " + petctrl_script.spawnedObject.transform.position
            //        + "\tPome_inva_position: " + petctrl_script.spawnedObject.transform.GetChild(0).transform.position);

            if (do_nothing) return;
            if (pet_move_flag)
            {
                //BoxCollider collider = petctrl_script.spawnedObject.transform.GetChild(0).GetComponent<BoxCollider>();
                //Rigidbody rg_body = petctrl_script.spawnedObject.transform.GetChild(0).GetComponent<Rigidbody>();
                //rg_body.useGravity = true;
                //collider.isTrigger = true;

                track_flag = true;
                anim = petctrl_script.spawnedObject.transform.GetChild(0).GetComponent<Animator>();
                anim.Play("Walk");
                //Debug.Log("pet is moving");
                touched_mesh_pose = Gb_points[game_global_success].transform.position;
                pet_move_flag = false;
            }

            if (touched_mesh_pose != Vector3.zero) move_to_point(touched_mesh_pose); //Vector3.zero는 초기값. 
            if (track_flag) return;
            else
            {
                anim = petctrl_script.spawnedObject.transform.GetChild(0).GetComponent<Animator>();
                anim.Play("Idle");
            }

            //if (!go_to_below) return;

            //if (time_remaining > 0)
            if(time_remaining > 0)
            {
                time_remaining -= Time.deltaTime;
            }
            //gamming_timer_radial_image.fillAmount = time_remaining / 10f;

            //check_pos(); //올바른 포즈로 촬영했는지 확인해주는 함수
            if (!check_pos_clicked) return;
            check_pos_clicked = false;
            //else //시간이 다 경과했을 경우.
                 //CASE1: 실제 시간이 다 경과되어서 넘어온 경우.(is_check_pos_true==false)
                 //CASE2: check_pos함수에 의해 넘어온 경우.(is_check_pos_true==true, 정답일 시 check_pos에서 time_remaining을 강제로 0으로 만)
            
            if (is_check_pos_true)//사진 찍기 성공한 경우
            {
                Debug.Log("game_mode_picture bt clicked 실행");
                Invoke("picture_next_bt_clicked", 7f);
                is_check_pos_true = false;
                //if (game_local_success < 2)
                //{

                //}
                //else
                {
                    //뭔가 리액션 관련된 함수나 애니메이션, 혹은 보상주기
                    //global success 를 1올려주는 타이밍에 Pet_move_flag도 True로 바꿔준다.

                    petctrl_script.set_text_speechBubble("사진을 찍었어요!");
                    Gb_points[game_global_success].SetActive(false); //global succes가 0이 되면 Gb_points도 clear 해준다.
                    game_global_success += 1;
                    if (game_global_success == 5) Gb_points.Clear();
                    //guide_image.sprite = guide_img_set[0]; //reset 이미지로
                    anim = petctrl_script.spawnedObject.transform.GetChild(0).GetComponent<Animator>();
                    var rand_idx = MakeRandomNumbers(4)[0];
                    if (rand_idx == 0)
                    {
                        anim.Play("277_Skill_Dance");
                    }
                    else if (rand_idx == 1)
                    {
                        anim.Play("067_Idle_Blend_LieOnBack");
                    }
                    else if (rand_idx == 2)
                    {
                        anim.Play("302_Stroll");
                    }
                    else if (rand_idx == 3)
                    {
                        anim.Play("226_Play_Left_Spin");
                    }
                    do_nothing = true;
                    //float loop_escape_time = 100.0f;
                    //while(true)
                    //{
                    //    loop_escape_time -= Time.deltaTime;
                    //    Debug.Log(loop_escape_time);
                    //    if (loop_escape_time < 0) break;
                    //}
                    Invoke("set_pet_move_flag_true", 7f);// 5초 후 다음 Target으로 이동
                    item_text.text = "찾은 개수: " + game_global_success.ToString() + "개";
                }
            }
            //else //사진 찍기 실패한 경우
            //{
            //    time_remaining = 10.0f;
            //}


            //gamming_take_picture_bt.SetActive(true);
            //gamming_shoot_bt.SetActive(false);
            //if(time_remaining < 0)
                
                //gamming_timer_radial_image.fillAmount = 1f;
            //time_remaining = 10.0f;
                
            

            //if (EventSystems.EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            //    return;

            //if (Input.touchCount > 0)//펫이 화면에 존재할 경우, 게임 오브젝트를 터치하면 그곳으로 이동시켜준다.  
            //{
            //    //Debug.Log("Pet is in the Scene, screen touched");
            //    touch_pos = InputSystem.Pointer.current.position.ReadValue();
            //    if (touch_pos.y / Screen.height > 0.15f)
            //    {
            //        var ray1 = Camera.main.ScreenPointToRay(touch_pos);
            //        var hasHit1 = Physics.Raycast(ray1, out var hit1, float.PositiveInfinity);
            //        if (hasHit1 && hit1.transform.name.StartsWith("item"))//item을 클릭 할 때만 움직이기
            //        {
            //            track_flag = true;
            //            anim = petctrl_script.spawnedObject.transform.GetChild(0).GetComponent<Animator>();
            //            anim.Play("Walk");
            //            Debug.Log("item is touched___gameMode");
            //            touched_mesh_pose = hit1.point;
            //        }
            //    }
            //}

        }

        private void set_pet_move_flag_true()
        {
            do_nothing = false;
            if (game_global_success < 5)
            {
                set_to_kinematic();
                pet_move_flag = true;
            }
            //else
            //{
            //    Debug.Log("게임 종료!");
            //    set_to_gravity();
            //    game_global_success = 0;
            //    game_local_success = 0;
            //    anim = petctrl_script.spawnedObject.transform.GetChild(0).GetComponent<Animator>();
            //    anim.Play("Idle");
            //    flag_game_started = false;
            //    flag_game_mode = false;
            //    gamming_panel.SetActive(false);
            //    time_text.text = "게임을 다시 한 번 해볼까요?";
            //    start_game_bt_no.SetActive(true);
            //    start_game_bt.SetActive(true);
            //}
        }

        public void check_pos()
        {


            //if (time_remaining < 7) //현재는 7초 밑으로 떨어지기만 하면 성공으로 취급
            
            AudioSource audio = GameObject.Find("Audio player_cam").GetComponent<AudioSource>();
            audio.Play();
            Debug.Log("사진 찍기 성공!");
            is_check_pos_true = true;
            time_remaining = 0;
            

            check_pos_clicked = true;
            return;
        }

        public void move_to_point(Vector3 hitpoint)
        {
            if (track_flag)
            {

                //if (Vector3.Distance(petctrl_script.spawnedObject.transform.position, hitpoint) > 0.1f)
                //{
                //    petctrl_script.spawnedObject.transform.position = Vector3.MoveTowards(petctrl_script.spawnedObject.transform.position, hitpoint, 0.01f);
                //    Vector3 dir = hitpoint - petctrl_script.spawnedObject.transform.position;
                //    dir.y = 0f;
                //    Quaternion rot = Quaternion.LookRotation(dir.normalized);
                //    petctrl_script.spawnedObject.transform.rotation = rot;
                //}
                //Debug.Log("track_flag is true, check the distance: " + Vector2.Distance(new Vector2(petctrl_script.spawnedObject.transform.position.x, petctrl_script.spawnedObject.transform.position.z),
                //                     new Vector2(hitpoint.x, hitpoint.z)));
                if (Vector2.Distance(new Vector2(petctrl_script.spawnedObject.transform.position.x, petctrl_script.spawnedObject.transform.position.z),
                                     new Vector2(hitpoint.x, hitpoint.z)) > 0.2f)
                {
                    petctrl_script.spawnedObject.transform.position = Vector3.MoveTowards(petctrl_script.spawnedObject.transform.position,
                                                                                          hitpoint,
                                                                                          0.01f);
                    Vector3 dir = hitpoint - petctrl_script.spawnedObject.transform.position;
                    dir.y = 0f;
                    Quaternion rot = Quaternion.LookRotation(dir.normalized);
                    petctrl_script.spawnedObject.transform.rotation = rot;
                    petctrl_script.spawnedObject.transform.GetChild(0).transform.rotation = rot;
                }
                else
                {
                    set_to_gravity();
                    track_flag = false;

                    //BoxCollider collider = petctrl_script.spawnedObject.transform.GetChild(0).GetComponent<BoxCollider>();
                    //Rigidbody rg_body = petctrl_script.spawnedObject.transform.GetChild(0).GetComponent<Rigidbody>();
                    //collider.enabled = true;
                    //rg_body.isKinematic = false;

                    Vector3 dir = Camera.main.transform.position - petctrl_script.spawnedObject.transform.position;
                    dir.y = 0f;
                    Quaternion rot = Quaternion.LookRotation(dir.normalized);
                    petctrl_script.spawnedObject.transform.rotation = rot;
                    petctrl_script.spawnedObject.transform.GetChild(0).transform.rotation = rot;
                    Debug.Log("Idle animation excute___gameMode");
                    anim = petctrl_script.spawnedObject.transform.GetChild(0).GetComponent<Animator>();
                    anim.Play("Idle");
                    //guide_image.sprite = guide_img_set[1];
                }

            }
        }

        public void start_scan()
        {
            if (!flag_scan_mode)
            {
                flag_scan_mode = true;
                flag_game_mode = false;
            }
            //list_points.Clear(); //
            //Meshing.meshPrefab = white_prefab;

            Arrow_L.SetActive(true);
            Arrow_R.SetActive(true);
            time_remaining = 15.0f;
            video_screen.SetActive(false);
            //scanning_panel.SetActive(true);
            shoot_bt.SetActive(false);
            time_text.enabled = true;
            //AudioSource audio = GameObject.Find("Audio player").GetComponent<AudioSource>();
            //audio.Play();

        }

        public void game_restartBt_no_click()
        {
            time_text.text = "";
            //게임 선택하는 scene으로 다시 전환
            SceneManagement.SceneManager.LoadScene("Chapter5");
        }

        public void start_game()
        {
            time_text.text = "";
            extract_points();
            Debug.Log("flag_game_mode: " + flag_game_mode);
            if (!flag_game_mode)
            {
                flag_game_mode = true;
                flag_scan_mode = false;
            }
            set_to_kinematic();//set to kinematic
            flag_game_started = true;
            pet_move_flag = true;
            //petctrl_script.game_start_msg.SetActive(false);
            start_game_bt.SetActive(false);
            start_game_bt_no.SetActive(false);

            //gamming_panel.SetActive(true);
            gamming_take_picture_bt.SetActive(false);
            gamming_shoot_bt.SetActive(false);
            //AudioSource audio = GameObject.Find("Audio player").GetComponent<AudioSource>();
            //if(audio.isPlaying) audio.Stop();

        }



        public void set_to_kinematic()
        {
            Rigidbody rg_body = petctrl_script.spawnedObject.transform.GetChild(0).GetComponent<Rigidbody>();
            rg_body.isKinematic = true;
            rg_body.useGravity = false;
            Debug.Log("Kinematic mode");
        }

        public void set_to_gravity()
        {
            Rigidbody rg_body = petctrl_script.spawnedObject.transform.GetChild(0).GetComponent<Rigidbody>();
            rg_body.useGravity = true;
            rg_body.isKinematic = false;
            Debug.Log("Gravity mode");
        }


        public static int[] MakeRandomNumbers(int maxValue, int randomSeed = 0)
        {
            return MakeRandomNumbers(0, maxValue, randomSeed);
        }

        /// <summary>
        /// ???????? ???? ???? ???? ?????? ??????
        /// </summary>
        /// <param name="minValue">??????(????)</param>
        /// <param name="maxValue">??????(????)</param>
        /// <param name="count">???? ????</param>
        /// <param name="isDuplicate">???? ???? ????</param>
        /// <param name="randomSeed">???? ????</param>
        /// <returns></returns>
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
}