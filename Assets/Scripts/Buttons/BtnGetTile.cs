
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using UnityEngine.Tilemaps;

public class BtnGetTile : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    //The sprite when it is dragged
    GameObject g;

    int location;

    public ItemGrid ig;

    public Tile tile;

    void Start()
    {
        SetLocation();

        this.GetComponent<Image>().sprite = tile.sprite;
    }

    private void OnDisable()
    {
        Destroy(g);
    }

    private void SetLocation()
    {
        location = int.Parse(this.name); //convert to int
        GetComponent<RectTransform>().localPosition = ig.grid[location];
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Create Empty GameObject
        g = new GameObject();

        //Set Parent to this button
        g.transform.parent = this.transform;

        //Add sprite
        Image gi = g.AddComponent<Image>();
        gi.sprite = tile.sprite;

    }
    public void OnDrag(PointerEventData eventData)
    {
        g.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(g);

        InvTileSlot[] a = FindObjectsOfType<InvTileSlot>(); // find inventory slot

        if (a == null) return;

        InvTileSlot cis=null;

        foreach(InvTileSlot s in a)
        {
            if (!MyFunctions.ScreenPosIsInsideRectTransform(eventData.position, s.GetComponent<RectTransform>(), 1.0f)) //not inside
                continue;
            else//inside
            {
                cis = s;
                break;
            }
        }

        if (cis == null) return;

        foreach(Transform t in cis.gameObject.GetComponentsInChildren<Transform>())
        {
            if (t == cis.transform)
                continue;
            else
            {
                if(t.gameObject.GetComponent<Image>()!=null)
                    t.gameObject.GetComponent<Image>().sprite = tile.sprite;
                else
                    t.gameObject.AddComponent<Image>().sprite = tile.sprite;
            }
        }

        cis.tile = tile;
        
        //if the current inventory slot is the active one
        if(cis = InvTileSlot.active)
        cis.Refresh();
    }

}
