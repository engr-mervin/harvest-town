
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BtnInventory : MonoBehaviour, IPointerClickHandler
{
    public GameObject inventory;
    public Animator anim;

    private void Awake()
    {
        anim = inventory.GetComponent<Animator>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!inventory.activeSelf)
        {
            OpenInventory();
        }
        else
        {
            StartCoroutine("CloseInventory");
        }
    }

    public void CloseInventoryFunc()
    {
        StartCoroutine("CloseInventory");
    }

    private IEnumerator CloseInventory()
    {
        anim.SetBool("Shown", false);
        yield return new WaitForSecondsRealtime(1.00f);
        Time.timeScale = 1.0f;
        inventory.SetActive(false);
        yield break;
    }
    public void OpenInventory()
    {
        inventory.SetActive(true);
        anim.SetBool("Shown", true);
        Time.timeScale = 0.0f;
    }
}
