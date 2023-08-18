using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGod : MonoBehaviour
{
    public static GameUI gameUI;
    public static GameLoader gameLoader;

    public void Initialize()
    {
        gameUI = Resources.Load("GameUI", typeof(GameUI)) as GameUI;

        gameLoader = Resources.Load("GameLoader", typeof(GameLoader)) as GameLoader;


        //MODIFY THIS IN THE FUTURE

        //LEORIO
        AO_BuyItems ao = GameObject.FindObjectOfType<AO_BuyItems>();
        ao.forSale.Clear();
        foreach (BC_Items item in gameLoader.itemIndexer.rawDatabase)
        {
            BC_SellItems a = new BC_SellItems();
            a.cost = item.rawCost;
            a.item = item;
            a.quantity = 99;
            a.perishable = false;

            ao.forSale.Add(a);
        }
    }
}
