using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BtnCloseBuild : MonoBehaviour, IPointerClickHandler
{
    public GameObject inv;
    public GameObject inv2;
    public void OnPointerClick(PointerEventData eventData)
    {
        Camera.main.GetComponent<FN_FollowPlayer>().enabled = true;
        inv.SetActive(false);
        inv2.SetActive(true);
    }
}
