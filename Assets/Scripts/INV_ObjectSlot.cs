using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class INV_ObjectSlot : MonoBehaviour, IPointerClickHandler //IRRELEVANT - OLD SCRIPT
{
    public GameObject cg;
    public string tabName;
    public GameObject panel;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GM.playerObj == null) return;
        if (panel.activeSelf) return;

        Vector2Int pivot = GM.playerMove.pivotPosition + GM.playerMove.lookDir;

        GameObject g = S_ObjectControls.CreateObject(cg.name, tabName, pivot);

        if (g == null) // if not successful on front, spawn at player pivot
        {
            pivot = GM.playerMove.pivotPosition;
            g = S_ObjectControls.CreateObject(cg.name, tabName, pivot);

            if (g == null)
                print("Not placeable");
        }

        if(g!=null)
        {
        }

    }
}
