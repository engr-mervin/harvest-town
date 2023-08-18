using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEditor;

public class WallsManager : BC_TouchControls
{
    [Header("User Driven Variables")]
    public DB_DefaultWalls wall;

    [SerializeField]
    private int maxX = 0, maxY = 0, minX=0, minY=0;

    public static List<STR_Walls> wallList = new List<STR_Walls>();
    public STR_Walls[] see;

    public override void ClearStaticList()
    {
        foreach (STR_Walls wall in wallList)
        {
            wall.DestroyThis();
        }
        wallList.Clear();
        print("cleared walls");
    }

    public static void CreateWalls(SaveFile load,GameLoader loader)
    {
        if (load.wallPosX == null) return;

        ITEM_WallpaperApplier wallMaker = loader.wallpaperLoader;
        GameObject marker = loader.wallMarker;

        for (int i = 0; i < load.wallPosX.Length; i++)
        {
            Vector3Int pos = new Vector3Int(load.wallPosX[i], load.wallPosY[i], 0);
            Vector3 position = new Vector3(pos.x + 0.50f, pos.y + 0.50f, 0f);

            GameObject wall = loader.walls[load.wallIndices[i]];

            GameObject currWall = GameObject.Instantiate(wall, SO_PlayerWalls.instance.transform);
            GameObject currMarker = GameObject.Instantiate(marker, SO_WallMarkers.instance.transform);
            SCO_Wallsets currWallset = wallMaker.wallsetsDatabase[load.wallPaperIndices[i]];

            currWall.transform.position = position;
            currMarker.transform.position = position;

            //APPLY WALLPAPER
            if (load?.wallPaperIndices != null && load?.wallPaperIndices?[i] != 0)
            {
                ApplyWallpaper(currWall, currWallset);
            }

            //ADD TO LIST
            STR_Walls newWall = new STR_Walls(pos, currMarker);
            newWall.SetWall(currWall);
            newWall.wallPaperIndex = load.wallPaperIndices[i];
        }

        //CLEAR TILES - FIXER ONLY REMOVE IN THE FUTURE
        foreach(STR_Walls wall in wallList)
        {
            if(HasDown(wall.pos))
            {
                S_Tilemap.walls.SetTile(wall.pos + Vector3Int.up, null);
            }
        }
        //ADD TILES
        foreach (STR_Walls wall in wallList)
        {
            Tile top = null;
            Tile bot = null;

            if (!HasDown(wall.pos))
            {
                top = S_Tilemap.walls.GetTile<Tile>(wall.pos + Vector3Int.up);
                bot = S_Tilemap.walls.GetTile<Tile>(wall.pos);
            }

            wall.SetTile(top, bot);

            wall.marker.SetActive(false);
        }
    }

    private static void ApplyWallpaper(GameObject wall,SCO_Wallsets wallset)
    {
        Sprite sprite = null;

        int wallIndex = wall.GetComponent<WallSaver>().wallIndex;

        if (wallIndex > 7) return;

        switch(wallIndex)
        {
            case 0:
                sprite = wallset.M1;
                break;
            case 1:
                sprite = wallset.L1;
                break;
            case 2:
                sprite = wallset.M1;
                break;
            case 3:
                sprite = wallset.R1;
                break;
            case 4:
                sprite = wallset.M4;
                break;
            case 5:
                sprite = wallset.L2;
                break;
            case 6:
                sprite = wallset.M2;
                break;
            case 7:
                sprite = wallset.R2;
                break;
        }
        wall.GetComponent<SpriteRenderer>().sprite = sprite;            
    }

    internal override void OnDisable()
    {
        base.OnDisable();

        foreach (STR_Walls wall in wallList)
        {
            wall.marker.SetActive(false);
        }

    }


    private void OnEnable()
    {
        foreach (STR_Walls wall in wallList)
        {
            wall.marker.SetActive(true);
        }
    }

    internal override void Update()
    {
        see = wallList.ToArray();

        base.Update();
    }

    internal override void SingleTouch()
    {
        mode = Mode.SingleTouch;

        Vector3Int current = MyFunctions.TouchtoTilePos(firstTouch);

        if (ListContains((Vector2Int)current))
        {
            RemoveWall(current);
        }
        else
        {
            if (!CheckValidity(current)) return;

            STR_Walls newWall = CreateWall(current);
        }

    }

