using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField]
    private LayerMask placeableOn;
   

    public bool CheckLayerMask(int layer)
    {
        if (placeableOn == (placeableOn | (1 << layer)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public bool LayerMaskContainsLayer(int layer)
    {
        return MyFunctions.LayerMaskContainsLayer(placeableOn, layer);
    }

}
