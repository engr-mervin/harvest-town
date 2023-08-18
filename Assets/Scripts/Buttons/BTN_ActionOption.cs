using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BTN_ActionOption : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector]
    public BC_ActionOption action;
    public Text display;

    public void SetSizeAndLoc(int number, int total, float width, float indHeight)
    {
        float top = (total * 0.50f) * indHeight;
        float centerY = top - (number - 0.50f) * indHeight;
        float centerX = 0;

        GetComponent<RectTransform>().localPosition = new Vector2(centerX, centerY);
        GetComponent<RectTransform>().sizeDelta = new Vector2(width - 16, indHeight - 10);
    }
    public void SetText(string text)
    {
        gameObject.name = text;
        display.text = text;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        action.ButtonClicked();
    }
}