    internal override void SingleTouchMoved(float wait)
    {
        mode = Mode.SingleTouchMoved;
        Vector3Int initial = MyFunctions.TouchtoTilePos(firstTouch);

        if (wait<0.30f)
        {
            Pan(touches[0], firstTouch, firstCamPos);
        }
        else
        {
            //get initial tile
            Vector3Int current = MyFunctions.TouchtoTilePos(touches[0].locationCurrent);
            Vector3Int diff = current - initial;

            bool Xmode = (Mathf.Abs(diff.x) >= Mathf.Abs(diff.y));


            if(Xmode&&diff.x<0)//left
            {
                minX = diff.x < minX ? diff.x : minX;

                for (int x=0;x>=diff.x;x--)
                {
                    Vector3Int currX = new Vector3Int(initial.x + x, initial.y, initial.z);

                    if (!CheckValidity(currX)) continue;
                    if (wallList.Find(c => c.pos == currX) != null) continue;

                    STR_Walls newWall = CreateWall(currX);
                }
                for(int x=diff.x-1;x>=minX; x--)
                {
                    Vector3Int currX = new Vector3Int(initial.x + x, initial.y, initial.z);
                    RemoveWall(currX);
                }
            }

            if (Xmode && diff.x > 0)//right
            {
                maxX = diff.x > maxX ? diff.x : maxX;

                for (int x = 0; x <= diff.x; x++)
                {
                    Vector3Int currX = new Vector3Int(initial.x + x, initial.y, initial.z);

                    if (!CheckValidity(currX)) continue;
                    if (wallList.Find(c => c.pos == currX) != null) continue;

                    STR_Walls newWall = CreateWall(currX);
                }
                for (int x = diff.x + 1; x <= maxX; x++)
                {
                    Vector3Int currX = new Vector3Int(initial.x + x, initial.y, initial.z);
                    RemoveWall(currX);
                }
            }

            if (Xmode && diff.x == 0)//right
            {
                minX = diff.x < minX ? diff.x : minX;
                maxX = diff.x > maxX ? diff.x : maxX;

                Vector3Int zero = new Vector3Int(initial.x, initial.y, initial.z);

                for (int x = diff.x - 1; x >= minX; x--)
                {
                    Vector3Int currX = new Vector3Int(initial.x + x, initial.y, initial.z);
                    RemoveWall(currX);
                }

                for (int x = diff.x + 1; x <= maxX; x++)
                {
                    Vector3Int currX = new Vector3Int(initial.x + x, initial.y, initial.z);
                    RemoveWall(currX);
                }

                for (int y = diff.y - 1; y >= minY; y--)
                {
                    Vector3Int currX = new Vector3Int(initial.x, initial.y + y, initial.z);
                    RemoveWall(currX);
                }

                for (int y = diff.y + 1; y <= maxX; y++)
                {
                    Vector3Int currX = new Vector3Int(initial.x, initial.y + y, initial.z);
                    RemoveWall(currX);
                }

                if (!CheckValidity(zero)) return;
                if (wallList.Find(c => c.pos == zero) != null) return;

                STR_Walls newWall = CreateWall(zero);

            }

            if (!Xmode && diff.y < 0)//down
            {
                minY = diff.y < minY ? diff.y : minY;

                for (int y = 0; y >= diff.y; y--)
                {
                    Vector3Int currX = new Vector3Int(initial.x, initial.y+y, initial.z);

                    if (!CheckValidity(currX)) continue;
                    if (wallList.Find(c => c.pos == currX) != null) continue;

                    STR_Walls newWall = CreateWall(currX);
                }
                for (int y = diff.y - 1; y >= minY; y--)
                {
                    Vector3Int currX = new Vector3Int(initial.x, initial.y+y, initial.z);
                    RemoveWall(currX);
                }
            }

            if (!Xmode && diff.y > 0)//up
            {
                maxY = diff.y > maxY ? diff.y : maxY;

                for (int y = 0; y <= diff.y; y++)
                {
                    Vector3Int currX = new Vector3Int(initial.x, initial.y+y, initial.z);

                    if (!CheckValidity(currX)) continue;
                    if (wallList.Find(c => c.pos == currX) != null) continue;

                    STR_Walls newWall = CreateWall(currX);
                }
                for (int y = diff.y + 1; y <= maxY; y++)
                {
                    Vector3Int currX = new Vector3Int(initial.x, initial.y+y, initial.z);
                    RemoveWall(currX);
                }
            }
        }
    }

