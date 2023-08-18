using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoney : MonoBehaviour
{
    public delegate void MoneyChanged();


    public void NewGame()
    {
        money = 100000;
        OnMoneyChanged?.Invoke();
    }

    public void LoadGame(SaveFile saved)
    {
        money = saved.playerMoney;
        OnMoneyChanged?.Invoke();
    }
    public event MoneyChanged OnMoneyChanged;
    public int money;

    public int totalExpended;

    public bool CanAfford(int cost)
    {
        if (money >= cost)
            return true;

        return false;
    }

    public void Reduce(int cost)
    {
        if(CanAfford(cost))
        {
            money -= cost;
            OnMoneyChanged?.Invoke();
        }
    }
    public void Gain(int profit)
    {
        money += profit;
        OnMoneyChanged?.Invoke();
    }
}
