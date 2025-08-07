using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Show_webacm : MonoBehaviour
{
    public RawImage display;
    WebCamTexture camTexture;
    private int currentIndex = 1;
    WebCamDevice device;

    private void Start()
    {
        if (camTexture != null)
        {
            display.texture = null;
            camTexture.Stop();
            camTexture = null;
        }
        WebCamDevice[] devices = WebCamTexture.devices;
        Debug.Log(devices.Length);
        for(int i = 0; i< devices.Length; i++)
        {
            Debug.Log(devices[i].name);
            if(devices[i].isFrontFacing)
            {
                currentIndex = i;
            }
        }
        device = WebCamTexture.devices[currentIndex];

        camTexture = new WebCamTexture(device.name);
        camTexture.requestedFPS = 10;
        display.texture = camTexture;
        //display.recTransform.localScale = new Vector3(-1, 1, 1);
        camTexture.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
