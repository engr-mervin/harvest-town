using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsCanvas : MonoBehaviour
{
    private void OnEnable()
    {
        Options.instance.OnCanvasEnable();
    }
}
