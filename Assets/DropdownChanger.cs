using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//ONLY ON FURNITURE TAB - (2)
public class DropdownChanger : MonoBehaviour
{
    [SerializeField]
    private Dropdown dropdown;
    [SerializeField]
    private GameObject furnitures;

    public Buying shopMaster;

    private void OnEnable()
    {
        dropdown.onValueChanged.AddListener(ValueChanged);
    }

    private void ValueChanged(int i)
    {
        List<GameObject> active = new List<GameObject>();

        if (i==0)
        {
            foreach (BTN_SellableItem t in furnitures.GetComponentsInChildren<BTN_SellableItem>(includeInactive: true))
            {
                Toggle(t, true);
                active.Add(t.gameObject);
            }
            shopMaster.RearrangeActive(active);
            return;
        }

        string folder = dropdown.options[i].text;

        
        foreach (BTN_SellableItem t in furnitures.GetComponentsInChildren<BTN_SellableItem>(includeInactive:true))
        {
            OBJ_ObjectSaveData saveData = t.item.item.GetComponent<ITEM_Furnitures>().furniture.GetComponent<OBJ_ObjectSaveData>();
            if (folder==saveData.baseFolder)
            {
                Toggle(t, true);
                active.Add(t.gameObject);
                continue;
            }
            Toggle(t, false);
        }

        shopMaster.RearrangeActive(active);
    }

    private void Toggle(BTN_SellableItem t,bool a)
    {
        t.gameObject.GetComponent<Image>().enabled = a;
        t.enabled = a;
        t.GetComponentInChildren<Text>().enabled = a;
    }

    private void OnDisable()
    {
        dropdown.onValueChanged.RemoveAllListeners();
        dropdown.value = 0;
    }
}
