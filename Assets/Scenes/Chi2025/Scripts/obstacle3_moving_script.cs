using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class obstacle3_moving_script : MonoBehaviour
{
    public GameManager gameManger_script;
    public shooting_obstacle shooting_script;
    public float velocity;
    Logger logger_script;

    // Start is called before the first frame update
    void Start()
    {
        gameManger_script = GameObject.Find("agility_game").GetComponent<GameManager>();
        shooting_script = GameObject.Find("agility_game").GetComponent<shooting_obstacle>();

        gameObject.transform.position = GameObject.Find("pomeLV0"+PlayerPrefs.GetInt("Level_pet").ToString()).transform.position
                                        + 12f * Vector3.right + 0f * Vector3.down + 0f*Vector3.back;
        var rigid = GetComponent<Rigidbody>();
        //rigid.velocity = new Vector3(-0.4f, 0, 0);
        rigid.linearVelocity = new Vector3(-2f, 0, 0);
        //Invoke("check_color", shooting_script.time_cube_create - 1f);//끝나기 1초전에 체크하고 있었네

        logger_script = GameObject.Find("logger_gb").GetComponent<Logger>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        gameObject.GetComponent<Renderer>().material.color = Color.red;

    }

    public void check_color()
    {
        if(gameObject.GetComponent<Renderer>().material.color == Color.red)
        {
            if (!SceneManager.GetActiveScene().name.Contains("tutorial"))
                logger_script.logger_master.insert_data("장애물 넘기 실패");
            gameManger_script.cnt_fail += 1;
            gameManger_script.succes_or_fail = false;
            Debug.Log("실패!");
        }
        else
        {
            if (!SceneManager.GetActiveScene().name.Contains("tutorial"))
                logger_script.logger_master.insert_data("장애물 넘기 성공");
            gameManger_script.cnt_succes += 1;
            gameManger_script.succes_or_fail = true;
            Debug.Log("성공!");
        }

        gameManger_script.show_game_log();

    }
}
