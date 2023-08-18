
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class BtnEraseTile : MonoBehaviour, IPointerClickHandler
{
    public TC_Build tc;
    public Sprite eraseMode;
    public Sprite applyMode;

    void Awake()
    {
        if (!tc.eraseMode)
        {
            GetComponent<Image>().sprite = applyMode;
        }
        else
        {
            GetComponent<Image>().sprite = eraseMode;
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (tc.eraseMode)
        {
            tc.eraseMode = false;
            GetComponent<Image>().sprite = applyMode;
        }
        else
        {
            tc.eraseMode = true;
            GetComponent<Image>().sprite = eraseMode;
        }
    }
}
