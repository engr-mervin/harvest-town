using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

[System.Serializable]
public class S_WorldBlocks : MonoBehaviour //singleton, responsible for creating world, loading world, saving world
{
    public GameObject worldBlockObject;
    public static S_WorldBlocks instance;//creates a singleton

    [SerializeField]
    public List<Block> worldList = new List<Block>(); //actual list

    public void Awake()
    {
        if(instance!=null)
        {
            Destroy(instance);
            instance = this;
        }
        else
        {
            instance = this;
        }    
    }//SINGLETON

    public static void PlaceObjectInWorld(ObjectTransform objectTrans, Vector2Int pivot)
    //this method adds an object into the world, based on the highest layer available
    {
        int a = GetHighestLayerIndex(objectTrans, pivot);
        Vector2Int[] vcs = objectTrans.AllBlockPoints(pivot);
        foreach(Vector2Int current in vcs)
        {
            Block b;

            if (S_WorldBlocks.GetBlockinPosition(current) == null)
            {
                b = S_WorldBlocks.AddBlockinPosition(current); 
                b.AddObject(objectTrans.gameObject, a);
            }
            else
            {
                b = S_WorldBlocks.GetBlockinPosition(current); 
                b.AddObject(objectTrans.gameObject, a);
            }
            
        }
    }

    public static Block GetBlockinPosition(Vector2Int pos)
    //Finds a block in position
    {
        return instance.worldList?.Find(b => (b.blockPosition.x == pos.x) && (b.blockPosition.y == pos.y));
    }

    public static Block AddBlockinPosition(Vector2Int pos)  //Adds a block in position
    {
        if (GetBlockinPosition(pos) != null)
        {
            Debug.Log("There is already a block at position " + pos);
            return null;
        }

        Block b = instance.worldBlockObject.AddComponent<Block>();
        b.blockPosition = pos;
        instance.worldList.Add(b);
        return b;
    }

    public static void DestroyAllObjects() //destroy all objects and lists
    {
        Destroy(OBJ_ObjectSaveList.instance.gameObject);
        //Delete all Blocks
        foreach(Block b in FindObjectsOfType<Block>())
            {
            UnityEngine.GameObject.Destroy(b);
            }
        //Clear Lists
        if(instance.worldList!=null)
        instance.worldList.Clear();
        STR_GridObjectWithLayer.list.Clear();
    }

    public static Block FindHighestBlock(GameObject g, Vector2Int pivot) //For stacking logic
    {
        ObjectTransform objectTrans = g.GetComponent<ObjectTransform>();
        Block highestBlock = null;
        float currentHighest = 0.0f;
        Vector2Int[] vc = objectTrans.AllBlockPoints(pivot);
        foreach(Vector2Int current in vc)
        { 
            if (S_WorldBlocks.GetBlockinPosition(current) != null&& S_WorldBlocks.GetBlockinPosition(current).currentBlockHeight>currentHighest)
            {
                    currentHighest = S_WorldBlocks.GetBlockinPosition(current).currentBlockHeight;
                    highestBlock = S_WorldBlocks.GetBlockinPosition(current);
            }
        }
        return highestBlock;
    }
    
    public static void RemoveObjectFromWorld(GameObject g) //removes an object from world
    {
        ObjectTransform objectTrans = g.GetComponent<ObjectTransform>();
        Vector2Int pivot = objectTrans.pivot;


        foreach (Vector2Int current in objectTrans.AllBlockPoints(pivot))
        { 
            Block b = GetBlockinPosition(current);
            b.RemoveObject(g);
        }
        if (objectTrans != null)
        {
            foreach (Vector2Int v in objectTrans.AllBlockPoints())
            {
                AStar_Grid.RefreshWalkable(v);
            }
        }
    }
    
    public static int GetHighestLayerIndex(ObjectTransform objectTrans, Vector2Int pivot)
    //get the highest layer index within a set of blocks
    {
        int a = 0;

        foreach(Vector2Int current in objectTrans.AllBlockPoints(pivot))
        {
            if (GetBlockinPosition(current) != null && GetBlockinPosition(current).topMostObjectIndex + 1 > a) //if there is an object already in a block
            {
                a = GetBlockinPosition(current).topMostObjectIndex + 1;
            }
        }
        return a;
    }

    public static bool ListContainsObject(GameObject g)
    {
        foreach(Block b in instance.worldList)
        {
            foreach(STR_BlockLayer bl in b.activeLayers)
            {
                if (bl.objectInLayer == g)
                    return true;
            }
        }
        return false;
    }

}

