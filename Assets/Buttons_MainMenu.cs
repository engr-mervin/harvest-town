using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons_MainMenu : MonoBehaviour
{
    public GameObject slots, main, credits, charCreation;

    public void NewGame()
    {
        MyFunctions.SingleToggle(charCreation, main);
    }

    public void LoadGame()
    {
        MyFunctions.SingleToggle(slots,main);
    }
    public void ExitGame()
    {
        Application.Quit();
        print("ExitGame");
    }

    public void ExitCC()
    {
        MyFunctions.SingleToggle(main, charCreation);
    }
    public void Credits()
    {
        MyFunctions.SingleToggle(credits, main);

    }

    public void Credits_Close()
    {
        MyFunctions.SingleToggle(main, credits);
    }

    public void OptionsButton()
    {
        MyFunctions.SingleToggle(Options.instance.canvas,null);
    }
}
