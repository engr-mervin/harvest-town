using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BTN_ActionObject : MonoBehaviour,IPointerClickHandler
{
    [HideInInspector]
    public GameObject actionObject;

    public Text display; //****

    public void SetSizeAndLoc(int number,int total,float width,float indHeight)
    {
        float top = (total *0.50f) * indHeight;
        float centerY = top - (number - 0.50f) * indHeight;
        float centerX = 0;

        GetComponent<RectTransform>().localPosition = new Vector2(centerX, centerY);
        GetComponent<RectTransform>().sizeDelta = new Vector2(width - 16, indHeight - 10);
    }
    public void SetText(string text)
    {
        print(text);
        gameObject.name = text;
        display.text = text;
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        SpawnNewWindow();

        NPCTalk();
    }
    
    public void SpawnNewWindow()
    {
        //GET ALL AVAILABLE OPTIONS
        BC_ActionOption[] options = actionObject.GetComponents<BC_ActionOption>();

        List<BC_ActionOption> temp = new List<BC_ActionOption>();

        for(int i=0;i<options.Length;i++)
        {
            if (options[i].isAvailable == true)
                temp.Add(options[i]);
        }

        options = temp.ToArray();

        //CREATE NEW WINDOW
        WIN_Options optionWindow = Instantiate(GameGod.gameUI.dialogueBoxOptions, StatePanel.interacting.transform).GetComponent<WIN_Options>();
        optionWindow.SetSizeAndLocation(options.Length + 1, 200, 40, new Vector2(0.8f, 0.8f));
        
        if(options.Length==1)
        {
            options[0].ButtonClicked();
            return;
        }
        //SPAWN OPTION BUTTONS
        for (int i = 0; i < options.Length; i++)
        {
            BTN_ActionOption buttonOption = Instantiate(GameGod.gameUI.actionOption, optionWindow.transform).GetComponent<BTN_ActionOption>();
            buttonOption.action = options[i];
            buttonOption.SetSizeAndLoc(i + 1, options.Length + 1, 200, 40);
            buttonOption.SetText(options[i].display);
        }

        //SPAWN EXIT BUTTON
        BTN_ActionExit buttonExit = Instantiate(GameGod.gameUI.actionExit, optionWindow.transform).GetComponent<BTN_ActionExit>();
        buttonExit.SetSizeAndLoc(options.Length + 1, options.Length + 1, 200, 40);
        buttonExit.SetText("Exit");
    }

    public void NPCTalk()
    {
        if (actionObject.GetComponent<NPC>() == null) return;

        //MOVE THIS TO NPC MOVEMENT SCRIPT***
        actionObject.GetComponent<PlayerMovement>().lookDir = GetLookDir(actionObject.transform.position, GM.player.transform.position);

        DialogueBox db = Instantiate(GameGod.gameUI.dialogueBox, StatePanel.interacting.transform).GetComponent<DialogueBox>();
        db.SetSizeAndLoc(0.80f, 0.35f, new Vector2(0.50f, -0.30f), actionObject.GetComponent<SpriteRenderer>());
        db.ShowDialogueBox(actionObject.GetComponent<NPC>().greetings);
    }

    public static Vector2Int GetLookDir(Vector3 looker, Vector3 lookOn)
    {
        float diffX = lookOn.x - looker.x;
        float diffY = lookOn.y - looker.y;
        int x, y;
        if(Mathf.Abs(diffX)>Mathf.Abs(diffY))
        {
            y = 0;
            if (diffX < 0)
            {
                x = -1;
            }
            else
            {
                x = 1;
            }
        }
        else
        {
            x = 0;
            if (diffY < 0)
            {
                y = -1;
            }
            else
            {
                y = 1;
            }
        }

        return new Vector2Int(x, y);

    }
}
