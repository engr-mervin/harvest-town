using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSorter : MonoBehaviour
{
    private Renderer myRenderer;

    public void Start()
    {
        myRenderer = gameObject.GetComponent<Renderer>();

        
        ComputeSortingOrder();

        Destroy(this);
    }

    void ComputeSortingOrder()
    {
        int f;
        if (GetComponent<BoxCollider2D>() != null)
            f = (int)(GetComponent<BoxCollider2D>().offset.y * 100);
        else
            f = -100;

        int basePos = 10000 - Mathf.FloorToInt(transform.position.y) * 100 - f;
        int stackedPos = basePos;

        myRenderer.sortingOrder = stackedPos;
    }
}
