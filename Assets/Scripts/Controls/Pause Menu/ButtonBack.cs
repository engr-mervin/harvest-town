using UnityEngine;
using UnityEngine.EventSystems;


public class ButtonBack : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Buttons_PauseMenu pm;

    public void OnPointerClick(PointerEventData eventData)
    {
        pm.Back();
    }
}
