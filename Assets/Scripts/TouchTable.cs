using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchTable
{
    public int touchID;
    public Touch touch;
    public Vector2 locationFirst;
    public Vector2 locationCurrent;

    public TouchTable(Touch newTouch, int newTouchID, Vector2 newLocationFirst)
    {
        touch = newTouch;
        touchID = newTouchID;
        locationFirst = newLocationFirst;
    }

}
