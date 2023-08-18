using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public List<INF_MyTile> tileInfo = new List<INF_MyTile>();

    public static Dictionary<Tile, INF_MyTile.Type> tileList = new Dictionary<Tile, INF_MyTile.Type>();

    public void Initialize() //call after making tilemap after walls and floors but before objects
    {
        foreach(INF_MyTile info in tileInfo)
        {
            if (info == null) continue;
            if (tileList.ContainsKey(info.tile)) continue;

            tileList.Add(info.tile, info.baseType);
        }
    }
    public static void AddNewTile(Tile tile, INF_MyTile.Type type)
    {
        tileList.Add(tile, type);
    }

    public static INF_MyTile.Type GetType(Tile tile)
    {
        INF_MyTile.Type type = INF_MyTile.Type.None;

        if (!tileList.ContainsKey(tile)) return type;

        tileList.TryGetValue(tile, out type);

        return type;
    }

    public static INF_MyTile.Type GetFloorType(Vector2Int pos)
    {
        Tile tile = S_Tilemap.floors.GetTile<Tile>((Vector3Int)pos);

        INF_MyTile.Type type = INF_MyTile.Type.None;

        if (tile == null) return type;

        if (!tileList.ContainsKey(tile)) return type;

        tileList.TryGetValue(tile, out type);

        return type;
    }

    public static INF_MyTile.Type GetWallType(Vector2Int pos)
    {
        Tile tile = S_Tilemap.walls.GetTile<Tile>((Vector3Int)pos);

        if(tile==null)
            return INF_MyTile.Type.None;

        INF_MyTile.Type type = INF_MyTile.Type.None;

        if (!tileList.ContainsKey(tile)) return type;

        tileList.TryGetValue(tile, out type);

        return type;
    }
}
