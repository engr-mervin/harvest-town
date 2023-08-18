using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectTransform))]
public class Stackable : MonoBehaviour
{
    public static float heightThreshold;

    public StackBase.Type stackableOn;
    private ObjectTransform objectTrans;

    private void Awake()
    {
        objectTrans = GetComponent<ObjectTransform>();
    }

    public bool CheckStack(StackBase baseObject) //check a stackbase gameobject name if it can be stacked
    {
        int a = (int)stackableOn;
        int b = (int)baseObject.type;

        if ((a & b) == 0) return false;

        return true;
    }

    public bool CanStackHere(Vector2Int blockPivot) // call on instantiating object
    {
        bool allEmpty = true;

        foreach (Vector2Int p in objectTrans.AllBlockPoints(blockPivot))//check if all empty
        {
            if (S_WorldBlocks.GetBlockinPosition(p) == null) continue;
            else
            {
                allEmpty = false;
                break;
            }
        }
        if(allEmpty)
        {
            return true;
        }

        foreach (Vector2Int p in objectTrans.AllBlockPoints(blockPivot))
        {
            if (CheckStack(S_WorldBlocks.GetBlockinPosition(p).topMostObject.GetComponent<StackBase>()))
                continue;
            else
                return false;
        }

        return true; //All are compatible
    }


    public void Stack()//Check if an moveobject should be moved upwards while moving
    {
        Block HB = S_WorldBlocks.FindHighestBlock(gameObject, objectTrans.pivot);


        //IF THERE ARE NO BLOCKS 
        if (HB == null)
        {
            UnstackObject();
            print("NOTHING TO STACK TO");

            return;
        }

        StackBase baseObject = HB.topMostObject.GetComponent<StackBase>();


        if (baseObject == null)
        {
            UnstackObject();
            print("NULL");
            return;
        }

        if (HB.topMostObjectIndex == 3) 
        {
            UnstackObject();
            print("BLOCK FULL");
            return;
        }

        StackObject(HB);
    }
    public bool ColorStack(Vector2Int pivot,Vector2Int vc)
    {
        Block HB = S_WorldBlocks.FindHighestBlock(gameObject, pivot);
        Block CB = S_WorldBlocks.GetBlockinPosition(vc);

        if(HB==null)
        {
            return true;
        }

        if(HB!=null&&CB==null)
        {
            return false;
        }
        //DIFFERENT HEIGHTS
        if(HB.currentBlockHeight!=CB.currentBlockHeight)
        {
            return false;
        }

        if(HB.topMostObject!=CB.topMostObject)
        {
            return false;
        }

        if(CB.topMostObject.GetComponent<StackBase>()==null)
        {
            return false;
        }

        if(!CheckStack(CB.topMostObject.GetComponent<StackBase>()))
        {
            return false;
        }

        return true;
    }
    public void StackObject(Block b)
    {
        if (GetComponent<BoxCollider2D>() != null)
            GetComponent<BoxCollider2D>().enabled = false;

        transform.parent = b.topMostObject.transform;
        GetComponent<SpriteRenderer>().sortingOrder = b.topMostObject.GetComponent<SpriteRenderer>().sortingOrder + 1;

        objectTrans.botElev = b.topMostObject.GetComponent<ObjectTransform>().topElev;
        objectTrans.topElev = objectTrans.botElev + objectTrans.height;
        
        transform.position = Positions.BlockToTransform(objectTrans.pivot) + new Vector3(0, b.currentBlockHeight, 0);
    }

    public void UnstackObject()
    {
        if (GetComponent<BoxCollider2D>() != null)
            GetComponent<BoxCollider2D>().enabled = true;

        transform.parent = S_ObjectControls.ObjectInstantiator.transform;
        transform.position = Positions.BlockToTransform(objectTrans.pivot);
    }
}
