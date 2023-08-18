using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticValues : MonoBehaviour
{
    public float StackingHeightTreshold;
    public float buttonTint;

    public delegate void SetParameters();
    public static event SetParameters OnSetParameters;

    public float dPadDistance;
    public float dPadSizePrimary;
    public float dPadSizeSecondary;
    public Vector3 dPadOffset;

    public float actionsDistance;
    public float actionsSize;
    public Vector3 actionsOffset;


    public float generalSize;
    public float generalDistance;
    public Vector3 generalOffset;

    public float objInvSize;
    public float objInvDistance;
    public Vector3 objInvOffset;

    public static float buttonAlphaTint;

    public static StaticValues instance;

    private void Start()
    {
        if(instance==null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        OnSetParameters?.Invoke();

        Stackable.heightThreshold = StackingHeightTreshold;
        buttonAlphaTint = buttonTint;


    }
}