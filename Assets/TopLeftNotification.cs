using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TopLeftNotification : MonoBehaviour
{
    public TMP_Text text;
    public Coroutine current;

    public void SetNotif(string notif)
    {
        text.text = notif;

        if (current != null)
            StopCoroutine(current);

        current = StartCoroutine(Beat());
    }

    private IEnumerator Beat()
    {
        for(int i = 1;i<=255;i+=2)
        {
            Color color = text.color;
            color.a = ((float)(i) / 255);
            text.color = color;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSecondsRealtime(1f);

        for (int i = 254; i >= 0; i -= 2)
        {
            Color color = text.color;
            color.a = ((float)(i)/255);
            text.color = color;
            yield return new WaitForEndOfFrame();
        }
        yield break;
    }
}
