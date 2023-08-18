using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class S_CreateWall
{
    public Vector2Int min;
    public Vector2Int max;

    public List<Vector3Int> check = new List<Vector3Int>();
    public List<Vector3Int> allNeighbors = new List<Vector3Int>();

    public static List<Vector3Int> openSpaces = new List<Vector3Int>();
    public static List<Vector3Int> closedSpaces = new List<Vector3Int>();

    public static List<Vector3Int> redded = new List<Vector3Int>();

    public static List<GameObject> openMarkers = new List<GameObject>();

    public static List<GameObject> redmarkers = new List<GameObject>();

    public static List<Vector3Int> offsetAbove = new List<Vector3Int>();
    public void AddToOpen(Vector3Int point)
    {
        if (closedSpaces.Contains(point))
            closedSpaces.Remove(point);

        if (!openSpaces.Contains(point))
        {
            openSpaces.Add(point);
            GameObject m = S_ObjectControls.CreateUI("Marker");
            m.transform.localScale = Vector3.one * 0.50f;
            m.transform.position = point + Vector3.one * 0.50f;
            openMarkers.Add(m);
        }
    }

    public void AddToClose(Vector3Int point)
    {
        if (!closedSpaces.Contains(point))
            closedSpaces.Add(point);

        if (openSpaces.Contains(point))
            openSpaces.Remove(point);
    }

    public S_CreateWall(ref List<STR_GridObject> markers)
    {
        foreach (GameObject g in redmarkers)
        {
            GameObject.Destroy(g);
        }
        redmarkers.Clear();
        redded.Clear();

        foreach (STR_GridObject gm in markers)
        {
            check.Add(gm.pos);
        }

        int totalcount;

        do
        {
            openSpaces.Clear();
            closedSpaces.Clear();
            foreach (GameObject g in openMarkers)
            {
                GameObject.Destroy(g);
            }
            openMarkers.Clear();

            totalcount = 0;

            //Get MAX,MIN X and Y
            ComputeMinMax(check);

            //Identify leftmost markers and right most markers
            int removecount;

            do //REMOVE INVALID WALLS
            {
                removecount = 0;
                int a = min.y;
                int b = max.y;
                int c = min.x;
                int d = max.x;
                for (int y = a; y <= b; y++)
                {
                    removecount += RemoveInvalidHor(ref markers, y);
                }
                for (int x = c; x <= d; x++)
                {
                    removecount += RemoveInvalidVer(ref markers, x);
                }

                totalcount += removecount;
            }
            while (removecount > 0);

            int opencount;

            do  //DEFINE OPEN SPACES
            {
                opencount = 0;
                opencount += CheckOpenSpace(ref markers);
            }
            while (opencount > 0);

            int openremovecount;

            do //REMOVE INVALID WALLS
            {
                openremovecount = 0;
                openremovecount += RemoveInvalidOpen(ref markers);

                totalcount += openremovecount;
            }
            while (openremovecount > 0);

            ComputeMinMax(check);
        }
        while (totalcount > 0);


        PassedColor(ref markers);

        if (redmarkers.Count == 0)
            StartBuildWalls(ref markers);
    }

    private void StartBuildWalls(ref List<STR_GridObject> markers)
    {
        Partition partition = Resources.Load("WallSet/Partitions", typeof(Partition)) as Partition;

        foreach (Vector3Int a in check)
        {
            STR_GridObject g = markers.Find(gm => gm.pos == a);
            WallSet wallset = g.g.GetComponent<WallSet>();

            //case outside walls
            //LEFT
            #region//OUTSIDE WALLS
            //outside wall left
            if (IsFurthest(a, Vector3Int.left, a.y) && Has(a, Vector3Int.up) && Has(a, Vector3Int.down)&&Has(a+Vector3Int.up,Vector3Int.up))
            {
                S_Tilemap.walls.SetTile(a + Vector3Int.up, partition.L2);
            }
            //outside wall left upper most == outside wall top outside corner left 
            //RIGHT
            if (IsFurthest(a, Vector3Int.right, a.y) && Has(a, Vector3Int.up) && Has(a, Vector3Int.down) && Has(a + Vector3Int.up, Vector3Int.up))
            {
                S_Tilemap.walls.SetTile(a+Vector3Int.up, partition.R2);
            }
            //outside wall rigt upper most == outside wall top outside corner right 
            //TOP
            //outside wall top
            if (IsFurthest(a, Vector3Int.up, a.x) && Has(a, Vector3Int.left) && Has(a, Vector3Int.right)&&!Has(a,Vector3Int.down))
            {
                S_Tilemap.walls.SetTile(a, wallset.B2);
                S_Tilemap.walls.SetTile(a + Vector3Int.up, wallset.B1);
            }
            //outside wall top t-junction
            if (IsFurthest(a, Vector3Int.up, a.x) && Has(a, Vector3Int.left) && Has(a, Vector3Int.right) && Has(a, Vector3Int.down))
            {
                S_Tilemap.walls.SetTile(a + Vector3Int.up, partition.D1);
            }
            //outside wall top outside corner right
            if (IsFurthest(a, Vector3Int.up, a.x)&& IsFurthest(a, Vector3Int.right, a.y) && Has(a, Vector3Int.left) && Has(a, Vector3Int.down))
            {
                S_Tilemap.walls.SetTile(a + Vector3Int.up, partition.R1);

                Vector3Int b = a + Vector3Int.down;
                if (Has(b, Vector3Int.down)&&!Has(b,Vector3Int.right)&&!Has(b,Vector3Int.left))
                    S_Tilemap.walls.SetTile(a , partition.R2);
            }
            //outside wall top outside corner left 
            if (IsFurthest(a, Vector3Int.up, a.x) && IsFurthest(a, Vector3Int.left, a.y) && Has(a, Vector3Int.right) && Has(a, Vector3Int.down))
            {
                S_Tilemap.walls.SetTile(a + Vector3Int.up, partition.L1);


                Vector3Int b = a + Vector3Int.down;
                if (Has(b, Vector3Int.down) && !Has(b, Vector3Int.right) && !Has(b, Vector3Int.left))
                    S_Tilemap.walls.SetTile(a, partition.L2);
            }
            //outside wall top inside corner right _|
            if(Has(a, Vector3Int.left) && Has(a, Vector3Int.up)&& IsFurthest(a+Vector3Int.left, Vector3Int.up, a.x-1) && IsFurthest(a+Vector3Int.up, Vector3Int.left, a.y+1))
            {
                S_Tilemap.walls.SetTile(a, wallset.C2);
                S_Tilemap.walls.SetTile(a + Vector3Int.up, wallset.C1);
            }
            //outside wall top inside corner left |_
            if (Has(a, Vector3Int.right) && Has(a, Vector3Int.up) && IsFurthest(a + Vector3Int.right, Vector3Int.up, a.x + 1) && IsFurthest(a + Vector3Int.up, Vector3Int.right, a.y + 1))
            {
                S_Tilemap.walls.SetTile(a, wallset.A2);
                S_Tilemap.walls.SetTile(a + Vector3Int.up, wallset.A1);
            }
            //BOTTOM
            //outside wall bottom
            if (IsFurthest(a, Vector3Int.down, a.x) && Has(a, Vector3Int.left) && Has(a, Vector3Int.right)&&!Has(a,Vector3Int.up))
            {
                S_Tilemap.walls.SetTile(a, partition.B1);
            }
            //t junction
            if (IsFurthest(a, Vector3Int.down, a.x) && Has(a, Vector3Int.left) && Has(a, Vector3Int.right) && Has(a, Vector3Int.up))
            {
                S_Tilemap.walls.SetTile(a, partition.U1);
                if(Has(a+Vector3Int.up,Vector3Int.up))
                    S_Tilemap.walls.SetTile(a+Vector3Int.up, partition.UD1);
            }
            //outside wall bottom outside corner right
            if (IsFurthest(a, Vector3Int.down, a.x) && IsFurthest(a, Vector3Int.right, a.y) && Has(a, Vector3Int.left) && Has(a, Vector3Int.up))
            {
                S_Tilemap.walls.SetTile(a, partition.R3);
                S_Tilemap.walls.SetTile(a + Vector3Int.up, partition.R2);
            }
            //outside wall bottom outside corner left
            if (IsFurthest(a, Vector3Int.down, a.x) && IsFurthest(a, Vector3Int.left, a.y) && Has(a, Vector3Int.right) && Has(a, Vector3Int.up))
            {
                S_Tilemap.walls.SetTile(a, partition.L3);
                S_Tilemap.walls.SetTile(a + Vector3Int.up, partition.L2);
            }
            //outside wall bottom inside corner right 
            if (Has(a, Vector3Int.left) && Has(a, Vector3Int.down) && IsFurthest(a + Vector3Int.left, Vector3Int.down, a.x - 1) && IsFurthest(a + Vector3Int.down, Vector3Int.left, a.y - 1))
            {
                S_Tilemap.walls.SetTile(a, partition.CR1);
            }
            //outside wall bottom inside corner left 
            if (Has(a, Vector3Int.right) && Has(a, Vector3Int.down) && IsFurthest(a + Vector3Int.right, Vector3Int.down, a.x + 1) && IsFurthest(a + Vector3Int.down, Vector3Int.right, a.y - 1))
            {
                S_Tilemap.walls.SetTile(a, partition.CR2);
            }
            #endregion

            #region//INSIDE WALLS
            //updown right & left
            if (IsInside(a) && Has(a, Vector3Int.up) && Has(a, Vector3Int.down))
            {
                Vector3Int b = a + Vector3Int.up;
                if (!(IsInside(b) && !Has(b, Vector3Int.right) && !Has(b, Vector3Int.left) && !Has(a, Vector3Int.up)))
                {
                    S_Tilemap.walls.SetTile(a + Vector3Int.up, partition.UD1);
                }
            }
            //normal inside wall
            if (IsInside(a) && Has(a, Vector3Int.left) && Has(a, Vector3Int.right)&&!Has(a,Vector3Int.down))
            {
                S_Tilemap.walls.SetTile(a, wallset.B2);
                S_Tilemap.walls.SetTile(a + Vector3Int.up, wallset.B1);
            }
            //normal inside wall end right
            if (IsInside(a) && Has(a, Vector3Int.left) && !Has(a, Vector3Int.right) && !Has(a, Vector3Int.down))
            {
                S_Tilemap.walls.SetTile(a, wallset.C2);
                S_Tilemap.walls.SetTile(a + Vector3Int.up, wallset.C1);
            }
            //normal inside wall end left   
            if (IsInside(a) && Has(a, Vector3Int.right) && !Has(a, Vector3Int.left) && !Has(a, Vector3Int.down))
            {
                S_Tilemap.walls.SetTile(a, wallset.A2);
                S_Tilemap.walls.SetTile(a + Vector3Int.up, wallset.A1);
            }
            //normal inside wall corner RIGHT
            if (IsInside(a) && Has(a, Vector3Int.left) && !Has(a, Vector3Int.right) && Has(a, Vector3Int.down)&&!Has(a, Vector3Int.up))
            {
                Vector3Int b = a+Vector3Int.left;

                if(!IsFurthest(b, Vector3Int.down, b.x))
                    S_Tilemap.walls.SetTile(a + Vector3Int.up, partition.IC1);
            }
            //normal inside wall corner LEFT
            if (IsInside(a) && Has(a, Vector3Int.right) && !Has(a, Vector3Int.left) && Has(a, Vector3Int.down) && !Has(a, Vector3Int.up))
            {
                Vector3Int b = a+Vector3Int.right;

                if (!IsFurthest(b,Vector3Int.down,b.x))
                    S_Tilemap.walls.SetTile(a + Vector3Int.up, partition.IC2);
            }

            //normal inside wall T-junction
            if (IsInside(a) && Has(a, Vector3Int.right) && Has(a, Vector3Int.left) && Has(a, Vector3Int.down) && !Has(a, Vector3Int.up))
            {
                S_Tilemap.walls.SetTile(a + Vector3Int.up, partition.D1);
            }

            //normal inside wall end down
            if (IsInside(a) && !Has(a, Vector3Int.right) && !Has(a, Vector3Int.left) && !Has(a, Vector3Int.down) && Has(a, Vector3Int.up))
            {
                S_Tilemap.walls.SetTile(a + Vector3Int.up, wallset.D1);
                S_Tilemap.walls.SetTile(a , wallset.D2);
            }
            //normal inside wall end up
            if (IsInside(a) && !Has(a, Vector3Int.right) && !Has(a, Vector3Int.left) && Has(a, Vector3Int.down) &&! Has(a, Vector3Int.up))
            {
                S_Tilemap.walls.SetTile(a, partition.U2);
            }


            #endregion
        }
    }
    private bool Has(Vector3Int pos, Vector3Int dir)
    {
        if (check.Contains(pos + dir))
            return true;
        else
            return false;
    }
    private bool IsInside(Vector3Int pos)
    {
        bool a = IsFurthest(pos, Vector3Int.up, pos.x);
        bool b = IsFurthest(pos, Vector3Int.down, pos.x);
        bool c = IsFurthest(pos, Vector3Int.left, pos.y);
        bool d = IsFurthest(pos, Vector3Int.right, pos.y);

        if (a || b || c || d)
            return false;
        else
            return true;
    }
    private bool IsFurthest(Vector3Int pos, Vector3Int dir,int a)
    {
        if (pos == GetFurthest(check, dir, a))
            return true;
        else
            return false;
    }
    private int RemoveInvalidHor(ref List<STR_GridObject> markers, int y)
    {
        int colored = 0;

        Vector3Int[] array = new Vector3Int[]
        {
            Vector3Int.left,
            Vector3Int.right,
        };
        foreach (Vector3Int dir in array)
        {
            Vector3Int current = GetFurthest(check, dir, y);

            while (GetNeighbors(current).Count <= 1 && check.Contains(current))
            {
                ColorRed(ref markers, current);
                colored++;
                current = GetFurthest(check, dir, y);
            }
            ComputeMinMax(check);
        }
        return colored;
    }

    private int RemoveInvalidVer(ref List<STR_GridObject> markers, int x)
    {
        int colored = 0;

        Vector3Int[] array = new Vector3Int[]
        {
            Vector3Int.up,
            Vector3Int.down,
        };
        foreach (Vector3Int dir in array)
        {
            Vector3Int current = GetFurthest(check, dir, x);

            while (GetNeighbors(current).Count <= 1 && check.Contains(current))
            {
                ColorRed(ref markers, current);
                colored++;
                current = GetFurthest(check, dir, x);
            }
            ComputeMinMax(check);
        }
        return colored;
    }

    private Vector3Int GetFurthest(List<Vector3Int> list, Vector3Int dir, int a)
    {
        int furthest = 0;

        Vector3Int current = new Vector3Int(0, 0, 0);

        if (dir.x != 0 && dir.y == 0) //left or right
        {
            switch (dir.x) // set initial vector and furthest int
            {
                case 1:
                    furthest = max.x;
                    break;
                case -1:
                    furthest = min.x;
                    break;
            }

            current.x = furthest;
            current.y = a;

            int i = 0;

            while (!list.Contains(current)||redded.Contains(current))
            {
                if (i > max.x - min.x + 1)
                    break;

                current -= dir;
                i++;
            }
        }

        if (dir.y != 0 && dir.x == 0) //up or down
        {
            switch (dir.y)
            {
                case 1:
                    furthest = max.y;
                    break;
                case -1:
                    furthest = min.y;
                    break;
            }
            current.x = a;
            current.y = furthest;

            int i = 0;

            while (!list.Contains(current) || redded.Contains(current))
            {
                if (i > max.y - min.y + 1)
                    break;

                current -= dir;
                i++;
            }
        }
        return current;
    }
    private int RemoveInvalidOpen(ref List<STR_GridObject> markers)
    {
        int removecount = 0;

        foreach(Vector3Int point in check.ToArray())
        {
            if (redded.Contains(point)) continue;

            if (GetOpenNeighbors(point).Count <= 1)
            {
                continue;
            }
            if (GetOpenNeighbors(point).Count == 2)
            {
                Vector3Int sum = GetOpenNeighbors(point)[0] + GetOpenNeighbors(point)[1] - 2 * point;
                if(sum==Vector3Int.zero)
                {
                    ColorRed(ref markers, point);
                    removecount++;
                }
            }
            if (GetOpenNeighbors(point).Count>=3)
            {
                ColorRed(ref markers, point);
                removecount++;
            }
        }

        return removecount;
    }
    private List<Vector3Int> GetOpenNeighbors(Vector3Int current)
    {

        List<Vector3Int> openNeighbors = new List<Vector3Int>
        {
            new Vector3Int(current.x + 1, current.y, current.z), // right
            new Vector3Int(current.x - 1, current.y, current.z), // left

            new Vector3Int(current.x, current.y + 1, current.z), //up
            new Vector3Int(current.x, current.y - 1, current.z) //down
        };

        foreach (Vector3Int neighbor in openNeighbors.ToArray())
        {
            if (openSpaces.Contains(neighbor))
                continue;
            else
                openNeighbors.Remove(neighbor);
        }

        return openNeighbors;
    }

    private int CheckOpenSpace(ref List<STR_GridObject> markers)
    {
        int opencount = 0;

        int a = min.y-1;
        int b = max.y+1;
        int c = min.x-1;
        int d = max.x+1;

        for (int y = a; y <= b; y++)
        {
            for (int x = c; x <= d; x++)
            {
                Vector3Int p = new Vector3Int(x, y, 0);
                if (check.Contains(p)&&!redded.Contains(p)) continue;
                if (openSpaces.Contains(p)) continue;

                bool left = IsOpen(p, Vector3Int.left);
                bool right = IsOpen(p, Vector3Int.right);
                bool up = IsOpen(p, Vector3Int.up);
                bool down = IsOpen(p, Vector3Int.down);

                if(!left&&!right&&!up&&!down)
                {
                    AddToClose(p);
                }
                else
                {
                    AddToOpen(p);
                    opencount++;
                }
            }
        }

        return opencount;
    }

    private bool IsOpen(Vector3Int p, Vector3Int dir)
    {
        Vector3Int extent = new Vector3Int();
        if (dir.y == 0)
        {
            switch (dir.x) // set initial vector and furthest int
            {
                case 1: //right
                    extent = new Vector3Int(max.x + 1, p.y, 0);
                    break;
                case -1: //left
                    extent = new Vector3Int(min.x - 1, p.y, 0);
                    break;
            }
        }
        if (dir.x == 0)
        {
            switch (dir.y) // set initial vector and furthest int
            {
                case 1: //up
                    extent = new Vector3Int(p.x, max.y + 1, 0);
                    break;
                case -1: //down
                    extent = new Vector3Int(p.x, min.y - 1, 0);
                    break;
            }
        }
        Vector3Int current = p;
        bool open = false;
        while(current!=extent)
        {
            if (check.Contains(current)&&!redded.Contains(current))
            {
                open = false;
                break;
            }
            if(openSpaces.Contains(current))
            {
                open = true;
                break;
            }
            current += dir;
        }

        if (current == extent)
        {
            open = true;
        }

        return open;
    }

    private int CheckSquare(ref List<STR_GridObject> markers)
    {

        int removecount = 0;

        foreach (Vector3Int pos in check.ToArray())
        {

            if (GetNeighbors(pos).Count < 2) continue;
            Vector3Int above = pos + Vector3Int.up;
            Vector3Int right = pos + Vector3Int.right;
            Vector3Int aboveright = pos + Vector3Int.up + Vector3Int.right;

            if (check.Contains(above) && check.Contains(right) && check.Contains(aboveright))
            {
                ColorRed(ref markers, pos);
                ColorRed(ref markers, above);
                ColorRed(ref markers, right);
                ColorRed(ref markers, aboveright);
                removecount += 4;
            }
        }
        return removecount;
    }
    private void PassedColor(ref List<STR_GridObject> markers)
    {
        foreach(Vector3Int pos in check)
        {
            if (redded.Contains(pos)) continue;
            if(markers.Find(gm=>gm.pos==pos)!=null)
            {
                ColorGreen(ref markers, pos);
            }
        }
    }

    private void ColorGreen(ref List<STR_GridObject> markers, Vector3Int pos)
    {
        return;
    }
    private void ColorRed(ref List<STR_GridObject> markers,Vector3Int pos)
    {
        redded.Add(pos);
        GameObject m = S_ObjectControls.CreateUI("Marker");
        m.GetComponent<Marker>().Red();
        m.transform.position = pos + Vector3.one * 0.50f;
        redmarkers.Add(m);
    }
    private List<Vector3Int> GetNeighbors(Vector3Int current)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>
        {
            new Vector3Int(current.x + 1, current.y, current.z), // right
            new Vector3Int(current.x - 1, current.y, current.z), // left

            new Vector3Int(current.x, current.y + 1, current.z), //up
            new Vector3Int(current.x, current.y - 1, current.z) //down
        };

        foreach (Vector3Int neighbor in neighbors.ToArray())
        {
            if (check.Contains(neighbor))
                continue;
            else
                neighbors.Remove(neighbor);
        }

        foreach (Vector3Int neighbor in neighbors.ToArray())
        {
            if (!allNeighbors.Contains(neighbor)&&check.Contains(neighbor))
            {
                allNeighbors.Add(neighbor);
            }
        }

        return neighbors;
    }

    private Vector2Int GetMin(List<Vector3Int> poss)
    {
        int minX = 130919;
        int minY = 130919;

        foreach(Vector3Int pos in poss)
        {
            if (pos.x < minX)
                minX = pos.x;

            if (pos.y < minY)
                minY = pos.y;
        }

        return new Vector2Int(minX, minY);

    }

    private Vector2Int GetMax(List<Vector3Int> poss)
    {
        int maxX = -130919;
        int maxY = -130919;

        foreach (Vector3Int pos in poss)
        {
            if (pos.x > maxX)
                maxX = pos.x;

            if (pos.y > maxY)
                maxY = pos.y;
        }

        return new Vector2Int(maxX, maxY);

    }

    private void ComputeMinMax(List<Vector3Int> p)
    {
        min = GetMin(p);
        max = GetMax(p);
    }
}
