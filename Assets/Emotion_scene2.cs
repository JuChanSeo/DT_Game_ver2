using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Emotion_scene2 : MonoBehaviour
{    public Button other_emotion;
    // Start is called before the first frame update
    void Start()
    {
         Button btn1= other_emotion.GetComponent<Button>();
        other_emotion.onClick.AddListener(BTN_OnClick1);
    }
     void BTN_OnClick1(){
        Debug.Log("Loaded");
        SceneManager.LoadScene("20_Emotion");        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
