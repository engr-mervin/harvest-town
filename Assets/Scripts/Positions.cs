using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Positions
{
    //block to tile position
    public static Vector2Int BlockToTile(Vector2Int blockPosition)//ok
    {
        //1,1 -> 0.5,0.5 -> 0,0
        int x = Mathf.FloorToInt((float)(blockPosition.x)/2);
        int y = Mathf.FloorToInt((float)(blockPosition.y) / 2);

        return new Vector2Int(x, y);
    }

    //tile to block position
    public static Vector2Int[] TileToBlock(Vector2Int tilePosition)//ok
    {
        Vector2Int[] result = new Vector2Int[4];
        //3,3 -> 6,6 + 6,7 + 7,6 + 7,7
        int x1 = tilePosition.x * 2;
        int y1 = tilePosition.y * 2;

        result[0] = new Vector2Int(x1, y1);
        result[1] = result[0] + new Vector2Int(0, 1);
        result[2] = result[0] + new Vector2Int(1, 0);
        result[3] = result[0] + new Vector2Int(1, 1);

        return result;
    }

    //block position to transform
    public static Vector3 BlockToTransform(Vector2Int blockPosition)//ok
    {
        float x = blockPosition.x * 0.50f + 0.5f;
        float y = blockPosition.y * 0.50f + 0.5f;

        return new Vector3(x, y, 0f);
    }
    public static Vector3 BlockToTransformCenter(Vector2Int blockPosition)//ok
    {
        float x = blockPosition.x * 0.50f + 0.25f;
        float y = blockPosition.y * 0.50f + 0.25f;

        return new Vector3(x, y, 0f);
    }
    //transform to block position
    public static Vector2Int TransformToBlock(Vector3 trans)//ok
    {
        int x = Mathf.FloorToInt(trans.x * 2);
        int y = Mathf.FloorToInt(trans.y * 2);

        return new Vector2Int(x, y);
    }

    //tile position to transform
    public static Vector3 TileToTransform(Vector2Int tilePosition)
    {
        float x = tilePosition.x + 0.50f;
        float y = tilePosition.y + 0.50f;

        return new Vector3(x, y, 0f);
    }
    
    //transform to tile position
    public static Vector2Int TransformToTile(Vector3 trans)
    {
        int x = Mathf.FloorToInt(trans.x);
        int y = Mathf.FloorToInt(trans.y);

        return new Vector2Int(x, y);
    }

    public static Vector2Int TouchToBlockPos(Vector2 touchPos)
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(touchPos); //converts to transform
        int x = Mathf.FloorToInt(point.x);
        int y = Mathf.FloorToInt(point.y);
        int z = 0;

        return TransformToBlock(new Vector3(x, y, z));
    }
}
