using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ctrl_statue : MonoBehaviour
{
    string ID;
    string PW;
    string PetName;


    public TMP_InputField ID_input;
    public TMP_InputField PW_input;
    public TMP_InputField PetName_input;
    public GameObject panel_hungry;
    public GameObject panel_sleep;
    public GameObject panel_bath;
    public GameObject panel_inti;
    public TextMeshProUGUI play_time_content;
    public TextMeshProUGUI play_time_Bt;
    public TextMeshProUGUI level_pet_text;

    int show_excuteTime_idx;


    Player_statu player_statu_script;

    // Start is called before the first frame update
    void Start()
    {
        show_excuteTime_idx = 0;
        player_statu_script = GameObject.Find("player_statu").GetComponent<Player_statu>();

        if (panel_hungry != null)
        {
            TMP_Text tmp_text_h = panel_hungry.gameObject.transform.GetChild(3).transform.GetComponent<TMP_Text>();
            Debug.Log(player_statu_script.Level_hungry.ToString());
            tmp_text_h.text = PlayerPrefs.GetInt("Level_hung").ToString();
        }
        if(panel_sleep != null) 
        {
            TMP_Text tmp_text_s = panel_sleep.gameObject.transform.GetChild(3).transform.GetComponent<TMP_Text>();
            Debug.Log(player_statu_script.Level_sleep.ToString());
            tmp_text_s.text = PlayerPrefs.GetInt("Level_slep").ToString();

        }
        if(panel_bath != null)
        {
            TMP_Text tmp_text_b = panel_bath.gameObject.transform.GetChild(3).transform.GetComponent<TMP_Text>();
            Debug.Log(player_statu_script.Level_bath.ToString());
            tmp_text_b.text = PlayerPrefs.GetInt("Level_bath").ToString();
        }
        if(panel_inti != null)
        {
            TMP_Text tmp_text_i = panel_inti.gameObject.transform.GetChild(3).transform.GetComponent<TMP_Text>();
            Debug.Log(player_statu_script.Level_intimity.ToString());
            tmp_text_i.text = PlayerPrefs.GetInt("Level_inti").ToString();
        }

        if(ID_input != null && PW_input != null && PetName_input != null && level_pet_text != null)
        {
            ID = player_statu_script.ID;
            PW = player_statu_script.Password;
            PetName = player_statu_script.PetName;
            ID_input.text = PlayerPrefs.GetString("ID");
            PW_input.text = PlayerPrefs.GetString("Password");
            PetName_input.text = PlayerPrefs.GetString("PetName");

            level_pet_text.text = PlayerPrefs.GetInt("Level_pet").ToString();
        }

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void show_play_time()
    {
        play_time_Bt.text = "다음";

        string Jpath_date = System.IO.Path.Combine(Application.persistentDataPath, "Jdata_date.json");
        Js_data js_data = new Js_data();
        string loadjson = System.IO.File.ReadAllText(Jpath_date);
        js_data = JsonUtility.FromJson<Js_data>(loadjson);

        int max_idx = js_data.excuteTime_per_date.Count - 1;
        play_time_content.text = js_data.excuteTime_per_date[show_excuteTime_idx];
        if(show_excuteTime_idx < max_idx)
        {
            show_excuteTime_idx += 1;
        }
        else
        {
            show_excuteTime_idx = 0;
        }
    }

    public void leve_pet_bt_clicked()
    {
        int lv_pet = PlayerPrefs.GetInt("Level_pet");
        string level_text = PlayerPrefs.GetInt("Level_pet").ToString();

        if(lv_pet < 5)
        {
            PlayerPrefs.SetInt("Level_pet", lv_pet + 1);
            level_pet_text.text = (lv_pet + 1).ToString();
        }
        else
        {
            PlayerPrefs.SetInt("Level_pet", 1);
            level_pet_text.text = "1";
        }
    }

    public void inc_bt_clicked_hungry()
    {
        TMP_Text tmp_text = panel_hungry.gameObject.transform.GetChild(3).transform.GetComponent<TMP_Text>();
        string num_text = tmp_text.text;        
        int level = int.Parse(num_text);
        if (level < 3) level += 1;

        player_statu_script.Level_hungry = level;
        PlayerPrefs.SetInt("Level_hung", player_statu_script.Level_hungry);
        tmp_text.text = level.ToString();
    }

    public void dec_bt_clicked_hungry()
    {
        TMP_Text tmp_text = panel_hungry.gameObject.transform.GetChild(3).transform.GetComponent<TMP_Text>();
        string num_text = tmp_text.text;
        int level = int.Parse(num_text);
        if(level > 1) level -= 1;

        player_statu_script.Level_hungry = level;
        PlayerPrefs.SetInt("Level_hung", player_statu_script.Level_hungry);
        tmp_text.text = level.ToString();
    }

    public void inc_bt_clicked_sleep()
    {
        TMP_Text tmp_text = panel_sleep.gameObject.transform.GetChild(3).transform.GetComponent<TMP_Text>();
        string num_text = tmp_text.text;
        int level = int.Parse(num_text);
        if (level < 3) level += 1;

        player_statu_script.Level_sleep = level;
        PlayerPrefs.SetInt("Level_slep", player_statu_script.Level_sleep);
        tmp_text.text = level.ToString();
    }

    public void dec_bt_clicked_sleep()
    {
        TMP_Text tmp_text = panel_sleep.gameObject.transform.GetChild(3).transform.GetComponent<TMP_Text>();
        string num_text = tmp_text.text;
        int level = int.Parse(num_text);
        if (level > 1) level -= 1;

        player_statu_script.Level_sleep = level;
        PlayerPrefs.SetInt("Level_slep", player_statu_script.Level_sleep);
        tmp_text.text = level.ToString();
    }
    public void inc_bt_clicked_bath()
    {
        TMP_Text tmp_text = panel_bath.gameObject.transform.GetChild(3).transform.GetComponent<TMP_Text>();
        string num_text = tmp_text.text;
        int level = int.Parse(num_text);
        if (level < 3) level += 1;

        player_statu_script.Level_bath = level;
        PlayerPrefs.SetInt("Level_bath", player_statu_script.Level_bath);
        tmp_text.text = level.ToString();
    }

    public void dec_bt_clicked_bath()
    {
        TMP_Text tmp_text = panel_bath.gameObject.transform.GetChild(3).transform.GetComponent<TMP_Text>();
        string num_text = tmp_text.text;
        int level = int.Parse(num_text);
        if (level > 1) level -= 1;

        player_statu_script.Level_bath = level;
        PlayerPrefs.SetInt("Level_bath", player_statu_script.Level_bath);
        tmp_text.text = level.ToString();
    }

    public void inc_bt_clicked_inti()
    {
        TMP_Text tmp_text = panel_inti.gameObject.transform.GetChild(3).transform.GetComponent<TMP_Text>();
        string num_text = tmp_text.text;
        int level = int.Parse(num_text);
        if (level < 3) level += 1;

        player_statu_script.Level_bath = level;
        PlayerPrefs.SetInt("Level_inti", player_statu_script.Level_bath);
        tmp_text.text = level.ToString();
    }

    public void dec_bt_clicked_inti()
    {
        TMP_Text tmp_text = panel_inti.gameObject.transform.GetChild(3).transform.GetComponent<TMP_Text>();
        string num_text = tmp_text.text;
        int level = int.Parse(num_text);
        if (level > 1) level -= 1;

        player_statu_script.Level_bath = level;
        PlayerPrefs.SetInt("Level_inti", player_statu_script.Level_bath);
        tmp_text.text = level.ToString();
    }

    public void save_bt_clicked()
    {
        if(ID_input.text == "" || PW_input.text == "" || PetName_input.text == "")
        {
            Debug.Log("ID, PW, Petname을 정확하게 적어주세요");
            return;
        }
        else
        {
            PlayerPrefs.SetString("ID", ID_input.text);
            PlayerPrefs.SetString("Password", PW_input.text);
            PlayerPrefs.SetString("PetName", PetName_input.text);
        }

        SceneManager.LoadScene("00_LoginPage_MZ");
    }

    public void coin_10000()
    {
        PlayerPrefs.SetInt("Coin", 10000);
    }

    public void plus_statu()
    {

    }

    public void minus_statu()
    {

    }

}
