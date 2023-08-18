
using UnityEngine;

internal class UI_Movement_Actions : BC_UIs //responsible for button interface
{
    private RectTransform actions;

    private float buttonDistance; //30-50
    private float buttonSize;
    private Vector3 offset;

    [SerializeField]
    private RectTransform buttonMove;
    [SerializeField]
    private RectTransform buttonInteract;
    [SerializeField]
    private Vector3 originalCenter;


    public override void DrawUI()
    {
        actions = GetComponent<RectTransform>();

        offset = StaticValues.instance.actionsOffset;
        buttonSize = StaticValues.instance.actionsSize;
        buttonDistance = StaticValues.instance.actionsDistance;

        actions.localPosition = originalCenter + offset;
        //Primary Buttons Locations & sizes
        buttonMove.localPosition = Vector3.left * buttonDistance/2;
        buttonInteract.localPosition = Vector3.right * buttonDistance/2;
        buttonMove.sizeDelta = Vector2.one * buttonSize;
        buttonInteract.sizeDelta = Vector2.one * buttonSize;
    }
}