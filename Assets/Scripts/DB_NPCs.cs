using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DB_NPCs : MonoBehaviour
{
    public static List<MB_NPCLocation> NPClist = new List<MB_NPCLocation>();


    public List<NPC> NPCs;
    
    private void Awake()
    {
        foreach (NPC npc in NPCs)
        {
            MB_NPCLocation member = new MB_NPCLocation(npc.gameObject, npc.GetComponent<PlayerMovement>());
            if(!NPClist.Contains(member))
            {
                NPClist.Add(member);
            }
        }
    }
    private void OnDestroy()
    {
        NPClist.Clear();
    }

    public static List<GameObject> FindNPC(Vector2Int location)
    {
        List<GameObject> npcs = new List<GameObject>();

        List<MB_NPCLocation> members = NPClist.FindAll(npc => npc.NPCloc.pivotPosition == location);

        foreach (MB_NPCLocation member in members)
        {
            npcs.Add(member.NPCObject);
        }
        return npcs;
    }
}
