using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ControllerScrollSnap : MonoBehaviour
{
    /// <summary>
    /// This script is made by Tiger Collins.
    /// https://tigercollins.myportfolio.com/
    /// https://twitter.com/_TigerCollins
    /// 
    /// Some aspects of this script are from the following;
    /// https://stackoverflow.com/questions/30766020/how-to-scroll-to-a-specific-element-in-scrollrect-with-unity-ui
    /// </summary>

    /// <summary>
    /// VARIABLES SEEN BY THE INSPECTOR
    /// </summary>

    [Header("UI TEMPLATE - SNAP SCROLL")]

    [Space(25)]

    [SerializeField]
    private InputTypeDetection inputTypeDetectionScript;
    [SerializeField]
    private ScrollRect scrollRect;
    [SerializeField]
    private RectTransform contentPanel;

    [Space(10)]

    [Tooltip("This resolves offset issues. Select if the scroll group moves vertically or horizontally")]
    public Orientation scrollOrientation = Orientation.horizontal;
    [SerializeField]
    private float scrollOffset;

    /// <summary>
    /// VARIABLES NOT SEEN BY INSPECTOR
    /// </summary>

    private float scrollRectCentreX;
    private float scrollRectCentreY;
    private float xAxis;
    private float yAxis;

    private float scrollOffsetX;
    private float scrollOffsetY;

    /// <summary>
    /// Everything within Awake is only to find references
    /// and/or inform the user of missing references.
    /// </summary>

    public enum Orientation
    {
        vertical,
        horizontal
    }

   

    public void Awake()
    {
        if (inputTypeDetectionScript == null)
        {
            inputTypeDetectionScript = FindObjectOfType<InputTypeDetection>();
            if (inputTypeDetectionScript == null)
            {
                Debug.LogError("Could not find InputTypeDetection. Add to scene to resolve null reference.");
            }
        }

        if(scrollRect==null)
        {
            Debug.LogError("Could not find the Scroll Rect. This script will not work unless assigned.");
        }
        
        if(contentPanel==null)
        {
            Debug.LogError("Could not find the Content Panel. This script will not work unless assigned.");
        }
    }


    /// <summary>
    /// Force updates the canvas and makes sure the gameobjects with the scroll rect
    /// are centred. 
    /// 
    /// This should be called using the Unity Event for OnEnter as part 
    /// of <see cref="UIElementSelected"/> or your own script. 
    /// </summary>
    public void SnapTo(RectTransform target)
    {
        if(inputTypeDetectionScript !=null && scrollRect != null && contentPanel !=null)
        {
            if (inputTypeDetectionScript.controlState == InputTypeDetection.ControlState.Controller || inputTypeDetectionScript.controlSchemeVisual.keyboardMostRecently)
            {
                UpdateCanvas();
                switch (scrollOrientation)
                {
                    case Orientation.horizontal:

                        contentPanel.anchoredPosition =
               (Vector2)scrollRect.transform.InverseTransformPoint(new Vector2(contentPanel.position.x + xAxis - scrollOffsetX, scrollRect.transform.position.y))
               - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);
                        break;

                    case Orientation.vertical:
                        contentPanel.anchoredPosition =
               (Vector2)scrollRect.transform.InverseTransformPoint(new Vector2(scrollRect.transform.position.x, (contentPanel.transform.position.y + yAxis) - scrollOffsetY))
               - (Vector2)scrollRect.transform.InverseTransformPoint(new Vector2(scrollRect.transform.position.x, target.position.y));
                        break;
                    default:
                        break;
                }

                
            }
        }

    }

    /// <summary>
    /// Force updates the canvas and makes sure the gameobjects with the scroll rect
    /// are centred. 
    /// 
    /// This should be called using the Unity Event for OnEnter as part 
    /// of <see cref="UIElementSelected"/> or your own script. 
    /// </summary>
    void UpdateCanvas()
    {
        Canvas.ForceUpdateCanvases();
        switch (scrollOrientation)
        {
            case Orientation.horizontal:

                scrollRectCentreY = contentPanel.rect.center.y;
                yAxis = scrollRectCentreY;
                xAxis = 0;
                scrollOffsetX = scrollOffset; 
                scrollOffsetY = 0;
                break;
            case Orientation.vertical:
                scrollRectCentreX = contentPanel.rect.center.x;
                xAxis = scrollRectCentreX;
                yAxis = 0;
                scrollOffsetX = 0;
                scrollOffsetY = scrollOffset;
                break;
            default:
                break;
        }

    }

}

/// <summary>
/// Following is setup as a class here for inspector presentation
/// </summary>
[System.Serializable]
public class Padding
{
    public float xScolAxis;
    public float yAxis;
}
