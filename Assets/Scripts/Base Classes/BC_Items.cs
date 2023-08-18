using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

[System.Serializable]
public class BC_Items : MonoBehaviour
{
    [Header("Editor Driven")]
    public string itemCode;

    public static bool stcRunning;
    public static Coroutine stc;


    [Header("Set This")]
    public int rawCost;
    public Sprite sprite;
    public Sprite descSprite;

    public string descName;
    public bool locked;
    public string description;

    public virtual bool SingleTouch(Vector2Int position)
    {
        return false;
    }

    public virtual IEnumerator SingleTouchCoroutine(Vector2Int position, INV_ItemSlot slot, Action tryNext)
    {
        if (stcRunning)
        {
            print("This shouldn't run!!");
            yield break;
        }

        if (slot.quantity == 0)
        {
            print("Ran out of item");
            yield break;
        }

        stcRunning = true;
        yield break;

    }

    public IEnumerator MovingCoroutine(Vector2Int position, AStar_Pathfinding.Type type, Action tryNext)
    {
        //GET PATH TO FRONT OF WALL
        List<AStar_Node> path = AStar_Pathfinding.instance.PathFindSmart(Positions.TileToBlock(GM.playerMove.pivotPosition)[0], position,type);
        //IF NO PATH IS FOUND
        if (path == null || path.Count == 0)
        {
            print("Floor is unreachable");
            stcRunning = false;
            tryNext?.Invoke();
            yield break;
        }

        Camera.main.GetComponent<FN_FollowPlayer>().Unfollow();

        yield return GM.playerMove.MoveToPosition(path);
        
        //The player is stucked along the path
        if (GM.playerMove.MoveFinished == false)
        {
            stcRunning = false;
            print("Move Coroutine has been broken due to player getting stucked");
            yield break;
        }
        else
        {
            yield break;
        }
    }

}
