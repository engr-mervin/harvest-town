
using UnityEngine;

[RequireComponent(typeof(ObjectTransform))]
public class Placeable:MonoBehaviour
{

    public INF_MyTile.Type baseType;

    public bool isFixed;

    private ObjectTransform objectTrans;
    public enum Type
    {
        Floor,
        Wall
    };
    public Type type;

    

    public void ButtonClicked()
    {
        GM.playerState.SetState(new MovingObjectState(GM.playerState,gameObject, MovingObject.Type.ExistingObject));
    }

    public void SetWallParent()
    {
        if (type != Type.Wall) return;

        Vector3Int block = (Vector3Int)Positions.BlockToTile(objectTrans.pivot);


        STR_Walls st = WallsManager.wallList.Find(c => c.pos == block + Vector3Int.down);
        STR_Walls st2 = WallsManager.wallList.Find(c => c.pos == block);

        if (st != null)
            transform.parent = st.wall.transform;
        else if(st2!=null)
            transform.parent = st2.wall.transform;
    }

    public bool CanMoveHere(Vector2Int pivot) //check if an object is possible to be placed on a tile
    {
        objectTrans = gameObject.GetComponent<ObjectTransform>();

        foreach (Vector2Int p in objectTrans.AllPoints(pivot))
        {
            if(type == Type.Floor)
            {
                if (((int)TileManager.GetFloorType(p) & (int)baseType) != 0)//compatible
                    continue;
                else
                    return false;
            }
            if (type == Type.Wall)
            {
                if (((int)TileManager.GetWallType(p) & (int)baseType) != 0)//compatible
                    continue;
                else
                    return false;
            }
        }
        return true;
    }


    public bool CanMoveToBlock(Vector2Int blockPivot) //check if an object is possible to be MOVED on a tile
    {
        objectTrans = gameObject.GetComponent<ObjectTransform>();

        foreach (Vector2Int p in objectTrans.AllBlockPoints(blockPivot))
        {
            Vector2Int converted = Positions.BlockToTile(p);
            if (p == blockPivot)
            {
                //print(p);
               // print(converted);
            }

            if (type == Type.Floor)
            {
                if (((int)TileManager.GetFloorType(converted) & (int)baseType) != 0)//compatible
                    continue;
                else
                    return false;
            }
            if (type == Type.Wall)
            {
                if (((int)TileManager.GetWallType(converted) & (int)baseType) != 0)//compatible
                    continue;
                else
                    return false;
            }
        }
        return true;
    }

    public bool CanMoveToBlock(Vector2Int blockPivot,Vector2Int[] points) //check if an object is possible to be MOVED on a tile
    {
        objectTrans = gameObject.GetComponent<ObjectTransform>();

        foreach (Vector2Int p in points)
        {
            Vector2Int converted = Positions.BlockToTile(p);
 
            if (type == Type.Floor)
            {
                if (((int)TileManager.GetFloorType(converted) & (int)baseType) != 0)//compatible
                    continue;
                else
                    return false;
            }
            if (type == Type.Wall)
            {
                if (((int)TileManager.GetWallType(converted) & (int)baseType) != 0)//compatible
                    continue;
                else
                    return false;
            }
        }
        return true;
    }
    
    private void Awake()
    {
        objectTrans = GetComponent<ObjectTransform>();
        string temp = gameObject.name;
        string temp2 = temp.Remove(temp.Length - 7);
    }
}
