using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotatable : MonoBehaviour
{
    private SpriteRenderer sRender;
    private BoxCollider2D bCollider;
    private ObjectTransform objectTrans;
    private OBJ_ObjectSaveData objectSave;

    private Vector2Int defaultSize = Vector2Int.zero;

    [System.Serializable]
    public struct State
    {
        public Vector2 offset;
        public Vector2 size;
        public Sprite sprite;
        public bool rotateSize;
    }
    public State[] states;

    public int currentIndex;

    public void Awake()
    {
        bCollider = GetComponent<BoxCollider2D>();
        sRender = GetComponent<SpriteRenderer>();
        objectTrans = GetComponent<ObjectTransform>();
        objectSave = GetComponent<OBJ_ObjectSaveData>();
    }

    public void RotateNext()
    {
        int a;
        if (currentIndex+1 == states.Length)
            a = 0;
        else
            a = currentIndex + 1;

        if(CheckRotate(a))
            Rotate(a);

    }

    public bool CheckRotate(int index)
    {
        Vector2Int temp;

        if (states[index].rotateSize)
        {
            temp = new Vector2Int(defaultSize.y, defaultSize.x);
        }
        else
        {
            temp = defaultSize;
        }
        ObjectTransform objectTrans = GetComponent<ObjectTransform>();

        Vector2Int[] vcs = AllBlockPoints(objectTrans.pivot, temp);

        Placeable p = GetComponent<Placeable>();

        if (p.CanMoveToBlock(objectTrans.pivot, vcs))
            return true;
        else
            return false;
    }
    
    public Vector2Int[] AllBlockPoints(Vector2Int blockPos,Vector2Int size)
    {
        Vector2Int[] result = new Vector2Int[4 * size.x * size.y];
        for (int x = 0; x < 2 * size.x; x++) //takes a rectangular array of blocks equivalent to size of moveObject
        {
            for (int y = 0; y < 2 * size.y; y++)
            {
                Vector2Int add = new Vector2Int(x, y);
                Vector2Int current = blockPos + add;

                result[2 * size.y * (x) + y] = current;
            }
        }
        return result;
    }
    public void Rotate(int index)
    {
        if (defaultSize == Vector2Int.zero)
            defaultSize = GetComponent<ObjectTransform>().size;

        if (index >= states.Length) return;

        objectSave.rotateIndex = index;
        currentIndex = index;
        sRender.sprite = states[index].sprite;
        bCollider.size = states[index].size;
        bCollider.offset = states[index].offset;

        if(states[index].rotateSize)
        {
            objectTrans.size = new Vector2Int(defaultSize.y, defaultSize.x);
        }
        else
        {
            objectTrans.size = defaultSize;
        }
    }
}
