using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BC_ActionOption : MonoBehaviour
{
    public string display;
    public bool isAvailable = true;

    public abstract void ButtonClicked();

}
