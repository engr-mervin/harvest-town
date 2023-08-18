using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BTN_Rotate : MonoBehaviour,IPointerClickHandler
{
    public MovingObject mo;

    public void OnPointerClick(PointerEventData eventData)
    {
        mo.Rotate();
    }
}
