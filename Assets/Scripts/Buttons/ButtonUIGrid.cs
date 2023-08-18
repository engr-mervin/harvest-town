using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ButtonUIGrid
{
    public Vector2[] grid;
    public ButtonUIGrid(Vector2 panelSize,Vector2Int gridSize, float offsetX = 0, float offsetY = 0)
    {
        //panelSize is the percentage size of the panel (like 40% of whole screen width and 30% of whole screen height)
        Vector2 actualPanelSize = new Vector2(panelSize.x * ScreenSize.x, panelSize.y * ScreenSize.y);
        //offset is the center offset in percentage
        Vector2 offset = new Vector2(offsetX * ScreenSize.x, offsetY * ScreenSize.y);
        //the number of spaces (3 buttons have 4 spaces, n buttons have n+1 spaces)
        int spaceX = gridSize.x + 1;
        int spaceY = gridSize.y + 1;
        //The actual spacing
        float actualSpaceX = actualPanelSize.x / spaceX;
        float actualSpaceY = actualPanelSize.y / spaceY;
        //The top left corner of panel
        float left = (-actualPanelSize.x / 2) + offset.x;
        float top = (actualPanelSize.y / 2) + offset.y;

        grid = new Vector2[gridSize.x * gridSize.y];

        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                //distance from top left
                float _x;
                float _y;

                _x = left + (x + 1) * actualSpaceX;
                _y = top - (y + 1) * actualSpaceY;

                grid[x + y * gridSize.x] = new Vector2(_x, _y);
            }
        }
    }
}
