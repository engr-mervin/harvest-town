using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InteractRayCast
{
    
    public static GameObject[] GetAllObjects(PlayerMovement rayCaster, float maxDistance = 1.0f)
    {
        RaycastHit2D[] allHit = Physics2D.RaycastAll(rayCaster.transform.position, rayCaster.lookDir, maxDistance);

        List<RaycastHit2D> allActionObjects = new List<RaycastHit2D>();

        List<GameObject> allObjects = new List<GameObject>();

        //Get All Action Objects within maxDistance
        foreach (RaycastHit2D hit in allHit)
        {
            if (hit.collider.GetComponent<WallSaver>() != null && hit.collider.GetComponentsInChildren<Transform>().Length >= 2)
            {
                foreach (Transform t in hit.collider.GetComponentsInChildren<Transform>())
                {
                    if (t.GetComponent<BC_ActionOption>() != null)
                    {
                        allActionObjects.Add(hit);
                        allObjects.Add(t.gameObject);
                    }
                }
            }
            if (hit.distance > maxDistance || hit.collider.GetComponent<BC_ActionOption>() == null)
            {
                continue;
            }
            allActionObjects.Add(hit);
        }

        if (allActionObjects.Count == 0) return null;

        GameObject nearestObject = null;

        float nearestDist = maxDistance;

        //Get nearest Action Object
        foreach (RaycastHit2D g in allActionObjects)
        {
            if (g.distance <= nearestDist)
            {
                nearestObject = g.collider.gameObject;
                nearestDist = g.distance;
            }
        }
        allObjects.Add(nearestObject);

        foreach (GameObject g in allObjects.ToArray())
        {
            if (g.GetComponent<BC_ActionOption>() == null)
                allObjects.Remove(g);
        }
        //If object, then get all objects in the block
        if (nearestObject.GetComponent<StackBase>() != null)
        {
            //get all above the object (children of object)
            Transform[] children = nearestObject.GetComponentsInChildren<Transform>();
            if (children.Length > 0)
            {
                foreach (Transform t in children)
                {
                    if (allObjects.Contains(t.gameObject)) continue;
                    if (t.GetComponent<BC_ActionOption>() == null) continue;

                    allObjects.Add(t.gameObject);
                }
            }
        }
        Debug.Log("Found " + allObjects.Count + " action objects.");
        foreach (GameObject ob in allObjects)
        {
            Debug.Log(ob.name);
        }
        //remove move of not topmost of object
        foreach (GameObject g in allObjects.ToArray())
        {
            bool highest = true;
            ObjectTransform objectTrans = g.GetComponent<ObjectTransform>();

            if (objectTrans == null)
                continue;

            foreach (Vector2Int vc in objectTrans.AllBlockPoints(objectTrans.pivot))
            {
                if (S_WorldBlocks.GetBlockinPosition(vc) == null)
                    break;

                if (S_WorldBlocks.GetBlockinPosition(vc).topMostObject == g)
                {
                    continue;
                }
                else
                {
                    highest = false;
                    break;
                }
            }

            if (!highest && g.GetComponents<BC_ActionOption>().Length == 1 && g.GetComponent<Placeable>() != null)
            {
                allObjects.Remove(g);
            }
        }

        return allObjects.ToArray();
    }

    public static GameObject GetNearestNPC()
    {
        RaycastHit2D[] allHit = Physics2D.RaycastAll(GM.playerMove.transform.position, GM.playerMove.lookDir, 1.0f);

        List<RaycastHit2D> allActionObjects = new List<RaycastHit2D>();

        //Get ALL NPCs with Action Option
        foreach (RaycastHit2D hit in allHit)
        {
            if (hit.distance > 1.0f || hit.collider.GetComponent<BC_ActionOption>() == null || hit.collider.GetComponent<PlayerMovement>() == null)
            {
                continue;
            }
            allActionObjects.Add(hit);
        }

        if (allActionObjects.Count == 0) return null;

        GameObject nearestNPC = allActionObjects[0].collider.gameObject;
        float nearest = allActionObjects[0].distance;

        for(int i =1;i<allActionObjects.Count;i++)
        {
            float a = allActionObjects[i].distance;

            if(a<nearest)
            {
                nearest = a;
                nearestNPC = allActionObjects[i].collider.gameObject;
            }

        }

        return nearestNPC;
    }
    public static GameObject GetNearestInteractable()
    {
        GameObject NPC = GetNearestNPC();
        GameObject obj = GetNearestObject();

        if (NPC == null && obj == null) return null;

        if (NPC == null && obj != null) return obj;

        if (NPC != null && obj == null) return NPC;
 
        if (GetNearestNPCDistance(NPC) <= GetNearestObjectDistance(obj))
            return NPC;
        else
            return obj;
    }

    public static GameObject GetLowestObject()
    {

        Vector2Int currentTile = Positions.TransformToBlock(GM.playerObj.transform.position);
        Vector2Int look = GM.playerMove.lookDir;

        Vector2Int center = currentTile + look;


        List<GameObject> results = new List<GameObject>();

        //GET FRONT BLOCKS

        if (look.y != 0)//THEN X
        {
            for (int x = currentTile.x - 1; x < currentTile.x + 2; x++)
            {
                Block b = S_WorldBlocks.GetBlockinPosition(new Vector2Int(x, center.y));

                if (b == null || results.Contains(b.topMostObject))
                    continue;

                results.Add(b.activeLayers[0].objectInLayer);
            }
        }

        if (look.x != 0)//THEN Y
        {
            for (int y = currentTile.y - 1; y < currentTile.y + 2; y++)
            {
                Block b = S_WorldBlocks.GetBlockinPosition(new Vector2Int(center.x, y));

                if (b == null || results.Contains(b.topMostObject))
                    continue;

                results.Add(b.activeLayers[0].objectInLayer);
            }
        }



        //REMOVE INVALID OBJECTS

        if (results.Count == 0) return null;

        foreach (GameObject g in results.ToArray())
        {
            ObjectTransform objectTrans = g.GetComponent<ObjectTransform>();

            if (objectTrans == null)
                results.Remove(g);
        }



        //GET NEAREST OBJECT

        if (results.Count == 0) return null;
        float nearest = results[0].GetComponent<ObjectTransform>().GetDistance(GM.playerMove.transform.position);
        GameObject result = results[0];

        for (int i = 1; i < results.Count; i++)
        {
            float a = results[i].GetComponent<ObjectTransform>().GetDistance(GM.playerMove.transform.position);

            if (a < nearest)
            {
                nearest = a;
                result = results[i];
            }
        }


        return result;
    }

    public static GameObject GetNearestObject()
    {
        Vector2Int currentTile = Positions.TransformToBlock(GM.playerObj.transform.position);
        Vector2Int look = GM.playerMove.lookDir;

        Vector2Int center = currentTile + look;


        List<GameObject> results = new List<GameObject>();

        //GET FRONT BLOCKS

        if (look.y != 0)//THEN X
        {
            for (int x = currentTile.x - 1; x < currentTile.x + 2; x++)
            {
                Block b = S_WorldBlocks.GetBlockinPosition(new Vector2Int(x, center.y));

                if (b == null || results.Contains(b.topMostObject))
                    continue;

                    results.Add(b.topMostObject);
            }
        }

        if (look.x != 0)//THEN Y
        {
            for (int y = currentTile.y - 1; y < currentTile.y + 2; y++)
            {
                Block b = S_WorldBlocks.GetBlockinPosition(new Vector2Int(center.x, y));

                if (b == null || results.Contains(b.topMostObject))
                    continue;

                    results.Add(b.topMostObject);
            }
        }

        //REMOVE INVALID OBJECTS

        if (results.Count == 0) return null;

        foreach (GameObject g in results.ToArray())
        {
            ObjectTransform objectTrans = g.GetComponent<ObjectTransform>();

            if (objectTrans == null)
                results.Remove(g);
        }


        foreach (GameObject g in results.ToArray())
        {
            ObjectTransform objectTrans = g.GetComponent<ObjectTransform>();
            foreach (Vector2Int v in objectTrans.AllBlockPoints())
            {
                Block b = S_WorldBlocks.GetBlockinPosition(v);
                if (b.topMostObject != g)
                {
                    results.Remove(g);
                }
                else
                    continue;
            }
        }

        //GET NEAREST OBJECT

        if (results.Count == 0) return null;
        float nearest = results[0].GetComponent<ObjectTransform>().GetDistance(GM.playerMove.transform.position);
        GameObject result = results[0];

        for (int i = 1; i < results.Count; i++)
        {
            float a = results[i].GetComponent<ObjectTransform>().GetDistance(GM.playerMove.transform.position);

            if (a < nearest)
            {
                nearest = a;
                result = results[i];
            }
        }


        return result;
    }
    public static float GetNearestObjectDistance(GameObject g)
    {
        float a = g.GetComponent<ObjectTransform>().GetDistance(GM.playerMove.transform.position);
        return a;
    }
    public static float GetNearestNPCDistance(GameObject g)
    {
        float a = Vector3.Distance(g.transform.position, GM.playerMove.transform.position);
        return a;
    }

}
