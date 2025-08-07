using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainCam_pos : MonoBehaviour
{
    public GameObject pet;
    Vector3 pos_diff;
    Vector3 org_rot;
    bool show_topview;
    // Start is called before the first frame update
    void Start()
    {
        pet = GameObject.Find("pomeLV0" + PlayerPrefs.GetInt("Level_pet").ToString());
        org_rot = transform.eulerAngles;
        pos_diff = GameObject.Find("setPos_caregame").transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(!show_topview)
        {
            transform.position = pet.transform.position - pos_diff;
        }
    }

    public void topview_bt_click()
    {
        if(show_topview == false)
        {
            show_topview = true;
            transform.position = new Vector3(-127.1f, 29.76f, -3.4f);
            transform.eulerAngles = new Vector3(28.24f, 85.6f, -2.319f);
        }
        else
        {
            show_topview = false;
            transform.eulerAngles = org_rot;
        }
    }
}
