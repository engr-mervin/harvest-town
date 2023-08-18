using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
[System.Serializable]
public class ITEM_Wallpaper : BC_Items
{
    public SCO_Wallsets wallset;

    public static STR_Walls FindNearestWall(Vector2Int target)
    {  //base of wall
        if (WallsManager.ListContains(target) && !WallsManager.ListContains(target + Vector2Int.down))
        {
            return WallsManager.GetWall(target) ;
        }
        //top of wall
        else if (WallsManager.ListContains(target + Vector2Int.down) && !WallsManager.ListContains(target + Vector2Int.down + Vector2Int.down))
        {
            return WallsManager.GetWall(target+Vector2Int.down);
        }
        else
        {
            return null;
        }
    }

    public override IEnumerator SingleTouchCoroutine(Vector2Int target, INV_ItemSlot slot,Action tryNext)
    {
        Vector2Int tile = Positions.BlockToTile(target);

        yield return base.SingleTouchCoroutine(target, slot, tryNext);

        if (slot.quantity == 0)
        {
            print("Ran out of item");
            stcRunning = false;
            tryNext?.Invoke();
            yield break;
        }

        STR_Walls actualWall = FindNearestWall(tile);

        //NO APPLIABLE WALL
        if (actualWall == null|| actualWall.wallPaperIndex == wallset.index)
        {
            print("There is no wall here or same wall.");
            stcRunning = false;
            tryNext?.Invoke();
            yield break;
        }

        Vector2Int targetBlock = Positions.TileToBlock((Vector2Int)actualWall.pos)[0];

        yield return MovingCoroutine(targetBlock, AStar_Pathfinding.Type.Front, tryNext);

        ApplyWallpaper(actualWall,slot);

        stcRunning = false;
        tryNext?.Invoke();
        yield break;
    }
    
    public void ApplyWallpaper(STR_Walls wall,INV_ItemSlot slot)
    {
        Vector2 target = (Vector2Int)wall.pos + new Vector2(0.50f, 0.50f);
        if (!GM.playerMove.SamePosition(target, 2.00f))
            return;

        Sprite change =null; 


        if (WallsManager.HasDown(wall.pos)) return;

        
        if(wall.wall.name=="Right(Clone)")
        {
            change = wallset.L1;
        }
        if (wall.wall.name == "LR(Clone)" || wall.wall.name== "None(Clone)")
        {
            change = wallset.M1;
        }
        if (wall.wall.name == "Left(Clone)")
        {
            change = wallset.R1;
        }


        if (wall.wall.name == "UR(Clone)")
        {
            change = wallset.L2;
        }
        if (wall.wall.name == "ND(Clone)")
        {
            change = wallset.M2;
        }
        if (wall.wall.name == "UL(Clone)")
        {
            change = wallset.R2;
        }

        if (wall.wall.name == "Up(Clone)")
        {
            change = wallset.M4;
        }

        GM.playerMove.lookDir = new Vector2Int(0, 1);
        wall.wall.GetComponent<SpriteRenderer>().sprite = change;
        wall.wallPaperIndex = wallset.index;
        slot.ReduceQuantity(1);

        return;
    }


    IEnumerator WallFade(STR_Walls fade)
    {
        if (fade == null)
            yield break;
        if (fade.wall == null)
            yield break;
        if (fade.wall.GetComponent<FN_SpriteFade>() != null && fade.wall.GetComponent<FN_SpriteFade>().state == FN_SpriteFade.State.Faded)
            yield break;

        SpriteRenderer last = fade.wall.GetComponent<SpriteRenderer>();

        last.color = new Color(last.color.r, last.color.g, last.color.b, 0.70f);

        yield return new WaitForSeconds(0.15f);

        last.color = new Color(last.color.r, last.color.g, last.color.b, 1.00f);

    }
}
