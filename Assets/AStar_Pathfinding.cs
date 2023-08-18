using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar_Pathfinding : MonoBehaviour
{
    public AStar_Grid grid;
    public static AStar_Pathfinding instance;
    public bool drawMarkers;

    public GameObject marker;
    public List<GameObject> markers = new List<GameObject>();
    
    public enum Type
    {
        Exact,
        Front
    };

    [SerializeField]
    private List<AStar_Node> path;

    private void Awake()
    {
        instance = this;
    }
    public void InstantiateMarkers()
    {
        if (drawMarkers == false) return;

        foreach(GameObject mark in markers)
        {
            Destroy(mark);
        }

        markers.Clear();

        foreach(AStar_Node node in path)
        {
            GameObject cm = Instantiate(marker);
            cm.transform.localScale = Vector3.one * 0.50f;

            cm.transform.position = Positions.BlockToTransform(node.position);
            markers.Add(cm);
        }
    }
    private void RetracePath(AStar_Node startNode, AStar_Node endNode)
    {
        List<AStar_Node> newPath = new List<AStar_Node>();

        AStar_Node current = endNode;

        while(current!=startNode)
        {
            newPath.Add(current);
            current = current.parent;
        }
        newPath.Reverse();

        path = newPath;
        
    }

    public List<AStar_Node> PathFindSmart(Vector2Int startPos, Vector2Int targetPos,Type type)
    {
        if (type == Type.Exact)
        {
            return PathFind(startPos, targetPos);
        }
        else if (type == Type.Front)
        {

            return PathFind(startPos, targetPos + Vector2Int.down*2);
        }
        else
            return null;

    }
    public void SetTurns(List<AStar_Node> nodes)
    {
        for(int i=0;i<nodes.Count;i++)
        {
            if (i == 0||i==nodes.Count-1)
            {
                nodes[i].turn = true;
                continue;
            }
            Vector2Int front = nodes[i + 1].position - nodes[i].position;
            Vector2Int behind = nodes[i].position - nodes[i - 1].position;

            if(front!=behind)
            {
                nodes[i].turn = true;
            }
        }
    }

    public List<AStar_Node> RemoveNonTurns(List<AStar_Node> nodes)
    {
        List<AStar_Node> newPath = new List<AStar_Node>();

        foreach(AStar_Node n in nodes.ToArray())
        {
            if (n.turn == true)
                newPath.Add(n);
            else
                continue;
        }
        return newPath;
    }

    public List<AStar_Node> PathFind(Vector2Int startPos, Vector2Int targetPos)
    {
        Heap<AStar_Node> openList = new Heap<AStar_Node>(grid.Size);
        HashSet<AStar_Node> closedList = new HashSet<AStar_Node>();

        if (!grid.IsInside(startPos) || !grid.IsInside(targetPos))
        {
            print("The starting position or target position is outside A* extents");
            return null;
        }
        bool adjustEnd =false;
        AStar_Node startNode = AStar_Grid.GetNode(startPos);

        AStar_Node endNode = AStar_Grid.GetNode(targetPos);

        if(endNode.walkable==false)
        {
            adjustEnd = true;
            endNode.walkable = true;
        }

        openList.AddItem(startNode);

        int iteration = 0;
        while(openList.Count>0)
        {
            AStar_Node currentNode = openList.RemoveFirst();
            closedList.Add(currentNode);

            if (iteration > Options.instance.pathfindingCalculations)//add to settings
            {
                return null;
            }

            iteration++;

            //PATH FOUND
            if (currentNode == endNode)
            {
                if(adjustEnd)
                {
                    endNode.walkable = false;
                }
                RetracePath(startNode, endNode);
                SetTurns(path);
                return RemoveNonTurns(path);
            }
            foreach (AStar_Node neighbour in grid.GetNeighbours(currentNode.position))
            {
                if (neighbour==null||neighbour.walkable == false || closedList.Contains(neighbour))
                {
                    continue;
                }

                int movementCost = currentNode.gCost + 1;

                if(neighbour.gCost>movementCost||!openList.Contains(neighbour))
                {
                    neighbour.parent = currentNode;
                    neighbour.gCost = movementCost;
                    neighbour.hCost = GetDistance(neighbour.position, targetPos);

                    if (!openList.Contains(neighbour))
                        openList.AddItem(neighbour);
                }
            }
        }
        //NO PATH IS FOUND
        if(openList.Count==0)
        {
            if(adjustEnd)
            {
                endNode.walkable = false;
            }
            path.Clear();
        }
        return path;
    }

    private AStar_Node FindHighestPriority(List<AStar_Node> openList)
    {
        if (openList.Count == 0) return null;

        AStar_Node highestPrio = openList[0];

        foreach (AStar_Node openNode in openList)
        {
            if (openNode.FCost < highestPrio.FCost|| openNode.FCost == highestPrio.FCost&& openNode.hCost < highestPrio.hCost)
                highestPrio = openNode;
        }
        return highestPrio;
    }
    private void ComputeCosts(AStar_Node node, Vector2Int startPos, Vector2Int targetPos)
    {
        node.hCost = GetDistance(node.position, targetPos);
        node.gCost = GetDistance(node.position, startPos);
    }

    private int GetDistance(Vector2Int start, Vector2Int end)
    {
        int distX = Mathf.Abs(end.x - start.x);
        int distY = Mathf.Abs(end.y - start.y);

        return (distX + distY);
    }

}
