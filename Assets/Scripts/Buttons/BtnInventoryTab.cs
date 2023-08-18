
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Tilemaps;


public class BtnInventoryTab : MonoBehaviour, IPointerClickHandler
{
    [Header("Set This")]
    public ShopTab activeTab;

    public Buying shopMaster;

    public void OnPointerClick(PointerEventData eventData)
    {
        shopMaster.SwitchTab(activeTab);
    }
}
