using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moving_hands : MonoBehaviour
{
    Vector3 local_pos;
    Vector3 target_pos;
    // Start is called before the first frame update
    void Start()
    {
        local_pos = gameObject.transform.localPosition;
        target_pos = local_pos + 1f * Vector3.forward + 0.7f * Vector3.down;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(local_pos, gameObject.transform.localPosition) < 0.2f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, target_pos, 0.2f*Time.deltaTime);
        }
        else
        {
            transform.localPosition = local_pos;
        }
    }
}
