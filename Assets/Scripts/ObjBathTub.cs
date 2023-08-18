using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjBathTub : MonoBehaviour
{
    public Sprite empty;
    public Sprite full;

    public bool hasWater = false;

    public void Interact(GameObject interactor)
    {
        if (!hasWater)
        {
            GetComponent<SpriteRenderer>().sprite = full;
            hasWater = true;
        }

        else
        {
            GetComponent<SpriteRenderer>().sprite = empty;
            hasWater = false;
        }

    }
}
