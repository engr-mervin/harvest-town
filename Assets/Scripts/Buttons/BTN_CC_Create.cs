using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class BTN_CC_Create : MonoBehaviour, IPointerClickHandler
{
    public InputField playerName;

    public void OnPointerClick(PointerEventData eventData)
    {
        SO_CC_Maker.instance.playerName = playerName.text;
        print(GM.instance);
        GM.instance.NewGameMethod(SO_CC_Maker.instance.resourceName,SO_CC_Maker.instance.playerName);
    }
}
