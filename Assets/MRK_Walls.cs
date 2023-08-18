using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MRK_Walls : MonoBehaviour
{
    public Sprite green;
    public Sprite red;

    public enum State
    {
        Green,
        Red
    }

    public State markerState;

    public void Red()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = red;
        markerState = State.Red;
    }
    public void Green()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = green;
        markerState = State.Green;
    }
}
