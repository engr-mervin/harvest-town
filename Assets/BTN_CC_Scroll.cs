using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BTN_CC_Scroll : MonoBehaviour,IPointerClickHandler
{
    [SerializeField]
    Reuse_ScrollManager mother;

    private enum Direction
    {
        Left,
        Right
    }

    [SerializeField]
    private Direction direction;

    public void OnPointerClick(PointerEventData eventData)
    {
        int i = 1;

        if (direction == Direction.Left)
            i = -1;

        int desired = i + mother.currentNumber;

        if (desired < 0 || desired >= mother.options.Length)//outside possible options
            return;


        Scroll(mother,desired);
    }

    public static void Scroll(Reuse_ScrollManager cur,int scroll)
    {
        cur.currentNumber = scroll;

        int left = cur.currentNumber - 1;
        int center = cur.currentNumber;
        int right = cur.currentNumber + 1;

        foreach(Reuse_ScrollOptions opt in cur.options)
        {
            opt.gameObject.SetActive(false);


            if (left>=0&&opt == cur.options[left])
            {
                opt.gameObject.SetActive(true);
                RectTransform rect = opt.GetComponent<RectTransform>();
                rect.localPosition = new Vector3(cur.left, rect.localPosition.y, rect.localPosition.z);

                rect.localScale = new Vector3(1f, 1f, 1f);
                continue;
            }

            if (opt == cur.options[center])
            {
                opt.gameObject.SetActive(true);
                RectTransform rect = opt.GetComponent<RectTransform>();
                rect.localPosition = new Vector3(cur.center, rect.localPosition.y, rect.localPosition.z);
                cur.marker.transform.SetParent(opt.transform,false);

                rect.localScale = new Vector3(1.25f, 1.25f,1f);

                opt.Set();
                continue;
            }

            if (right<cur.options.Length&&opt == cur.options[right])
            {
                opt.gameObject.SetActive(true);
                RectTransform rect = opt.GetComponent<RectTransform>();
                rect.localPosition = new Vector3(cur.right, rect.localPosition.y, rect.localPosition.z);

                rect.localScale = new Vector3(1f, 1f, 1f);
                continue;
            }



        }
    }


}
