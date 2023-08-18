
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using UnityEngine.Tilemaps;

public class B_Inv_Get_Wall : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    //The sprite when it is dragged
    GameObject g;

    int location;

    public ItemGrid ig;

    public GameObject wallset;

    void Start()
    {
        SetLocation();

        this.GetComponent<Image>().sprite = wallset.GetComponent<WallSet>().face.sprite;
    }

    private void OnDisable()
    {
        Destroy(g);
    }

    private void SetLocation()
    {
        location = int.Parse(this.gameObject.name); //convert to int
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
        gi.sprite = wallset.GetComponent<WallSet>().face.sprite;

    }
    public void OnDrag(PointerEventData eventData)
    {
        g.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(g);

        InvWallSlot[] a = FindObjectsOfType<InvWallSlot>(); // find inventory slot

        if (a == null) return;

        InvWallSlot cis = null;

        foreach (InvWallSlot s in a)
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

        foreach (Transform t in cis.gameObject.GetComponentsInChildren<Transform>())
        {
            if (t == cis.transform)
                continue;
            else
            {
                if (t.gameObject.GetComponent<Image>() != null)
                    t.gameObject.GetComponent<Image>().sprite = wallset.GetComponent<WallSet>().face.sprite;
                else
                    t.gameObject.AddComponent<Image>().sprite = wallset.GetComponent<WallSet>().face.sprite;
            }
        }

        cis.wallset = wallset;

        //if the current inventory slot is the active one
        if (cis = InvWallSlot.active)
            cis.Refresh();
    }

}
