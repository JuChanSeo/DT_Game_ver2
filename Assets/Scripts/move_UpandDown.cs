using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_UpandDown : MonoBehaviour
{
    float move_dist = 50f;
    float move_speed = 250f;

    Vector2 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // PingPong 함수를 이용해 위아래로 반복 움직임
        float newY = startPos.y + Mathf.PingPong(Time.time * move_speed, move_dist * 2) - move_dist;
        transform.position = new Vector2(startPos.x, newY);
    }
}
