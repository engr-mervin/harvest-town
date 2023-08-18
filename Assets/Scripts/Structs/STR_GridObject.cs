using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class STR_GridObject
{
    public GameObject g;
    public Vector3Int pos;

    public STR_GridObject(GameObject _g, Vector3Int _pos)
    {
        pos = _pos;
        g = _g;
    }
}
