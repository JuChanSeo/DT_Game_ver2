using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringAnim : MonoBehaviour
{
    float time;
    public Vector3 setactive_pos_cur;
    Vector3 setactive_pos_pre;

    private void Start()
    {
        setactive_pos_cur = this.transform.position;
        setactive_pos_pre = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = 2 * Vector3.one * (1 - time/2);
        if(time>1f)
        {
            time = 0;
            resetScale();
        }

        //if (time < 0.4f) //특정 위치에서 원점으로 이동
        //{
        //    this.transform.position = setactive_pos_cur + new Vector3(40 - 30 * time, 40 - 30 * time, 0);
        //}
        //else if (time < 0.5f) // 튕기고
        //{
        //    this.transform.position = setactive_pos_cur + new Vector3(time - 0.4f, time - 0.4f, 0) * 10;
        //}
        //else if (time < 0.6f) //다시 제자리로
        //{
        //    this.transform.position = setactive_pos_cur + new Vector3(0.6f - time, 0.6f - time, 0) * 10;
        //}
        //else if (time < 0.7f) //튕기고
        //{
        //    this.transform.position = setactive_pos_cur + new Vector3((time - 0.6f) / 2, (time - 0.6f) / 2, 0) * 10;
        //}
        //else if (time < 0.8f) //다시 제자리
        //{
        //    this.transform.position = setactive_pos_cur + new Vector3(0.05f - (time - 0.7f) / 2, 0.05f - (time - 0.7f) / 2, 0) * 10;
        //}
        //else
        //{
        //    this.transform.localPosition = setactive_pos_cur + Vector3.zero;
        //    resetAnim();
        //}

        time += Time.deltaTime;

    }

    public void resetAnim()
    {
        time = 0;
    }

    public void resetScale()
    {
        time = 0;
        transform.localScale = Vector3.one;
    }


}
