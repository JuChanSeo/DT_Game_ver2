using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;


public class Petctrl : MonoBehaviour
{
	[SerializeField]
    GameObject m_PlacedPrefab;
    public GameObject PlacedPrefab
    {
        get => m_PlacedPrefab;
        set => m_PlacedPrefab = value;
    }
	public GameObject spawnedObject { get; private set; }
	public GameObject[] Pets_copy;

	Vector2 Center_device;
	Vector3 touched_mesh_pose;
	public bool not_move_pet;
	public GameObject guide_pet;
	public GameObject guide_circle;
	public GameObject Load_pet_bt;
	public GameObject blink_circle;
	public Slider energy;
	public Slider fatigue;
	public Slider cleanliness;
	public Slider intimity;
	public Slider exp;
	public Text Coin_cnt;
	public GameObject speechbubble;
	public GameObject cross_img;

	Player_statu player_statu_script;
	bgm_player bgm_player_;
	GameObject video_screen;
	game_mode game_mode_script;
	Vector2 touch_pos;

    Animator anim;
	private float min_y;
	int cnt;


	interact_pet interact_pet_script;
	public bool track_flag;

	// Start is called before the first frame update
	void Start()
    {
		//spawnedObject = Instantiate(GameObject.Find("pomeLV0" + PlayerPrefs.GetInt("Level_pet").ToString()));
		speechbubble.SetActive(false);

		//      TMP_Text txt_bubble = speechbubble.transform.GetChild(0).transform.GetComponent<TMP_Text>();
		//txt_bubble.text = "테스트용 말풍선 입니다";

		cnt = 0;
		min_y = 0;
		Center_device = new Vector2(Screen.width / 2f, Screen.height / 2f);
		touch_pos = new Vector2(0, 0);
		touched_mesh_pose = Vector3.zero;
		player_statu_script = GameObject.Find("player_statu").GetComponent<Player_statu>();
		if(GameObject.Find("Audio player") != null) bgm_player_ = GameObject.Find("Audio player").GetComponent<bgm_player>();
		if (Coin_cnt != null) Coin_cnt.text = "X " + player_statu_script.Coin.ToString();
		//Coin_cnt.text = "X " + player_statu_script.Coin.ToString();
        //Debug.Log(SceneManager.GetActiveScene().name + "\t" + player_statu_script.gameObject.name);
		if (SceneManager.GetActiveScene().name == "Game3" || SceneManager.GetActiveScene().name == "Tutorial_Game3")
		{
			Debug.Log("Game3 Scene Start1111");
			blink_circle.SetActive(false);
			video_screen = GameObject.Find("video screen");
			Load_pet_bt.SetActive(false);
			game_mode_script = GameObject.Find("Scripts").GetComponent<game_mode>();
			return;
		}
		//Debug.Log("for check");
		//Only excute when active Scene is "Chapter5"
		GameObject loading_pet = GameObject.Find("Loading_pet");
		Destroy(loading_pet, 6f);
		Invoke("SetPosition", 6f);

		interact_pet_script = Pets_copy[PlayerPrefs.GetInt("Level_pet")].GetComponent<interact_pet>();
		for(int i =0; i<6; i++)
        {
			if(i != PlayerPrefs.GetInt("Level_pet"))
			{
				Pets_copy[i].SetActive(false);
			}

        }
	}

