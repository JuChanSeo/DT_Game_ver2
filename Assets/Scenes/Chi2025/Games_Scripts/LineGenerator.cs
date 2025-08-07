using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class LineGenerator : MonoBehaviour
{
    public UILineRenderer linePrefab;
    public Canvas canvas;


    lineUI activeLine;

    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:

                    UILineRenderer newLine = Instantiate(linePrefab);
                    //newLine.transform.parent = canvas.transform;
                    newLine.transform.SetParent(canvas.transform);
                    activeLine = newLine.GetComponent<lineUI>();
                    break;
                
                case TouchPhase.Moved:

                    var touch_2d = new Vector2(touch.position.x, touch.position.y);
                    activeLine.UpdateLineui(touch_2d);
                    break;
                
                case TouchPhase.Ended:

                    activeLine = null;
                    break;
            }
        }


        //if (Input.GetMouseButtonDown(0))
        //{
        //    UILineRenderer newLine = Instantiate(linePrefab);
        //    activeLine = newLine.GetComponent<lineUI>();
        //}

        //if(Input.GetMouseButtonUp(0))
        //{
        //    activeLine = null;
        //}

        //if(activeLine != null)
        //{
        //    Debug.Log("check");
            
        //    {
        //        Touch touch = Input.GetTouch(0);
        //        Vector3 touch_3d = new Vector3(touch.position.x, touch.position.y, 0.3f);
        //        var drawing_pos = Camera.main.ScreenToWorldPoint(touch_3d);
        //        drawing_pos.z = Camera.main.transform.position.z + 1f;
        //        //Debug.Log("Drawing pos: " + drawing_pos.x + "\t" + drawing_pos.y + "\t" + drawing_pos.z);
        //        Debug.Log("drawing line");
        //        activeLine.UpdateLineui(new Vector2(touch.position.x, touch.position.y));
        //    }
        //    //Vector2 mousePos2d = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    //Vector3 mousePos = new Vector3(mousePos2d.x, mousePos2d.y, 0.1f);
        //    //activeLine.UpdateLine(mousePos2d);
        //}
        
    }
}
