using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class help_panel : MonoBehaviour
{

    public GameObject help_panel_entire;
    public GameObject[] panels;
    public GameObject explain_panel;

    GameObject selected_panel;
    int current_idx;
    float time;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(help_panel_entire.transform.position);
        help_panel_entire.transform.position = new Vector3(1198, 818, 0);
        time = 0;
        current_idx = 0;
        for (int i = 0; i < panels.Length; i++)
        {
            for (int j = 0; j < panels[i].transform.childCount; j++)
            {
                panels[i].transform.GetChild(j).gameObject.SetActive(false);
                //Debug.Log(panel_buf.transform.GetChild(j).gameObject.transform.name);
            }

            panels[i].SetActive(false);
        }

        help_panel_entire.SetActive(false);
    }

    private void Update()
    {
        if (selected_panel == null || selected_panel.activeSelf == false)
        {
            return;
        }
        time += Time.deltaTime;

        if(time > 5f)
        {
            
            {
                next_bt_clicked();
            }
            time = 0;
        }
    }

    public void help_bt_clicked()
    {
        if (help_panel_entire.activeSelf == true) help_panel_entire.SetActive(false);
        else help_panel_entire.SetActive(true);

        if (explain_panel != null && explain_panel.activeSelf == true)
            Destroy(explain_panel, 10f);
    }

    public void hungry_panel_clicked()
    {
        time = 0;
        current_idx = 0;
        selected_panel = panels[0];
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }
        selected_panel.SetActive(true);
        selected_panel.transform.GetChild(0).gameObject.SetActive(true);

        if (explain_panel != null && explain_panel.activeSelf == true)
            Destroy(explain_panel);
    }

    public void sleep_panel_clicked()
    {
        time = 0;
        current_idx = 0;
        selected_panel = panels[1];
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }
        selected_panel.SetActive(true);
        selected_panel.transform.GetChild(0).gameObject.SetActive(true);

        if (explain_panel != null && explain_panel.activeSelf == true)
            Destroy(explain_panel);

    }

    public void bath_panel_clicked()
    {
        time = 0;
        current_idx = 0;
        selected_panel = panels[2];
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }
        selected_panel.SetActive(true);
        selected_panel.transform.GetChild(0).gameObject.SetActive(true);

        if (explain_panel != null && explain_panel.activeSelf == true)
            Destroy(explain_panel);

    }

    public void play_panel_clicked()
    {
        time = 0;
        current_idx = 0;
        selected_panel = panels[3];
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }
        selected_panel.SetActive(true);
        selected_panel.transform.GetChild(0).gameObject.SetActive(true);

        if (explain_panel != null && explain_panel.activeSelf == true)
            Destroy(explain_panel);

    }

    public void next_bt_clicked()
    {
        time = 0;
        if (selected_panel.activeSelf == false) return;

        //현재 패널을 끄고
        selected_panel.transform.GetChild(current_idx).gameObject.SetActive(false);

        //인덱스를 하나 증가시켜 준 뒤
        if (current_idx+1 == selected_panel.transform.childCount) current_idx = 0;
        else current_idx += 1;

        //다음 패널을 킨다
        selected_panel.transform.GetChild(current_idx).gameObject.SetActive(true);

        if (explain_panel != null && explain_panel.activeSelf == true)
            Destroy(explain_panel);

    }

    public void previous_bt_clicked()
    {
        time = 0;
        if (selected_panel.activeSelf == false) return;

        //현재 패널을 끄고
        selected_panel.transform.GetChild(current_idx).gameObject.SetActive(false);

        //인덱스를 하나 감소 시킨 뒤
        if (current_idx == 0) current_idx = selected_panel.transform.childCount - 1;
        else current_idx -= 1;

        //다음 패널을 킨다
        selected_panel.transform.GetChild(current_idx).gameObject.SetActive(true);

        if (explain_panel != null && explain_panel.activeSelf == true)
            Destroy(explain_panel);

    }

}
