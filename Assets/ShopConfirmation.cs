using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopConfirmation : MonoBehaviour
{
    public int state = 0;
    public Text text;
    public BTN_SellableItem current;
    public int currentQty;
    public int currentCost;
    public void CantStore()
    {
        text.text = "INVENTORY FULL.";
        state = 1;
    }
    public void CantAfford()
    {
        text.text = "NOT ENOUGH MONEY.";
        state = 2;
    }
    public void SetBuyText(BTN_SellableItem sell,int quantity, int cost)
    {
        text.text = "BUY " + quantity + " " + sell.item.item.descName + " FOR " + cost + "?";
        currentQty = quantity;
        current = sell;
        currentCost = cost;
        state = 3;
    }

    public void Ok()
    {
        if (state == 1 || state == 2)
            Cancel();

        else if (state==3)
        {
            INV_ItemSlot.SendObject(current.item.item.itemCode, currentQty);
            if (current.item.perishable)
            {
                current.ReduceStock(currentQty);
            }
            //reduce Money
            GM.playerMoney.Reduce(currentCost);

            gameObject.SetActive(false);
        }

        else
            gameObject.SetActive(false);
    }

    public void Cancel()
    {
        gameObject.SetActive(false);
    }
}
