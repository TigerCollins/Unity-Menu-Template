using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIElementSelected : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField]
    private EventSystem eventSystem;
    [SerializeField]
    [Tooltip("Only needed if UI Icons change at runtime")]
    private InputTypeDetection inputDetection;
    [SerializeField]
    private bool isSelected;
    [SerializeField]
    private bool isHovered;
    

    [Header("Visual Options")]
    [SerializeField]
    private bool canFade;
    [SerializeField]
    [Range(.1f, 2f)]
    private float fadeDuration;
    [SerializeField]
    [Tooltip("Used when selected.")]
    private MainColourChoice mainColourMode;
    [SerializeField]
    [Tooltip("Used when not selected.")]
    private SecondColourChoice secondColourMode;
    [SerializeField]
    private ColourPresets colourOptions;
    [SerializeField]
    private Image selectedIcon;

    [Header("Icon Changer")]
    [SerializeField]
    private bool showWhenMouseActive;
    public OtherObjectControl otherObjectControl;
    public ToggleGroupSettings toggleGroupSettings;
   
    
    [Space(5)]

    public UnityEvents unityEvents;


    //PRIVATE
    private bool previousSelectState;
    private bool previousHoverState;
    private int previousMainEnumInt;
    private int previousSecondEnumInt;
    private Coroutine colourCoroutine;

    public enum SecondColourChoice
    {
        [InspectorName("No Colour (Invisible)")]
        invisible,
        [InspectorName("Use the primary colour")]
        primary,
        [InspectorName("Use the secondary colour")]
        secondary,
        [InspectorName("Use a random Other Colour")]
        random

    }
    public enum MainColourChoice
    {
        [InspectorName("No Colour (Invisible)")]
        invisible,
        [InspectorName("Use the primary colour")]
        primary,
        [InspectorName("Use the secondary colour")]
        secondary,
        [InspectorName("Use a random Other Colour")]
        random
    }

    public void Awake()
    {
        if (eventSystem == null)
        {
            eventSystem = FindObjectOfType<EventSystem>();
        }

        if (inputDetection == null)
        {
            inputDetection = FindObjectOfType<InputTypeDetection>();
        }

        else
        {
            inputDetection.onControlSchemeChange.AddListener(OnControlSchemeChange);
            inputDetection.keyboardChange.AddListener(OnControlSchemeChange);
            inputDetection.mouseChange.AddListener(OnControlSchemeChange);
        }

        if(toggleGroupSettings.attachedToggle ==null && TryGetComponent(out Toggle newToggle))
        {
            toggleGroupSettings.attachedToggle = newToggle;
        }

     
        IsSelectedCheck();
        SetMainColour();
        SetSecondColour();
        OnControlSchemeChange();
       
        previousMainEnumInt = (int)mainColourMode;
        previousSecondEnumInt = (int)secondColourMode;

        if (TryGetComponent(out Toggle toggle))
        {
            toggleGroupSettings.attachedToggle = toggle;
            CheckIsOn();
        }


    }

    void Start()
    {
       

    }

    void Update()
    {
        CheckBoolState();
        CheckColourEnum();
        IsSelectedCheck();


     
    }

    public void IsSelectedCheck()
    {
        if (eventSystem.currentSelectedGameObject == gameObject && !isSelected)
        {
            isSelected = true;
        }

        else if (eventSystem.currentSelectedGameObject != gameObject && isSelected)
        {
            isSelected = false;
        }
    }

    public void OnControlSchemeChange()
    {

        if (otherObjectControl.adjustIcon && isSelected && toggleGroupSettings.affectedByInputChange)
        {
            switch (inputDetection.controlState)
            {
                case InputTypeDetection.ControlState.KeyboardAndMouse:
                    if (inputDetection.controlSchemeVisual.keyboardMostRecently)
                    {
                        if (otherObjectControl.showWhenUsing.withKeyboard == true )
                        {
                            for (int i = 0; i < otherObjectControl.otherObjects.Length; i++)
                            {
                                if (otherObjectControl.otherObjects[i] != null)
                                {
                                    otherObjectControl.otherObjects[i].SetActive(true);
                                }
                            }
                        }

                        else
                        {
                            for (int i = 0; i < otherObjectControl.otherObjects.Length; i++)
                            {
                                if (otherObjectControl.otherObjects[i] != null)
                                {
                                    otherObjectControl.otherObjects[i].SetActive(false);
                                }
                            }
                        }
                    }

                    else if (inputDetection.controlSchemeVisual.mouseMostRecently)
                    {
                        if (otherObjectControl.showWhenUsing.withMouse == true)
                        {
                            for (int i = 0; i < otherObjectControl.otherObjects.Length; i++)
                            {
                                if (otherObjectControl.otherObjects[i] != null)
                                {
                                    otherObjectControl.otherObjects[i].SetActive(true);
                                }
                            }
                        }

                        else
                        {
                            for (int i = 0; i < otherObjectControl.otherObjects.Length; i++)
                            {
                                if (otherObjectControl.otherObjects[i] != null)
                                {
                                    otherObjectControl.otherObjects[i].SetActive(false);
                                }
                            }
                        }

                    }



                    break;
                case InputTypeDetection.ControlState.Controller:
                    if (otherObjectControl.showWhenUsing.withController)
                    {
                        for (int i = 0; i < otherObjectControl.otherObjects.Length; i++)
                        {
                            if (otherObjectControl.otherObjects[i] != null)
                            {
                                otherObjectControl.otherObjects[i].SetActive(true);
                            }
                        }
                    }

                    else
                    {
                        for (int i = 0; i < otherObjectControl.otherObjects.Length; i++)
                        {
                            if (otherObjectControl.otherObjects[i] != null)
                            {
                                otherObjectControl.otherObjects[i].SetActive(false);
                            }
                        }
                    }

                    break;
                case InputTypeDetection.ControlState.Touch:
                    if (otherObjectControl.showWhenUsing.withTouch)
                    {
                        for (int i = 0; i < otherObjectControl.otherObjects.Length; i++)
                        {
                            if (otherObjectControl.otherObjects[i] != null)
                            {
                                otherObjectControl.otherObjects[i].SetActive(true);
                            }
                        }
                    }

                    else
                    {
                        for (int i = 0; i < otherObjectControl.otherObjects.Length; i++)
                        {
                            if (otherObjectControl.otherObjects[i] != null)
                            {
                                otherObjectControl.otherObjects[i].SetActive(false);
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        else if (otherObjectControl.adjustIcon && !isSelected && toggleGroupSettings.affectedByInputChange)
        {
            switch (inputDetection.controlState)
            {
                case InputTypeDetection.ControlState.KeyboardAndMouse:
                    for (int i = 0; i < otherObjectControl.otherObjects.Length; i++)
                    {
                        if (otherObjectControl.otherObjects[i] != null)
                        {
                            otherObjectControl.otherObjects[i].SetActive(false);
                        }
                    }
                    break;
                case InputTypeDetection.ControlState.Controller:

                    for (int i = 0; i < otherObjectControl.otherObjects.Length; i++)
                    {
                        if (otherObjectControl.otherObjects[i] != null)
                        {
                            otherObjectControl.otherObjects[i].SetActive(false);
                        }
                    }
                    break;
                case InputTypeDetection.ControlState.Touch:

                    for (int i = 0; i < otherObjectControl.otherObjects.Length; i++)
                    {
                        if (otherObjectControl.otherObjects[i] != null)
                        {
                            otherObjectControl.otherObjects[i].SetActive(false);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public void CheckIsOn()
    {
        
        if(toggleGroupSettings.attachedToggle != null && otherObjectControl.adjustIcon)
        {
            if(toggleGroupSettings.attachedToggle.isOn)
            {
                for (int i = 0; i < otherObjectControl.otherObjects.Length; i++)
                {
                    if (otherObjectControl.otherObjects[i] != null)
                    {
                        otherObjectControl.otherObjects[i].SetActive(true);
                    }
                }
            }

            else
            {
                for (int i = 0; i < otherObjectControl.otherObjects.Length; i++)
                {
                    if (otherObjectControl.otherObjects[i] != null)
                    {
                        otherObjectControl.otherObjects[i].SetActive(false);
                    }
                }
            }
        }

        else if(toggleGroupSettings.attachedToggle == null)
        {
            Debug.LogError("Could not find Toggle. Please resolve null reference.");
        }

        else if(!otherObjectControl.adjustIcon)
        {
            Debug.LogWarning("Cannot change other objects please allow for icons to be changed by changing 'Can Changing Other Objects'(toggleGroupSettings.attachedToggle) to true.");

        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        SetMainColour();
       
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
 
                SetSecondColour();
    }

    void CheckBoolState()
    {
        if(isSelected != previousSelectState)
        {
            if(isSelected == true)
            {
                unityEvents.onSelectEnter.Invoke();
                SetMainColour();
                OnControlSchemeChange();
            }

            else
            {

                unityEvents.onSelectExit.Invoke();
                SetSecondColour();
                OnControlSchemeChange(); 
    
            }
            previousSelectState = isSelected;
        }

        if(isHovered != previousHoverState)
        {
            if(!isSelected)
            {
                if (isHovered)
                {
                    SetMainColour();
                    unityEvents.onHoverEnter.Invoke();
                }

                else
                {
                    SetSecondColour();
                    unityEvents.onHoverExit.Invoke();
                }
            }
            
           
            previousHoverState = isHovered;
        }
    }

    void CheckColourEnum()
    {
        if((int)mainColourMode != previousMainEnumInt)
        {
            unityEvents.onColourModeChange.Invoke();
            previousMainEnumInt = (int)mainColourMode;
        }
         if((int)secondColourMode != previousSecondEnumInt)
        {
            unityEvents.onColourModeChange.Invoke();
            previousSecondEnumInt = (int)secondColourMode;
        }

    }

    public void SetMainColour()
    {
        if(isSelected || isHovered)
        {
            switch (mainColourMode)
            {
                case MainColourChoice.invisible:
                    if (colourCoroutine != null)
                    {
                        StopCoroutine(colourCoroutine);
                    }
                    colourCoroutine = StartCoroutine(LerpColour(canFade, colourOptions.invisibleInk, fadeDuration));
                    break;
                case MainColourChoice.primary:
                    if (colourCoroutine != null)
                    {
                        StopCoroutine(colourCoroutine);
                    }
                    colourCoroutine = StartCoroutine(LerpColour(canFade, colourOptions.primaryColour, fadeDuration));
                    break;
                case MainColourChoice.secondary:
                    if (colourCoroutine != null)
                    {
                        StopCoroutine(colourCoroutine);
                    }
                    colourCoroutine = StartCoroutine(LerpColour(canFade, colourOptions.secondaryColour, fadeDuration));
                    break;
                case MainColourChoice.random:
                    if (colourOptions.otherColours.Length > 0)
                    {
                        if (colourCoroutine != null)
                        {
                            StopCoroutine(colourCoroutine);
                        }
                        colourCoroutine = StartCoroutine(LerpColour(canFade, colourOptions.otherColours[Random.Range(0, colourOptions.otherColours.Length)], fadeDuration));
                    }
                    else
                    {
                        Debug.LogError("Couldn't find any colours with other colours, using primary colour instead...");
                        selectedIcon.color = colourOptions.primaryColour;
                    }
                    break;
                default:
                    break;
            }
        }
      
    }
   

    public void SetSecondColour()
    {
        if (!isSelected && !isHovered)
        {
           
            switch (secondColourMode)
            {
                
                case SecondColourChoice.invisible:
                    if (colourCoroutine != null)
                    {
                        StopCoroutine(colourCoroutine);
                    }
                    colourCoroutine = StartCoroutine(LerpColour(canFade, colourOptions.invisibleInk, fadeDuration));
                    break;
                case SecondColourChoice.primary:
                    if (colourCoroutine != null)
                    {
                        StopCoroutine(colourCoroutine);
                    }
                    colourCoroutine = StartCoroutine(LerpColour(canFade, colourOptions.primaryColour, fadeDuration));
                    break;
                case SecondColourChoice.secondary:
                    if (colourCoroutine != null)
                    {
                        StopCoroutine(colourCoroutine);
                    }
                    colourCoroutine = StartCoroutine(LerpColour(canFade, colourOptions.secondaryColour, fadeDuration));
                    break;
                case SecondColourChoice.random:
                    if (colourOptions.otherColours.Length > 0)
                    {
                        if (colourCoroutine != null)
                        {
                            StopCoroutine(colourCoroutine);
                        }
                        colourCoroutine =  StartCoroutine(LerpColour(canFade, colourOptions.otherColours[Random.Range(0, colourOptions.otherColours.Length)], fadeDuration));
                    }
                    else
                    {
                        if (colourCoroutine != null)
                        {
                            StopCoroutine(colourCoroutine);
                        }
                        Debug.LogError("Couldn't find any colours with other colours, using invisible colour instead...");
                        colourCoroutine = StartCoroutine(LerpColour(canFade, colourOptions.invisibleInk, fadeDuration));
                    }
                    break;
                default:
                    break;
            }
        }

    }

    public void OnDisable()
    {
        if(selectedIcon!=null)
        {
            selectedIcon.color = colourOptions.invisibleInk;

        }
    }

    public IEnumerator LerpColour(bool canFade, Color targetColour, float fadeTime)
    {
        if(selectedIcon!=null)
        {
            if (canFade)
            {
                float elapsedTime = 0;
                while (elapsedTime <= fadeTime) //If the timer is still going
                {
                    elapsedTime += Time.deltaTime;
                    selectedIcon.color = Color.Lerp(selectedIcon.color, targetColour, elapsedTime / fadeTime);
                    yield return null;
                }
            }

            else
            {
                selectedIcon.color = targetColour;
                if (colourCoroutine != null)
                {
                    StopCoroutine(colourCoroutine);
                }

            }
        }
       
    }
}

[System.Serializable]
public class ColourPresets
{
    public Color primaryColour;
    public Color secondaryColour;
    [ReadOnlyInspector]
    public Color invisibleInk = new Color(0, 0, 0, 0);
    [Tooltip("This is used for the random colour preset")]
    public Color[] otherColours;
}

[System.Serializable]
public class UnityEvents
{

    [Header("For when a function is called from hovering.")]
    public UnityEvent onHoverEnter;  
    [Header("For when a function is called when hovering stops.")]
    public UnityEvent onHoverExit;
    [Header("Called when this script is no longer selected.")]
    public UnityEvent onSelectEnter;
    [Header("Called when this script is no longer selected.")]
    public UnityEvent onSelectExit;
    [Header("Called when Primary and Secondary modes are changed at runtime.")]
    public UnityEvent onColourModeChange;
}

[System.Serializable]
public class OtherObjectControl
{
    [LabelOverride("Can Adjust Other Objects")]
    public bool adjustIcon;
    public GameObject[] otherObjects;

    public OtherObjectOptions showWhenUsing;

}

[System.Serializable]
public class OtherObjectOptions
{
    [LabelOverride("Touch")]
    public bool withTouch;
    [LabelOverride("Controller")]
    public bool withController;
    [LabelOverride("Mouse")]
    public bool withMouse;
    [LabelOverride("Keyboard")]
    public bool withKeyboard;
}

[System.Serializable]
public class ToggleGroupSettings
{

    public bool isInToggleGroup;
    public bool affectedByInputChange;
    public Toggle attachedToggle;

}



