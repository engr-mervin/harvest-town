using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UI_PauseMenu : BC_UIs
{
    [Range(0.5f, 1.0f)]
    public float pauseMenuHeight;
    [Range(0.4f, 1.0f)]
    public float pauseMenuWidth;


    public GameObject inv;


    public GameObject[] movementKeys;

    public TintBackground panel;
    public Image pauseMenuImage;

    public Button buttonResume;
    public Button buttonOptions;
    public Button buttonExit;
    public Button buttonBack;

    public Button buttonSave;
    public Button buttonLoad;

    public Button buttonSlot1;

    public Button buttonSlot2;
    public Button buttonSlot3;

    public float spacing;
    public float topBottomSpacing;
    public float leftRightSpacing;


    public Button[] allButtons;
    public Button[] menu1;
    public Button[] menu2;
    public Button[] menu3;

    public int activeMenu;

    public override void DrawUI()
    {
        pauseMenuImage = gameObject.GetComponent<Image>();
        pauseMenuImage.rectTransform.sizeDelta = new Vector2(pauseMenuWidth * ScreenSize.x, pauseMenuHeight * ScreenSize.y);
        pauseMenuImage.rectTransform.localPosition = new Vector2(0f, 0f);

        allButtons = gameObject.GetComponentsInChildren<Button>();

        menu1 = new Button[3]
        {
            buttonResume,
            buttonOptions,
            buttonExit
        };

        menu2 = new Button[3]
        {
            buttonSave,
            buttonLoad,
            buttonBack
        };

        menu3 = new Button[4]
        {
            buttonSlot1,
            buttonSlot2,
            buttonSlot3,
            buttonBack
        };

    }

    public void SetSizeandLocation(Button b, int i, int j)
    {
        float buttonHeight;
        float buttonWidth;
        float yLoc;

        buttonHeight = (pauseMenuImage.rectTransform.sizeDelta.y - 2*topBottomSpacing-(j-1) * spacing) / j;
        buttonWidth = (pauseMenuImage.rectTransform.sizeDelta.x - 2 * leftRightSpacing);
        yLoc = (pauseMenuImage.rectTransform.sizeDelta.y / 2) - topBottomSpacing-((i-1) * spacing) - ((i - 0.50f) * (buttonHeight));

        b.GetComponent<RectTransform>().sizeDelta = new Vector2(buttonWidth, buttonHeight);
        b.GetComponent<RectTransform>().localPosition = new Vector2(0.0f, yLoc);
    }

}
