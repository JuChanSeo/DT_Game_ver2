using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;  // List<>을 사용하기 위한 using 문 추가
//using Newtonsoft.Json;
using TMPro;

public class Tutorial_Name : MonoBehaviour
{   
    public TMP_Text agentAnswer; 
    public AudioSource audioSource;

    private void PlayQuestionAudio(string audioFileName)
    {
        AudioClip audioClip = Resources.Load<AudioClip>(audioFileName);
        if (audioClip != null)
        {
            // Debug.Log(audioClip.name);
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("Audio clip not found: " + audioFileName);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        // Debug.Log(agentAnswer.text);
        // if (agentAnswer.text.Contains("안녕")){
        //     Debug.Log("맞음");
        //     // PlayQuestionAudio("")
        // }
    }
}
