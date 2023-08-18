
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Tilemaps;


public class BtnBuildTab : MonoBehaviour, IPointerClickHandler
{
    public GameObject[] tabs;

    public GameObject activeTab;
    public void OnPointerClick(PointerEventData eventData)
    {
        foreach(GameObject g in tabs)
        {
            if (g == activeTab)
                g.SetActive(true);
            else
                g.SetActive(false);
        }
    }
}
