using UnityEngine;

internal class UI_Movement_General : BC_UIs
{
    private RectTransform general;

    private float buttonDistance; //30-50
    private float buttonSize;
    private Vector3 offset;

    [SerializeField]
    private RectTransform buttonPause;
    [SerializeField]
    private RectTransform buttonBuild;
    [SerializeField]
    private RectTransform buttonPhone;
    [SerializeField]
    private Vector3 originalCenter;

    public override void DrawUI()
    {
        general = GetComponent<RectTransform>();
        offset = StaticValues.instance.generalOffset;
        buttonSize = StaticValues.instance.generalSize;
        buttonDistance = StaticValues.instance.generalDistance;

        general.localPosition = originalCenter + offset;

        if (buttonBuild != null)
        {
            buttonBuild.localPosition = Vector3.left * buttonDistance;
            buttonBuild.sizeDelta = Vector2.one * buttonSize;
        }
        if (buttonPhone != null)
        {
            buttonPhone.localPosition = Vector3.right * buttonDistance;
            buttonPhone.sizeDelta = Vector2.one * buttonSize;
        }
        if (buttonPause != null)
        {
            buttonPause.localPosition = Vector3.zero;
            buttonPause.sizeDelta = Vector2.one * buttonSize;
        }
       
        
    }
}