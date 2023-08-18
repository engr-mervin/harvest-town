using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class GridTile
{
    public Tile tile;
    public Vector3Int pos;

    public GridTile(Tile _tile, Vector3Int _pos)
    {
        pos = _pos;
        tile = _tile;
    }
}
