using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class AStar_Node :IHeapItem<AStar_Node>
{
    public Vector2Int position;
    public bool turn;
    public bool walkable;

    public int gCost=0;
    public int hCost=0;

    public AStar_Node parent;
    int heapIndex;

    public int FCost
    {
        get{return gCost + hCost;} 
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public AStar_Node(Vector2Int _pos)
    {
        position = _pos;
    }

    public int CompareTo(AStar_Node nodeToCompare)
    {
        int compare = FCost.CompareTo(nodeToCompare.FCost);
        if (compare == 0)
            compare = hCost.CompareTo(nodeToCompare.hCost);

        return -compare;
    }
}
