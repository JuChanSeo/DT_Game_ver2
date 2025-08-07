using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System;

[Serializable]
public class Js_data
{
    public string saved_date;
    public List<string> excuteTime_per_date;
}

public class Player_statu : MonoBehaviour
{
    string Jpath_date;

    public string PetName;
    public string ID;
    public string Password;

    public int Level_pet;
    public int Level_hungry;
    public int Level_sleep;
    public int Level_bath;
    public int Level_intimity;
    public int Level_c4;
    public int Coin;
    public float energy;
    public float fatigue;
    public float intimity;
    public float cleanliness;
    public float exp;
    public float accumlated_time;

    
    public float min_y;
    int set_level_to_1;
    bool add_flag;
    float accumlated_delta;

    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        //Debug.Log(gameObject.name);
        //if (gameObject.transform.parent.name != "DontDestroyOnLoad") DontDestroyOnLoad(gameObject);
        //DontDestroyOnLoad(gameObject);
        //Debug.Log(gameObject.transform.parent.name);

        //Load from json
        //Load from playerprefs
        Level_pet = PlayerPrefs.GetInt("Level_pet");
        Level_hungry = PlayerPrefs.GetInt("Level_hung");
        Level_sleep = PlayerPrefs.GetInt("Level_slep");
        Level_bath = PlayerPrefs.GetInt("Level_bath");
        Level_intimity = PlayerPrefs.GetInt("Level_inti");
        PetName = PlayerPrefs.GetString("PetName");
        ID = PlayerPrefs.GetString("ID");
        Password = PlayerPrefs.GetString("Password");
        Coin = PlayerPrefs.GetInt("Coin");
        energy = PlayerPrefs.GetFloat("energy"); // 높으면 좋은 것
        fatigue = PlayerPrefs.GetFloat("fatigue");  // 낮으면 좋은 것
        intimity = PlayerPrefs.GetFloat("intimity"); // 높으면 좋은 것
        cleanliness = PlayerPrefs.GetFloat("cleanliness"); // 높으면 좋은것
        exp = PlayerPrefs.GetFloat("exp"); //경험치
        set_level_to_1 = PlayerPrefs.GetInt("set_level_to_1");
        //PlayerPrefs.SetFloat("accumlated_time", 0);
        accumlated_time = PlayerPrefs.GetFloat("accumlated_time");

        Debug.Log(PetName + "\t" + ID + "\t" + Password + "\t" + energy + "\t" + fatigue);
        //Debug.Log(Level_hungry + "\t" + Level_sleep + "\t" + Level_bath);
        if (set_level_to_1 == 0)
        {
            Level_pet = 1;
            Level_hungry = 1;
            Level_sleep = 1;
            Level_bath = 1;
            Level_intimity = 1;
            PlayerPrefs.SetInt("Level_pet", 1);
            PlayerPrefs.SetInt("Level_hung", 1);
            PlayerPrefs.SetInt("Level_slep", 1);
            PlayerPrefs.SetInt("Level_bath", 1);
            PlayerPrefs.SetInt("Level_inti", 1);
            PlayerPrefs.SetInt("set_level_to_1", 1);

            Debug.Log("level initialize");
        }

        if((ID == "" || Password == "" || PetName == "") && (SceneManager.GetActiveScene().name == "00_LoginPage_MZ"))
        {
            SceneManager.LoadScene("01_Statue_MZ");
        }

