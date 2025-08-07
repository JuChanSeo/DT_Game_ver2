using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setPos_careGame : MonoBehaviour
{
    public GameObject[] Pets_copy;
    public Transform transform_pet;
    GameObject pet;
    // Start is called before the first frame update
    void Start()
    {
        pet = Pets_copy[PlayerPrefs.GetInt("Level_pet")];
        pet.transform.position = transform_pet.position;
        pet.transform.rotation = transform_pet.rotation;

        for (int i = 1; i < 6; i++)
        {
            if (i != PlayerPrefs.GetInt("Level_pet"))
                Pets_copy[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //pet.transform.position = transform_pet.position;
        //pet.transform.rotation = transform_pet.rotation;

    }
}
