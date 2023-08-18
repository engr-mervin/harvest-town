using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TintBackground : MonoBehaviour
{
    public Image panelColor;
    public Color color;
    // Start is called before the first frame update
    void Awake()  
    {
        panelColor = GetComponent<Image>();

    }

    public void Tint()
    {
        panelColor.color = color;
    }

    public void UnTint()
    {
        panelColor.color = Color.clear;
    }
}
