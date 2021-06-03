using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

/// <summary>
/// CODE BY TIGER COLLINS https://tigercollins.myportfolio.com/ , 
/// https://twitter.com/_TigerCollins , https://www.linkedin.com/in/tiger-collins-2b8619120/
/// Feel free to modify or expand upon this script. Please credit myself if you redistribute this
/// script or use this asset in a project.
/// </summary>

/// <summary>
/// This script handles control for <see cref="Toggle"></see> within <see cref="ToggleGroup"></see> that 
/// control <see cref="CanvasGroup"></see>. The idea is that canvas groups hold sub menus and this scripts 
/// scrolls through it with looping. This script also tracks the toggles when selected by a mouse.
/// </summary>
public class MouselessToggleControl : MonoBehaviour
{
    [Header("MENU TEMPLATE - Choose which options menu to open")]
    [SerializeField]
    [Multiline(2)]
    private string componentPurpose = "NOTE - This is only for the inspector to avoid confusion between components";
    [Space(35)]

    /// <summary>
    /// Visible to the inspector.
    /// </summary>
    [SerializeField]
    private GameObject toggleHolder;
    [ReadOnlyInspector]
    [SerializeField]
    private int toggleValue;
    [SerializeField]
    private bool loopToggleGroup;

    /// <summary>
    /// Invisible to the inspector.
    /// </summary>
   /// [HideInInspector]
    public List<Toggle> selectableToggles = new List<Toggle>();
    private int selectableToggleCount;


    /// <summary>
    /// Everything in <see cref="Awake"></see> is to ensure that <see cref="InputTypeDetection"></see> is found
    /// and only objects that are active or have a <see cref="Toggle"></see> attached, are not tracked in the
    /// for selecting.
    /// </summary>
    void Awake()
    {
        for (int i = 0; i < toggleHolder.transform.childCount; i++)
        {
            if(toggleHolder.transform.GetChild(i).gameObject.activeSelf)
            {
                GameObject possibleToggle = toggleHolder.transform.GetChild(i).gameObject;
                if (possibleToggle.TryGetComponent(out Toggle toggle))
                {
                    selectableToggles.Add(toggle);
                }
            } 
        }  
        selectableToggleCount = selectableToggles.Count - 1; ;
    }

    /// <summary>
    /// This functions is designed with the new <see cref="InputSystem"></see> in mind. The functions
    /// <see cref="ReceiveInputDecrease(InputAction.CallbackContext)"></see> and
    /// <see cref="ReceiveInputIncrease(InputAction.CallbackContext)"></see> are used to call this function.
    /// If you wish to not use the new Input System or the <see cref="PlayerInput"></see> component, you can 
    /// just call this function directly from a script the controls input with a true or false argument
    /// that determines if the next selected item is up (false) or down (true) the list (from the perspective 
    /// of the Unity Inspector).
    /// 
    /// The looping logic is handled by the <see cref="CurrentToggleInt"></see> so the int can be changed
    /// from other lines/functions and not have to run through <see cref="MenuControl(bool)"></see> or copy
    /// the logic from said function.
    /// </summary>
    public void MenuControl(bool isPositive)
    {
        if (isPositive)
        {
            CurrentToggleInt++;
        }

        else
        {
            CurrentToggleInt--;
        }
        selectableToggles[CurrentToggleInt].isOn = true;
    }

    public int CurrentToggleInt
    {
        get
        {
            return toggleValue;
        }
        set
        {
            if (value > selectableToggleCount && loopToggleGroup)
            {
                toggleValue = 0;
            }

            else if(value < 0 && loopToggleGroup)
            {
                toggleValue = selectableToggleCount;
            }

            else if(!loopToggleGroup && value < 0)
            {
                toggleValue = 0;
            }

            else if(!loopToggleGroup && value > selectableToggleCount)
            {
                toggleValue = selectableToggleCount;
            }

            else
            {
                toggleValue = value;
            }
        }
    }

    /// <summary>
    /// The functions <see cref="ReceiveInputDecrease(InputAction.CallbackContext)"></see> and
    /// <see cref="ReceiveInputIncrease(InputAction.CallbackContext)"></see> are used for different 
    /// arguments that call <see cref="MenuControl(bool)"></see> and they take into account the bugs 
    /// with <see cref="InputSystem"></see> as of ver. 1.0.2 and prior.
    /// 
    /// INPUT SYSTEM BUG: calling a function at the press, hold and release states instead of the 
    /// desired. Currently, this is resolved by doing the If statement that is within the fucntion.
    /// The following represents the different states of the input;
    /// --<see cref="InputAction.CallbackContext.performed"></see> = Press.
    /// --<see cref="InputAction.CallbackContext.canceled"></see> = Release.
    /// Change the callbackContext bool below to change to a press/release state.
    /// </summary>
    public void ReceiveInputIncrease(InputAction.CallbackContext callbackContext)
    {
        if(callbackContext.performed)
        {
            MenuControl(true);
        }
    }
    public void ReceiveInputDecrease(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            MenuControl(false);
        }
    }

    /// <summary>
    /// This function does not change which bool is on but simply tracks it for when a mouse press or 
    /// other input methods invoke <see cref="Toggle.onValueChanged"></see>. This is done so if a mouse
    /// or other input method is used, a player can swap to a keyboard or controller and not have 
    /// </summary>
    public void OnPointerClick(Toggle toggle)
    {
        if(toggle.isOn && selectableToggles != null)
        {
            int index = 0;
            foreach (Toggle toggleItem in selectableToggles)
            {
                if(toggle == toggleItem)
                {
                    CurrentToggleInt = index;
                    return;
                }
                index++;
            }
        }
    }
}
