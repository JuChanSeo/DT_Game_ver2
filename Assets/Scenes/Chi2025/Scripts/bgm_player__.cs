using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgm_player__ : MonoBehaviour
{
    AudioSource audio_;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        audio_ = GameObject.Find("Audio player").GetComponent<AudioSource>();
        audio_.Play();
        Debug.Log("배경음악 실행");
    }


}
