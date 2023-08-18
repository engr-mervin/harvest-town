using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MainMenu_Slots : BC_UIs
{
    [SerializeField]
    private RectTransform slot0, slot1, slot2, slot3, back;

    [SerializeField]
    private Vector2 size;

    [SerializeField]
    private float height;

    private void Awake()
    {
        Vector2[] locs = SF_UIs.CreateVerticalButtonsOneRow(5, height);

        slot0.localPosition = locs[0];
        slot0.sizeDelta = size;

        slot1.localPosition = locs[1];
        slot1.sizeDelta = size;

        slot2.localPosition = locs[2];
        slot2.sizeDelta = size;

        slot3.localPosition = locs[3];
        slot3.sizeDelta = size;

        back.localPosition = locs[4];
        back.sizeDelta = size;
    }
}