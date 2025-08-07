using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ground_script : MonoBehaviour
{
    public interact_pet interact_pet_script;
    // Start is called before the first frame update
    void Start()
    {
        interact_pet_script = GameObject.Find("pomeLV05").GetComponent<interact_pet>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        interact_pet_script.set_ball_velocity_0();
    }
}
