using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuantitySlide : MonoBehaviour
{
    public Buying shopMaster;
    public Text text;
    public int quantity;

    public int max;
    public int min;
    public Text total;

    private void OnEnable()
    {
        Reset();
    }

    public void Reset()
    {
        quantity = 1;
        text.text = quantity.ToString();

        SetTotal();
    }

    private void SetTotal()
    {
        total.text = "TOTAL: " + (shopMaster.chosenItem.item.cost * quantity).ToString();
    }
    public void Increase()
    {
        if (quantity >=max)
            return;
        quantity += 1;
        text.text = quantity.ToString();

        SetTotal();
    }

    public void Decrease()
    {
        if (quantity <=min)
            return;
        quantity -= 1;
        text.text = quantity.ToString();

        SetTotal();
    }

    public void OnDisable()
    {
        total.text = "";
    }
}

