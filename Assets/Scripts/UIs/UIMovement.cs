using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMovement : MonoBehaviour //responsible for button interface
{
    [Header("Button Locations")]
    [SerializeField]
    private Vector2 dPadMovementArea;
    [SerializeField]
    private float buttonDistance;

    [Header("Primary Buttons")]
    [SerializeField]
    private Button buttonUp;
    [SerializeField]
    private Button buttonDown;
    [SerializeField]
    private Button buttonLeft;
    [SerializeField]
    private Button buttonRight;
    private RectTransform buttonUpRT, buttonDownRT, buttonLeftRT, buttonRightRT;

    [Header("Secondary Buttons")]
    [SerializeField]
    private Button buttonUpLeft;
    [SerializeField]
    private Button buttonDownLeft;
    [SerializeField]
    private Button buttonUpRight;
    [SerializeField]
    private Button buttonDownRight;
    private RectTransform buttonUpLeftRT, buttonDownLeftRT, buttonUpRightRT, buttonDownRightRT;

    [Header("Tertiary Buttons")]
    [SerializeField]
    private Button buttonInteract;
    [SerializeField]
    private Button buttonPhone;
    [SerializeField]
    private Button buttonPause;
    [SerializeField]
    private Button buttonMove;
    [SerializeField]
    private Button buttonBuild;

    private RectTransform buttonInteractRT, buttonPhoneRT, buttonPauseRT, buttonMoveRT, buttonBuildRT;

    [Header("Others")]
    [SerializeField]
    private Button buttonOpenItem;
    private RectTransform buttonOpenItemRT;

    [Header("Button Sizes")]
    [SerializeField]
    private Vector2 primarySize;
    [SerializeField]
    private Vector2 secondarySize;
    [SerializeField]
    private Vector2 tertiarySize;

    [SerializeField]
    private float alphaTint;

    public Button[] primaryButtons;
    public Button[] secondaryButtons;
    public Button[] tertiaryButtons;
    public Button[] allButtons;

    private void Awake()
    {
        buttonUpRT = buttonUp.GetComponent<RectTransform>();
        buttonDownRT = buttonDown.GetComponent<RectTransform>();
        buttonLeftRT = buttonLeft.GetComponent<RectTransform>();
        buttonRightRT = buttonRight.GetComponent<RectTransform>();

        buttonUpLeftRT = buttonUpLeft.GetComponent<RectTransform>();
        buttonDownLeftRT = buttonDownLeft.GetComponent<RectTransform>();
        buttonUpRightRT = buttonUpRight.GetComponent<RectTransform>();
        buttonDownRightRT = buttonDownRight.GetComponent<RectTransform>();

        buttonInteractRT = buttonInteract.GetComponent<RectTransform>();
        buttonPhoneRT = buttonPhone.GetComponent<RectTransform>();
        buttonPauseRT = buttonPause.GetComponent<RectTransform>();
        buttonMoveRT = buttonMove.GetComponent<RectTransform>();

        buttonOpenItemRT = buttonOpenItem.GetComponent<RectTransform>();
        buttonBuildRT = buttonBuild.GetComponent<RectTransform>();

        primaryButtons = new Button[] { buttonUp, buttonLeft, buttonDown, buttonRight };
        secondaryButtons = new Button[] { buttonUpLeft, buttonUpRight, buttonDownLeft, buttonDownRight };
        tertiaryButtons = new Button[] { buttonInteract, buttonPhone, buttonPause, buttonMove, buttonBuild };
    }
    private void Start()
    {
        //DPad CenterPoint
        Vector3 centerPoint = new Vector3((dPadMovementArea.x / 2 - 0.50f) * ScreenSize.x, (dPadMovementArea.y / 2 - 0.50f) * ScreenSize.y, 0f);

        //Primary Buttons Locations & sizes
        buttonUp.GetComponent<RectTransform>().localPosition = centerPoint + Vector3.up * buttonDistance;
        buttonDownRT.localPosition = centerPoint + Vector3.down * buttonDistance;
        buttonLeftRT.localPosition = centerPoint + Vector3.left * buttonDistance;
        buttonRightRT.localPosition = centerPoint + Vector3.right * buttonDistance;
        buttonUpRT.sizeDelta = primarySize;
        buttonDownRT.sizeDelta = primarySize;
        buttonLeftRT.sizeDelta = primarySize;
        buttonRightRT.sizeDelta = primarySize;

        //Secondary Buttons Locations & sizes
        buttonUpLeftRT.localPosition = centerPoint + Vector3.up * buttonDistance + Vector3.left * buttonDistance;
        buttonDownLeftRT.localPosition = centerPoint + Vector3.down * buttonDistance + Vector3.left * buttonDistance;
        buttonUpRightRT.localPosition = centerPoint + Vector3.up * buttonDistance + Vector3.right * buttonDistance;
        buttonDownRightRT.localPosition = centerPoint + Vector3.down * buttonDistance + Vector3.right * buttonDistance;
        buttonUpLeftRT.sizeDelta = secondarySize;
        buttonDownLeftRT.sizeDelta = secondarySize;
        buttonUpRightRT.sizeDelta = secondarySize;
        buttonDownRightRT.sizeDelta = secondarySize;

        //Tertiary Buttons Locations  & sizes
        buttonInteractRT.localPosition = new Vector3(-centerPoint.x, centerPoint.y, 0f);
        buttonPhoneRT.localPosition = new Vector3(-centerPoint.x, -centerPoint.y, 0f);
        buttonPauseRT.localPosition = new Vector3(-centerPoint.x - 60f, -centerPoint.y, 0f);
        buttonBuildRT.localPosition = new Vector3(-centerPoint.x - 120f, -centerPoint.y, 0f);
        buttonMoveRT.localPosition = new Vector3(-centerPoint.x - 60f, centerPoint.y, 0f);
        buttonOpenItemRT.localPosition = new Vector3((ScreenSize.x / 2) - 15f, 0f, 0f);

        buttonInteractRT.sizeDelta = tertiarySize;
        buttonPhoneRT.sizeDelta = tertiarySize;
        buttonPauseRT.sizeDelta = tertiarySize;
        buttonMoveRT.sizeDelta = tertiarySize;
        buttonBuildRT.sizeDelta = tertiarySize;
    }
}
