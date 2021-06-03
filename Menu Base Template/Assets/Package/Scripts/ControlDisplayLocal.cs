using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
[System.Serializable]
//[ExecuteInEditMode]
public class ControlDisplayLocal : MonoBehaviour
{
    public ControlType controlType;

    [Header("Script and Object References")]
    private ControlDisplayController controlDisplayController;
    [Tooltip("Only needed for controller input")]
    public Image controllerImage;
    public TextMeshProUGUI inputText;

    [Space(10)]

    [SerializeField]
    private CanvasGroup canvasGroup;

    [Header("Input Details")]
   [LabelOverride("Current Control")]
    [ReadOnlyInspector]
    public string displayString;
    [ReadOnlyInspector]
    public string controlPath;
    [ReadOnlyInspector]
    public string deviceLayout;
    [SerializeField]
   
    private ControlDisplayDropdown controlDropdown;




    //PRIVATE OR HIDDEN
    
    private bool inEditMode;
    private Image previousImage;


    public enum ControlType
    {
        keyboardAndMouse,
        controller
    }


    // Start is called before the first frame update
    void Awake()
    {
        if (!inEditMode)
        {
            if (gameObject.GetComponent<ControlDisplayDropdown>() == null)
            {
                gameObject.AddComponent<ControlDisplayDropdown>();
            }

            else
            {
                controlDisplayController = FindObjectOfType<ControlDisplayController>();
                if (controlDisplayController.controlIconScripts.Contains(this) == false)
                {
                    controlDisplayController.controlIconScripts.Add(this);
                }
                controlDisplayController.UpdateControlDisplay();
            }

            inputText.text = displayString;

        }
    }

    public void SetState()
    {
        if(!inEditMode && controlDisplayController!=null)
        {
            switch (controlDisplayController.inputType.controlState)
            {
                case InputTypeDetection.ControlState.KeyboardAndMouse:
                    if (controlType == ControlType.keyboardAndMouse)
                    {
                        canvasGroup.alpha = 1;
                    }

                    if (controlType == ControlType.controller)
                    {
                  
                        canvasGroup.alpha = 0;
                    }

                    break;
                case InputTypeDetection.ControlState.Controller:
                    if (controlType == ControlType.keyboardAndMouse)
                    {
                       
                        canvasGroup.alpha = 0;
                    }

                    else if (controlType == ControlType.controller)
                    {
                        
                        canvasGroup.alpha = 1;
                    }
                    break;
                case InputTypeDetection.ControlState.Touch:
                    if (controlType == ControlType.keyboardAndMouse)
                    {
                        canvasGroup.alpha = 0;
                    }

                    else if (controlType == ControlType.controller)
                    {
                        canvasGroup.alpha = 0;
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public void UpdateBindingDisplay()
    {
        controlDropdown.UpdateBindingDisplay();

    }

    public void Update()
    {
        EditModeCheck();
        AddDropdownComponent();

        if (inEditMode == true)
        {
            if (controlType == ControlType.keyboardAndMouse && controllerImage != null)
            {
                previousImage = controllerImage;
                controllerImage = null;
            }

            else if (previousImage != null)
            {
                controllerImage = previousImage;
            }
        }
    }

    public void EditModeCheck()
    {
        if (Application.isPlaying)
        {
            inEditMode = false;
        }

        else
        {
            inEditMode = true;
        }
    }

    public void AddDropdownComponent()
    {
        if(inEditMode)
        {
            if (gameObject.GetComponent<ControlDisplayDropdown>() == null)
            {
                Debug.LogWarning("Couldn't find 'ControlDisplayDropdown' attached to " + gameObject.name + ", attaching now");
                controlDropdown = gameObject.AddComponent<ControlDisplayDropdown>();
            }
        }      
    }

    private void OnEnable()
    {
        controlDisplayController.UpdateControlDisplay();
    }
}

