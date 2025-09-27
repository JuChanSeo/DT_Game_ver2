using UnityEngine;
using TextSpeech;
using TMPro;

public class voice_interact_script : MonoBehaviour
{
    VoiceController voiceController_script;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        voiceController_script = GameObject.Find("VoiceController").GetComponent<VoiceController>();
        SpeechToText.Instance.onResultCallback = OnFinalSpeechResult;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void start_listening()
    {
        voiceController_script.StartListening();
        Invoke("stop_listening", 2f);
    }

    public void stop_listening()
    {
        voiceController_script.StoptListening();
    }

    void OnFinalSpeechResult(string result)
    {
        Debug.Log("FinalSpeech 실행\t" + result);
    }

}
