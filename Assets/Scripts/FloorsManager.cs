using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class FloorsManager : BC_TouchControls
{
    [Header("User Driven Variables")]
    public GameObject greenMarker;

    public SCO_Carpets floor;
    public static List<STR_Floors> floorList = new List<STR_Floors>();

    private List<STR_Floors> selectFloor = new List<STR_Floors>();

    public STR_Floors[] see;


    public override void ClearStaticList()
    {
        foreach (STR_Floors floor in floorList)
        {
            Destroy(floor.marker);
        }
        floorList.Clear();
        selectFloor.Clear();
    }

    private void OnEnable()
    {
        foreach (STR_Floors floor in floorList)
        {
            floor.marker.SetActive(true);
        }
        foreach (STR_Walls wall in WallsManager.wallList)
        {
            wall.marker.GetComponent<MRK_Walls>().Red();
            wall.marker.SetActive(true);
            wall.wall.SetActive(false);
        }
    }

    internal override void OnDisable()
    {
        base.OnDisable();

        foreach (STR_Floors floor in floorList)
        {
            floor.marker.SetActive(false);
        }

        foreach (STR_Walls wall in WallsManager.wallList)
        {
            wall.marker.GetComponent<MRK_Walls>().Green();
            wall.marker.SetActive(false);
            wall.wall.SetActive(true);
        }
    }

    internal override void SingleTouchMoved(float wait)
    {
        mode = Mode.SingleTouchMoved;

        if (wait < 0.30f)
        {
            Pan(touches[0], firstTouch, firstCamPos);
        }
        else
        {
            SelectMultipleFloors();
        }

    }

    internal override void FingersReleased()
    {
        selectFloor.Clear();
    }

    public static STR_Floors GetFloor(Vector2Int position)
    {
        return floorList.Find(f => f.pos ==(Vector3Int)position);
    }
    private void SelectMultipleFloors()
    {
        if (touchPosLastFrame == touches[0].locationCurrent) return; 

        Vector3Int corner1 = MyFunctions.TouchtoTilePos(touches[0].locationFirst);
        Vector3Int corner2 = MyFunctions.TouchtoTilePos(touches[0].locationCurrent);

        Vector3Int bottomLeft = new Vector3Int();
        Vector3Int topRight = new Vector3Int();

        if (corner1.x <= corner2.x)
        {
            bottomLeft.x = corner1.x;
            topRight.x = corner2.x;
        }
        else
        {
            bottomLeft.x = corner2.x;
            topRight.x = corner1.x;
        }

        if (corner1.y <= corner2.y)
        {
            bottomLeft.y = corner1.y;
            topRight.y = corner2.y;
        }
        else
        {
            bottomLeft.y = corner2.y;
            topRight.y = corner1.y;
        }

        for (int x = bottomLeft.x; x >= bottomLeft.x && x <= topRight.x; x++)
        {
            for (int y = bottomLeft.y; y >= bottomLeft.y && y <= topRight.y; y++)
            {
                Vector3Int current = new Vector3Int(x, y, 0);

                if (floorList.Find(m => m.pos == current) == null)
                {
                    STR_Floors add = AddFloor(current);

                    if(add!=null)
                        selectFloor.Add(add);
                }
                else
                {
                    continue;
                }
            }
        }

        foreach (STR_Floors floor in selectFloor.ToArray())
        {
            if (floor.pos.x < bottomLeft.x || floor.pos.x > topRight.x || floor.pos.y < bottomLeft.y || floor.pos.y > topRight.y)
                RemoveFloor(floor.pos);
        }
    }
    public static void CreateFloors(SaveFile load,GameLoader gameLoader)
    {
        if (load.floorPosX == null) return;

        GameObject marker = gameLoader.wallMarker;

        ITEM_CarpetApplier carpetApplier = gameLoader.carpetLoader;

        for (int i = 0; i < load.floorPosX.Length; i++)
        {
            Vector3Int pos = new Vector3Int(load.floorPosX[i], load.floorPosY[i], 0);

            GameObject currMarker = GameObject.Instantiate(marker, GameObject.FindObjectOfType<SO_FloorMarkers>().transform);

            currMarker.transform.position = new Vector3(pos.x + 0.50f, pos.y + 0.50f, 0f);

            SCO_Carpets currCarpet = carpetApplier.carpetsDatabase[load.floorIndex[i]];

            //ADD TO LIST
            STR_Floors newFloor = new STR_Floors(pos, currCarpet, currMarker);
        }

        foreach(STR_Floors floor in floorList)
        {
            floor.marker.SetActive(false);
        }
    }

    internal override void Update()
    {
        see = floorList.ToArray();

        base.Update();
    }
    internal override void SingleTouch()
    {
        mode = Mode.SingleTouch;

        Vector3Int current = MyFunctions.TouchtoTilePos(firstTouch);

        if (floorList.Find(c => c.pos == current) != null)
        {
            RemoveFloor(current);
        }
        else
        {
            AddFloor(current);
        }

    }

    private STR_Floors AddFloor(Vector3Int current)
    {
        if (!CheckValidity(current)) return null;

        GameObject m = GameObject.Instantiate(greenMarker);

        STR_Floors newFloor = new STR_Floors(current, floor, m);

        return newFloor;
    }

    public static void RemoveFloor(Vector3Int pos)
    {
        STR_Floors current = floorList.Find(cn => cn.pos == pos);


        Vector2Int[] blocks = Positions.TileToBlock((Vector2Int)pos);

        //DO NOT REMOVE WALLS WITH BLOCKS
        foreach (Vector2Int v in blocks)
        {
            if (S_WorldBlocks.GetBlockinPosition(v) != null)
                return;
        }

        if (current != null)
        {
            if (current.carpet != null)
            {
                S_Tilemap.floors.SetTile(pos, null);
                Destroy(current.marker);
            }

            floorList.Remove(current);
        }
    }


    private bool CheckValidity(Vector3Int pos)
    {
        return WallsManager.wallList.Find(c => c.pos == pos) == null;
    }
}
