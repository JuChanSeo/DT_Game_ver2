using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class higlight_blink : MonoBehaviour
{
    float time;
    UnityEngine.UI.Image img_gb;
    // Start is called before the first frame update
    void Start()
    {
        img_gb = GetComponent<UnityEngine.UI.Image>();
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if(time > .5f)
        {
            time = 0;
            img_gb = GetComponent<UnityEngine.UI.Image>();
            Color c = img_gb.color;
            if (c.a == 0)
            {
                c.a = 1;
                img_gb.color = c;   
            }
            else
            {
                c.a = 0;
                img_gb.color = c;
            }
        }
    }

}
