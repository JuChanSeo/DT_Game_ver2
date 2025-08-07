using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class master_mode : MonoBehaviour
{
    private float lastTouchTime;
    private const float doubleTouchDelay = 0.5f;
    int double_touch_cnt;
        
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name != "Statue_MZ")
        {
            if(GameObject.Find("master_mode") == null) DontDestroyOnLoad(gameObject);
            double_touch_cnt = 0;
            lastTouchTime = Time.time;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (Time.time - lastTouchTime < doubleTouchDelay) // 더블터치 판정
                    {
                        if(touch.position.x > 2000 && touch.position.y<500)
                        {
                            //Debug.Log("double_touch_cnt:" + double_touch_cnt);
                            double_touch_cnt++;
                        }
                    }
                    else
                    {
                        double_touch_cnt = 0;
                    }

                    break;

                case TouchPhase.Ended:
                    lastTouchTime = Time.time;
                    if (double_touch_cnt == 10)
                    {
                        double_touch_cnt = 0;
                        SceneManager.LoadScene("01_Statue_MZ");
                    }

                    break;
            }
        }
    }

}
