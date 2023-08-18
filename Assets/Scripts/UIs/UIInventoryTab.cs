using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryTab : MonoBehaviour
{
    public string tabName;

    private void Awake()
    {
        tabName = gameObject.name;
        print(tabName);
    }
}
