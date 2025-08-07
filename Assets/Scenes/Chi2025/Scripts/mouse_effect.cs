using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouse_effect : MonoBehaviour
{
    public GameObject starObject;
    GameObject canvas;
    UnityEngine.UI.Image image;
    public bool dontshowCursor;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        transform.SetParent(canvas.transform);
        image = GetComponent<UnityEngine.UI.Image>();

        Color c;
        c = image.color;
        c.a = 0;
        image.color = c;

    }

    // Update is called once per frame
    void Update()
    {
        if (dontshowCursor) return;

        if(transform.parent.transform.name != "Canvas")
        {
            canvas = GameObject.Find("Canvas");
            transform.parent = canvas.transform;

        }
        if (Input.touchCount > 0)
        {
            //Debug.Log("logging");
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                Color c;
                c = image.color;
                c.a = 1f;
                image.color = c;
            }
            if (touch.phase == TouchPhase.Moved)
            {
                starCreat();
            }
            if (touch.phase == TouchPhase.Ended)
            {
                Color c;
                c = image.color;
                c.a = 0;
                image.color = c;
            }

        }

        gameObject.transform.position = Input.mousePosition;
    }

    void starCreat()
    {
        Vector3 mPos = (new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        Instantiate(starObject, mPos, Quaternion.identity);
    }
}
