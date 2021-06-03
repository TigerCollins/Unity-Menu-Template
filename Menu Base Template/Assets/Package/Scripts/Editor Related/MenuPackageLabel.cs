
using UnityEngine;
using System.Collections;
using UnityEditor;

//Creates a custom Label on the inspector for all the scripts named ScriptName
// Make sure you have a ScriptName script in your
// project, else this will not work.


public class MenuPackageLabel : Editor
{
    public override void OnInspectorGUI()
    {
        GUILayout.Label("This is a Label in a Custom Editor");
    }
}
