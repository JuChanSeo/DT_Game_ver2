
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Background_scene : MonoBehaviour
{
    public Button next_scene; 
 
    // Start is called before the first frame update
    void Start()
    {
        Button btn1= next_scene.GetComponent<Button>();
        next_scene.onClick.AddListener(BTN_OnClick);
    

    }

    void BTN_OnClick(){
        Debug.Log("Loaded");
        SceneManager.LoadScene("40_Episode_selection");        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

