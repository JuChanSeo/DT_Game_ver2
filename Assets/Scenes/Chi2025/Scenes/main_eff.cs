using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main_eff : MonoBehaviour
{
    AudioSource audio_;
    //public AudioClip bgm;
    public AudioClip BBok3;
    public AudioClip button1;
    public AudioClip doginit_sound_cut;
    public AudioClip foot_sound;
    public AudioClip HagHag01;
    public AudioClip Jiruuu_joy_moment;
    public AudioClip jump_0;
    public AudioClip sweep;
    public AudioClip swosh3;

    // Start is called before the first frame update
    void Start()
    {
        audio_ = GameObject.Find("main_effect_player").GetComponent<AudioSource>();
    }

    public void sound_BBok3()
    {
        audio_.PlayOneShot(BBok3);
    }

    public void sound_button1()
    {
        audio_.PlayOneShot(button1);
    }

    public void sound_doginit_sound_cut()
    {
        audio_.PlayOneShot(doginit_sound_cut);
    }

    public void sound_foot_sound()
    {
        audio_.PlayOneShot(foot_sound);
    }

    public void sound_HagHag01()
    {
        audio_.PlayOneShot(HagHag01);
    }

    public void sound_Jiruuu_joy_moment()
    {
        audio_.PlayOneShot(Jiruuu_joy_moment);
    }

    public void sound_jump_0()
    {
        audio_.PlayOneShot(jump_0);
    }

    public void sound_sweep()
    {
        audio_.PlayOneShot(sweep);
    }

    public void sound_swosh3()
    {
        audio_.PlayOneShot(swosh3);
    }

}
