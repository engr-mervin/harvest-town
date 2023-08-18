using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSorter : MonoBehaviour
{
    public Renderer hair,body,eyes,outfit;
    private MovementAnimation movementAnim;

    public void Awake()
    {

        movementAnim = GetComponent<MovementAnimation>();

        ComputeSortingOrder();
    }

    void LateUpdate()
    {
        if (movementAnim == null) return;

        if (!movementAnim.isSitting)
        {
            ComputeSortingOrder();
        }
    }

    public void ComputeSortingOrder()
    {
        int basePos = (int)(10000 - (transform.position.y - 0.50f) * 100);

        if (body == null)
        {
            GetComponent<Renderer>().sortingOrder = basePos;
            return;
        }
        body.sortingOrder = basePos;
        hair.sortingOrder = basePos + 3;
        eyes.sortingOrder = basePos + 1;
        outfit.sortingOrder = basePos + 2;
    }
}
