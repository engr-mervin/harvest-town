
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonOptions : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Buttons_PauseMenu pm;

    public void OnPointerClick(PointerEventData eventData)
    {
        pm.OptionsButton();
    }
}

