using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenLibrary : MonoBehaviour
{
    public AnimationCurves[] animationCurve;

   
}

//PURELY JUST A HOLDER FOR THE INSPECTOR
[System.Serializable]
public class AnimationCurves
{
    [Tooltip("This is just for a animation curve name.")]
    public string name; //No purpose. Just a name preview in the inspector.
    [Tooltip("Set the animation curve for this scale preset.")]
    public AnimationCurve animationCurve = AnimationCurve.Linear(0, 1, 1, 0);
}

