using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class trackingEyeprefab : MonoBehaviour
{
    public UnityEngine.UI.Image marker_2d;
    GameObject eyeprefab_loc;
    ARFaceManager facemanager_script;
     
    // Start is called before the first frame update
    void Start()
    {
        facemanager_script = GameObject.Find("XR Origin").GetComponent<ARFaceManager>();
        eyeprefab_loc = facemanager_script.facePrefab;
        marker_2d.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(eyeprefab_loc != null)
        {
            if(!marker_2d.gameObject.activeSelf) marker_2d.gameObject.SetActive(true);
            var eyeprefab_loc_screen = Camera.main.WorldToScreenPoint(eyeprefab_loc.transform.position);
            Debug.Log(eyeprefab_loc.transform.position + "\t" + eyeprefab_loc_screen);
        }
    }
}
