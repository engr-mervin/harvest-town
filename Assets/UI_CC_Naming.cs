using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CC_Naming : BC_UIs
{ 
    [SerializeField]
    private RectTransform inputField, create;

    [SerializeField]
    private Vector2 size;

    [SerializeField]
    private Vector2Int gridSize;


    public override void DrawUI()
    {
        Vector2[] locs = SF_UIs.CreateTwoDimButtons(size, gridSize);

        inputField.localPosition = locs[0];
        create.localPosition = locs[1];
    }
}