	// Update is called once per frame
	void Update()
    {
        {
			if (Coin_cnt != null) Coin_cnt.text = "X " + PlayerPrefs.GetInt("Coin").ToString();
			if (energy != null) energy.value = PlayerPrefs.GetFloat("energy");
			if (fatigue != null) fatigue.value = PlayerPrefs.GetFloat("fatigue");
			if (cleanliness != null) cleanliness.value = PlayerPrefs.GetFloat("cleanliness");
			if (intimity != null) intimity.value = PlayerPrefs.GetFloat("intimity");
			if (exp != null) exp.value = PlayerPrefs.GetFloat("exp");
		}


		if (not_move_pet)
		{
			return;
		}

		//Debug.Log("check1432");


		var ray1 = Camera.main.ScreenPointToRay(Center_device);
		var hasHit1 = Physics.Raycast(ray1, out var hit1, float.PositiveInfinity);
		if (hasHit1 && hit1.transform.name.StartsWith("Mesh") && min_y > hit1.point.y)
		{
			min_y = hit1.point.y;
		}

		//if pet is in the scene and game mode is not set to true
		//flag_game_started가 True라면(게임 스타트 버튼이 눌렸다면) 실행하지 않는다


		if (spawnedObject != null && spawnedObject.activeSelf == true && !interact_pet_script.bool_ball_play)
        {

            if (Mathf.Abs((hit1.point.y - min_y) / min_y) < 0.05 && !track_flag
				&& hit1.transform.name.StartsWith("Mesh"))//바닥근처를 터치할 경우에만
			{
				//Debug.Log("Pet is in the Scene, screen touched");
				//touch_pos = Pointer.current.position.ReadValue();
				//if(touch_pos.y/Screen.height > 0.15f)
				//Debug.Log("track_flag: " + track_flag + "\tdistance: " +
				//	Vector2.Distance(new Vector2(spawnedObject.transform.position.x, spawnedObject.transform.position.z),
				//						new Vector2(hit1.point.x, hit1.point.z)));

				if (Vector2.Distance(new Vector2(spawnedObject.transform.position.x, spawnedObject.transform.position.z),
										new Vector2(hit1.point.x, hit1.point.z)) > 1f)
				{
					if (!track_flag)
					{
						//set_to_kinematic();
						track_flag = true;
					}
					//BoxCollider collider = spawnedObject.transform.GetChild(0).GetComponent<BoxCollider>();
					//Rigidbody rg_body = spawnedObject.transform.GetChild(0).GetComponent<Rigidbody>();
					//collider.enabled = true;
					//rg_body.useGravity = true;
					//Debug.Log("Walk animation excute");
					anim.Play("Walk_ahead");
					//Debug.Log("touched coordinate: " + hit1.point);
					//Debug.Log("tracking point 저장: " + hit1.point);
					//Debug.Log((hit1.point.y - game_mode_script.min_y) / game_mode_script.min_y + "\t" + 
					//		   hit1.point.y + "\t" + game_mode_script.min_y);
					guide_circle.transform.position = hit1.point;
					Invoke("disappear_circle", 0.2f);
					touched_mesh_pose = hit1.point;
				}

			}
			

			if (touched_mesh_pose != Vector3.zero) //Vector3.zero는 초기값.
                move_to_point(touched_mesh_pose);

        }

	}

	void disappear_circle()
    {
		guide_circle.transform.position = new Vector3(1000, 1000, 1000);
    }

	private void excute_gameMode()
    {
		game_mode_script.start_game();
	}
	public void Load_pet_true()
    {
		blink_circle.SetActive(false);
		SetPosition();

		if(SceneManager.GetActiveScene().name == "Game3")
        {
			Load_pet_bt = GameObject.Find("Load_pet");
			Load_pet_bt.SetActive(false);
			Invoke("excute_gameMode", 3f);
			game_mode_script.time_text.text = "";

		}
		//Load_pet = true;
		//Destroy(Load_pet_bt);
		//game_mode_script.start_game_bt.SetActive(true);

	}
	public void SetPosition()
	{
		Debug.Log("SetPosition 실행!");
		// Project from the middle of the screen to look for a hit point on the detected surfaces.
		//???????? ?????? ???????????? ??????????
		//m_RaycastManager = GetComponent<ARRaycastManager>();

		//var touch = InputWrapper.GetTouch(i);
		//var pointer = getPointer(touch.fingerId);

		//Debug.Log($"Center_device x,y = {Center_device.x}, {Center_device.y}");

		var ray = Camera.main.ScreenPointToRay(Center_device);
		//Debug.Log("ray.origin:" + ray.origin);

		var hasHit = Physics.Raycast(ray, out var hit, float.PositiveInfinity);
		//Debug.Log("hit.point:" + hit.point.x + "\t" + hit.point.y + "\t" + hit.point.z);
		//var origin = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		//var hit_gb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		//origin.transform.localScale = Vector3.one * 0.01f;
		//hit_gb.transform.localScale = Vector3.one * 0.1f;
		//origin.transform.position = ray.origin;
		//hit_gb.transform.position = hit.point;


		//Debug.DrawRay(ray.origin, hit.point, Color.red);
		if (hasHit)
		{
			if (spawnedObject == null)
			{
				if (cross_img != null && cross_img.activeSelf == true)
                {
					cross_img.SetActive(false);
                }
                //spawnedObject = Instantiate(m_PlacedPrefab, hit.point,
                //                            new Quaternion(0f, Camera.main.transform.rotation.y + 180f, 0f, 0f));
                //spawnedObject = Instantiate(GameObject.Find("pomeLV0" + PlayerPrefs.GetInt("Level_pet").ToString()), hit.point,
                //                            new Quaternion(0f, Camera.main.transform.rotation.y + 180f, 0f, 0f));
                //spawnedObject = Instantiate(Pets_copy[1]);
                spawnedObject = Pets_copy[PlayerPrefs.GetInt("Level_pet")];
                spawnedObject.transform.position = hit.point;
                //spawnedObject.transform.position = new Vector3(0, 0, 0.9f);
				spawnedObject.transform.eulerAngles = new Vector3(0, 180f+ Camera.main.transform.eulerAngles.y, 0);
				anim = spawnedObject.GetComponent<Animator>();
				//spawnedObject.transform.eulerAngles = new Vector3(0f, 180f + Camera.main.transform.eulerAngles.y, 0f);
				//spawnedObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				//spawnedObject.transform.localScale = Vector3.one * 0.5f;
				spawnedObject.SetActive(true);
				anim.Play("Bark");
				if(bgm_player_ != null)bgm_player_.dog_sound_excute();

			}
			else
			{
				spawnedObject.transform.position = hit.point;
				spawnedObject.transform.eulerAngles = new Vector3(0f, 180f + Camera.main.transform.eulerAngles.y, 0f);
				if (spawnedObject.activeSelf == false)
				{
					spawnedObject.transform.position = hit.point;
					spawnedObject.SetActive(true);
				}
				else
					spawnedObject.SetActive(false);
			}

		}
  //      if (spawnedObject.transform.GetChild(1).gameObject.activeSelf == true)
  //      {
  //          spawnedObject.transform.GetChild(1).gameObject.SetActive(false);
		//	spawnedObject.transform.GetChild(2).gameObject.SetActive(false);
		//	spawnedObject.transform.GetChild(3).gameObject.SetActive(false);
		//	spawnedObject.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.SetActive(false);

		//}
	}

