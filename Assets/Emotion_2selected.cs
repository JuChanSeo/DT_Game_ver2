using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems; 
using TMPro;

public class Emotion_2selected : MonoBehaviour
{
    public GameObject m_Image;
    string current_emo;
    public Sprite question_sprite;
    public string[] emotion_names; 

    public TextMeshProUGUI selected_emotion; 
    private Color[] emotion_colors; 
    public SendDataEp sendDataEp;

    
    // Start is called before the first frame update
    void Start()
    {
        
         
        
        emotion_names =new string[]{"?", "화가난", "스트레스", "초조한", "불안한", "근심하는", "언짢은",
                                     "놀란", "신나는", "황홀한", "기쁜", "희망찬", "행복한",
                                    "침울한", "실망한", "지루한", "절망한", "쓸쓸한", "지친", 
                                    "평온한", "만족한", "감동적인", "여유로운", "고요한", "안정적인"};
        //  emotion_colors = new Color[]{(0f, 0f, 0f, 255f), (255f, 42f, 65f, 255f),(255f, 119f, 52f, 255f), (255f, 168f, 0f, 255f) };
        //  new Color(0.447f, 0.6f, 1f, 1f);
    }

    // Update is called once per frame
     void Update()
    {  
      OnButtonClick();
    }

    public void OnButtonClick(){
        var go =  EventSystem.current.currentSelectedGameObject;
        if (go !=null){
            // Debug.Log(go.name);
            if(go.name != "Selected"){
                if(go.name !=current_emo){
                        // Debug.Log(go.name);
                        current_emo=go.name;
                        sendDataEp.UpdateEmotion(go.name);
                        sendDataEp.Send();
                        var emo_num=go.name.Replace("Emo_option", "");
                        // current_emo=int.Parse(current_emo)-int.Parse(1);
                        // Debug.Log(emotion_names[int.Parse(emo_num)]+emo_num);
                        if(emo_num!="Next_Q"){
                            if(emo_num!="Home_Button"){
                                selected_emotion.text=emotion_names[int.Parse(emo_num)];

                        Button Selected_btn= m_Image.GetComponent<Button>();
                        Color customColor=new Color(255/255f, 255/255f, 255/255f);                        
                        if((emo_num=="1")||(emo_num=="4")){
                            customColor=new Color(255/255f, 42/255f, 65/255f);                        
                        }
                        if((emo_num=="2")||(emo_num=="5")){
                            customColor=new Color(255/255f, 119/255f, 52/255f);                        
                        }
                        if((emo_num=="3")||(emo_num=="6")){
                            customColor=new Color(255/255f, 168/255f, 0/255f);                        
                        }

                        if((emo_num=="7")||(emo_num=="10")){
                            customColor=new Color(234/255f, 206/255f, 38/255f);                        
                        }
                        if((emo_num=="8")||(emo_num=="11")){
                            customColor=new Color(255/255f, 190/255f, 42/255f);                        
                        }
                        if((emo_num=="9")||(emo_num=="12")){
                            customColor=new Color(147/255f, 124/255f, 16/255f);                        
                        }

                        if((emo_num=="13")||(emo_num=="16")){
                            customColor=new Color(42/255f, 65/255f, 255/255f);                           
                        }if((emo_num=="14")||(emo_num=="17")){
                            customColor=new Color(42/255f, 136/255f, 255/255f);                         
                        }if((emo_num=="15")||(emo_num=="18")){
                            customColor=new Color(0/255f, 209/255f,255/255f);                        
                        }

                        if((emo_num=="19")||(emo_num=="22")){
                            customColor=new Color(211/255f, 255/255f, 42/255f);                        
                        }if((emo_num=="20")||(emo_num=="23")){
                            customColor=new Color(64/255f, 255/255f, 42/255f);                        
                        }if((emo_num=="21")||(emo_num=="24")){
                            customColor=new Color(12/255f, 183/255f,11/255f);                        
                        }
                        // Color customColor=new Color(255/255f, 42/255f, 65/255f);


                        ColorBlock cb = Selected_btn.colors;
                        cb.normalColor = customColor;
                        Selected_btn.colors=cb; 
                        // Image_Renderer.material.SetColor("_Color", customColor);
                            }
                        }
                        
                    
                }
            }
        }
    }
}
