using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class B_Build_Wall : MonoBehaviour, IPointerClickHandler //B is for buttons
{
    public TC_Build tc;
    public Sprite build;
    public Sprite closeBuild;
    public GameObject[] toggle;
    public void OnPointerClick(PointerEventData eventData)
    {
        MyFunctions.ToggleBool(ref tc.buildWallMode);

        if(tc.buildWallMode)
        {
            GetComponent<Image>().sprite = build;
            foreach(STR_GridObject gm in tc.wallsMarker)
            {
                gm.g.SetActive(true);
            }
            MyFunctions.ToggleObjects(toggle);

            S_Tilemap.walls.GetComponent<TilemapRenderer>().enabled = false;
        }
        else
        {
            GetComponent<Image>().sprite = closeBuild;
            foreach (STR_GridObject gm in tc.wallsMarker)
            {
                gm.g.SetActive(false);
            }
            MyFunctions.ToggleObjects(toggle);

            S_Tilemap.walls.GetComponent<TilemapRenderer>().enabled = true;
        }
    }
}
