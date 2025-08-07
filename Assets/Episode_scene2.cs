using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Episode_scene2 : MonoBehaviour
{
    public Button other_period;
    // Start is called before the first frame update
    void Start()
    {
        Button btn1= other_period.GetComponent<Button>();
        other_period.onClick.AddListener(BTN_OnClick1);
    }
    void BTN_OnClick1(){
        Debug.Log("Loaded");
        SceneManager.LoadScene("40_Episode_selection");        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
