using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTab : MonoBehaviour
{
    public void DestroyItems()
    {
        foreach (BTN_SellableItem item in GetComponentsInChildren<BTN_SellableItem>(includeInactive:true))
        {
            BTN_SellableItem.OnItemExhausted -= item.Reorder;
            Destroy(item.gameObject);
        }
    }
}
