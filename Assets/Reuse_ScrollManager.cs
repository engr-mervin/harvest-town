using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Reuse_ScrollManager : MonoBehaviour
{
    public int currentNumber = 0;

    public GameObject marker;

    public Reuse_ScrollOptions[] options;

    public Image preview;

    public float left, center, right;


    public void Awake()
    {
        foreach(Reuse_ScrollOptions rso in options)
        {
            rso.mother = this;
        }
    }
}
