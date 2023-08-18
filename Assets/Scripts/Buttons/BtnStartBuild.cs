using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BtnStartBuild : MonoBehaviour, IPointerClickHandler
{
    //Turn off player gameobject
    public GameObject inv;
    public GameObject inv2;

    public void OnPointerClick(PointerEventData eventData)
    {
        Camera.main.GetComponent<FN_FollowPlayer>().enabled = false;
        inv.SetActive(true);
        inv2.SetActive(false);
    }
}
