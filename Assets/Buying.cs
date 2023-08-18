using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buying : MonoBehaviour
{
    [Header("Script Driven")]
    List<BC_SellItems> currentItems;

    public BTN_SellableItem chosenItem;

    //public bool qtyShown;

    public AO_BuyItems currentSeller;

    public ShopTab currentTab;

    [Header("Set This")]
    public UI_BuyItems window;
    public ShopDescription shopDesc;
    public GameObject qtyWindow;
    public GameObject closeBuy;
    public DragPanel dragParent;
    public ShopTab[] shopTabs;
    public ShopConfirmation shopConfirm;

    public void InstantiateItems(GameObject seller)
    {
        AO_BuyItems cSeller = seller.GetComponent<AO_BuyItems>();
        currentItems = cSeller.forSale;

        window.ComputeGrid();
        currentSeller = cSeller;

        InstantiateButtons<ITEM_Wallpaper>(0);
        InstantiateButtons<ITEM_Carpets>(1);
        InstantiateButtons<ITEM_Furnitures>(2);
    }
    private void InstantiateButtons<T>(int tabNumber) where T : BC_Items
    {
        int currentCount = 0;

        List<BC_SellItems> currentObjects = new List<BC_SellItems>();

        //SEGREGATE ITEMS FOR SALE
        foreach (BC_SellItems sell in currentItems)
        {
            if (sell == null) continue;

            if (sell.item.gameObject.GetComponent<T>() != null)
            {
                currentCount++;
                currentObjects.Add(sell);
            }
        }

        GameObject parent = shopTabs[tabNumber].gameObject;


        //SET PARENT, LOCATION, INITIALIZE DRAG
        for (int i = 0; i < currentCount; i++)
        {
            GameObject wp = Instantiate(GameGod.gameUI.buyItemButton, parent.transform);
            wp.GetComponent<BTN_SellableItem>().SetLocation(window.grid[i]);
            wp.GetComponent<BTN_SellableItem>().SetButton(currentObjects[i],currentSeller,i,tabNumber,this);

            wp.GetComponent<DragButton>().Initialize();
        }

        SwitchTab(currentTab);
    }

    public void RearrangeActive(List<GameObject> active)
    {
        //SET PARENT, LOCATION, INITIALIZE DRAG
        for (int i = 0; i < active.Count; i++)
        {
            print(i);
            active[i].GetComponent<BTN_SellableItem>().SetLocation(window.grid[i]);

            active[i].GetComponent<DragButton>().Initialize();
        }
    }
    public void SwitchTab(ShopTab current)
    {
        foreach (ShopTab st in shopTabs)
        {
            if (st == current)
            {
                st.gameObject.SetActive(true);
                currentTab = current;
                DragButton[] buttons = st.GetComponentsInChildren<DragButton>(includeInactive: true);
                dragParent.SetButtons(buttons);
            }
            else
            {
                st.gameObject.SetActive(false);
            }
        }
    }

    public void SetCurrentItem(BTN_SellableItem button)
    {
        chosenItem = button;
        shopDesc.SetDescription(button);
        shopDesc.SetID(button);
        SetActiveQuantity();
    }

    public void SetActiveQuantity()
    {
        qtyWindow.SetActive(true);
        qtyWindow.GetComponentInChildren<QuantitySlide>().Reset();
    }
    private void OnDisable()
    {
        qtyWindow.SetActive(false);
        shopDesc.ClearDescription();
        chosenItem = null;
        foreach (ShopTab st in shopTabs)
        {
            st.DestroyItems();
        }
    }

}
