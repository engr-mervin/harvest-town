using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player:MonoBehaviour // this class is responsible for player details
{
    //at the start of the game this will be asked
    [SerializeField]
    public string playerName;
    public string playerType;

    public Character character;
    public AnimatorOverrideController animatorOverride;

    public enum Gender
    {
        Boy,
        Girl
    }
    public Text nameText;

    [SerializeField]
    private Gender _playerGender;
    public Gender playerGender { get { return _playerGender; } }


    //these are the only setters
    public void ModifyName(string name) 
    {
        playerName = name;
    }
    public void ModifyGender(Gender gender)
    {
        _playerGender = gender;
    }

    private void Start()
    {
        Initialize();
    }
    public void SetAnimation(Character playerChar)
    {
        animatorOverride["IDLE_B"] = playerChar.idleBack;
        animatorOverride["IDLE_F"] = playerChar.idleFront;
        animatorOverride["IDLE_L"] = playerChar.idleLeft;
        animatorOverride["IDLE_R"] = playerChar.idleRight;
        animatorOverride["PHONE_OFF"] = playerChar.phoneOff;
        animatorOverride["PHONE_ON"] = playerChar.phoneOn;
        animatorOverride["RUN_B"] = playerChar.runBack;
        animatorOverride["RUN_F"] = playerChar.runFront;
        animatorOverride["RUN_L"] = playerChar.runLeft;
        animatorOverride["RUN_R"] = playerChar.runRight;
        animatorOverride["SIT_L"] = playerChar.sitLeft;
        animatorOverride["SIT_R"] = playerChar.sitRight;
        animatorOverride["STAND_B"] = playerChar.standBack;
        animatorOverride["STAND_F"] = playerChar.standFront;
        animatorOverride["STAND_L"] = playerChar.standLeft;
        animatorOverride["STAND_R"] = playerChar.standRight;
    }


    public void Initialize()
    {
        name = playerName;
        nameText.text = playerName;
        GetComponent<PlayerData>().playerType = playerType;

        SetAnimation(character);
    }
}
