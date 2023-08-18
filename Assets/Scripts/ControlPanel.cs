using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanel : MonoBehaviour
{
    public float dPadPanelSize;
    public Vector2Int dPadGrid;
    public Vector2 dPadOffset;

    public Vector2 actionPanel;
    public Vector2Int actionGrid;
    public Vector2 actionOffset;

    public Vector2 topPanel;
    public Vector2Int topGrid;
    public Vector2 topOffset;

    public static Vector2[] topGridResult;
    public static Vector2[] dPadGridResult;
    public static Vector2[] actionGridResult;

    private void Awake()
    {
        Vector2 dPadPanel = new Vector2(dPadPanelSize, dPadPanelSize);

        topGridResult = new ButtonUIGrid(topPanel, topGrid, topOffset.x, topOffset.y).grid;
        dPadGridResult = new ButtonUIGrid(dPadPanel, dPadGrid, dPadOffset.x, dPadOffset.y).grid;
        topGridResult = new ButtonUIGrid(actionPanel, actionGrid, actionOffset.x, actionOffset.y).grid;
    }
}