    private STR_Walls CreateWall(Vector3Int current)
    {
        Vector2Int[] blocks = Positions.TileToBlock((Vector2Int)current);

        //DO NOT REMOVE WALLS WITH BLOCKS
        foreach (Vector2Int v in blocks)
        {
            if (S_WorldBlocks.GetBlockinPosition(v) != null)
                return null;
        }

        GameObject m = GameObject.Instantiate(wall.marker);
        STR_Walls newWall = new STR_Walls(current,m);
        CreateWallObject(newWall);
        return newWall;

    }

    public static bool HasRight(Vector3Int pos)
    {
        Vector3Int right = pos + Vector3Int.right;
        return(ListContains(right));
    }

    public static bool HasLeft(Vector3Int pos)
    {
        Vector3Int left = pos + Vector3Int.left;
        return (ListContains(left));
    }

    public static bool HasUp(Vector3Int pos)
    {
        Vector3Int up = pos + Vector3Int.up;
        return (ListContains(up));
    }

    public static bool HasDown(Vector3Int pos)
    {
        Vector3Int down = pos + Vector3Int.down;
        return (ListContains(down));
    }

    private void RemoveWall(Vector3Int pos)
    {
        Vector2Int[] blocks = Positions.TileToBlock((Vector2Int)pos);

        //DO NOT REMOVE WALLS WITH BLOCKS
        foreach(Vector2Int v in blocks)
        {
            if (S_WorldBlocks.GetBlockinPosition(v) != null)
                return;
        }

        CleanRemoveWallMember(pos);

        RefreshAround(pos);
    }
    
    private void RefreshAround(Vector3Int pos) //used to refresh all walls around a point
    {
        if (HasDown(pos))
        {
            RefreshWall(pos + Vector3Int.down);
        }
        if (HasUp(pos))
        {
            RefreshWall(pos + Vector3Int.up);
        }
        if (HasLeft(pos))
        {
            RefreshWall(pos + Vector3Int.left);
        }
        if (HasRight(pos))
        {
            RefreshWall(pos + Vector3Int.right);
        }
    }

    private void CreateWallObject(STR_Walls newWall)
    {
        foreach (Vector2Int blocks in Positions.TileToBlock((Vector2Int)(newWall.pos)))
        {
            print(blocks);
            AStar_Grid.GetNode(blocks).walkable = false;
        }

        RefreshWall(newWall.pos);
        RefreshAround(newWall.pos);
    }

    private void AddTile(Vector3Int pos)
    {
        if (!HasDown(pos))
        {
            S_Tilemap.walls.SetTile(pos, wall.Bot);
            S_Tilemap.walls.SetTile(pos + Vector3Int.up, wall.Top);
            GetWall(pos).SetTile(wall.Top, wall.Bot);

        }

        if(HasUp(pos))
        {
            S_Tilemap.walls.SetTile(pos + Vector3Int.up + Vector3Int.up, null);
        }
    }

    private void CleanRemoveWallMember(Vector3Int pos)
    {
        STR_Walls current = GetWall(pos);
        if (current != null)
        {
            //REMOVE FROM LIST
            wallList.Remove(current);
            //STRUCT METHOD
            current.DestroyThis();

        }
    }

    
    public static bool ListContains(Vector2Int position)
    {
        return wallList.Find(w => w.pos == (Vector3Int)position) != null;
    }

    public static STR_Walls GetWall(Vector2Int position)
    {
        return wallList.Find(w => w.pos == (Vector3Int)position);
    }

    public static bool ListContains(Vector3Int position)
    {
        return wallList.Find(w => w.pos == position) != null;
    }

    public static STR_Walls GetWall(Vector3Int position)
    {
        return wallList.Find(w => w.pos == position);
    }


