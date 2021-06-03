using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerSnapInstantiated : MonoBehaviour
{
    public ControllerScrollSnap scrollSnap;
    [ReadOnlyInspector]
    public ControllerScrollSnap.Orientation orientation;
    public Toggle toggleParent;


    public bool useStringToFindObject;
    public string targetGameObjectName;

    private GameObject foundObject;

    // Start is called before the first frame update
    void Awake()
    {
        if(useStringToFindObject)
        {
            foundObject = GameObject.Find(targetGameObjectName);
            if(foundObject != null)
            {
                if(foundObject.TryGetComponent(out ControllerScrollSnap localScrollSnap))
                {
                    scrollSnap = localScrollSnap;
                }
            }

            else
            {
                Debug.LogError("Could not find GameObject with the name " + targetGameObjectName);
            }
        }

        else if(toggleParent != null)
        {
            if (toggleParent.transform.parent.TryGetComponent(out ControllerScrollSnap localControlSnap))
            {
                scrollSnap = localControlSnap;
                orientation = localControlSnap.scrollOrientation;
            }
        }

        else
        {
            Debug.LogError("Could not find ResolutionToggle script for ControlleSnapInstantiated. Please assign variable");

        }
    }

    public void CallSnapTo()
    {
        if(scrollSnap !=null && toggleParent != null )
        {
            if(toggleParent.TryGetComponent(out RectTransform rectTransform))
            {
                scrollSnap.SnapTo(rectTransform);

            }

        }
    }
}
