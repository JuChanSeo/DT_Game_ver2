using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effect_sound : MonoBehaviour
{
    AudioSource audio_;
    //public AudioClip bgm;
    public AudioClip effect1;
    public AudioClip dog_sound;
    public AudioClip fail_sound;
    public AudioClip succes_sound;
    public AudioClip jump_sound;
    public AudioClip getitem_sound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
