using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BTN_SellableItem : MonoBehaviour,IPointerClickHandler //the button on shop
{
    public delegate void ItemExhausted(int index,int tabNumber);

    public static event ItemExhausted OnItemExhausted;

    public BC_SellItems item;

    public int currentTabNumber;

    private int currentIndex;
    private AO_BuyItems seller;
    private Buying shopMaster;

    [Header("Set This")]
    public Text qtyText;

    //WHEN INSTANTIATED
    public void SetButton(BC_SellItems forSale,AO_BuyItems cSeller,int index,int tabNumber,Buying shop)
    {
        item = forSale;

        qtyText.text = (forSale.quantity).ToString();

        GetComponent<Image>().sprite = forSale.item.sprite;

        seller = cSeller;

        currentIndex = index;

        currentTabNumber = tabNumber;

        shopMaster = shop;
    }

    public void Awake()
    {
        OnItemExhausted += this.Reorder;
    }

    public void Reorder(int index, int tabNumber)
    {
        if (currentTabNumber != tabNumber) return;

        if (currentIndex < index) return;

        currentIndex--;

        GetComponent<RectTransform>().localPosition = transform.parent.parent.GetComponent<UI_BuyItems>().grid[currentIndex];
    }
    //WHEN BUY BUTTON IS CLICKED
    public void ReduceStock(int qty)
    {
        item.quantity -= qty;

        qtyText.text = item.quantity.ToString();

        if (item.quantity == 0)
        {
            OnItemExhausted -= Reorder;

            Destroy(gameObject);
            
            seller.forSale.RemoveAt(seller.forSale.FindIndex(sell => sell == item));

            OnItemExhausted?.Invoke(currentIndex,currentTabNumber);
        }
    }

    //SHOW BUY QUANTITY WINDOW
    public void OnPointerClick(PointerEventData eventData)
    {
        shopMaster.SetCurrentItem(this);
    }

    public void SetLocation(Vector2 loc)
    {
        GetComponent<RectTransform>().localPosition = loc;
    }
}
