using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Interacting : MonoBehaviour
{
    public void SpawnFirstSet(PlayerMovement actorMove)
    {
        GameObject objects = InteractRayCast.GetNearestInteractable();

        //IF NONE THEN DON'T DO ANYTHING
        if (objects == null)
        {
            Debug.Log("This shouldn't run.");
            GM.playerState.SetState(new MovementState(GM.playerState));
        }

        //SPAWN WINDOW
        WIN_Options optionWindow = GameObject.Instantiate(GameGod.gameUI.dialogueBoxOptions, transform).GetComponent<WIN_Options>();

        BTN_ActionObject objectOption = GameObject.Instantiate(GameGod.gameUI.actionObject, optionWindow.transform).GetComponent<BTN_ActionObject>();
        objectOption.actionObject = objects;
        objectOption.SpawnNewWindow();
        objectOption.NPCTalk();
        return;

    }

    public void OnDisable()
    {
        foreach(Transform t in GetComponentsInChildren<Transform>())
        {
            if (t==null||transform == t)
                continue;

            Destroy(t.gameObject);
        }
    }
}
