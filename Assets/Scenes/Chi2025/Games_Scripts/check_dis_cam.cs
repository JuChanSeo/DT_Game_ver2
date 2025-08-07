using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class check_dis_cam : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject icon;
    bool flag_icon;
    void Start()
    {
        icon.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        var dis = Vector2.Distance(new Vector2(gameObject.transform.position.x, gameObject.transform.position.z),
                                   new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.z));
        //Debug.Log("dis: " + dis);

        if (dis < 0.5f)
        {
            if(!flag_icon)
            {
                flag_icon = true;
            }
        }
        else
        {
            flag_icon = false;
            icon.SetActive(false);
        }


        if(flag_icon)
        {
            icon.SetActive(true);
        }
    }
}
