using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class STR_Walls
{
    public Vector3Int pos;
    public GameObject wall { get; private set; }
    public GameObject marker;
    public Tile bot { get; private set; }
    public Tile top { get; private set; }

    public int wallPaperIndex = 0;
    public int wallIndex;

    public STR_Walls(Vector3Int _pos, GameObject _marker)
    {
        pos = _pos;
        marker = _marker;

        marker.transform.position = pos + new Vector3(0.50f, 0.50f, 0f);
        marker.transform.parent = SO_WallMarkers.instance.transform;

        WallsManager.wallList.Add(this);

        FloorsManager.RemoveFloor(pos);
    }

    public void SetWall(GameObject _wall)
    {
        wall = _wall;

        FN_SpriteFade sFade = wall.GetComponent<FN_SpriteFade>();

        if(sFade!=null)
            sFade.Subscribe();

        wallIndex = wall.GetComponent<WallSaver>().wallIndex;
    }

    public void SetTile(Tile _top, Tile _bottom)
    {
        top = _top;
        bot = _bottom;
    }
    public GameObject[] RemoveWallOnly()
    {
        GameObject[] children;

        if (wall!=null && wall.transform.childCount != 0)
        {
            children = new GameObject[wall.transform.childCount];

            for (int i = 0; i < wall.transform.childCount; i++)
            {
                children[i] = wall.transform.GetChild(i).gameObject;
                children[i].transform.parent = null;
            }
        }
        else
        {
            children = null;
        }

        if (wall != null)
        {
            if(wall.GetComponent<FN_SpriteFade>()!=null)
                wall.GetComponent<FN_SpriteFade>().Unsubscribe();

            GameObject.Destroy(wall);
        }

        return children;

        

    }

    public void DestroyThis()
    {

        if (bot != null)
        {
            S_Tilemap.walls.SetTile(pos, null);
        }

        if (top != null)
        {
            S_Tilemap.walls.SetTile(pos + Vector3Int.up, null);
        }

        foreach(Vector2Int v in Positions.TileToBlock((Vector2Int)pos))
        {
            AStar_Grid.RefreshWalkable(v);
        }


        if (wall != null && wall.GetComponent<FN_SpriteFade>() != null)
            wall.GetComponent<FN_SpriteFade>().Unsubscribe();

        if (wall != null)
            GameObject.Destroy(wall);

        if (marker != null)
            GameObject.Destroy(marker);


    }
}
