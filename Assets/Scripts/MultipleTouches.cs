using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MultipleTouches : MonoBehaviour
{
    public readonly List<TouchTable> touchesLeft = new List<TouchTable>();
    public readonly List<TouchTable> touchesRight = new List<TouchTable>();
    
    void Update()
    {
        int i = 0;
        while (i<Input.touchCount)
        {
            Touch t = Input.GetTouch(i);
            TouchTable currentTouch;
            if (t.phase == TouchPhase.Began)
            {
                if (t.position.x < Screen.width / 2)
                {
                    touchesLeft.Add(new TouchTable(t, t.fingerId, t.position));
                }
                else
                {
                    touchesRight.Add(new TouchTable(t, t.fingerId, t.position));
                }
            }
            if (t.phase != TouchPhase.Ended)
            {
                if (touchesLeft.Find(a => a.touchID == t.fingerId) == null)
                {
                    if (touchesRight.Find(a => a.touchID == t.fingerId) == null)
                    {
                        currentTouch = null;
                    }
                    else
                    {
                        currentTouch = touchesRight.Find(a => a.touchID == t.fingerId);
                    }
                }
                else
                {
                    currentTouch = touchesLeft.Find(a => a.touchID == t.fingerId);
                } 
                currentTouch.locationCurrent = t.position;


            }
            if (t.phase == TouchPhase.Ended)
            {
                if (touchesLeft.Find(a => a.touchID == t.fingerId) == null)
                {
                    if (touchesRight.Find(a => a.touchID == t.fingerId) == null)
                    {
                        currentTouch = null;
                        
                    }
                    else
                    {
                        currentTouch = touchesRight.Find(a => a.touchID == t.fingerId);
                        touchesRight.Remove(currentTouch);
                    }
                }
                else
                {
                    currentTouch = touchesLeft.Find(a => a.touchID == t.fingerId);
                    touchesLeft.Remove(currentTouch);
                }
                

            }
            i++;
        }
        
    }
}
