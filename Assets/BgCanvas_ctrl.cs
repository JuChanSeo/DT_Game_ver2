using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class BgCanvas_ctrl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string fileName = "test_filename" + ".png";
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        if(File.Exists(filePath))
        {
            var rawimage = gameObject.GetComponent<RawImage>();
            var byteTexture = File.ReadAllBytes(filePath);
            var texture = new Texture2D(0, 0);
            texture.LoadImage(byteTexture);
            rawimage.texture = texture;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