	public void move_to_point(Vector3 hitpoint)
    {
		if(track_flag)
        {
			if (Vector2.Distance(new Vector2(spawnedObject.transform.position.x, spawnedObject.transform.position.z),
									 new Vector2(hitpoint.x, hitpoint.z)) > 0.2f)
			{
                //Debug.Log(Vector3.Distance(spawnedObject.transform.position, hitpoint));
                //spawnedObject.transform.position = Vector3.MoveTowards(spawnedObject.transform.position,
                //													  hitpoint /*- 0.5f * Vector3.up*/,
                //													  0.01f);

                Vector3 dir = hitpoint - spawnedObject.transform.position;
				dir.y = 0f;
				Quaternion rot = Quaternion.LookRotation(dir.normalized);


				spawnedObject.transform.position = Vector3.MoveTowards(spawnedObject.transform.position,
																	  hitpoint,
																	  0.01f);
				spawnedObject.transform.rotation = rot;
				spawnedObject.transform.GetChild(0).transform.rotation = rot;
				spawnedObject.transform.GetChild(0).transform.localPosition = Vector3.zero; 

				//Vector3 dir = hitpoint - spawnedObject.transform.position;
				//dir.y = 0f;
				//Quaternion rot = Quaternion.LookRotation(dir.normalized);
				//spawnedObject.transform.rotation = rot;

			}
			else
            {
				track_flag = false;
				//BoxCollider collider = spawnedObject.transform.GetChild(0).GetComponent<BoxCollider>();
				//Rigidbody rg_body = spawnedObject.transform.GetChild(0).GetComponent<Rigidbody>();
				//collider.enabled = true;
				//rg_body.useGravity = true;
				
				//set_to_gravity();
                Vector3 dir = Camera.main.transform.position - spawnedObject.transform.position;
				dir.y = 0f;
				Quaternion rot = Quaternion.LookRotation(dir.normalized);
				spawnedObject.transform.rotation = rot;
				spawnedObject.transform.GetChild(0).transform.rotation = rot;
				spawnedObject.transform.GetChild(0).transform.localPosition = Vector3.zero;
				Debug.Log("Idle animation excute");
				anim.Play("Idle");

				//track_flag = false;
				//Vector3 dir = Camera.main.transform.position - spawnedObject.transform.position;
				//dir.y = 0f;
				//Quaternion rot = Quaternion.LookRotation(dir.normalized);
				//spawnedObject.transform.rotation = rot;
				//Debug.Log("Idle animation excute___gameMode");
				//anim = spawnedObject.transform.GetChild(0).GetComponent<Animator>();
				//anim.Play("Idle");
			}

		}
	}

	private void set_to_kinematic()
	{
		Rigidbody rg_body = spawnedObject.transform.GetChild(0).GetComponent<Rigidbody>();
		rg_body.isKinematic = true;
		rg_body.useGravity = false;
		Debug.Log("Kinematic mode");
	}

	private void set_to_gravity()
	{
		Rigidbody rg_body = spawnedObject.transform.GetChild(0).GetComponent<Rigidbody>();
		rg_body.useGravity = true;
		rg_body.isKinematic = false;
		Debug.Log("Gravity mode");
	}

	private void Set_Pome_Idle()
    {
		anim.Play("Idle");
	}
	void SetLookDirection(Vector3 inputAxes)
	{
		// Get the camera's y rotation, then rotate inputAxes by the rotation to get up/down/left/right according to the camera
		Quaternion yRotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
		Vector3 lookDirection = (yRotation * inputAxes).normalized;
		spawnedObject.transform.rotation = Quaternion.LookRotation(lookDirection);
	}

	public void pet_reaction_true()
    {
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
	}

