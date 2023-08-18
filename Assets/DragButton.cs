using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragButton : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    RectTransform rt;
    Vector3 current;

    public DragPanel parent;

    public float limitMaxY;
    public float limitMinY;

    public void Initialize()
    {
        rt = GetComponent<RectTransform>();
        current = rt.localPosition;

        Check();
    }

    //ACTIVATES/DEACTIVATES BUTTON BASED ON ITS Y COORD
    public void Check() 
    {
        current = rt.localPosition;
        float y = current.y;
        if (y > limitMaxY || y < limitMinY)
        {
            gameObject.SetActive(false);
            return;
        }

        if (gameObject.activeSelf == false)
        {
            gameObject.SetActive(true);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        print("button start");
        parent.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        parent.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        print("button end");
        parent.OnEndDrag(eventData);
    }
}
