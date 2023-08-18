using UnityEditor;
using System.Collections.Generic;
using UnityEngine;

public class RemoveMissing:Editor
{
    [MenuItem("MyMenu/SelectMissing")]

    static void SelectMissing(MenuCommand command)
    {
        Transform[] ts = FindObjectsOfType<Transform>();
        List<GameObject> selection = new List<GameObject>();
        foreach (Transform t in ts)
        {
            Component[] cs = t.gameObject.GetComponents<Component>();
            foreach (Component c in cs)
            {
                if (c == null)
                {
                    selection.Add(t.gameObject);
                }
            }
        }
        Selection.objects = selection.ToArray();
    }
}