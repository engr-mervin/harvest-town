using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar_Grid : MonoBehaviour
{
    public static Vector2Int bottomLeft;
    public static Vector2Int topRight;
    public static AStar_Grid instance;

    public static void RefreshWalkable(Vector2Int block)
    {
        Vector2Int tile = Positions.BlockToTile(block);
        AStar_Node node = GetNode(block);

        //contained in walls
        if(WallsManager.ListContains(tile))
        {
            node.walkable = false;
            return;
        }

        //has a block
        if(S_WorldBlocks.GetBlockinPosition(block)!=null)
        {
            node.walkable = false;
            return;
        }

        //char
        //
        node.walkable = true;
    }
    public void SetInstance()
    {
        if (instance != null)
        {
            Destroy(instance);
        }

        instance = this;
    }

    public void Initialize(Vector2Int bl, Vector2Int tr)
    {
        Populate(bl, tr);
        SetWalkableNodes();
    }

    public static int lengthX
    {
        get
        {
            return topRight.x - bottomLeft.x + 1;
        }
    }
    public static int lengthY
    {
        get
        {
            return topRight.y - bottomLeft.y + 1;
        }
    }
    [SerializeField]
    public static AStar_Node[,] worldNodes;

    public int Size
    {
        get
        {
            return lengthX* lengthY;
        }
    }
    public void Populate(Vector2Int bl,Vector2Int tr)
    {
        bottomLeft = bl;
        topRight = tr;
        //bottom Left and top right will be based on how large the house is
        //populate nodes
        int gridX = topRight.x - bottomLeft.x + 1;
        int gridY = topRight.y - bottomLeft.y + 1;

        worldNodes = new AStar_Node[gridX, gridY];

        for (int x = bottomLeft.x; x <= topRight.x; x++)
        {
            for (int y = bottomLeft.y; y <= topRight.y; y++)
            {
                int posX = x - bottomLeft.x;
                int posY = y - bottomLeft.y;
                AStar_Node currentNode = new AStar_Node(new Vector2Int(x, y));
                currentNode.walkable = true;
                worldNodes[posX, posY] = currentNode;
            }
        }
    }
    //call this after populating wall Lists and after object making
    public void SetWalkableNodes()
    {
        List<Vector2Int> unwalkable = new List<Vector2Int>(); //LIST OF NODE[IDs]

        foreach(STR_Walls tile in WallsManager.wallList)
        {
            foreach(Vector2Int block in Positions.TileToBlock((Vector2Int)tile.pos))
            {
                unwalkable.Add(block);
            }
        }

        foreach(Block block in S_WorldBlocks.instance.worldList)
        {
            unwalkable.Add(block.blockPosition);
        }

        foreach (Vector2Int vc in unwalkable)
        {
            AStar_Node node = GetNode(vc);
            if (node == null) continue;
            node.walkable = false;
        }
    }
    public void SetWalkableNodesBackup()
    {
        foreach (AStar_Node node in worldNodes)
        {
            Vector3Int pos = new Vector3Int(node.position.x, node.position.y, 0);

            if (WallsManager.wallList.Find(w => w.pos == pos) != null)
            {
                node.walkable = false;
                continue;
            }

            //ADD LOGIC FOR OBJECTS
            if (S_WorldBlocks.instance.worldList.Find(block => block.blockPosition == node.position))
            {
                node.walkable = false;
                continue;
            }

            node.walkable = true;
        }
    }
    public static Vector2Int BlockToNodeID(Vector2Int block)
    {
        int x = block.x - bottomLeft.x;
        int y = block.y - bottomLeft.y;

        return new Vector2Int(x, y);
    }

    public static Vector2Int NodeIDToBlock(Vector2Int node)
    {
        int x = node.x + bottomLeft.x;
        int y = node.y + bottomLeft.y;

        return new Vector2Int(x, y);
    }


    public List<AStar_Node> GetNeighbours(Vector2Int pos)
    {
        List<AStar_Node> neighbours = new List<AStar_Node>()
        {
            GetNode(pos + Vector2Int.up),
            GetNode(pos + Vector2Int.down),
            GetNode(pos + Vector2Int.left),
            GetNode(pos + Vector2Int.right),
        };
        return neighbours;
    }
    
    public static AStar_Node GetNode(Vector2Int block)
    {
        Vector2Int res = BlockToNodeID(block);

        if (res.x>=lengthX||res.x<0||res.y>=lengthY||res.y<0)
            return null;
        return worldNodes[res.x, res.y];
    }


    public bool IsInside(Vector2Int query)
    {
        if (query.x > topRight.x || query.x < bottomLeft.x || query.y > topRight.y || query.y < bottomLeft.y)
        {
            print(query + " is outside the extents of pathfinding");
            return false;
        }
        else
        {
            return true;
        }
    }
}
