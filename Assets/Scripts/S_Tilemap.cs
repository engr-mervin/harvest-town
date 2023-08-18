using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class S_Tilemap : MonoBehaviour //handles queries about tilemaps
{
    public static Tilemap walls;
    public static Tilemap floors;

    public Tilemap _floors;
    public Tilemap _walls;

    public static Tilemap[] allTilemaps;

    public void Awake()
    {
        walls = _walls;
        floors = _floors;

        allTilemaps = new Tilemap[]
        {
            walls,
            floors,
        };
    }

    public static bool IsVoid(Vector2Int pos) //ask if a position doesn't contain a tile
    {
        Vector3Int posB = MyFunctions.Vector2IntToTileLocation(pos);

        foreach (Tilemap tm in allTilemaps)
        {
            if (tm.GetTile(posB) != null) //check if there is a tile
            {
                return false;
            }
        }

        return true;
    }

    public static int GetTileType(Vector2Int pos) //get the TYPES of a position
    {
        //convert position
        Vector3Int posB = MyFunctions.Vector2IntToTileLocation(pos);

        int a = 0;

        foreach(Tilemap tm in allTilemaps)
        {
            if (tm.GetTile(posB) != null) //check if there is a tile
            {
                int b = (int)(GetTileTypeFromName(tm.GetTile(posB).name));
                a |= b;
            }
        }

        return a;
    }
    public static INF_MyTile.Type GetTileTypeFromName(string tileName) //get the tile map type of a name
    {
        INF_MyTile myTile = Resources.Load("MyTile/" + tileName, typeof(INF_MyTile)) as INF_MyTile;

        return myTile.baseType;
    }
    public static Tilemap GetTilemapFromName(string tileName) //get the tile map type of a name
    {
        INF_MyTile myTile = Resources.Load("MyTile/" + tileName, typeof(INF_MyTile)) as INF_MyTile;

        return myTile.GetTilemap();
    }

}
