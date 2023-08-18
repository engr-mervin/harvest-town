using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BTN_AddQuantity : MonoBehaviour,IPointerClickHandler,IPointerDownHandler,IPointerUpHandler
{
    public QuantitySlide qtySlide;
    bool isDown;
    Coroutine last;
    public void OnPointerClick(PointerEventData eventData)
    {
        qtySlide.Increase();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDown = true;
        if (last != null)
        {
            StopCoroutine(last);
        }
        last = StartCoroutine(ContinuousIncrease());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDown = false;
    }

    IEnumerator ContinuousIncrease()
    {
        yield return new WaitForSeconds(0.40f);

        if (isDown == false)
            yield break;

        int i = 0;
        int multiplier = 1;

        while (isDown == true)
        {
            i++;

            if (i % (10 * multiplier) == 0 && i != 0)
            {
                i = 0;
                multiplier *= 2;
                print(i + " and " + multiplier);
            }

            for(int k=0;k<multiplier;k++)
            {
                qtySlide.Increase();
            }
            yield return new WaitForSeconds(0.075f);
        }
        yield break;
    }
}
