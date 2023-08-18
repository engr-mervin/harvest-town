using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyText : MonoBehaviour
{
    private TMP_Text tmp;

    public void OnEnable()
    {
        tmp = GetComponent<TMP_Text>();

        StartCoroutine(Subscribe());
        
    }

    public IEnumerator Subscribe()
    {
        int i = 1;
        while (true)
        {
            print(i++);
            print(GM.playerMoney);
            if (GM.playerMoney != null)
            {
                print("Money subscribed");
                GM.playerMoney.OnMoneyChanged += MoneyUpdate;
                MoneyUpdate();
                yield break;
            }

            yield return new WaitForEndOfFrame();
        }

    }
    private void MoneyUpdate()
    {
        print("MONEY UPDATED");
        tmp.text = IntToMoney(GM.playerMoney.money);
    }

    public static string IntToMoney(int money)
    {
        char[] characters = money.ToString().ToCharArray();
        print(money+","+characters.Length);

        string result="";

        for(int i=0;i<characters.Length;i++)
        {
            if((characters.Length-i-1)%3==0&&i!=(characters.Length-1))
            {
                result += characters[i].ToString() + ",";
            }
            else
            {
                result += characters[i].ToString();
            }
        }

        return result;
    }

}
