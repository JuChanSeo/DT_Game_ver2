using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


[Serializable]
public class weekly_quest
{
    public DateTime Nowtime;
    public int quest_step;
    public string quest_str;
    public int care_game_cnt;
    public bool complete;

    public void init_quest(int q_step, string msg)
    {
        Nowtime = DateTime.Now;
        quest_step = q_step;//playerprefs 활용?
        quest_str = msg;
        care_game_cnt = 0;
        complete = false;
    }

}


public class QuestManager_weekly : MonoBehaviour
{
    private string m_filePath;
    weekly_quest weekly_q = new weekly_quest();
    private BinaryFormatter binaryform = new BinaryFormatter();
    Quest_panel quest_panel_script;
    Logger logger_script;
    List<string> quest_str_list = new List<string>() {
        "먹이주기 게임 20회 이상 완료", //0
        "강아지 2단계로 성장시키기",//1
        "잠자기 게임 20회 이상 완료",//2
        "강아지 3단계로 성장시키기",//3
        "목욕하기 게임 20회 이상 완료",//4
        "강아지 4단계로 성장시키기",//5
        "친해지기 게임 20회 이상 완료",//6
        "강아지 5단계로 성장시키기",//7
        "산책하기 게임 20회 이상 완료",//8 
        "", "", "", "", "", "", "", "",
        "", "", "", "", "", "", "", "",
        "", "", "", "", "", "", "", "",};

    // Start is called before the first frame update
    void Start()
    {
        m_filePath = Application.persistentDataPath + "/quest_weekly.dat";
        logger_script = GameObject.Find("logger_gb").GetComponent<Logger>();
        //init_quest(0, quest_str_list[0]);
        if (!File.Exists(m_filePath))
        {
            init_quest(0, "먹이주기 게임 20회이상 완료");
            return;
        }
        else
        {
            LoadData();
        }

        if (!SceneManager.GetActiveScene().name.Contains("Login")) return;

        Debug.Log("weekly 현황: " + weekly_q.quest_step.ToString() + "\t" + weekly_q.quest_str +
        "\t" + weekly_q.care_game_cnt + "\t" + weekly_q.complete);

        //7일이 지나면 퀘스트를 초기화 한다.
        TimeSpan TimeCal = DateTime.Now - weekly_q.Nowtime;
        if (TimeCal.Days >= 7)
        {
            Debug.Log("퀘스트가 초기화 되었습니다.");
            weekly_q.quest_step++;
            logger_script.logger_master.insert_data("주간 퀘스트 초기화. 업데이트된 퀘스트: " + quest_str_list[weekly_q.quest_step]);
            init_quest(weekly_q.quest_step, quest_str_list[weekly_q.quest_step]);
            SaveData();
        }
        else
        {
            Debug.Log("주간 퀘스트 초기화까지 " + (7 - TimeCal.Days).ToString() + "일 남았습니다.");
        }

        //panel에 할당된 퀘스트 갯수와, 성공한 퀘스트 갯수를 표시 해준다.
        if (GameObject.Find("Quest_panel_control") != null)
        {
            quest_panel_script = GameObject.Find("Quest_panel_control").GetComponent<Quest_panel>();
            bool res = quest_panel_script.check_panel_weekly(weekly_q.quest_step, weekly_q.care_game_cnt, quest_str_list[weekly_q.quest_step]);
            weekly_q.complete = res;
            SaveData();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void init_quest(int cnt, string str)
    {

        weekly_q.init_quest(cnt, str);
        SaveData();

    }


    public void SaveData()
    {
        try
        {
            using (Stream ws = new FileStream(m_filePath, FileMode.Create))
            {
                binaryform.Serialize(ws, weekly_q);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    public void LoadData()
    {
        using (Stream rs = new FileStream(m_filePath, FileMode.Open))
        {
            weekly_q = (weekly_quest)binaryform.Deserialize(rs);
        }
    }

    public void caregame_plus(string gametype)
    {
        LoadData();
        if(gametype == "feed")
        {
            if(weekly_q.quest_step == 0)
                weekly_q.care_game_cnt += 1;
        }
        else if(gametype == "sleep")
        {
            if (weekly_q.quest_step == 2)
                weekly_q.care_game_cnt += 1;
        }
        else if(gametype == "bath")
        {
            if (weekly_q.quest_step == 4)
                weekly_q.care_game_cnt += 1;
        }
        else if(gametype == "intimate")
        {
            if (weekly_q.quest_step == 6)
                weekly_q.care_game_cnt += 1;
        }
        else if(gametype == "walking")
        {
            if (weekly_q.quest_step == 8)
                weekly_q.care_game_cnt += 1;
        }
        SaveData();
    }




}
 