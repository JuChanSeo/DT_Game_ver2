using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 

public class emotion_selected : MonoBehaviour
{  
    // public UnityEngine.UI.Image m_Image;
    string current_emo;
    public GameObject m_Image;
    public Sprite emoji1_sprite;
    public Sprite emoji2_sprite;
    public Sprite emoji3_sprite;
    public Sprite emoji4_sprite;
    public Sprite emoji5_sprite;
    public Sprite emoji6_sprite;
    public Sprite emoji7_sprite;
    public Sprite emoji8_sprite;
    public Sprite emoji9_sprite;
    public Sprite question_sprite;
    public SendDataEp sendDataEp;


    void Start()
    {   
        m_Image.GetComponent<Image>().sprite=question_sprite;
       
    }
    
    void Update()
    {  
      OnButtonClick();
    }

    public void OnButtonClick(){
        var go =  EventSystem.current.currentSelectedGameObject;
        if (go !=null){
            if(go.name !=current_emo){
                sendDataEp.UpdateEmotion(go.name);
                sendDataEp.Send();
                current_emo=go.name;
                // Sprite emoji = Resources.Load<Sprite>(current_emo);
                if(current_emo.Contains("1")){
                    m_Image.GetComponent<Image>().sprite=emoji1_sprite;
                }
                if(current_emo.Contains("2")){
                    m_Image.GetComponent<Image>().sprite=emoji2_sprite;
                }if(current_emo.Contains("3")){
                    m_Image.GetComponent<Image>().sprite=emoji3_sprite;
                }if(current_emo.Contains("4")){
                    m_Image.GetComponent<Image>().sprite=emoji4_sprite;
                }if(current_emo.Contains("5")){
                    m_Image.GetComponent<Image>().sprite=emoji5_sprite;
                }if(current_emo.Contains("6")){
                    m_Image.GetComponent<Image>().sprite=emoji6_sprite;
                }if(current_emo.Contains("7")){
                    m_Image.GetComponent<Image>().sprite=emoji7_sprite;
                }if(current_emo.Contains("8")){
                    m_Image.GetComponent<Image>().sprite=emoji8_sprite;
                }if(current_emo.Contains("9")){
                    m_Image.GetComponent<Image>().sprite=emoji9_sprite;
                }
                
            }
        }
    }

    
}
