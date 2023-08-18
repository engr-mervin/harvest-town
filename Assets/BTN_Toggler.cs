using UnityEngine.EventSystems;
using UnityEngine;

public class BTN_Toggler : MonoBehaviour,IPointerClickHandler
{
    public GameObject[] disable;
    public GameObject[] enable;

    public void OnPointerClick(PointerEventData eventData)
    {
        foreach (GameObject d in disable)
        {
            d.SetActive(false);
        }
        foreach (GameObject e in enable)
        {
            e.SetActive(true);
        }
    }
}
