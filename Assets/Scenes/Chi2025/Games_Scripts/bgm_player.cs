using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class bgm_player : MonoBehaviour
{
    AudioSource audio_;
    //public AudioClip bgm;
    public AudioClip effect1;
    public AudioClip dog_sound;
    public AudioClip fail_sound;
    public AudioClip succes_sound;
    public AudioClip jump_sound;
    public AudioClip getitem_sound;
    public AudioClip fly_catch_sound;

    // Start is called before the first frame update
    void Start()
    {
        audio_ = GameObject.Find("Audio player").GetComponent<AudioSource>();
        
        if(audio_.clip != null) audio_.Play();

        if(SceneManager.GetActiveScene().name.Contains("LoginPage"))
        {
            excute_narration("01");
            StartCoroutine(excute_sound("02", 4f));
        }

        if(SceneManager.GetActiveScene().name.Contains("AR_interaction_MZ"))
        {
            excute_narration("03");
        }

    }

    public void butoon_effect()
    {
        audio_.PlayOneShot(effect1);
    }

    public void dog_sound_excute()
    {
        audio_.PlayOneShot(dog_sound);
    }

    public void fail_sound_excute()
    {
        audio_.PlayOneShot(fail_sound);
    }

    public void success_sound_excute()
    {
        audio_.PlayOneShot(succes_sound);
    }

    public void jump_sound_excute()
    {
        audio_.PlayOneShot(jump_sound);
    }

    public void getitem_sound_excute()
    {
        audio_.PlayOneShot(getitem_sound);
    }

    public void fly_catch_sound_excute()
    {
        Debug.Log("파리잡기 사운드 실행");
        audio_.PlayOneShot(fly_catch_sound);
    }

    public void excute_narration(string number)
    {
        AudioClip clip_ = Resources.Load("narration/" + number) as AudioClip;
        audio_.PlayOneShot(clip_);
    }

    
    public IEnumerator excute_sound(string str, float delay)
    {
        yield return new WaitForSeconds(delay);
        AudioClip clip_ = Resources.Load("narration/" + str) as AudioClip;
        audio_.PlayOneShot(clip_);
    }

}
