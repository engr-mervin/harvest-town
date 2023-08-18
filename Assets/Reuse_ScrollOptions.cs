using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Reuse_ScrollOptions : MonoBehaviour,IPointerClickHandler
{
    [HideInInspector]
    public Reuse_ScrollManager mother;

    [SerializeField]
    private Sprite frontView;


    public void OnPointerClick(PointerEventData eventData)
    {
        int ind=0;

        for(int i =0;i<mother.options.Length;i++)
        {
            if (mother.options[i]==this)
            {
                ind = i;
                break;
            }
        }

        BTN_CC_Scroll.Scroll(mother, ind);
    }


    public void Set()
    {
        mother.preview.sprite = frontView;
    }

}
