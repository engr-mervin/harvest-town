
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
internal class UI_Movement_DPad : BC_UIs //responsible for button interface
{
    private RectTransform dPad;

    private float buttonDistance;
    private float primarySize;
    private float secondarySize;
    private Vector3 offset;

    [Header("Primary Buttons")]
    [SerializeField]
    private RectTransform up;
    [SerializeField]
    private RectTransform down;
    [SerializeField]
    private RectTransform left;
    [SerializeField]
    private RectTransform right;

    [Header("Secondary Buttons")]
    [SerializeField]
    private RectTransform UL;
    [SerializeField]
    private RectTransform DL;
    [SerializeField]
    private RectTransform UR;
    [SerializeField]
    private RectTransform DR;
    [SerializeField]
    private Vector3 originalCenter;

    private List<RectTransform> primary;
    private List<RectTransform> secondary;

    public void Awake()
    {
        dPad = GetComponent<RectTransform>();

        primary = new List<RectTransform>()
        {
            up,down,left,right
        }; 
        secondary = new List<RectTransform>()
        {
            UL,UR,DL,DR
        };
    }

    public void UpdateDPad()
    {
        offset = StaticValues.instance.dPadOffset;
        buttonDistance = StaticValues.instance.dPadDistance; //30-50
        primarySize = StaticValues.instance.dPadSizePrimary; //30-50
        secondarySize = StaticValues.instance.dPadSizeSecondary; //20-40

        dPad.localPosition = originalCenter+offset;
        //Primary Buttons Locations & sizes
        if (up != null)
        {
            up.localPosition = Vector3.up * buttonDistance;
            up.sizeDelta = Vector2.one * primarySize;
        }

        if (down != null)
        {
            down.localPosition = Vector3.down * buttonDistance;
            down.sizeDelta = Vector2.one * primarySize;
        }

        if (left != null)
        {
            left.localPosition = Vector3.left * buttonDistance;
            left.sizeDelta = Vector2.one * primarySize;
        }

        if (right != null)
        {
            right.localPosition = Vector3.right * buttonDistance;
            right.sizeDelta = Vector2.one * primarySize;
        }

        //Secondary Buttons Locations & sizes
        if (UL != null)
        {
            UL.localPosition = Vector3.up * buttonDistance + Vector3.left * buttonDistance;
            UL.sizeDelta = Vector2.one * secondarySize;
        }
        if (DL != null)
        {
            DL.localPosition = Vector3.down * buttonDistance + Vector3.left * buttonDistance;
            DL.sizeDelta = Vector2.one * secondarySize;
        }
        if (UR != null)
        {
            UR.localPosition = Vector3.up * buttonDistance + Vector3.right * buttonDistance;
            UR.sizeDelta = Vector2.one * secondarySize;
        }
        if (DR != null)
        {
            DR.localPosition = Vector3.down * buttonDistance + Vector3.right * buttonDistance;
            DR.sizeDelta = Vector2.one * secondarySize;
        }
    }
    private void DisableThis()
    {
        dPad.gameObject.SetActive(false);
    }
    private void EnableThis()
    {
        dPad.gameObject.SetActive(true);
    }

    public void PrimaryButtonDown()
    {
        if (GM.playerMove.state != PlayerMovement.State.Movement) return;
        
    }
    public void PrimaryButtonUp()
    {
        if (GM.playerMove.state != PlayerMovement.State.Movement) return;
        
    }
    public void ShowSecondaryButtons()
    {
        foreach (RectTransform rt in secondary)
        {
            rt.gameObject.SetActive(true);
        }
    }
    public void HideSecondaryButtons()
    {
        foreach (RectTransform rt in secondary)
        {
            rt.gameObject.SetActive(false);
        }
    }

    public void ResetTint()
    {
        foreach (RectTransform rt in secondary)
        {
            MyFunctions.UntintButton(rt.GetComponent<Button>());
        }
    }

}