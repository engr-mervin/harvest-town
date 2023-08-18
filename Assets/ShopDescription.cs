using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ShopDescription : MonoBehaviour
{
    public Text descName;
    public Text descDesc;
    public Image descImage;
    public Text ID;

    public void SetDescription(BTN_SellableItem btn)
    {
        if (btn.item.item.descSprite != null)
        {
            descImage.sprite = btn.item.item.descSprite;
            descImage.color = Color.white;
        }
        else if (btn.item.item.sprite != null)
        {
            descImage.sprite = btn.item.item.sprite;
            descImage.color = Color.white;
        }
        else
            descImage.color = new Color(0, 0, 0, 0);

        descName.text = btn.item.item.descName;
        descDesc.text = btn.item.item.description;
    }

    public void SetID(BTN_SellableItem btn)
    {
        ITEM_Furnitures fur = btn.item.item.GetComponent<ITEM_Furnitures>();

        string states = "1";

        if (fur != null)
        {
            Rotatable rotate = fur.furniture.GetComponent<Rotatable>();

            if (rotate != null)
                states = rotate.states.Length.ToString();
        }

        string itemCode = btn.item.item.itemCode;

        string cost = btn.item.cost.ToString();

        ID.text = "ID: " + itemCode + System.Environment.NewLine + System.Environment.NewLine + "STATES: " + states + System.Environment.NewLine + System.Environment.NewLine + "COST: " + cost;
    }

    public void ClearDescription()
    {
        descImage.sprite = null;
        ID.text = "";
        descName.text = "";
        descDesc.text = "";
        descImage.color = new Color(0, 0, 0, 0);
    }
}
