using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacle1_moving_script : MonoBehaviour
{
    public GameManager gameManger_script;
    public shooting_obstacle shooting_script;
    public float velocity;

    // Start is called before the first frame update
    void Start()
    {
        gameManger_script = GameObject.Find("agility_game").GetComponent<GameManager>();
        shooting_script = GameObject.Find("agility_game").GetComponent<shooting_obstacle>();

        gameObject.transform.position = GameObject.Find("pomeLV05").transform.position
                                        + 2f * Vector3.right + 0.07f * Vector3.up + 0.05f*Vector3.forward;
        var rigid = GetComponent<Rigidbody>();
        rigid.linearVelocity = new Vector3(-0.4f, 0, 0);
        //rigid.velocity = new Vector3(0, 0, -0.4f);
        Invoke("check_color", shooting_script.time_cube_create - 1f);//끝나기 1초전에 체크하고 있었네
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        gameObject.GetComponent<Renderer>().material.color = Color.red;

    }

    private void check_color()
    {
        if(gameObject.GetComponent<Renderer>().material.color == Color.red)
        {
            gameManger_script.cnt_fail += 1;
            gameManger_script.succes_or_fail = false;
            Debug.Log("실패!");
        }
        else
        {
            gameManger_script.cnt_succes += 1;
            gameManger_script.succes_or_fail = true;
            Debug.Log("성공!");
        }

    }
}
