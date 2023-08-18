using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetItemImage : MonoBehaviour
{
    public string tab;

    public void GetImage()
    {
       
        BtnGetObject bci = GetComponentInParent<BtnGetObject>();

        string name = bci.gameObject.name;
        Debug.Log("Objects/" + tab + "/" + name);
        GameObject g = (GameObject)(Resources.Load("Objects/" + tab + "/" + name));

        Debug.Log(g);

        GetComponent<Image>().sprite = g.GetComponent<SpriteRenderer>().sprite;
    }
}
