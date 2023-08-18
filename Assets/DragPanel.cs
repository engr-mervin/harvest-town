using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class DragPanel : MonoBehaviour,IDragHandler,IEndDragHandler,IBeginDragHandler
{
    public float limitMaxY = 132.1429f;
    public float limitMinY = -82.1429f;

    public int frames =15;

    [SerializeField]
    private float dragFactor; //0.30F

    private DragButton[] buttons;

    //COROUTINE TO SNAP AT THE END OF DRAG
    private Coroutine roll;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (roll != null)
            StopCoroutine(roll);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 current = (Vector3)eventData.delta;

        //NO X MOVEMENT
        current.x = 0;
        
        foreach (DragButton item in buttons)
        {
            if (item == null) continue;

            item.GetComponent<RectTransform>().localPosition += current * dragFactor;

            item.Check();
        }
    }

    //SET BUTTON CHILDREN WHEN CHANGING TABS
    public void SetButtons(DragButton[] children) 
    {
        buttons = children;

        foreach (DragButton item in children)
        {
            if (item == null) continue;

            item.parent = this;

            item.Check();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float topMost = buttons[0].GetComponent<RectTransform>().localPosition.y;
        float bottomMost = topMost;

        for (int i = 0; i < buttons.Length; i++) //compute highest and lowest dragButton
        {
            float currentY = buttons[i].GetComponent<RectTransform>().localPosition.y;

            if (currentY < bottomMost)
                bottomMost = currentY;

            if (currentY > topMost)
                topMost = currentY;
        }

        //one row
        if(bottomMost==topMost)
        {
            roll = StartCoroutine(Roll(limitMaxY - topMost, buttons));

            return;
        }

        float diff1 = topMost - limitMaxY;
        float diff2 = bottomMost - limitMinY;

        //if bottom button is higher than bottom of panel, roll down only when the top difference is greater than bottom diff
        if (bottomMost > limitMinY && diff2 < diff1)
        {
            roll = StartCoroutine(Roll(limitMinY - bottomMost, buttons));
            return;
        }

        if (bottomMost > limitMinY && diff2 > diff1)
        {
            roll = StartCoroutine(Roll(limitMaxY - topMost, buttons));
            return;
        }

        //if bottom button is higher than top of panel, roll down only until it reaches first row
        if (bottomMost > limitMaxY)
        {
            roll = StartCoroutine(Roll(limitMaxY - bottomMost, buttons));
            return;
        }

        //if top button is lower than top of panel
        if (topMost < limitMaxY)
        {
            roll = StartCoroutine(Roll(limitMaxY - topMost, buttons));
            return;
        }

    }
    
    //SNAP COROUTINE
    IEnumerator Roll(float move,DragButton[] list)
    {
        //SNAP TO LOCATION IN 15 FRAMES
        float step = move / frames;
        Vector3 stepv = new Vector3(0, step, 0);

        for(int i = 0;i<frames;i++)
        {
            foreach (DragButton item in list)
            {
                if (item == null) continue;

                item.GetComponent<RectTransform>().localPosition += stepv;
                item.Check();
            }
            yield return new WaitForEndOfFrame();
            
        }
        yield break;
    }

}
