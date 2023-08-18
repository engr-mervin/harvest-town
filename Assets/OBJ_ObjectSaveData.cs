using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class OBJ_ObjectSaveData : MonoBehaviour
{
    [SerializeField]
    public int objectIndex; //accessed by editor script
    public string itemIndex;
    public Vector2Int pivot { get; private set; }
    public int layer { get; private set; }

    public int rotateIndex;
    public bool recenteredX;
    public bool recenteredY;

    public string baseFolder;

    private void Awake()
    {
        OBJ_ObjectSaveList.instance.objectsList.Add(this);
    }

    public void SetPosition(Vector2Int newPivot)
    {
        pivot = newPivot;
    }

    public void SetLayer(int newLayer)
    {
        layer = newLayer;
    }
}
