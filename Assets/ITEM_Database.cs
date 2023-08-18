using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ITEM_Database : MonoBehaviour
{
    public BC_Items[] rawDatabase;

    [System.Serializable]
    public struct ItemData
    {
        public string itemCode;
        public BC_Items item;
    }

    public List<ItemData> itemDatabase = new List<ItemData>();

    public GameObject GetObject(string code)
    {
        return GameGod.gameLoader.itemIndexer.itemDatabase.Find(x => x.itemCode == code).item.gameObject;
    }
    public bool TryGetObject(string code,out GameObject item)
    {
        if (GameGod.gameLoader.itemIndexer.itemDatabase.Find(x => x.itemCode == code).item.gameObject == null)
        {
            item = null;
            return false;
        }

        item = GameGod.gameLoader.itemIndexer.itemDatabase.Find(x => x.itemCode == code).item.gameObject;
        return true;
    }
}
