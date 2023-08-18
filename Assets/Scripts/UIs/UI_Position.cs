using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Position : MonoBehaviour
{
    public Vector3 worldPos;
    public void Compute()
    {
        UI_Position parent = transform.parent.GetComponent<UI_Position>();

        if (parent == null)
        {
            worldPos = GetComponent<RectTransform>().localPosition;

            foreach (UI_Position child in GetComponentsInChildren<UI_Position>(includeInactive:true))
            {
                if (child == this)
                    continue;
                else
                    child.SetPosFromParent(this);
            }
        }
    }

    public void SetPosFromParent(UI_Position parent)
    {
        worldPos = parent.worldPos+ GetComponent<RectTransform>().localPosition;
    }

}

