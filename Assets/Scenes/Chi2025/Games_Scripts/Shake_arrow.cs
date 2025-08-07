using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake_arrow : MonoBehaviour
{
    float y_max;
    float y_min;

    float cur_dir;
    float cur_val;
    // Start is called before the first frame update
    void Start()
    {
        y_max = 0.2f;
        y_min = -0.2f;
        cur_dir = 1;
        cur_val = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(cur_val > y_max)
        {
            cur_dir = -1;
        }
        else if(cur_val < y_min)
        {
            cur_dir = 1;
        }
        cur_val += cur_dir * 0.02f;

        Vector3 pos = gameObject.transform.position;
        pos.y += cur_dir * 0.005f;
        gameObject.transform.position = pos;
    }
}
