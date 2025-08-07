using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class blink_text : MonoBehaviour
{
    float time;
    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time < 1f)
            text.text = "여기를 터치해주세요!";
        else if (time < 2f)
            text.text = "";

        if(time>2f)
        {
            time = 0;
        }

    }

}
