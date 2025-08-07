using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
using System.Linq;

public class lineUI : MonoBehaviour
{
    public UILineRenderer UIlinerender;

    List<Vector2> points;

    public void UpdateLineui(Vector2 position)
    {
        //Debug.Log("drawing line in side lineUI script");
        if (points == null)
        {
            points = new List<Vector2>();
            SetPointui(position);
            return;
        }
        if (Vector2.Distance(points.Last(), position) > .1f)
        {
            SetPointui(position);
        }
    }

    void SetPointui(Vector2 point)
    {
        points.Add(point);
        Vector2[] uiline = points.ToArray();
        UIlinerender.Points = uiline; 
    }


}

