using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePanel : MonoBehaviour
{
    private enum Panel
    {
        Movement,
        MovingObject,
        AddingFloors,
        AddingWalls,
        Buying,
        Paused,
        HotBar,
        Interacting
    }

    [SerializeField]
    private Panel panel;

    public static GameObject movement;
    public static GameObject movingObject;
    public static GameObject addingFloors;
    public static GameObject addingWalls;
    public static GameObject buying;
    public static GameObject paused;
    public static GameObject hotBar;
    public static GameObject interacting;

    public void Initialize()
    {
        switch(panel)
        {
            case Panel.Movement:
                movement = gameObject;
                break;
            case Panel.MovingObject:
                movingObject = gameObject;
                break;
            case Panel.AddingFloors:
                addingFloors = gameObject;
                break;
            case Panel.AddingWalls:
                addingWalls = gameObject;
                break;
            case Panel.Buying:
                buying = gameObject;
                break;
            case Panel.Paused:
                paused = gameObject;
                break;
            case Panel.HotBar:
                hotBar = gameObject;
                break;
            case Panel.Interacting:
                interacting = gameObject;
                break;
        }
    }

    public static void Terminate()
    {
        movement= null;
        movingObject = null;
        addingFloors = null;
        addingWalls = null;
        buying = null;
        paused = null;
        hotBar = null;
        interacting = null;
    }

}

