using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class care_effect : MonoBehaviour
{
    AudioSource audio_;
    //public AudioClip bgm;
    public AudioClip bubble0;
    public AudioClip correct;
    public AudioClip dog_eating0;
    public AudioClip dog_init_sound_cut;
    public AudioClip false2;
    public AudioClip flies_59723;
    public AudioClip haghag01;
    public AudioClip reward_popup;
    public AudioClip soap_bubble_pop;
    public AudioClip water_well;

    // Start is called before the first frame update
    void Start()
    {
        audio_ = GameObject.Find("care_effect_player").GetComponent<AudioSource>();
    }

    public void sound_bubble0()
    {
        audio_.PlayOneShot(bubble0);
    }

    public void sound_correct()
    {
        audio_.PlayOneShot(correct);
    }

    public void sound_dog_eating0()
    {
        audio_.PlayOneShot(dog_eating0);
    }

    public void sound_dog_init_sound_cut()
    {
        audio_.PlayOneShot(dog_init_sound_cut);
    }

    public void sound_false2()
    {
        audio_.PlayOneShot(false2);
    }

    public void sound_flies_59723()
    {
       audio_.PlayOneShot(flies_59723);
    }

    public void sound_haghag01()
    {
        audio_.PlayOneShot(haghag01);
    }

    public void sound_reward_popup()
    {
        audio_.PlayOneShot(reward_popup);
    }

    public void sound_soap_bubble_pop()
    {
        audio_.PlayOneShot(soap_bubble_pop);
    }

    public void sound_water_well()
    {
        audio_.PlayOneShot(water_well);
    }

    public void soap_and_bubble()
    {
        Debug.Log("soap_bubble_실행");
        if (!audio_.isPlaying)
        {
            sound_bubble0();
            sound_soap_bubble_pop();
        }
    }

}
