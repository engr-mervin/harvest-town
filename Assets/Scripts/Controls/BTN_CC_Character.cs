using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BTN_CC_Character : MonoBehaviour, IPointerClickHandler
{
    public string resourceName;
    public GameObject enable;
    public GameObject disable;

    public void OnPointerClick(PointerEventData eventData)
    {
        SO_CC_Maker.instance.resourceName = resourceName;

        enable.SetActive(true);
        disable.SetActive(false);
    }
}
