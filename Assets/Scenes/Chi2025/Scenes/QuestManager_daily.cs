using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.EventSystems;


[Serializable]
public class daily_quest
{
    public int q_feed;
    public int q_sleep;
    public int q_bath;
    public int q_intimate;
    public int day_created;

    public int q_feed_cur;
    public int q_sleep_cur;
    public int q_bath_cur;
    public int q_intimate_cur;

    public bool complete_quest;

    public void assign_num()
    {
        q_feed = UnityEngine.Random.Range(1, 4);
        q_sleep = UnityEngine.Random.Range(1, 4);
        q_bath = UnityEngine.Random.Range(1, 4);
        q_intimate = UnityEngine.Random.Range(1, 4);
        day_created = DateTime.Now.Day;

        q_feed_cur = 0;
        q_sleep_cur = 0;
        q_bath_cur = 0;
        q_intimate_cur = 0;
        Debug.Log(day_created+"\t in assign_num");

        complete_quest = false;
    }
}

 
public class QuestManager_daily : MonoBehaviour
{

    daily_quest daily_q = new daily_quest();
    Quest_panel quest_panel_script;
    private string m_filePath;
    private BinaryFormatter binaryform = new BinaryFormatter();
    public GameObject Quest_complete_panel;
    Logger logger_script;

    // Start is called before the first frame update
    void Start()
    {
        if(Quest_complete_panel != null) Quest_complete_panel.SetActive(false);
        m_filePath = Application.persistentDataPath + "/quest_daily.dat";
        logger_script = GameObject.Find("logger_gb").GetComponent<Logger>();
        //퀘스트 관리 파일이 없을 경우 만들어 주고 초기화 한다.
        if (!File.Exists(m_filePath))
        {
            init_quest();
            return;
        }
        //파일이 있을 경우 불러온다.
        else
        {
            LoadData();

            if(daily_q.q_feed == daily_q.q_feed_cur &&
               daily_q.q_sleep == daily_q.q_sleep_cur &&
               daily_q.q_bath == daily_q.q_bath_cur &&
               daily_q.q_intimate == daily_q.q_intimate_cur &&
               !daily_q.complete_quest)
            {
                daily_q.complete_quest = true;
                SaveData();
                //save를 해줘야지...
                Debug.Log("일일 퀘스트 완료!");
                if (Quest_complete_panel != null) Quest_complete_panel.SetActive(true);
                PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") + 5);
                Invoke("quest_complete_panel_disappear", 5f);
            }

        }

        //퀘스트 관리 파일에 기록된 날짜와 오늘 날짜가 다르면 초기화 한.
        if(daily_q.day_created != DateTime.Now.Day)
        {
            logger_script.logger_master.insert_data("일일 퀘스트 초기화");
            Debug.Log("daily quest reload");
            init_quest();
        }

        //panel에 할당된 퀘스트 갯수와, 성공한 퀘스트 갯수를 표시 해준다.
        if(GameObject.Find("Quest_panel_control") != null)
        {
            quest_panel_script = GameObject.Find("Quest_panel_control").GetComponent<Quest_panel>();
            quest_panel_script.init_panel_daily(daily_q.q_feed, daily_q.q_sleep, daily_q.q_bath, daily_q.q_intimate,
                                          daily_q.q_feed_cur, daily_q.q_sleep_cur, daily_q.q_bath_cur, daily_q.q_intimate_cur);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void quest_complete_panel_disappear()
    {
        if (Quest_complete_panel != null) Quest_complete_panel.SetActive(false);
    }


    public void init_quest()
    {
        //daily_q에 돌보기 별 랜덤 숫자를 부여하고, 횟수를 모두 0으로 초기화 한다.(날짜도 초기화)
        daily_q.assign_num();
        SaveData();

    }

    public void set_checkbox()
    {

    }

    public void SaveData()
    {
        try
        {
            using (Stream ws = new FileStream(m_filePath, FileMode.Create))
            {
                binaryform.Serialize(ws, daily_q);
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
            daily_q = (daily_quest)binaryform.Deserialize(rs);
        }
        Debug.Log("load_daily_quest 실행");
        Debug.Log(daily_q.q_feed + "\t" + daily_q.q_sleep + "\t" + daily_q.q_bath + "\t" + daily_q.q_intimate);
    }

    public void feed_plus()
    {
        LoadData();
        if (daily_q.q_feed_cur < daily_q.q_feed) daily_q.q_feed_cur += 1;
        SaveData();
    }

    public void sleep_plus()
    {
        LoadData();
        if (daily_q.q_sleep_cur < daily_q.q_sleep) daily_q.q_sleep_cur += 1;
        SaveData();
    }

    public void bath_plus()
    {
        LoadData();
        if (daily_q.q_bath_cur < daily_q.q_bath) daily_q.q_bath_cur += 1;
        SaveData();
    }

    public void intimate_plus()
    {
        LoadData();
        if (daily_q.q_intimate_cur < daily_q.q_intimate) daily_q.q_intimate_cur += 1;
        SaveData();
    }

    public static int[] MakeRandomNumbers(int maxValue, int randomSeed = 0)
    {
        return MakeRandomNumbers(0, maxValue, randomSeed);
    }
    public static int[] MakeRandomNumbers(int minValue, int maxValue, int randomSeed = 0)
    {
        if (randomSeed == 0)
            randomSeed = (int)DateTime.Now.Ticks;

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

