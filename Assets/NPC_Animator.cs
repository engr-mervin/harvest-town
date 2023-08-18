using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC_Animator : MonoBehaviour
{ 
    public string playerName;
    public string playerType;

    public Text nameText;

    [SerializeField]
    private Character character;
    [SerializeField]
    private AnimatorOverrideController animatorOverride;

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

        SetAnimation(character);
    }
}
