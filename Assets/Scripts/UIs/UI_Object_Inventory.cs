using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

internal class UI_Object_Inventory : BC_UIs
{
    private RectTransform objectInventory;

    private float buttonDistance; //30-50
    private float buttonSize; //35 default , 30-50
    private Vector3 offset;

    
    [SerializeField]
    private RectTransform button0,button1,button2,button3,button4,button5,button6,button7,trash;

    [SerializeField]
    private Vector3 originalCenter;

    public override void DrawUI()
    {
        objectInventory = GetComponent<RectTransform>();

        offset = StaticValues.instance.objInvOffset;
        buttonSize = StaticValues.instance.objInvSize;
        buttonDistance = StaticValues.instance.objInvDistance;


        objectInventory.localPosition = originalCenter + offset;
        //Primary Buttons Locations & sizes
        button0.localPosition = Vector3.left * buttonDistance * 3.50f;
        button1.localPosition = Vector3.left * buttonDistance * 2.50f;
        button2.localPosition = Vector3.left * buttonDistance * 1.50f;
        button3.localPosition = Vector3.left * buttonDistance * 0.50f;

        button4.localPosition = Vector3.right * buttonDistance * 0.50f;
        button5.localPosition = Vector3.right * buttonDistance * 1.50f;
        button6.localPosition = Vector3.right * buttonDistance * 2.50f;
        button7.localPosition = Vector3.right * buttonDistance * 3.50f;
        trash.localPosition = Vector3.right * buttonDistance * 4.50f;

        button0.sizeDelta = Vector2.one * buttonSize;
        button1.sizeDelta = Vector2.one * buttonSize;
        button2.sizeDelta = Vector2.one * buttonSize;
        button3.sizeDelta = Vector2.one * buttonSize;

        button4.sizeDelta = Vector2.one * buttonSize;
        button5.sizeDelta = Vector2.one * buttonSize;
        button6.sizeDelta = Vector2.one * buttonSize;
        button7.sizeDelta = Vector2.one * buttonSize;


        trash.sizeDelta = Vector2.one * buttonSize;
    }
}
