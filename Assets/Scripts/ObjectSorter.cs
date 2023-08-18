using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSorter : MonoBehaviour
{
    private Renderer myRenderer;

    private Vector3 lastPosition;
    public Vector3 lastlastPosition;

    public delegate void ObjectMoved();
    public event ObjectMoved onObjectMoved;

    private ObjectTransform objectTrans;



    public void Awake()
    {
        objectTrans = GetComponent<ObjectTransform>();
        myRenderer = gameObject.GetComponent<Renderer>();

        if (transform.parent != S_ObjectControls.ObjectInstantiator.transform)
        {
            return;
        }
        else
        {
            ComputeSortingOrderFloor();
        }
    }

    private void Update()
    {
        if (transform.position == lastPosition) return;
        lastlastPosition = lastPosition;
        lastPosition = transform.position;

        if (onObjectMoved != null)
        {
            onObjectMoved();
        }

        if (transform.parent != S_ObjectControls.ObjectInstantiator.transform)
        {
            SortingOrderBasedonParent();
        }
        else
        {
            ComputeSortingOrderFloor();
        }
    }
    private void SortingOrderBasedonParent()
    {
        myRenderer.sortingOrder = transform.parent.GetComponent<SpriteRenderer>().sortingOrder + 1;
    }
    private void ComputeSortingOrderFloor()
    {
        int f;

        f = (int)(GetComponent<BoxCollider2D>().offset.y * 100);

        int basePos = 10000 - (objectTrans.pivot.y) * 50 - f;
        int offset = (int)((transform.position.y - objectTrans.pivot.y/2 - 0.25f) * 10);
        int stackedPos = basePos + offset;

        myRenderer.sortingOrder = stackedPos;
    }

}
