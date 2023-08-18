using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ITEM_Carpets : BC_Items
{
    public SCO_Carpets carpet;

    public override IEnumerator SingleTouchCoroutine(Vector2Int position, INV_ItemSlot slot, Action tryNext)
    {
        yield return base.SingleTouchCoroutine(position, slot, tryNext);

        if (slot.quantity == 0)
        {
            print("Ran out of item");
            stcRunning = false;
            tryNext?.Invoke();
            yield break;
        }

        STR_Floors actualFloor = FloorsManager.GetFloor(Positions.BlockToTile(position));

        //NO APPLIABLE CARPET
        if (actualFloor == null || actualFloor.carpet.index == carpet.index)
        {
            print(actualFloor+" IS NULL");
            stcRunning = false;
            tryNext?.Invoke();
            yield break;
        }

        yield return MovingCoroutine(position, AStar_Pathfinding.Type.Exact, tryNext);

        print("starasfsadfat");
        ApplyCarpet(actualFloor, slot);

        stcRunning = false;
        tryNext?.Invoke();
        yield break;
    }

    private void ApplyCarpet(STR_Floors floor, INV_ItemSlot slot)
    {
        Vector2 target = (Vector2Int)floor.pos + new Vector2(0.50f, 0.50f);
        if (!GM.playerMove.SamePosition(target, 1.00f))
        {
            print("Didn't reached the destination");
            return;

        }

        if (floor.carpet.index == carpet.index)
        {
            print("Same carpet"); 
            return;
        }

        floor.ChangeTile(carpet);

        slot.ReduceQuantity(1);

        print("Successfully applied a carpet");
    }
}
