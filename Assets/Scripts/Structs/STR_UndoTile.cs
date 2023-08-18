using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class STR_UndoTile
{
    public Tile tile;
    public Vector3Int pos;
    public Tilemap tilemap;

    public enum Type
    {
        Single,
        StartMultiple,
        Multiple
    };

    public Type type;

    public STR_UndoTile(Tile _tile, Vector3Int _pos,Tilemap _tilemap,Type _type = Type.Single)
    {
        tile = _tile;
        pos = _pos;
        tilemap = _tilemap;
        type = _type;
    }
}