        string json = JsonUtility.ToJson(this);
        Debug.Log(json);
        accumlated_delta = 0;
        Jpath_date = Path.Combine(Application.persistentDataPath, "Jdata_date.json");
        Jload();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Chapter1")
        {
        }
    }

    public void print_statue()
    {
        string json = JsonUtility.ToJson(this);
        Debug.Log(json);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void check_the_level()
    {
        if(true) //어떤 조건이 만족되면 pet의 level을 올려 준다.
        {
            this.Level_pet += 1;
            PlayerPrefs.SetInt("Level_pet", Level_pet);
        }
    }
    // Update is called once per frame

    public void change_statu(float _energy, float _fatigue, float _cleanliness, float _intimity)
    {
        if(energy + _energy < 0)
        {
            energy = 0;
        }
        else if (energy + _energy > 1)
        {
            energy = 1;
        }
        else
        {
            energy += _energy;
        }

        if(fatigue + _fatigue < 0)
        {
            fatigue = 0;
        }
        else if(fatigue + _fatigue > 1)
        {
            fatigue = 1;
        }
        else
        {
            fatigue += _fatigue;
        }

        if (cleanliness + _cleanliness < 0)
        {
            cleanliness = 0;
        }
        else if (cleanliness + _cleanliness > 1)
        {
            cleanliness = 1;
        }
        else
        {
            cleanliness += _cleanliness;
        }

        if (intimity + _intimity < 0)
        {
            intimity = 0;
        }
        else if (intimity + _intimity > 1)
        {
            intimity = 1;
        }
        else
        {
            intimity += _intimity;
        }

        PlayerPrefs.SetFloat("energy", energy);
        PlayerPrefs.SetFloat("fatigue", fatigue);
        PlayerPrefs.SetFloat("intimity", intimity);
        PlayerPrefs.SetFloat("cleanliness", cleanliness);
    }

    public int choose_higlight()
    {
        float min;
        int min_idx = 0; //enegry:0, fatigue:1, cleanliness:2, intimity:3
        float[] arr = { 0, 0, 0, 0 };
        arr[0] = energy;
        arr[1] = 1 - fatigue;
        arr[2] = intimity;
        arr[3] = cleanliness;

        min = arr[0];
        for(int i = 1; i < 4; i++)
        {
            if(min > arr[i])
            {
                min = arr[i];
                min_idx = i;
            }
        }        
        return min_idx; //enegry:0, fatigue:1, cleanliness:2, intimity:3
    }

    public void check_info()
    {

        PageNavigation pageNavigation_script;
        pageNavigation_script = GameObject.Find("Manager").GetComponent<PageNavigation>();
        //pageNavigation_script.Chapter5();

    }

    void Update()
    {
        if (Math.Round(Time.time) % 5 == 0)
        {
            if (add_flag == true)
            {
                PlayerPrefs.SetFloat("accumlated_time", PlayerPrefs.GetFloat("accumlated_time") + 5);
                add_flag = false;
                //Debug.Log("accumlated_time: " + PlayerPrefs.GetFloat("accumlated_time") + "\t" +
                //          "현재: " + Math.Round(Time.time));
            }
        }
        else
        {
            add_flag = true;
        }

    }

    public void Jsave()
    {

    }

    public void Jload()
    {
        Js_data js_data = new Js_data();

        if(!File.Exists(Jpath_date))
        {
            js_data.saved_date = DateTime.Now.ToString("MM/dd");
            js_data.excuteTime_per_date = new List<string>();
            string json = JsonUtility.ToJson(js_data, true);
            File.WriteAllText(Jpath_date, json);
            Debug.Log(Jpath_date + "\t" + js_data.saved_date);
        }
        else
        {
            JSave();
        }
    }

    public void JSave()
    {
        Js_data js_data = new Js_data();
        string loadjson = File.ReadAllText(Jpath_date);
        js_data = JsonUtility.FromJson<Js_data>(loadjson);

        if (js_data.saved_date != DateTime.Now.ToString("MM/dd"))
        {
            js_data.excuteTime_per_date.Add(js_data.saved_date + " 실행시간: "
                                              + Math.Truncate(PlayerPrefs.GetFloat("accumlated_time") / 60).ToString() + "분 "
                                              + (PlayerPrefs.GetFloat("accumlated_time") % 60).ToString() + "초");
            js_data.saved_date = DateTime.Now.ToString("MM/dd");
            PlayerPrefs.SetFloat("accumlated_time", 0);

        }

        string json = JsonUtility.ToJson(js_data, true);
        File.WriteAllText(Jpath_date, json);
    }

    public void Jsend()
    {
        //httprequest library 사용
    }


}
