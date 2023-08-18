using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BuyItems : BC_UIs
{
    public Vector2[] grid;

    public Vector2 panelSize;
    public Vector2Int gridSize;
    public Vector2 offsetGrid;



    public void ComputeGrid()
    {
        grid = SF_UIs.CreateTwoDimButtonContinuousY(panelSize, gridSize, offsetGrid.x, offsetGrid.y);
    }

}
