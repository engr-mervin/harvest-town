using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class WallMarker
{
    public GameObject g;
    public Vector3Int pos;
    public WallSet ws;

    public WallMarker(GameObject _g, Vector3Int _pos,WallSet _ws)
    {
        pos = _pos;
        g = _g;
        ws = _ws;
    }
}
