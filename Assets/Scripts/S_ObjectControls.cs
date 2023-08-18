using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class S_ObjectControls : MonoBehaviour
{

    public static GameObject ObjectInstantiator;
    private void Awake()
    {
        ObjectInstantiator = this.gameObject;
    }

    private void Update()
    {
    }

    public static GameObject CreateObject(string name, string tabName, Vector2Int pivot)//Create an object
    {
        GameObject g = (GameObject)(Resources.Load("Objects/" + tabName + "/" + name));
        //Check Block Base Compatibility
        bool compatible = g.GetComponent<Placeable>().CanMoveHere(pivot);
        //Check Block Object Compatibility
        bool objectcompatible;

        if (S_WorldBlocks.GetBlockinPosition(pivot) != null)
        {
            Block b = S_WorldBlocks.GetBlockinPosition(pivot);

            if (g.GetComponent<Stackable>() == null)
            { 
                objectcompatible = false; 
            }
            else
            {
                objectcompatible = g.GetComponent<Stackable>().CheckStack(b.topMostObject.GetComponent<StackBase>());
            }
        }
        else
        {
            objectcompatible = true;
        }
        if (!compatible||!objectcompatible)
        {
            return null;
        }
        GameObject create;
        //Create Object
        create = (GameObject)Instantiate(Resources.Load("Objects/" + tabName + "/" + name));
        create.GetComponent<ObjectTransform>().MoveToBlock(pivot);

        //Set Parent
        create.transform.parent = ObjectInstantiator.transform;
        create.transform.position = MyFunctions.Vector2InttoTransform(pivot);

        //Return Object
        return create;
    }

    
    public static GameObject InstantiateObject(GameObject prefab, Vector2Int blockPivot)//latest
    {
        Placeable p = prefab.GetComponent<Placeable>();

        //IF NOT PLACEABLE
        if (p == null) return null;

        bool tileCompatible = p.CanMoveToBlock(blockPivot);


        if (!tileCompatible) return null;

        GameObject create = Instantiate(prefab, ObjectInstantiator.transform);

        create.GetComponent<ObjectTransform>().MoveToBlock(blockPivot);

        return create;
    }

    public static GameObject InstantiateObjectBackup(GameObject prefab, Vector2Int blockPivot)//latest
    {
        Placeable p = prefab.GetComponent<Placeable>();
        Stackable s = prefab.GetComponent<Stackable>();
        ObjectTransform o = prefab.GetComponent<ObjectTransform>();

        //IF NOT PLACEABLE
        if (p == null) return null;

        bool tileCompatible = p.CanMoveToBlock(blockPivot);

        //IF OBJECT IS NOT STACKABLE THEN CHECK IF ALL POINTS HAVE NO ITEM
        if (s == null)
        {
            foreach (Vector2Int v in o.AllBlockPoints(blockPivot))
            {
                Block b = S_WorldBlocks.GetBlockinPosition(v);
                if (b != null) return null;
            }
        }

        print("this ran");
        //BOOL TO CHECK IF IT CAN BE INSTANTIATED AGAINST STACKBASES
        bool s2 = true;

        if (s != null)
        {
            foreach (Vector2Int v in o.AllBlockPoints(blockPivot))
            {
                Block b = S_WorldBlocks.GetBlockinPosition(v);
                if (b == null)
                    continue;
                else
                    s2 = s.CanStackHere(blockPivot);

                if (s2 == false)
                    break;
            }
        }

        //ASSIGN BLOCK COMPATIBLE BOOL
        bool blockCompatible = (s == null) ? true : s2;
        print(tileCompatible + " " + blockCompatible);
        if (!tileCompatible || !blockCompatible) return null;

        GameObject create = Instantiate(prefab, ObjectInstantiator.transform);

        create.GetComponent<ObjectTransform>().MoveToBlock(blockPivot);

        return create;

    }
    public static GameObject SpawnObject(GameObject prefab, Vector2Int pivot)//Create an object
    {
        //Check Block Base Compatibility
        bool compatible = prefab.GetComponent<Placeable>().CanMoveHere(pivot);
        //Check Block Object Compatibility
        bool objectcompatible;

        if (S_WorldBlocks.GetBlockinPosition(pivot) != null)
        {
            Block b = S_WorldBlocks.GetBlockinPosition(pivot);

            if (prefab.GetComponent<Stackable>() == null)
            {
                objectcompatible = false;
            }
            else
            {
                objectcompatible = prefab.GetComponent<Stackable>().CheckStack(b.topMostObject.GetComponent<StackBase>());
            }
        }
        else
        {
            objectcompatible = true;
        }

        print(compatible + " and " + objectcompatible);
        if (!compatible || !objectcompatible)
        {
            return null;
        }
        GameObject create;
        //Create Object
        create = Instantiate(prefab);
        create.GetComponent<ObjectTransform>().MoveToBlock(pivot);

        //Set Parent
        create.transform.parent = ObjectInstantiator.transform;

        //Return Object
        return create;
    }
    public static GameObject CreateUI(string name,Transform parent)
    {
        GameObject g = (GameObject)Instantiate(Resources.Load(name));
        g.transform.SetParent(parent, false);
        return g;
    }

    public static GameObject CreateUI(string name) // overloaded function
    {
        GameObject g = (GameObject)Instantiate(Resources.Load(name));
        g.transform.SetParent(ObjectInstantiator.transform, false);
        return g;
    }

    public static INF_MyTile.Type GetRequiredTileType(GameObject queryObject) //get the tile type of an object
    {
        Placeable movable = queryObject.GetComponent<Placeable>();

        return movable.baseType;
    }

    public static void DestroyObject(GameObject destroy) // cleanly destroy an object from lists
    {
        OBJ_ObjectSaveData objectData = destroy.GetComponent<OBJ_ObjectSaveData>();
        //Remove in Grid Object List (What's in what position)
        STR_GridObjectWithLayer.list.RemoveAll(gridObj => gridObj.blockObject == destroy);
        //Remove in Object Data List (Saving game)
        OBJ_ObjectSaveList.instance.objectsList.Remove(objectData);
        //Remove in World Blocks List (Object Collection)
        if (S_WorldBlocks.ListContainsObject(destroy))
            S_WorldBlocks.RemoveObjectFromWorld(destroy);

        ObjectTransform objectTrans = destroy.GetComponent<ObjectTransform>();

        if (objectTrans != null)
        {
            foreach (Vector2Int v in objectTrans.AllBlockPoints())
            {
                AStar_Grid.RefreshWalkable(v);
            }
        }

        UnityEngine.GameObject.Destroy(destroy);
    }
}