	public void pet_reaction_hungry_true()
    {
		anim.Play("075_Idle_Eat_Loop");
		spawnedObject.transform.GetChild(3).transform.gameObject.SetActive(true);
		Invoke("pet_reaction_hungry_false", 10f);
	}

	public void pet_reaction_hungry_false()
    {
		anim.Play("Idle");
		spawnedObject.transform.GetChild(3).transform.gameObject.SetActive(false);
	}		


	public void pet_reaction_sleep()
	{
		anim.Play("071_Idle_Blend_Sleep 0");
		Invoke("pet_sleep_idle", 10f);
	}
	void pet_sleep_idle()
    {
		anim.Play("290_Sleep_Blend_Idle 0");
	}
		
	public void pet_idle()
    {
		anim.Play("Idle");
	}

	public void pet_reaction_false()
    {
        anim.Play("073_Idle_Disappoint");
		Invoke("pet_idle", 2f);
	}

	public void heart_effect_true()
	{
		spawnedObject.transform.GetChild(1).transform.gameObject.SetActive(true);
		Invoke("heart_effect_false", 4f);
	}

	public void shower_effect_true()
    {
		anim.Play("165_Pat_Left_Loop2");
		spawnedObject.transform.GetChild(2).transform.gameObject.SetActive(true);
		Invoke("shower_effect_false", 10f);
	}

	void heart_effect_false()
    {
		spawnedObject.transform.GetChild(1).transform.gameObject.SetActive(false);
    }

	void shower_effect_false()
    {
		anim.Play("Idle");
		spawnedObject.transform.GetChild(2).transform.gameObject.SetActive(false);
	}

	public void set_text_speechBubble(string message)
    {
		speechbubble.SetActive(true);
		if (speechbubble.gameObject.activeSelf == true)
		{
			//Vector3 screenPos = Camera.main.WorldToScreenPoint(spawnedObject.transform.position + 0.5f * Vector3.up
			//													+ 0.3f * Vector3.right);
			speechbubble.transform.position = spawnedObject.transform.position + 0.4f * Vector3.up
											  + 0.15f * Vector3.right + 0.1f*Vector3.back;

		}

		TMP_Text txt_bubble = speechbubble.transform.GetChild(0).transform.GetComponent<TMP_Text>();
		//Debug.Log(speechbubble.transform.GetChild(0).transform.name);
		txt_bubble.text = message;
        Invoke("init_destroy_speechBubble", 3f);
		
	}

	void init_destroy_speechBubble()
    {
		TMP_Text txt_bubble = speechbubble.transform.GetChild(0).transform.GetComponent<TMP_Text>();
		txt_bubble.text = "";
		speechbubble.SetActive(false);
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








	public void set_pet_speed_1()
    {
		anim.speed = 1f;
	}

	public void pet_idle_sit()
	{
		anim.Play("070_Idle_Blend_Sit");
	}

	public void pet_sit_idle()
	{
		anim.Play("262_Sit_Blend_Idle");
	}

	public void pet_idle_sit_idle()
	{
		pet_idle_sit();
		Invoke("pet_sit_idle", 4f);
	}

	public void pet_lying_begin()
    {
		anim.Play("009_Ball_Lying_Begin");
	}

	public void pet_lying_end()
    {
		anim.Play("010_Ball_Lying_End");
	}

	public void pet_idle_lying_idle()
    {
		pet_lying_begin();
		Invoke("pet_lying_end", 4f);
    }

	public void pet_jump()
    {
		anim.Play("002_Ball_Jump");
	}

	public void pet_wait()
	{
		anim.Play("311_Stroll_Wag");
	}

	public void pet_hand()
	{
		anim.Play("281_Skill_Lhand");
	}

	public void pet_bark()
	{
		Debug.Log("pet_bark 함수 실행");
		anim.Play("049_Expression_Bark");
		bgm_player_.dog_sound_excute();
	}

	public void pet_RTurn()
	{
		anim.Play("078_Idle_Right_Twirl");
	}

	public void pet_LTurn()
	{
		anim.Play("076_Idle_Left_Twirl");
	}

	public void pet_lay_begin ()
	{
		anim.Play("067_Idle_Blend_LieOnBack_1");
	}

	public void pet_lay_end()
	{
		anim.Play("086_LieOnBack_Blend_Idel_1");
	}

	public void pet_idle_lay_idle()
    {
		anim.speed = 2f;
		pet_lay_begin();
		Invoke("set_pet_speed_1", 3f);
		Invoke("pet_lay_end", 4f);
	}

	public void pet_come()
    {
		anim.Play("322_Walk_Slow");
		Invoke("Set_Pome_Idle", 3f);
	}

	public void play_anim_and_idel(string anim_name)
    {
		anim.Play(anim_name);
		Invoke("pet_idle", 4f);
	}
	
}
