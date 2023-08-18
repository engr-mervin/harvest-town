using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InvWallSlot : MonoBehaviour, IPointerClickHandler
{

    public GameObject wallset;
    public TC_Build tc;
    public GameObject marker;

    public static InvWallSlot active;

    public void Awake()
    {
        if (marker.GetComponent<RectTransform>().localPosition == GetComponent<RectTransform>().localPosition)
            active = this;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Refresh();
        marker.GetComponent<RectTransform>().localPosition = GetComponent<RectTransform>().localPosition;
        active = this;
    }
    public void Refresh()
    {
        tc.activeWall = wallset;
    }
}
