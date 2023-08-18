using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBuild : MonoBehaviour //responsible for button interface
{
    [Header("Button Locations")]
    [SerializeField]
    private Vector2 dPadMovementArea;
    [SerializeField]
    private float buttonDistance;
    [SerializeField]
    private Button buttonCloseBuild;

    private RectTransform buttonCloseBuildRT;

    [Header("Others")]
    [SerializeField]
    private Button buttonOpenInv;
    private RectTransform buttonOpenInvRT;

    [Header("Button Sizes")]
    [SerializeField]
    private Vector2 tertiarySize;

    [SerializeField]
    private float alphaTint;

    private void Awake()
    {
        buttonOpenInvRT = buttonOpenInv.GetComponent<RectTransform>();
        buttonCloseBuildRT = buttonCloseBuild.GetComponent<RectTransform>();

    }
    private void Start()
    {
        //DPad CenterPoint
        Vector3 centerPoint = new Vector3((dPadMovementArea.x / 2 - 0.50f) * ScreenSize.x, (dPadMovementArea.y / 2 - 0.50f) * ScreenSize.y, 0f);


        //Tertiary Buttons Locations  & sizes
        buttonCloseBuildRT.localPosition = new Vector3(-centerPoint.x - 120f, -centerPoint.y, 0f);
        buttonOpenInvRT.localPosition = new Vector3((ScreenSize.x / 2) - 15f, 0f, 0f);

        buttonCloseBuildRT.sizeDelta = tertiarySize;
    }
}
