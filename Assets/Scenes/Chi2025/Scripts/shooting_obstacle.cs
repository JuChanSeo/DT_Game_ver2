using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class shooting_obstacle : MonoBehaviour
{
    public GameObject cube_prefab;
    public float time_cube_create;
    GameObject cube_instantiate1;
    GameObject cube_instantiate2;
    public float currTime;//inspector 건드리지 않는다
    public bool start_counting;
    public TextMeshProUGUI time_cube_create_text;

    // Start is called before the first frame update
    void Start()
    {
        //cube_instantiate1 = Instantiate(cube_prefab);
        //Destroy(cube_instantiate1, time_cube_create);

    }

    // Update is called once per frame
    void Update()
    {
        if(time_cube_create_text != null)   time_cube_create_text.text = ((int)currTime % 60).ToString();
        if (start_counting)
            currTime += Time.deltaTime;

        if (currTime > time_cube_create)
        {
            create_and_destroy_cube();
            currTime = 0;
        }
    }


    public void create_and_destroy_cube()
    {
        if (!start_counting) start_counting = true;

        if (cube_instantiate2 == null)
        {
            cube_instantiate1 = Instantiate(cube_prefab);
            Invoke("ck_color", time_cube_create - 1f);//끝나기 1초전에 체크하고 있었네
            Destroy(cube_instantiate1, time_cube_create);
        }

        if(cube_instantiate1 == null)
        {
            cube_instantiate2 = Instantiate(cube_prefab);
            Destroy(cube_instantiate2, time_cube_create);
        }
    }

    void ck_color()
    {
        if(cube_instantiate1 != null)
        {
            cube_instantiate1.gameObject.GetComponent<obstacle3_moving_script>().check_color();
        }
        else if(cube_instantiate2 != null)
        {
            cube_instantiate2.gameObject.GetComponent<obstacle3_moving_script>().check_color();
        }
    }
    
}
