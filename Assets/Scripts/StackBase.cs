using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackBase : MonoBehaviour
{
    [Header("User Driven")]
    private float stackHeight;

    [SerializeField]
    private Transform stackPivot;
    [SerializeField]
    private Vector2Int[] placeables;


    [System.Flags]
    public enum Type
    {
        None = 0,
        Table = 1,
        Bed = 2,
        Cabinet = 4,
        Bookshelf = 8,
        Speaker = 16,
        Electronics = 32,
        WettableElectronics = 64,
        LowCabinet = 128,
    }

    public Type type;

    [Header("Script Driven")]
    [SerializeField]
    private GameObject aboveObject;

    void Awake()
    {
        stackHeight = stackPivot.position.y - transform.position.y;
    }
    public bool CanStackHere(Vector2Int add)
    {
        foreach (Vector2Int v in placeables)
        {
            if (v == add)
                return true;
        }

        return false;
    }

    public bool StackCompatible(Stackable stackingObject)
    {
        int compare = (int)type | (int)stackingObject.stackableOn;

        if (compare == 0) return false;

        return true;
    }

    public void StackHere(Stackable stackingObject)
    {
        stackingObject.transform.position += new Vector3(0, stackHeight, 0);
        aboveObject = stackingObject.gameObject;

        stackingObject.GetComponent<ObjectTransform>().layer = this.GetComponent<ObjectTransform>().layer + 1;
    }
}
