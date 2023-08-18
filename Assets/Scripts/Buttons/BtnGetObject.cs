
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using UnityEngine.Tilemaps;

public class BtnGetObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    //The sprite when it is dragged
    GameObject g;

    int location;

    public ItemGrid ig;

    private Image im;

    public GameObject objectPrefab;

    void Start()
    {
        SetLocation();
        im = GetComponent<Image>();
        im.sprite = objectPrefab.GetComponent<SpriteRenderer>().sprite;

        im.preserveAspect = true;
    }

    private void SetLocation()
    {
        location = int.Parse(this.name); //convert to int
        print(location);
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
        gi.sprite = im.sprite;

    }
    public void OnDrag(PointerEventData eventData)
    {
        g.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(g);

        INV_ObjectSlot[] a = FindObjectsOfType<INV_ObjectSlot>(); // find inventory slot

        if (a == null) return;

        INV_ObjectSlot cis = null;

        foreach (INV_ObjectSlot s in a)
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
                    t.gameObject.GetComponent<Image>().sprite = im.sprite;
                else
                    t.gameObject.AddComponent<Image>().sprite = im.sprite;

                t.GetComponent<Image>().preserveAspect = true;
            }
        }

        cis.tabName = GetComponentInParent<UIInventoryTab>().tabName;
        cis.cg = objectPrefab;
    }

}
