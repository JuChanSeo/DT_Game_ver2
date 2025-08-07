using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class mainPage_milestone : MonoBehaviour
{
    public TMP_InputField inputf_N;
    public Slider time_lim_slider;
    public TextMeshProUGUI time_lim_text;


    // Start is called before the first frame update
    void Start()
    {
        time_lim_slider.value = PlayerPrefs.GetFloat("milestone_time_lim");
        Debug.Log("저장된 시간: "+ PlayerPrefs.GetFloat("milestone_time_lim"));
        if (PlayerPrefs.GetFloat("milestone_time_lim") == 0)
        {
            Debug.Log("시간 최초세팅!");
            PlayerPrefs.SetFloat("milestone_time_lim", 10f);
        }
        time_lim_slider.value = PlayerPrefs.GetFloat("milestone_time_lim");
        time_lim_text.text = time_lim_slider.value.ToString("n2");


        inputf_N.text = PlayerPrefs.GetInt("milestone_pnum").ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void save_bt_click()
    {
        Debug.Log(inputf_N.text);
        PlayerPrefs.SetInt("milestone_pnum", int.Parse(inputf_N.text));

    }

    public void Gesture_click()
    {
        SceneManager.LoadScene("sample_gesture_milestone");
    }

    public void Voice_click()
    {
        SceneManager.LoadScene("milestone_voice");
    }

    public void Face_click()
    {
        SceneManager.LoadScene("milestone_face");
    }

    public void show_time_lim_val()
    {
        PlayerPrefs.SetFloat("milestone_time_lim", time_lim_slider.value);
        time_lim_text.text = time_lim_slider.value.ToString("n2");
    }
}
