using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TextSpeech;
using TMPro;


public class VoiceController : MonoBehaviour
{
    const string LANG_CODE = "ko-KR";

    [SerializeField]
    TextMeshProUGUI uiText;

    private void Start()
    {
        Setup(LANG_CODE);

#if UNITY_ANDROID
        SpeechToText.Instance.onResultCallback = OnFinalSpeechResult;
#endif
        SpeechToText.Instance.onResultCallback = OnFinalSpeechResult;
        TextToSpeech.Instance.onStartCallBack = OnSpeakStart;
        TextToSpeech.Instance.onDoneCallback = OnSpeakStop;

        //CheckPermission();
    }

    void CheckPermission()
    {
//#if UNITY_ANDROID
//            if(!Permission.HasUserAuthorizedPermission(Permission.Microphone))
//        {
//            Permission.RequestUserPermission(Permission.Microphone);
//        }
//#endif

    }

#region Text to Speech
    public void StartSpeaking(string message)
    {
        TextToSpeech.Instance.StartSpeak(message);
    }

    public void StopSpaeking()
    {
        TextToSpeech.Instance.StopSpeak();
    }

    void OnSpeakStart()
    {
        Debug.Log("Talking started...");
    }

    void OnSpeakStop()
    {
        Debug.Log("Talking stoped...");
    }
#endregion


#region Speech to Text
    public void StartListening()
    {
        SpeechToText.Instance.StartRecording();
    }

    public void StoptListening()
    {
        SpeechToText.Instance.StopRecording();
    }

    void OnFinalSpeechResult(string result)
    {
        //uiText.text = result;
    }

    void OnPartialSpeechResult(string result)
    {
        uiText.text = result;
    }
#endregion
    void Setup(string code)
    {
        TextToSpeech.Instance.Setting(code, 1, 1);
        SpeechToText.Instance.Setting(code);
    }
}
