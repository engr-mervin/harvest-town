using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox:MonoBehaviour
{
    public static bool isShown;

    public Image dialogueBox;

    public Image characterBox;

    public Image character;

    public SpriteRenderer speakerSprite;

    public delegate void DBoxEnabled();

    public static event DBoxEnabled OnDBoxEnable;

    public delegate void DBoxDisabled();

    public static event DBoxDisabled OnDBoxDisable;


    private void Awake()
    {
        foreach (DialogueBox db in FindObjectsOfType<DialogueBox>())
        {
            if (db != this)
                Destroy(db.gameObject);
        }

        dialogueBox = GetComponent<Image>();
    }

    public void SetSizeAndLoc(float dBoxX, float dBoxY, Vector2 loc,SpriteRenderer speaker)
    {
        Vector2 raw = new Vector2(dBoxX, dBoxY);

        Vector2 a = MyFunctions.BottomLeftToCenterLocation(loc); //location from center raw

        Vector2 b = ScreenSize.xy; //whole size

        Vector2 c = Vector2.Scale(raw, b); //size

        Vector2 d = Vector2.Scale(a,b); //location from center

        if ((c.x / 2) + d.x > (b.x / 2)) //exceeds screen at right
            d.x = (b.x - c.x) / 2;

        if ((c.y / 2) + d.y > (b.y / 2)) //exceeds screen at top
            d.y = (b.y - c.y) / 2;

        if ((-c.x / 2) + d.x < (-b.x / 2)) //exceeds screen at left
            d.x = (-b.x + c.x) / 2;

        if ((-c.y / 2) + d.y < (-b.y / 2)) //exceeds screen at bottom
            d.y = (-b.y + c.y) / 2;

        //c is full size
        //d is center
        //character is 25%
        //dialogue is 75%

        //set sizes
        dialogueBox.rectTransform.sizeDelta = new Vector2(c.x*0.75f,c.y);
        dialogueBox.rectTransform.localPosition = new Vector2(d.x + c.x / 8, d.y);


        characterBox.rectTransform.sizeDelta = new Vector2(c.x * 0.25f, c.y);
        characterBox.rectTransform.localPosition = new Vector2(- (4*c.x / 8),0f);

        //set image
        speakerSprite = speaker;
    }
    private void Update()
    {
        if(speakerSprite!=null)
            character.sprite = speakerSprite.sprite;
    }


    public void ShowDialogueBox(string message)
    {
        OnDBoxEnable?.Invoke();

        isShown = true;

        dialogueBox.GetComponentInChildren<Text>().text = message;

        gameObject.SetActive(true);
    }

    public void DestroyDialogueBox()
    {
        OnDBoxDisable?.Invoke();

        isShown = false;
        Destroy(this.gameObject);
    }

}
