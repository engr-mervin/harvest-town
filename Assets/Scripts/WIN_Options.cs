using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WIN_Options : MonoBehaviour
{
    public static bool isShown;

    [HideInInspector]
    public Image DBoxOptions;

    public delegate void DBoxEnabled();

    public static event DBoxEnabled OnDBoxEnable;

    public delegate void DBoxDisabled();

    public static event DBoxDisabled OnDBoxDisable;

    public static WIN_Options instance;


    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);

        instance = this;
    }

    public void ShowDialogueBox(string message)
    {
        OnDBoxEnable?.Invoke();

        isShown = true;
        DBoxOptions.GetComponentInChildren<Text>().text = message;
        DBoxOptions.gameObject.SetActive(true);
    }

    public void DestroyThis()
    {
        OnDBoxDisable?.Invoke();

        isShown = false;
        Destroy(this.gameObject);
    }

    public void SetSizeAndLocation(int buttons, float width, float indHeight,Vector2 loc)
    {
        DBoxOptions = GetComponent<Image>();

        Vector2 c = new Vector2(width, buttons*indHeight);

        Vector2 a = MyFunctions.BottomLeftToCenterLocation(loc); //location from center raw

        Vector2 b = ScreenSize.xy; //whole size

        Vector2 d = Vector2.Scale(a, b); //location from center

        if ((c.x / 2) + d.x > (b.x / 2)) //exceeds screen at right
            d.x = (b.x - c.x) / 2;

        if ((c.y / 2) + d.y > (b.y / 2)) //exceeds screen at top
            d.y = (b.y - c.y) / 2;

        if ((-c.x / 2) + d.x < (-b.x / 2)) //exceeds screen at left
            d.x = (-b.x + c.x) / 2;

        if ((-c.y / 2) + d.y < (-b.y / 2)) //exceeds screen at bottom
            d.y = (-b.y + c.y) / 2;

        DBoxOptions.rectTransform.sizeDelta = c;
        DBoxOptions.rectTransform.localPosition = d;

    }

}
