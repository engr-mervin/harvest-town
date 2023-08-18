using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SaveTileMap 
{
    public List<TileData> tileData = new List<TileData>();

    public Vector3Int BL;
    public Vector3Int TR;

    public SaveTileMap(Tilemap tm)
    {
        Bounds bounds = tm.localBounds;

        BL = MyFunctions.Vector3toVector3Int(bounds.min) - 15 * Vector3Int.one; //factor of safety
        TR = MyFunctions.Vector3toVector3Int(bounds.max) + 15 * Vector3Int.one;

        int distX = TR.x - BL.x + 1;
        int distY = TR.y - BL.y + 1;

        for(int x = 0; x<distX;x++)
        {
            for (int y = 0; y < distX; y++)
            {
                Vector3Int pos = new Vector3Int(x+BL.x,y+BL.y,0);

                Tile current = tm.GetTile<Tile>(pos);

                if (current == null) continue;

                TileData currentTD = new TileData(pos.x, pos.y, current.name);

                tileData.Add(currentTD);
            }
        }

    }
}
