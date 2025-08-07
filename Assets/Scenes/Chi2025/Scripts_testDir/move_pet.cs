using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class move_pet: MonoBehaviour
{
    //public TextMeshProUGUI text_pome_loc;
    public GameManager gameManger_script;
    public interact_pet interact_pet_script;
    Rigidbody rgbody;
    int cnt_mission;

    // Start is called before the first frame update
    void Start()
    {
        rgbody = transform.GetComponent<Rigidbody>();
        interact_pet_script = transform.GetComponent<interact_pet>();
        //gameManger_script = GameObject.Find("agility_game").GetComponent<GameManager>();
        if(SceneManager.GetActiveScene().name == "base_interaction")
        {
            gameObject.transform.Rotate(Vector3.up * 180);
            gameObject.transform.Rotate(Vector3.right * 0);
        }
        else if(SceneManager.GetActiveScene().name == "agility_touch")
        {
            gameObject.transform.Rotate(Vector3.right * 10);
            gameObject.transform.Rotate(Vector3.up * 90);
            gameObject.transform.Rotate(Vector3.back * 20);
        }
        //gameObject.transform.Rotate(Vector3.up * 28);
        //gameObject.transform.Rotate(Vector3.right * 73);
        //gameObject.transform.Rotate(Vector3.forward * 129);


        for(int i = 3; i<gameObject.transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.SetActive(false);
        }

        cnt_mission = 0;
        //invoke_walk_after_3sec();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(gameObject.transform.position);
        //text_pome_loc.text = gameObject.transform.localPosition.ToString();
        //text_pome_loc.text = gameObject.transform.position.ToString();
    }

    public void invoke_walk_after_3sec()
    {
        Invoke("walk_rotate_pet", 3f);
    }

    public void walk_rotate_pet()
    {//5초 후 idel, 미션 실행
        transform.eulerAngles = new Vector3(0,90,0);
        interact_pet_script.anim.Play("Walk_ahead");
        rgbody.linearVelocity = new Vector3(1f, 0, 0);
        Invoke("idle_stop_pet", 3.5f);
    }

    public void idle_stop_pet()
    {
        transform.eulerAngles = Vector3.zero;
        interact_pet_script.anim.Play("Idle");
        rgbody.linearVelocity = Vector3.zero;
        if (cnt_mission == 0) interact_pet_script.ballplay_bt_clicked();
        else if (cnt_mission == 1) interact_pet_script.face_bt_click();
        else if (cnt_mission == 2) interact_pet_script.bool_virtual_hang();
        else if (cnt_mission == 3) interact_pet_script.feed_bt_click();
        cnt_mission++;
    }

    public void move_plus()
    {
        gameObject.transform.position += Vector3.forward * 0.1f;
    }

    public void move_minus()
    {
        gameObject.transform.position += Vector3.back * 0.1f;
    }

    public void move_up()
    {
        gameObject.transform.position += Vector3.up * 0.01f;
    }

    public void move_down()
    {
        gameObject.transform.position += Vector3.down * 0.01f;
    }

    public void scale_up()
    {
        gameObject.transform.localScale += new Vector3(1, 1, 1)*0.02f;
    }

    public void scale_down()
    {
        gameObject.transform.localScale -= new Vector3(1, 1, 1) * 0.02f;
    }

    public void rot_y()
    {
        Debug.Log("rotation 실행");
        gameObject.transform.Rotate(Vector3.up*10);
    }

    public void rot_x()
    {
        gameObject.transform.Rotate(Vector3.right * 10);
    }

    private void OnTriggerEnter(Collider other)
    {
    }
}
