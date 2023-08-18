using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MB_NPCLocation
{
    public GameObject NPCObject;
    public PlayerMovement NPCloc;

    public MB_NPCLocation(GameObject obj,PlayerMovement pivot)
    {
        NPCObject = obj;
        NPCloc = pivot;
    }
}