    private void RefreshWall(Vector3Int pos)
    {
        STR_Walls current = GetWall(pos);

        GameObject g = null;

        GameObject[] children = current.RemoveWallOnly();
        
        //---------------------------------------------------------------------------------------//
        //case none
        if (!HasUp(pos) && !HasDown(pos) && !HasLeft(pos) && !HasRight(pos))
        {
            g = Instantiate(wall.none);
        }
        //case up only
        if (HasUp(pos) && !HasDown(pos) && !HasLeft(pos) && !HasRight(pos))
        {
            g = Instantiate(wall.up);
        }
        //case down only
        if (!HasUp(pos) && HasDown(pos) && !HasLeft(pos) && !HasRight(pos))
        {
            g = Instantiate(wall.down);
        }
        //case left only
        if (!HasUp(pos) && !HasDown(pos) && HasLeft(pos) && !HasRight(pos))
        {
           g = Instantiate(wall.left);
        }
        //case right only
        if (!HasUp(pos) && !HasDown(pos) && !HasLeft(pos) && HasRight(pos))
        {
            g = Instantiate(wall.right);
        }
        //---------------------------------------------------------------------------------------//
        //case up and down
        if (HasUp(pos) && HasDown(pos) && !HasLeft(pos) && !HasRight(pos))
        {
            g = Instantiate(wall.UD);
        }
        //case up and left
        if (HasUp(pos) && !HasDown(pos) && HasLeft(pos) && !HasRight(pos))
        {
            g = Instantiate(wall.UL);
        }
        //case up and right
        if (HasUp(pos) && !HasDown(pos) && !HasLeft(pos) && HasRight(pos))
        {
            g = Instantiate(wall.UR);
        }
        //case down and left
        if (!HasUp(pos) && HasDown(pos) && HasLeft(pos) && !HasRight(pos))
        {
            g = Instantiate(wall.DL);
        }
        //case down and right
        if (!HasUp(pos) && HasDown(pos) && !HasLeft(pos) && HasRight(pos))
        {
            g = Instantiate(wall.DR);
        }
        //case left and right
        if (!HasUp(pos) && !HasDown(pos) && HasLeft(pos) && HasRight(pos))
        {
            g = Instantiate(wall.LR);
        }
        //---------------------------------------------------------------------------------------//
        //no right
        if (HasUp(pos) && HasDown(pos) && HasLeft(pos) && !HasRight(pos))
        {
            g = Instantiate(wall.NR);
        }
        //no left
        if (HasUp(pos) && HasDown(pos) && !HasLeft(pos) && HasRight(pos))
        {
            g = Instantiate(wall.NL);
        }
        //no up
        if (!HasUp(pos) && HasDown(pos) && HasLeft(pos) && HasRight(pos))
        {
            g = Instantiate(wall.NU);
        }
        //no down
        if (HasUp(pos) && !HasDown(pos) && HasLeft(pos) && HasRight(pos))
        {
            g = Instantiate(wall.ND);
        }
        //---------------------------------------------------------------------------------------//
        //all
        if (HasUp(pos) && HasDown(pos) && HasLeft(pos) && HasRight(pos))
        {
            g = Instantiate(wall.ALL);
        }

        AddTile(pos);

        g.transform.position = pos + new Vector3(0.50f, 0.50f, 0f);
        g.transform.parent = SO_PlayerWalls.instance.transform;

        if (children != null)
        {
            foreach (GameObject a in children)
            {
                a.transform.parent = g.transform;
            }
        }

        current.SetWall(g);

        ReSprite(current);
    }

    private void ReSprite(STR_Walls wall)
    {
        if (wall.wallPaperIndex == 0) return;

        GameLoader gameLoader = Resources.Load("GameLoader", typeof(GameLoader)) as GameLoader;
        ITEM_WallpaperApplier wallMaker = gameLoader.wallpaperLoader;

        ApplyWallpaper(wall.wall, wallMaker.wallsetsDatabase[wall.wallPaperIndex]);
    }

    internal override void FingersReleased()
    {
        minX = 0;
        minY = 0;
        maxX = 0;
        maxY = 0;
    }

    private bool CheckValidity(Vector3Int pos)
    {
        if (HasUp(pos) && HasLeft(pos) && HasUp(pos + Vector3Int.left))
            return false;
        if (HasUp(pos) && HasRight(pos) && HasUp(pos + Vector3Int.right))
            return false;
        if (HasDown(pos) && HasLeft(pos) && HasDown(pos + Vector3Int.left))
            return false;
        if (HasDown(pos) && HasRight(pos) && HasDown(pos + Vector3Int.right))
            return false;

        return true;
    }
}
