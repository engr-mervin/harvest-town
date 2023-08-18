using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrid : MonoBehaviour //replace with button grid
{
    public Vector2Int sizeOfButtons;
    public Vector2Int spacing;
    public Vector2Int numberOfButtons;
    public Vector2Int calculatedSpacing;
    public Vector2 panelSize;
    public Vector2 offset;

    public Vector2 bottomLeft;
    public Vector2 topRight;

    public Vector2Int gridSize;

    public RectTransform openItem;
    public RectTransform closeItem;


    public Vector2[] grid;
    

}
