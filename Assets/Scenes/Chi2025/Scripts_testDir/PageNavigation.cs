using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PageNavigation : MonoBehaviour
{
    Logger logger_script;
    private void Start()
    {
        logger_script = GameObject.Find("logger_gb").GetComponent<Logger>();
    }

    public void main__()
    {
        StartCoroutine(main());
    }

    public IEnumerator main()
    {
        logger_script.logger_master.insert_data("뒤로가기 버튼 클릭");
        yield return StartCoroutine(logger_script.logger_master.send_data_immediately());
        SceneManager.LoadScene("00_LoginPage_MZ");
    }

    public void Statue()
    {
        SceneManager.LoadScene("01_Statue");
    }

    public void AR_interaction__()
    {
        StartCoroutine(AR_interaction());
    }

    public IEnumerator AR_interaction()
    {
        logger_script.logger_master.insert_data("10_AR_interaction_MZ 페이지 Load");
        yield return StartCoroutine(logger_script.logger_master.send_data_immediately());
        SceneManager.LoadScene("10_AR_interaction_MZ");
    }

    public void game_feed__()
    {
        StartCoroutine(game_feed());
    }

    public IEnumerator game_feed()
    {
        logger_script.logger_master.insert_data("먹이주기 게임으로 바로 넘어가기 버튼 클릭");
        yield return StartCoroutine(logger_script.logger_master.send_data_immediately());
        SceneManager.LoadScene("20_Virtual__feeding");
    }

    public void game_sleep__()
    {
        StartCoroutine(game_sleep());
    }

    public IEnumerator game_sleep()
    {
        logger_script.logger_master.insert_data("잠자기 게임으로 바로 넘어가기 버튼 클릭");
        yield return StartCoroutine(logger_script.logger_master.send_data_immediately());
        SceneManager.LoadScene("21_Virtual__sleeping");
    }

    public void game_wash__()
    {
        StartCoroutine(game_wash());
    }

    public IEnumerator game_wash()
    {
        logger_script.logger_master.insert_data("목욕하기 게임으로 바로 넘어가기 버튼 클릭");
        yield return StartCoroutine(logger_script.logger_master.send_data_immediately());
        SceneManager.LoadScene("22_Virtual__washing");
    }

    public void game_intimate__()
    {
        StartCoroutine(game_intimate());
    }

    public IEnumerator game_intimate()
    {
        logger_script.logger_master.insert_data("친해지기 게임으로 바로 넘어가기 버튼 클릭");
        yield return StartCoroutine(logger_script.logger_master.send_data_immediately());
        SceneManager.LoadScene("23_Virtual_intimating");
    }

    public void game_agility__()
    {
        StartCoroutine(agility_main());
    }

    public IEnumerator agility_main()
    {
        logger_script.logger_master.insert_data("산책하기 게임으로 바로 넘어가기 버튼 클릭");
        yield return StartCoroutine(logger_script.logger_master.send_data_immediately());
        SceneManager.LoadScene("24_Virtual_agility");
    }

    public void game_feed_tutorial__()
    {
        StartCoroutine(game_feed_tutorial());
    }

    public IEnumerator game_feed_tutorial()
    {
        logger_script.logger_master.insert_data("먹이주기 버튼 클릭");
        yield return StartCoroutine(logger_script.logger_master.send_data_immediately());
        SceneManager.LoadScene("20_Virtual__feeding_tutorial");
    }

    public void game_sleep_tutorial__()
    {
        StartCoroutine(game_sleep_tutorial());
    }

    public IEnumerator game_sleep_tutorial()
    {
        logger_script.logger_master.insert_data("잠자기 버튼 클릭");
        yield return StartCoroutine(logger_script.logger_master.send_data_immediately());
        SceneManager.LoadScene("21_Virtual__sleeping_tutorial");
    }

    public void game_wash_tutorial__()
    {
        StartCoroutine(game_wash_tutorial());
    }

    public IEnumerator game_wash_tutorial()
    {
        logger_script.logger_master.insert_data("목욕하기 버튼 클릭");
        yield return StartCoroutine(logger_script.logger_master.send_data_immediately());
        SceneManager.LoadScene("22_Virtual__washing_tutorial");
    }

    public void game_intimate_tutorial__()
    {
        StartCoroutine(game_intimate_tutorial());
    }

    public IEnumerator game_intimate_tutorial()
    {
        logger_script.logger_master.insert_data("친해지기 버튼 클릭");
        yield return StartCoroutine(logger_script.logger_master.send_data_immediately());
        SceneManager.LoadScene("23_Virtual_intimating_tutorial");
    }

    public void game_agility_tutorial__()
    {
        StartCoroutine(agility_tutorial());
    }

    public IEnumerator agility_tutorial()
    {
        logger_script.logger_master.insert_data("산책하기 버튼 클릭");
        yield return StartCoroutine(logger_script.logger_master.send_data_immediately());
        SceneManager.LoadScene("24_Virtual_agility_tutorial");
    }

    public void agility_Gesture()
    {
        SceneManager.LoadScene("Agility_G");
    }

    public void agility_Gesture_Voice()
    {
        SceneManager.LoadScene("Agility_G_V");
    }
    public void agility_Gesture_Face()
    {
        SceneManager.LoadScene("Agility_G_F");
    }

    public void test_scene_0()
    {
        SceneManager.LoadScene("test_scene_0");
    }

    public void agility_Touch()
    {
        SceneManager.LoadScene("Agility_T");
    }

    public void agility_Touch_Voice()
    {
        SceneManager.LoadScene("Agility_T_V");
    }

    public void agility_Touch_Face()
    {
        SceneManager.LoadScene("Agility_F");
    }

    public void agility_Voice()
    {
        SceneManager.LoadScene("Agility_V");
    }

    public void agility_Face()
    {
        SceneManager.LoadScene("Agility_F");
    }

    public void base_interaction()
    {
        SceneManager.LoadScene("base_interaction");
    }

    public void Agility()
    {
        SceneManager.LoadScene("agility_touch");
    }

    public void quit_app()
    {
        Application.Quit();
    }

}
