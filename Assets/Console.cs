using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class Console : MonoBehaviour,IPointerClickHandler
{
    public TMP_InputField input;

    public void OnPointerClick(PointerEventData eventData)
    {
        string[] all = input.text.Split(' ');

        if(all[0]=="tp")
        {
            Teleport(all);
            input.text = "";
            return;
        }
        if (all[0]=="so")
        {
            SendObject(all);
            input.text = "";
            return;
        }
        if(all[0]=="mo")
        {
            AddMoney(all[1]);
        }
    }

    void AddMoney(string money)
    {
        if(!int.TryParse(money,out int result))
        {
            return;
        }
        GM.playerMoney.Gain(result);
    }
    void Teleport(string[] parameter)
    {
        if (parameter.Length != 3) return;

        if (!int.TryParse(parameter[1], out int xNum))
        {
            return;
        }

        if (!int.TryParse(parameter[2], out int yNum))
        {
            return;
        }

        GM.playerMove.transform.position = new Vector3Int(xNum, yNum, 0);
    }

    void SendObject(string[] parameter)
    {
        if (parameter.Length != 3) return;

        string item = parameter[1];
        string quantity = parameter[2];

        if (!GameGod.gameLoader.itemIndexer.TryGetObject(item, out _)) return;

        if (!int.TryParse(quantity, out int qty))
        {
            return;
        }

        INV_ItemSlot.SendObject(item, qty);
    }
}
