using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class BTN_Buy : MonoBehaviour,IPointerClickHandler
{
    [Header("Set This")]
    public QuantitySlide qty;
    public Buying shopMaster;

    public void OnPointerClick(PointerEventData eventData)
    {
        BC_SellItems forSale = shopMaster.chosenItem.item;

        BC_Items sold = forSale.item;

        if (forSale == null) return;

        if (forSale.perishable==true&& qty.quantity > forSale.quantity) return; //not enough items

        int totalCost = qty.quantity * shopMaster.chosenItem.item.cost;

        if (!GM.playerMoney.CanAfford(totalCost)) //not enough money
        {
            shopMaster.shopConfirm.gameObject.SetActive(true);
            shopMaster.shopConfirm.CantAfford();
            return;
        }

        if(INV_ItemSlot.CanHandleObjects(sold.itemCode,qty.quantity)) //buy successful
        {
            shopMaster.shopConfirm.gameObject.SetActive(true);
            shopMaster.shopConfirm.SetBuyText(shopMaster.chosenItem,qty.quantity,qty.quantity*forSale.cost);
        }
        else
        {
            shopMaster.shopConfirm.gameObject.SetActive(true);
            shopMaster.shopConfirm.CantStore();
        }

    }
}
