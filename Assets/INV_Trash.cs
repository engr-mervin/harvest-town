using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class INV_Trash : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    public Animator anim;

    public void OnPointerEnter(PointerEventData eventData)
    {
        anim.SetBool("isInside", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        anim.SetBool("isInside", false);
    }

}
