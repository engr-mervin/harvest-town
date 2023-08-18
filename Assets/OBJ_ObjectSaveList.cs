using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBJ_ObjectSaveList : MonoBehaviour
{
    public static OBJ_ObjectSaveList instance;
    public List<OBJ_ObjectSaveData> objectsList;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
            instance = this;
        }
        else
        {
            instance = this;
        }
    }

    public static void Remove(OBJ_ObjectSaveData obj)
    {
        instance.objectsList.Remove(obj);
    }
}
