using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;






public class StartSettings : MonoBehaviour
{
    public Player player;
    public FadeRender tree;
    public FollowPlayer mainCam;

    public InputField moveSpeed;
    public InputField treeFade;
    public InputField cameraSmoothing;
    public InputField joystick;

    public Canvas startMenu;


    public void StartButton()
    {
        
        tree.fadeAlpha = float.Parse(treeFade.text);
        mainCam.smoothing = float.Parse(cameraSmoothing.text);


        Debug.Log(moveSpeed.text);
        Debug.Log(treeFade.text);
        Debug.Log(cameraSmoothing.text);
        Debug.Log(joystick.text);

    }

    
}
