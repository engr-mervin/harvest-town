using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnControlPanel : MonoBehaviour
{
    public enum GridType
    {
        DPad,
        Action,
        TopBar,
    };

    public GridType grid;

    public int index;

    private void Start()
    {
        Vector2[] actualGrid;

        switch(grid)
        {
        case GridType.DPad:
            actualGrid = ControlPanel.dPadGridResult;
            break;
        case GridType.Action:
            actualGrid = ControlPanel.actionGridResult;
            break;
        case GridType.TopBar:
            actualGrid = ControlPanel.topGridResult;
            break;
        default:
            actualGrid = ControlPanel.dPadGridResult;
            break;
        }
        GetComponent<RectTransform>().localPosition = actualGrid[index];
    }
}
