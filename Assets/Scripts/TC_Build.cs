using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class TC_Build : BC_TouchControls
{
    [Header("Button Driven Variables")]
    public bool eraseMode = false;
    public bool selectMode = false;
    public bool buildWallMode = false;

    public Tile active;
    public Tilemap activeTileMap;

    public GameObject activeWall;

    [Header("User Driven Variables")]
    public Button undo;
    public Button redo;


    public STR_GridObject[] see;


    public List<STR_UndoTile> undoTiles;
    public List<STR_UndoTile> redoTiles;

    public List<GridTile> selectCol = new List<GridTile>();
    public List<GridTile> selectUncol = new List<GridTile>();
    public List<STR_GridObject> selectMarker = new List<STR_GridObject>();

    public List<Vector3Int> walls = new List<Vector3Int>();
    public List<STR_GridObject> wallsMarker = new List<STR_GridObject>();


    internal override void OnDisable()
    {
        base.OnDisable();

        selectCol.Clear();
        selectUncol.Clear();

        foreach(STR_GridObject g in selectMarker)
        {
            Destroy(g.g);
        }

        selectMarker.Clear();
    }

    private void Awake()
    {
        touches = new List<UniqueTouch>();
        undoTiles = new List<STR_UndoTile>();
        redoTiles = new List<STR_UndoTile>();
    }

    internal override void Update()
    {
        see = wallsMarker.ToArray();
        base.Update();
    }

    private void Select(UniqueTouch ut)
    {
        //start with no pan
        if (touchPosLastFrame == ut.locationCurrent) return; // the touch is not moved
        touchPosLastFrame = ut.locationCurrent;

        Vector3 corner1 = Camera.main.ScreenToWorldPoint(ut.locationFirst);
        Vector3 corner2 = Camera.main.ScreenToWorldPoint(ut.locationCurrent);

        Vector3Int bottomLeft = new Vector3Int();
        Vector3Int topRight = new Vector3Int();

        if (corner1.x <= corner2.x)
        {
            bottomLeft.x = Mathf.FloorToInt(corner1.x);
            topRight.x = Mathf.FloorToInt(corner2.x);
        }
        else
        {
            bottomLeft.x = Mathf.FloorToInt(corner2.x);
            topRight.x = Mathf.FloorToInt(corner1.x);
        }

        if (corner1.y <= corner2.y)
        {
            bottomLeft.y = Mathf.FloorToInt(corner1.y);
            topRight.y = Mathf.FloorToInt(corner2.y);
        }
        else
        {
            bottomLeft.y = Mathf.FloorToInt(corner2.y);
            topRight.y = Mathf.FloorToInt(corner1.y);
        }

        for (int x = bottomLeft.x; x >= bottomLeft.x && x <= topRight.x; x++)
        {
            for (int y = bottomLeft.y; y >= bottomLeft.y && y <= topRight.y; y++)
            {
                Vector3Int current = new Vector3Int(x, y, 0);

                Tile tilec = S_Tilemap.walls.GetTile<Tile>(current);
                Tile tileu = S_Tilemap.floors.GetTile<Tile>(current);

                if (selectMarker.Find(m => m.pos == current) == null) 
                {
                    GameObject m = S_ObjectControls.CreateUI("Marker");
                    m.transform.position = current+ new Vector3(0.50f,0.50f,0f);
                    selectMarker.Add(new STR_GridObject(m, current));
                }

                if (tilec != null && selectCol.Find(gt => gt.pos == current) == null) //there is a tile and it is not in the list
                    selectCol.Add(new GridTile(tilec, current));

                if (tileu != null && selectUncol.Find(gt => gt.pos == current) == null)
                    selectUncol.Add(new GridTile(tileu, current));
            }
        }

        foreach (GridTile gtc in selectCol.ToArray())
        {
            if (gtc.pos.x < bottomLeft.x || gtc.pos.x > topRight.x || gtc.pos.y < bottomLeft.y || gtc.pos.y > topRight.y)
                selectCol.Remove(gtc);
        }

        foreach (GridTile gtu in selectUncol.ToArray())
        {
            if (gtu.pos.x < bottomLeft.x || gtu.pos.x > topRight.x || gtu.pos.y < bottomLeft.y || gtu.pos.y > topRight.y)
                selectUncol.Remove(gtu);
        }
        foreach (STR_GridObject m in selectMarker.ToArray())
        {
            if (m.pos.x < bottomLeft.x || m.pos.x > topRight.x || m.pos.y < bottomLeft.y || m.pos.y > topRight.y)
            {
                Destroy(m.g);
                selectMarker.Remove(m);
            }
        }

    }
    
    internal override void SingleTouch()
    {
        mode = Mode.SingleTouch;

        if (selectMode) return;
        if (buildWallMode)
        {
             ApplyHighlight(firstTouch,false);
        }
        else
        {
            if (!eraseMode)
                ApplyTile();
            else 
                EraseTile();
        }
    }

    internal override void SingleTouchMoved(float wait)
    {
        mode = Mode.SingleTouchMoved;

        if (wait < 0.50f)
        {
            Pan(touches[0], firstTouch, firstCamPos);
        }
        else if (selectMode)
            Select(touches[0]);
        else if (buildWallMode)
            ApplyHighlight(touches[0].locationCurrent,true);
    }

    private void ApplyTile()
    {
        if (GM.playerObj == null) return;
        if (active == null)
        {
            return;
        }
        //get point
        Vector3Int pointA = MyFunctions.TouchtoTilePos(firstTouch);
        Vector2Int pointA2 = new Vector2Int(pointA.x,pointA.y);

        if (active == activeTileMap.GetTile<Tile>(pointA))
            return;

        //if there is an object in the position and the tile you're applying isn't compatible with  that object
        if (S_WorldBlocks.GetBlockinPosition(pointA2)!=null)//Tmap.GetTileType(pointA2)!=Tmap.GetTileTypeFromName(active.name))
            return;

        //set undo tile
        Tilemap tm;
        if (S_Tilemap.walls.GetTile<Tile>(pointA) != null) //case 1  there is a wall
            tm = S_Tilemap.walls;
        else if (S_Tilemap.floors.GetTile<Tile>(pointA) != null) //case 2 there is a floor
            tm = S_Tilemap.floors;
        else//case 3 there is nothing
            tm = activeTileMap;

        STR_UndoTile ut = new STR_UndoTile(tm.GetTile<Tile>(pointA), pointA, tm);
        undoTiles.Add(ut);

        //apply tile
        activeTileMap.SetTile(pointA, active);

        //erase tile
        if(tm!=activeTileMap) // if there is a collideable and you're placing uncollideable remove it
        tm.SetTile(pointA, null);

        //undo redo
        undo.interactable = true;
        redoTiles.Clear();
        redo.interactable = false;
    }

    private void EraseTile()
    {

        if (GM.playerObj == null) return;
        //get point
        Vector3 point = Camera.main.ScreenToWorldPoint(firstTouch);
        int x = Mathf.FloorToInt(point.x);
        int y = Mathf.FloorToInt(point.y);
        int z = 0;
        Vector3Int pointA = new Vector3Int(x, y, z);

        if (S_Tilemap.walls.GetTile<Tile>(pointA) == null && S_Tilemap.floors.GetTile<Tile>(pointA) == null) return;

        //set undo tile
        
        //remove tile
        if(S_Tilemap.walls.GetTile<Tile>(pointA)!=null)
        {
            STR_UndoTile ut = new STR_UndoTile(S_Tilemap.walls.GetTile<Tile>(pointA), pointA, S_Tilemap.walls);
            undoTiles.Add(ut);

            S_Tilemap.walls.SetTile(pointA, null);
            undo.interactable = true;
            redoTiles.Clear();
            redo.interactable = false;

        }   
        else
        {
            STR_UndoTile ut = new STR_UndoTile(S_Tilemap.floors.GetTile<Tile>(pointA), pointA, S_Tilemap.floors);
            undoTiles.Add(ut);

            S_Tilemap.floors.SetTile(pointA, null);
            undo.interactable = true;
            redoTiles.Clear();
            redo.interactable = false;
        }
    }

    private void ApplyHighlight(Vector2 currentPos,bool pan)
    {
        if (activeWall == null) return;
        Vector3Int current = MyFunctions.TouchtoTilePos(currentPos);

        if (walls.Contains(current)&&!pan)
        {
            walls.Remove(current);
            STR_GridObject a = wallsMarker.Find(wm => wm.pos == current);
            Destroy(a.g);
            wallsMarker.Remove(a);
        }
        else if(!walls.Contains(current))
        {
            GameObject m = GameObject.Instantiate(activeWall);
            m.transform.position = current + new Vector3(0.50f, 0.50f, 0f);

            walls.Add(current);
            wallsMarker.Add(new STR_GridObject(m, current));
        }
    }

}
