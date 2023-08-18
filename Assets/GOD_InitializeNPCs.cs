using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOD_InitializeNPCs : MonoBehaviour
{
    public List<NPC_Animator> npcs = new List<NPC_Animator>();

    private void Awake()
    {
        foreach(NPC_Animator npc in FindObjectsOfType<NPC_Animator>())
        {
            npc.Initialize();
        }
    }
}
