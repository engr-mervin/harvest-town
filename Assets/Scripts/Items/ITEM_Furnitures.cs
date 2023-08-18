using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ITEM_Furnitures : BC_Items
{
    public GameObject furniture;

    public override bool SingleTouch(Vector2Int position)
    {
        return false;
    }

    public override IEnumerator SingleTouchCoroutine(Vector2Int position, INV_ItemSlot slot, Action tryNext)
    {
        //basic checks - item check, stc initialize
        yield return StartCoroutine(base.SingleTouchCoroutine(position,slot,tryNext));
        

        PlaceFurniture(position, slot);

        stcRunning = false;
        tryNext?.Invoke();
        yield break;
    }

    private void PlaceFurniture(Vector2Int position, INV_ItemSlot slot)
    {
        GameObject g = S_ObjectControls.InstantiateObject(furniture, position);

        if (g != null)
        {
            slot.ReduceQuantity(1);
            GM.playerState.SetState(new MovingObjectState(GM.playerState,g, MovingObject.Type.NewObject));
        }


    }
}
