using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Quest_panel : MonoBehaviour
{
    string m_filePath;

    public GameObject feed_panel;
    public GameObject sleep_panel;
    public GameObject bath_panel;
    public GameObject intimate_panel;

    public GameObject weekly_panel_check;
    public TextMeshProUGUI weekly_panel_quest;
    public TextMeshProUGUI weekly_panel_cur_quest;

    Logger logger_script;

    // Start is called before the first frame update
    void Start()
    {
        m_filePath = Application.persistentDataPath + "/quest_daily.dat";
        logger_script = GameObject.Find("logger_gb").GetComponent<Logger>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void init_panel_daily(int feed_daily_Q, int sleep_daily_Q, int bath_daily_Q, int intimate_daily_Q,
                           int feed_daily_Q_cur, int sleep_daily_Q_cur, int  bath_daily_Q_cur, int intimate_daily_Q_cur)
    {
        //네모 투명박스 하루 할당 갯수만큼 디스플레이에 띄우기
        for(int i=1; i<=3; i++)
        {
            if (i > feed_daily_Q) feed_panel.gameObject.transform.GetChild(i).gameObject.SetActive(false);
            if (i > sleep_daily_Q) sleep_panel.gameObject.transform.GetChild(i).gameObject.SetActive(false);
            if (i > bath_daily_Q) bath_panel.gameObject.transform.GetChild(i).gameObject.SetActive(false);
            if (i > intimate_daily_Q) intimate_panel.gameObject.transform.GetChild(i).gameObject.SetActive(false);
        }

        //할당된 네모박스들 중에서 퀘스트 완료 된 만큼 체크표시
        for(int i=1; i<= feed_daily_Q; i++)
        {
            if (i > feed_daily_Q_cur) feed_panel.gameObject.transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }

        for (int i = 1; i <= sleep_daily_Q; i++)
        {
            if (i > sleep_daily_Q_cur) sleep_panel.gameObject.transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }

        for (int i = 1; i <= bath_daily_Q; i++)
        {
            if (i > bath_daily_Q_cur) bath_panel.gameObject.transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }

        for (int i = 1; i <= intimate_daily_Q; i++)
        {
            if (i > intimate_daily_Q_cur) intimate_panel.gameObject.transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }

        if(feed_daily_Q == feed_daily_Q_cur || sleep_daily_Q == sleep_daily_Q_cur ||
           bath_daily_Q == bath_daily_Q_cur || intimate_daily_Q == intimate_daily_Q_cur)
        {
            logger_script.logger_master.insert_data("일일 퀘스트 완료! 날짜: " + DateTime.Now.ToString());
        }
    }


    public bool check_panel_weekly(int weekly_quest_step, int weekly_caregame_cnt, string weekly_quest_msg)
    {
        bool result;
        weekly_panel_quest.text = weekly_quest_msg;

        //quest_step을 2로 나누기 했을 때 나머지가 0인 경우와 1인 경우로 나누어서
        if (weekly_quest_step % 2 == 0 && weekly_quest_step <= 8)//돌보기 게임 20회
        {
            weekly_panel_cur_quest.text = "현재 횟수: " + weekly_caregame_cnt.ToString() + "회";

            if (weekly_caregame_cnt >= 20)
            {
                //퀘스트 완료
                logger_script.logger_master.insert_data("주간 퀘스트 완료!");
                weekly_panel_check.SetActive(true);
                result = true;
            }
            else
            {
                //퀘스트 미완료
                weekly_panel_check.SetActive(false);
                result = false;
            }
        }
        else if (weekly_quest_step % 2 == 1 && weekly_quest_step <= 8)//강아지 성장
        {
            int target_pet_level;
            if (weekly_quest_step == 1) target_pet_level = 2;
            else if (weekly_quest_step == 3) target_pet_level = 3;
            else if (weekly_quest_step == 5) target_pet_level = 4;
            else if (weekly_quest_step == 7) target_pet_level = 5;
            else target_pet_level = 0;

            weekly_panel_cur_quest.text = "현재 단계: " + PlayerPrefs.GetInt("Level_pet").ToString() + "단계";

            if (PlayerPrefs.GetInt("Level_pet") > target_pet_level)
            {
                //퀘스트 완료
                weekly_panel_check.SetActive(true);
                result = true;
            }
            else
            {
                //퀘스트 미완료
                weekly_panel_check.SetActive(false);
                result = false;
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

}
