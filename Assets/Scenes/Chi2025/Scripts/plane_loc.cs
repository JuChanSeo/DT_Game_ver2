using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plane_loc : MonoBehaviour
{
    public GameObject pome;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = pome.transform.position + Vector3.down * 0.015f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
