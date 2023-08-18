using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[System.Serializable]
public class Block : MonoBehaviour
{
    public Vector2Int blockPosition;
    public int maxLayers;

    public static int maxBlockLayers = 4;

    public STR_BlockLayer[] allLayers;

    public GameObject topMostObject;
    public int topMostObjectIndex = -1;

    public float currentBlockHeight;

    public List<STR_BlockLayer> activeLayers = new List<STR_BlockLayer>();


    public Block() //BLOCK CREATOR  
    {
        allLayers = new STR_BlockLayer[Block.maxBlockLayers];

        for (int i = 0; i<allLayers.Length;i++)
        {
            allLayers[i] = new STR_BlockLayer(i);
        }
    }
    
    public void AddObject(GameObject add,int index) //use to add an object to the block with a specified index
    {
        if (allLayers[index].objectInLayer != null)
            print("Replaced " + allLayers[index].objectInLayer.name + " with " + add.name);

        allLayers[index].objectInLayer = add;

        RearrangeLayers();
    }

    public bool AddObjectOnTop(GameObject add) //use to add an object to the top of the block
    {
        if (topMostObjectIndex == maxBlockLayers-1)
        {
            print("Can't add anymore to this block, operation failed");
            return false;
        }

        allLayers[topMostObjectIndex+1].objectInLayer = add;


        RearrangeLayers();
        return true;
    }

    public bool RemoveObject(GameObject remove) // use to remove an object from the array
    {
        bool a = false;
        foreach(STR_BlockLayer b in allLayers)
        {
            if(remove == b.objectInLayer)
            {
                
                b.objectInLayer = null;
                a = true;
            }
        }
        if (a)
        {
            RearrangeLayers();
        }
        return a;
    }


    private void RearrangeLayers() //rearranges the block layers
    {
       foreach(STR_BlockLayer layer in allLayers) //adds layers with objects removes layers with none
        {
            if(layer.objectInLayer!=null)
            {
                if (!activeLayers.Contains(layer))
                    activeLayers.Add(layer);
                else
                    continue;
            }
            else
            {
                if (activeLayers.Contains(layer))
                    activeLayers.Remove(layer);
                else
                    continue;

            }
        }
        if (activeLayers.Count == 0)
        {
            S_WorldBlocks.instance.worldList.Remove(this);
            Destroy(this);
            return;
        }
        else
        {
            SetHeight();
            SetProjectedObjects();
        }
    }

    private void SetProjectedObjects() // sets the topmost and first interactable object in the block
    {
        foreach (STR_BlockLayer layer in activeLayers)
        {
            int i = -1;
            if (layer.layerIndex>=i)
            {
                i = layer.layerIndex;
                topMostObject = layer.objectInLayer;
                topMostObjectIndex = i;
            }
        }
    }

    private void SetHeight() //sets block height
    {
        float height = 0.0f;

        foreach (STR_BlockLayer layer in activeLayers)
        {
            height += layer.objectInLayer.GetComponent<ObjectTransform>().height;
        }

        currentBlockHeight = height;
    }
}
//NOTES
//1. A block can have a layer 2 and a null layer 1
//Study feasibility of using only the topmost block layer's type and disregard other's type in block layer's class
