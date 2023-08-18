using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(UI_Object_Inventory))]
public class S_HotBar : MonoBehaviour
{
    public INV_ItemSlot activeSlot;

    public GameObject activeSlotMarker;

    private UI_Object_Inventory objectInv;
    private void Awake()
    {
        objectInv = GetComponent<UI_Object_Inventory>();
    }

    public void SetActiveSlot(INV_ItemSlot slot)
    {
        activeSlot = slot;
        activeSlotMarker.transform.SetParent(slot.transform, false);
    }
}
