using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WallSetsCycleBtn : MonoBehaviour, IPointerClickHandler
{
    public WallSets ws;
    public int add;
    public Text text;

    private int lastValue;
    private void Awake()
    {
        ws =  GetComponentInParent<WallSets>();
        text.text = (ws.currentActive+1).ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        lastValue = ws.currentActive;
        if (ws.currentActive + add < 0 || ws.currentActive + add >= ws.wallSets.Length) return;
        do
        {
            if (ws.currentActive + add < 0 || ws.currentActive + add >= ws.wallSets.Length)
            {
                ws.currentActive = lastValue;
                break;
            }

            ws.currentActive += add;
            
        }
        while (ws.wallSets[ws.currentActive] == null);

        text.text = (ws.currentActive + 1).ToString();
        ws.Refresh();
    }
}
