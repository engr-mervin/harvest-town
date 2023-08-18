using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralButtons : MonoBehaviour
{
    public GameObject[] enable;
    public GameObject[] disable;
    public void Phone()
    {
        return;
    }
    public void Pause()
    {
        GM.playerState.SetState(new PauseState(GM.playerState));
    }

    public void Console()
    {
        MyFunctions.Toggle(enable, disable);
    }

    
}

