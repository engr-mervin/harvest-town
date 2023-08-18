using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recenterable : MonoBehaviour
{
    public bool recenteredX
    {
        get;
        private set;
    }
    public bool recenteredY
    {
        get;
        private set;
    }

    public void ToggleRecenterX()
    {
        if (recenteredX)
        {
            UnrecenterX();
        }
        else
        {
            RecenterX();
        }
    }
    public void ToggleRecenterY()
    {
        if (recenteredY)
        {
            UnrecenterY();
        }
        else
        {
            RecenterY();
        }
    }
    private void RecenterX()
    {
        //Get Size component
        ObjectTransform objectTrans = GetComponent<ObjectTransform>();

        //set bool
        recenteredX = true;
        GetComponent<OBJ_ObjectSaveData>().recenteredX = true;

        //move sprite by 0.5
        transform.position += Vector3.right * 0.50f;

        //add 1 to size x
        objectTrans.size += Vector2Int.right;

    }
    private void RecenterY()
    {

        //Get Size component
        ObjectTransform objectTrans = GetComponent<ObjectTransform>();

        //set bool
        recenteredY = true;
        GetComponent<OBJ_ObjectSaveData>().recenteredY = true;

        //move sprite by 0.5
        transform.position += Vector3.up * 0.50f;

        //add 1 to size x
        objectTrans.size += Vector2Int.up;
    }

    private void UnrecenterX()
    {
        ObjectTransform objectTrans = GetComponent<ObjectTransform>();

        //set bool
        recenteredX = false;
        GetComponent<OBJ_ObjectSaveData>().recenteredX = false;

        //move sprite by 0.5
        transform.position -= Vector3.right * 0.50f;

        //add 1 to size x
        objectTrans.size -= Vector2Int.right;
    }

    private void UnrecenterY()
    {

        ObjectTransform objectTrans = GetComponent<ObjectTransform>();

        //set bool
        recenteredY = false;
        GetComponent<OBJ_ObjectSaveData>().recenteredY = false;

        //move sprite by 0.5
        transform.position -= Vector3.up * 0.50f;

        //add 1 to size x
        objectTrans.size -= Vector2Int.up;
    }
}
