using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CC_BaseChar : BC_UIs
{
    [SerializeField]
    private RectTransform bg, c1, c2, c3, c4, c5, c6;

    [SerializeField]
    private Vector2 size;

    [SerializeField]
    private Vector2 offset;

    [SerializeField]
    private Vector2Int gridSize;


    public override void DrawUI()
    {
        Vector2[] grid = SF_UIs.CreateTwoDimButtons(size, gridSize, offset.x, offset.y);

        bg.sizeDelta = SF_UIs.SetImageSize(size);

        c1.localPosition = grid[0];
        c2.localPosition = grid[1];
        c3.localPosition = grid[2];
        c4.localPosition = grid[3];
        c5.localPosition = grid[4];
        c6.localPosition = grid[5];
    }
}
