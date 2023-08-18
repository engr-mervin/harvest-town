using UnityEngine;
using UnityEngine.Tilemaps;
public class INF_MyTile:MonoBehaviour
{
    public Tile tile;
    public MyTileMap tilemap;

    [System.Flags]
    public enum Type
    {
        None = 0,
        Floor = 1,
        WallBottom = 2,
        WallTop = 4,
        Sidewall = 8,
        Backwall = 16,
        Road = 32,
        Soil = 64,
        TilledSoil = 128,
        Water = 256
    };

    public enum MyTileMap
    {
        Walls,
        Floors,
        Outside,
    };

    public Type baseType;

    public Tilemap GetTilemap()
    {
        Tilemap result;
        switch (tilemap)
        {
            case MyTileMap.Walls:
                result = S_Tilemap.walls;
                break;
            case MyTileMap.Floors:
                result = S_Tilemap.floors;
                break;
            default:
                result = null;
                break;
        }
        return result;
    }
}
