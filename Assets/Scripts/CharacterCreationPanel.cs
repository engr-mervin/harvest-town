using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCreationPanel : MonoBehaviour
{
    public Vector2 size;
    public Vector2 size2;
    public Vector2Int gridSize;
    public Vector2 offset;

    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;
    public Button button5;
    public Button button6;

    public InputField inputName;
    public Button okBtn;


    public static string chosenCharacter;
    public static string chosenName;

    private void Awake()
    {
    }
}
