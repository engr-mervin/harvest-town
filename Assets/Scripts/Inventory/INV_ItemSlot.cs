using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class INV_ItemSlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public static int maxCapacity = 100;
    public static INV_ItemSlot[] slots = new INV_ItemSlot[8];

    public BC_Items item;

    public Image qty;

    [SerializeField]
    private Image sprite;

    public int quantity;

    private S_HotBar hotBar;


    [SerializeField]
    private Text text;

    private GameObject g;

    [SerializeField]
    private int invNumber;

    //for saving

    public void Awake()
    {
        slots[invNumber] = this;

        hotBar = GetComponentInParent<S_HotBar>();
    }

    public void ReduceQuantity(int amount)
    {
        quantity -= amount;
        SetText(quantity);

        if (quantity == 0)
        {
            ClearThisSlot();
        }

    }
    
    public static bool CanHandleObjects(string objectCode, int quantity)
    {
        int capacity=0;

        //Get Max Capacity of slots
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                capacity += (maxCapacity);
            }
            //HAS AN INVENTORY SLOT WITH THE SAME ITEM
            if (slots[i]?.item?.itemCode == objectCode)
            {
                capacity += (maxCapacity - slots[i].quantity);
            }
        }

        if (quantity > capacity)
        {
            print("Too much items");
            return false;
        }
        else
            return true;
    }

    
    public int IncreaseQuantity(int amount)//RETURNS EXCESS
    {
        if (quantity + amount <= maxCapacity)
        {
            quantity += amount;
            SetText(quantity);
            return 0;
        }
        else
        {
            int increase = maxCapacity - quantity;
            quantity += increase;
            SetText(quantity);
            return amount - increase;
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {

        hotBar.SetActiveSlot(this);

    }

    void SetText(int a)
    {
        if (a == 0)
            text.text = "";
        else
            text.text = a.ToString();

        if (a >= 100)
            text.fontSize = 8;
        else
            text.fontSize = 10;
    }

    public int SetItem(string code, int _qty)
    {
        GameObject orig = GameGod.gameLoader.itemIndexer.GetObject(code);
        GameObject copy = Instantiate(orig, transform);

        item = copy.GetComponent<BC_Items>();
        sprite.sprite = item.sprite;
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1.00f);

        qty.enabled = true;

        return IncreaseQuantity(_qty);
    }
    
    public static void SendObject(string objectCode,int quantity)
    {
        int current = quantity;
        while(current>0)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                //HAS AN INVENTORY SLOT WITH THE SAME ITEM
                if (slots[i]?.item?.itemCode == objectCode)
                {
                    if (slots[i].quantity == INV_ItemSlot.maxCapacity)
                        continue;

                    current = slots[i].IncreaseQuantity(current);
                    print(current);
                    if (current == 0)
                        break;
                }
            }

            if (current == 0)
                break;

            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item == null)
                {
                    current = slots[i].SetItem(objectCode, current);
                    if (current == 0)
                        break;
                }
            }
        }

    }

    public void ClearThisSlot()
    {
        Destroy(item.gameObject);
        item = null;
        quantity = 0;
        text.text = "";
        sprite.sprite = null;

        qty.enabled = false;

        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.00f);
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item == null) return;

        //Create Empty GameObject
        g = new GameObject();

        //Set Parent to this button
        g.transform.parent = this.transform;

        //Add sprite
        Image gi = g.AddComponent<Image>();
        gi.sprite = sprite.sprite;

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item == null) return;

        g.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (item == null) return;

        Destroy(g);

        INV_ItemSlot chosenSlot = null;

        bool hit = false;

        foreach (INV_ItemSlot s in INV_ItemSlot.slots)
        {
            if (s.gameObject.activeInHierarchy == false) continue;

            if (!MyFunctions.ScreenPosIsInsideRectTransform(eventData.position, s.GetComponent<RectTransform>(), 1.0f)) //not inside
                continue;
            else//inside
            {
                chosenSlot = s;
                hit = true;
                break;
            }
        }

        if (hit == false)
        {
            foreach (INV_Trash t in FindObjectsOfType<INV_Trash>())
            {
                if (t.gameObject.activeInHierarchy == false) continue;

                if (!MyFunctions.ScreenPosIsInsideRectTransform(eventData.position, t.GetComponent<RectTransform>(), 1.0f)) //not inside
                    continue;
                else//inside
                {
                    ClearThisSlot();
                }
            }
        }

        if (chosenSlot == this) return; //SAME SLOT

        if (chosenSlot == null) return; //NO INVENTORY SLOT AT DRAG END

        if (chosenSlot.item != null & chosenSlot.item != this)  //NOT THE SAME ITEM -- SWAP****
        {
            string itemA = item.itemCode;
            string itemB = chosenSlot.item.itemCode;
            int qtyA = quantity;
            int qtyB = chosenSlot.quantity;

            ClearThisSlot();
            chosenSlot.ClearThisSlot();

            SetItem(itemB, qtyB);
            chosenSlot.SetItem(itemA, qtyA);
        }

        if (chosenSlot.item == this.item) //SAME ITEM
        {
            chosenSlot.quantity += quantity;
            ClearThisSlot();
        }
        if (chosenSlot.item == null)//VACANT SLOT
        {
            chosenSlot.SetItem(item.itemCode, quantity);
            ClearThisSlot();
        }
    }
}
