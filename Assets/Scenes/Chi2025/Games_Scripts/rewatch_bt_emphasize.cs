using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rewatch_bt_emphasize : MonoBehaviour
{
    float time;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(time < 15f)
        {
            time += Time.deltaTime;
            if(time > 12f)
            {
                transform.localScale = Vector3.one * (1 * time);
            } 
        }
        else
        {
            resetAnim();
        }
    }

    public void resetAnim()
    {
        time = 0;
        transform.localScale = Vector3.one;
    }
}
