using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class BTN_DecreaseQuantity : MonoBehaviour,IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public QuantitySlide qtySlide;
    bool isDown = false;

    Coroutine last;
    public void OnPointerClick(PointerEventData eventData)
    {
        qtySlide.Decrease();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        isDown = true;
        if (last != null)
        {
            StopCoroutine(last);
        }
        last = StartCoroutine(ContinuousDecrease());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDown = false;
    }

    IEnumerator ContinuousDecrease()
    {
        yield return new WaitForSeconds(0.40f);

        if (isDown == false)
            yield break;

        int i = 0;
        int multiplier = 1;

        while (isDown == true)
        {
            i++;

            if (i % (10 * multiplier) == 0&&i!=0)
            {
                i = 0;
                multiplier *= 2;

            }

            for (int k = 0; k < multiplier; k++)
            {
                qtySlide.Decrease();
            }
            yield return new WaitForSeconds(0.075f);
        }
        yield break;
    }
}
