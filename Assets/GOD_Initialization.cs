using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOD_Initialization : MonoBehaviour
{
    public StatePanel[] statePanels;
    public TC_Movement initialize;
    public BC_TouchControls[] tcs;

    public void Initialize()
    {
        foreach(StatePanel s in statePanels)
        {
            s.Initialize();
        }

    }

    public void SetTCMovement()
    {
        initialize.Initialize();
    }
    public void ClearLists()
    {
        foreach(BC_TouchControls btc in tcs)
        {
            btc.ClearStaticList();
        }
    }
}
