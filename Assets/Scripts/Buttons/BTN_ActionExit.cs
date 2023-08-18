using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BTN_ActionExit : MonoBehaviour,IPointerClickHandler
{
    public Text display;
    // Start is called before the first frame update
    public void SetText(string text)
    {
        gameObject.name = text;
        display.text = text;
    }
    public void SetSizeAndLoc(int number, int total, float width, float indHeight)
    {
        float top = (total * 0.50f) * indHeight;
        float centerY = top - (number - 0.50f) * indHeight;
        float centerX = 0;

        GetComponent<RectTransform>().localPosition = new Vector2(centerX, centerY);
        GetComponent<RectTransform>().sizeDelta = new Vector2(width - 16, indHeight - 10);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        EndAction();
    }

    public void EndAction()
    {
        GM.playerState.SetState(new MovementState(GM.playerState));
    }
}
