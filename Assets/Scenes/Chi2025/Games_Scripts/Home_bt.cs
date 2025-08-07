using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Home_bt : MonoBehaviour
{
    public GameObject home_bt;
    Logger logger_script;

    private void Start()
    {
        logger_script = GameObject.Find("logger_gb").GetComponent<Logger>();
    }

    private void Update()
    {
        
    }

    public void home_bt_click__()
    {
        StartCoroutine(home_bt_click());
    }

    public IEnumerator home_bt_click()
    {
        //c1_script.re_init();
        //c2_script.sleep_bt_reset();
        //c3_script.re_init();
        //c4_script.re_init();

        logger_script.logger_master.insert_data("강아지 데려오기 버튼 클릭");
        yield return StartCoroutine(logger_script.logger_master.send_data_immediately());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }



}
