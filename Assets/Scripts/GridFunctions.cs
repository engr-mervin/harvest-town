using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridFunctions
{
    public static List<GameObject> GetAllObjects(Vector2Int v)
    {
        List<GameObject> all = new List<GameObject>();

        if (S_WorldBlocks.GetBlockinPosition(v) != null)
        {
            //objects
            foreach (STR_BlockLayer bl in S_WorldBlocks.GetBlockinPosition(v).activeLayers)
            {
                all.Add(bl.objectInLayer);
            }
        }
        //npcs
        if (DB_NPCs.FindNPC(v) != null)
        {
            foreach (GameObject g in DB_NPCs.FindNPC(v))
            {
                all.Add(g);
            }
        }

        return all;
    }
    public static List<GameObject> GetAllActionObjects(Vector2Int v)
    {
        List<GameObject> all = new List<GameObject>();

        if (S_WorldBlocks.GetBlockinPosition(v) != null)
        {
            //objects
            foreach (STR_BlockLayer bl in S_WorldBlocks.GetBlockinPosition(v).activeLayers)
            {
                if (bl.objectInLayer.GetComponents<BC_ActionOption>().Length == 0)
                    continue;
                all.Add(bl.objectInLayer);
            }
        }
        //npcs
        if (DB_NPCs.FindNPC(v) != null)
        {
            foreach (GameObject g in DB_NPCs.FindNPC(v))
            {
                if (g.GetComponents<BC_ActionOption>().Length == 0)
                    continue;
                all.Add(g);
            }
        }

        return all;
    }
}
