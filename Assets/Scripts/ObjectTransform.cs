using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
[RequireComponent(typeof(OBJ_ObjectSaveData))]
public class ObjectTransform : MonoBehaviour
{
    [Header("Set This")]
    [SerializeField]
    public Vector2Int size;
    public Transform top;

    public bool specialBlock;

    private Stackable stackable;

    public Vector2Int pivot { get; private set; } //bottom left grid of object

    private OBJ_ObjectSaveData saveData;

    public static List<ObjectTransform> objectTransList = new List<ObjectTransform>();


    [Header("Don't Edit")]
    [SerializeField]
    public float height;

    public int layer; //block layer

    public float botElev;
    public float topElev;



    public Vector2Int[] currentPoints;

    public float GetDistance(Vector3 position)
    {
        float dist = Vector3.Distance(position, Positions.BlockToTransformCenter(AllBlockPoints()[0]));

        for(int i = 1; i < AllBlockPoints().Length; i++)
        {
            float a = Vector3.Distance(position, Positions.BlockToTransformCenter(AllBlockPoints()[i]));

            if (a < dist)
                dist = a;
        }
        return dist;
    }
    private void Awake()
    {
        stackable = GetComponent<Stackable>();
        saveData = GetComponent<OBJ_ObjectSaveData>();
        
        pivot = Positions.TransformToBlock(transform.position);
        Snap(transform);

        if (!objectTransList.Contains(this))
            objectTransList.Add(this);

        ComputeCurrentPoints();
    }

    private void ComputeCurrentPoints()
    {
        currentPoints = AllPoints(pivot);
    }

    private void Snap(Transform t) //Snap objects for default world creation
    {
        if (specialBlock)
        {

            t.position = Positions.BlockToTransformCenter(pivot);
        }
        else
        {
            t.position = Positions.BlockToTransform(pivot);
        }
    }


    public void MoveToBlock(Vector2Int blockPosition)
    {
        Vector2Int delta = blockPosition - pivot;
        pivot = blockPosition;
        transform.position += new Vector3(delta.x, delta.y, 0f)/2;

        //For saving
        saveData.SetPosition(blockPosition);

        currentPoints = AllPoints(pivot);

        if (stackable != null)
        {
            stackable.Stack();
        }

    }
    public void MoveToBlockByDeltaChildren(Vector2Int delta)
    {
        print("run");
        Vector2Int blockPosition = delta + pivot;
        pivot = blockPosition;

        //For saving
        saveData.SetPosition(blockPosition);

        currentPoints = AllPoints(pivot);

    }
    public void RemoveFromList() //call when taking an object
    {
        if (!objectTransList.Contains(this))
            print("Object is not in list, this shouldnt run");
        else
            objectTransList.Remove(this);
    }

    
    public void RefreshLayer()//call when stacking
    {
        Block b = S_WorldBlocks.GetBlockinPosition(GetComponent<ObjectTransform>().pivot); //find block
        saveData.SetLayer(b.activeLayers.Find(bl => bl.objectInLayer == gameObject).layerIndex);
    }

    public Vector2Int[] AllBlockPoints(Vector2Int blockPos)
    {
        if (specialBlock)
        {
            Vector2Int[] result = new Vector2Int[size.x * size.y];
            for (int x = 0; x < size.x; x++) //takes a rectangular array of blocks equivalent to size of moveObject
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector2Int add = new Vector2Int(x, y);
                    Vector2Int current = blockPos + add;

                    result[2 * size.y * (x) + y] = current;
                }
            }
            return result;
        }
        else
        {
            Vector2Int[] result = new Vector2Int[4 * size.x * size.y];
            for (int x = 0; x < 2 * size.x; x++) //takes a rectangular array of blocks equivalent to size of moveObject
            {
                for (int y = 0; y < 2 * size.y; y++)
                {
                    Vector2Int add = new Vector2Int(x, y);
                    Vector2Int current = blockPos + add;

                    result[2 * size.y * (x) + y] = current;
                }
            }
            return result;
        }
    }
    public Vector2Int[] AllBlockPoints()
    {
        if(specialBlock)
        {
            Vector2Int[] result = new Vector2Int[size.x * size.y];
            for (int x = 0; x < size.x; x++) //takes a rectangular array of blocks equivalent to size of moveObject
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector2Int add = new Vector2Int(x, y);
                    Vector2Int current = pivot + add;

                    result[2 * size.y * (x) + y] = current;
                }
            }
            return result;
        }
        else
        {
            Vector2Int[] result = new Vector2Int[4 * size.x * size.y];
            for (int x = 0; x < 2 * size.x; x++) //takes a rectangular array of blocks equivalent to size of moveObject
            {
                for (int y = 0; y < 2 * size.y; y++)
                {
                    Vector2Int add = new Vector2Int(x, y);
                    Vector2Int current = pivot + add;

                    result[2 * size.y * (x) + y] = current;
                }
            }
            return result;
        }
        
    }
    public Vector2Int[] AllPoints(Vector2Int pivotPoint)
    {
        Vector2Int[] result = new Vector2Int[size.x*size.y];
        for (int x = 0; x < size.x; x++) //takes a rectangular array of blocks equivalent to size of moveObject
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector2Int add = new Vector2Int(x, y);
                Vector2Int current = pivotPoint + add;

                result[size.y * (x) + y] = current;
            }
        }
        return result;
    }
}
