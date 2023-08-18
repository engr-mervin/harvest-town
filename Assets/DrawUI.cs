using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawUI : MonoBehaviour
{
    public BC_UIs[] drawList;
    public UI_Position[] UIpos;

    public bool runOnAwake;
    private void Awake()
    {
        if (runOnAwake)
            DrawUIs();
    }
    public void DrawUIs()
    {
        foreach(BC_UIs ui in drawList)
        {
            ui.DrawUI();
        }
        if (UIpos == null) return;

        foreach(UI_Position up in UIpos)
        {
            up.Compute();
        }
    }
    

 
}
