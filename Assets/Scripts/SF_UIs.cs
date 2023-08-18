using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class SF_UIs //Static Functions
{

    public static void SubscribeAllTCs(List<BC_TouchControls> TCList)
    {
        foreach (BC_TouchControls tc in TCList)
        {
            tc.Subscribe();
        }
    }
    public static Vector2[] CreateVerticalButtonsOneRow(int yButton, float y = 1,float xLoc = 0.50f)
    {
        float menuHeight = (y * ScreenSize.y); // menu height

        float locX = (xLoc-0.50f) * ScreenSize.x; //center X

        float spacing = menuHeight / (yButton);

        float topMost = menuHeight / 2; //top edge


        Vector2[] grid = new Vector2[yButton];

        for (int i = 0; i < grid.Length; i++)
        {
            float _x = locX;
            float _y = (topMost - ((i+0.50f)*spacing));

            grid[i] = new Vector2(_x, _y);
        }

        return grid;

    }
    public static Vector2Int PanelSize(float x, float y)
    {
        int menuHeight = (int)(y * ScreenSize.y); // menu height
        int menuWidth = (int)(x * ScreenSize.x); // menu width

        return new Vector2Int(menuWidth, menuHeight);
    }
    public static Vector2 SetImageSize(Vector2 size)
    {
        return new Vector2(size.x * ScreenSize.x, size.y * ScreenSize.y);
    }
    public static Vector2[] CreateTwoDimButtons(Vector2 panelSize, Vector2Int gridSize, float offsetX = 0, float offsetY = 0) //two dimensional buttons
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

        Vector2[] grid = new Vector2[gridSize.x * gridSize.y];

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

        return grid;
    }
    public static Vector2[] CreateTwoDimButtonContinuousY(Vector2 panelSize, Vector2Int gridSize, float offsetX = 0, float offsetY = 0) //two dimensional buttons
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

        Vector2[] grid = new Vector2[gridSize.x * gridSize.y*10];

        for (int y = 0; y < gridSize.y*10; y++)
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

        return grid;
    }
}
