using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STR_GridObjectWithLayer
{
    public GameObject blockObject;
    public Vector2Int gridPosition;
    public int layer;

    public STR_GridObjectWithLayer(GameObject obj,Vector2Int pos,int _layer)
    {
        blockObject = obj;
        gridPosition = pos;
        layer = _layer;
    }

    public static List<STR_GridObjectWithLayer> list = new List<STR_GridObjectWithLayer>();
}
