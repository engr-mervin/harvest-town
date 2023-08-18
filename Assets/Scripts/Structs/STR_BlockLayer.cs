using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class STR_BlockLayer
{
    public GameObject objectInLayer;
    public int layerIndex;   
    
    public STR_BlockLayer(int index)
    {
        layerIndex = index;
    }
}

