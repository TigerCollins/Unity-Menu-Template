using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;


public class InputTypeDetection : MonoBehaviour
{
    //PUBLIC OR SEEN IN INSPECTOR
    [Header("MENU TEMPLATE - INDEPENDENT SCRIPT")]
    [ReadOnlyInspector]

    [Space(20)]

    [LabelOverride("Current Control Scheme")]
    public ControlState controlState;

    [SerializeField]
    private PlayerInput playerInputModule;
    [Space(5)]
    public CurrentControlInput controlSchemeVisual;

    [Space(10)]

    public UnityEvent onControlSchemeChange;

    //PRIVATE OR UNSEEN IN INSPECTOR
    private string previousControlScheme;
    [HideInInspector]
    public InputDevice inputControl;
    private bool previousKeyboardState;
    [HideInInspector]
    public UnityEvent keyboardChange;
    private bool previousMouseState;
    [HideInInspector]
    public UnityEvent mouseChange;
    private bool mouseNotKeyboard;

    public void Awake()
    {
        if (Gamepad.current != null)
        {
            inputControl = Gamepad.current;
        }

        if (Keyboard.current != null)
        {
            inputControl = Keyboard.current;
            controlSchemeVisual.keyboardMostRecently = true;
            mouseNotKeyboard = false;
        }

        if (Mouse.current != null)
        {
            inputControl = Mouse.current;
        }
    }

    //Displayed as "Current Control Scheme" in the inspector
    public enum ControlState
    {
        KeyboardAndMouse,
        Controller,
        Touch
    }

    public String currentControlScheme
    {
        get
        {
            return playerInputModule.currentControlScheme;
        }

    }

    public void InputDeviceFunction()
    {
        if (Gamepad.current != null)
        {
            if (Gamepad.current.wasUpdatedThisFrame)
            {
                inputControl = Gamepad.current;
            }
        }

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wasUpdatedThisFrame)
            {
                inputControl = Keyboard.current;
            }
        }

        if (Mouse.current != null)
        {
            if (Mouse.current.wasUpdatedThisFrame)
            {
                inputControl = Mouse.current;
            }
        }
        ////////// Debug.Log(inputControl.layout);
    }

    // Update is called once per frame
    void Update()
    {
        InputDeviceFunction();
        TrackControlScheme();
        KeyboardMouseCheck();
    }

    /// <summary>
    /// So the unity event can take affect, this checks every frame for what the current control scheme is and then invokes the event.
    /// 
    /// A unity event was chosen over just leaving all of the methods in Update() so that it can be customised by the developer without 
    /// opening code they're not familiar with.
    /// 
    /// This runs in update so that it can check the current control scheme.
    /// </summary>

    void TrackControlScheme()
    {
        if (currentControlScheme != previousControlScheme)
        {
            ChangeControlBool(); // Changes enum to reflect change
            onControlSchemeChange.Invoke(); //Invokes unity event
            previousControlScheme = currentControlScheme;
        }
    }

    public void KeyboardMouseCheck()
    {
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wasUpdatedThisFrame != previousKeyboardState && mouseNotKeyboard != false)
            {
                previousKeyboardState = Keyboard.current.wasUpdatedThisFrame;
                keyboardChange.Invoke();
                onControlSchemeChange.Invoke();
                controlSchemeVisual.keyboardMostRecently = true;
                controlSchemeVisual.mouseMostRecently = false;
                mouseNotKeyboard = false;
            }
        }

        if (Mouse.current != null)
        {
            if (Mouse.current.wasUpdatedThisFrame != previousMouseState && mouseNotKeyboard != true)
            {
                previousMouseState = Mouse.current.wasUpdatedThisFrame;
                mouseChange.Invoke();
                onControlSchemeChange.Invoke();
                controlSchemeVisual.keyboardMostRecently = false;
                controlSchemeVisual.mouseMostRecently = true;
                mouseNotKeyboard = true;
            }
        }
    }

    /// <summary>
    /// This updates the control scheme enum. 
    /// </summary>
    public void ChangeControlBool()
    {
        if (currentControlScheme == "Gamepad")
        {
            controlState = ControlState.Controller;
        }

        else if (currentControlScheme == "Keyboard&Mouse")
        {
            controlState = ControlState.KeyboardAndMouse;
        }

        else
        {
            controlState = ControlState.Touch;
        }

    }

    /// <summary>
    /// Unless the developer wants to check the bools instead of the enum, this is for display in the inspector
    /// 
    /// If the control scheme bools are not being changed, make sure this function is being called in 
    /// the unity event in the inspector attached to this script. Alternatively, it can be placed within 
    /// the TrackControlScheme() function within its if statement.
    /// </summary>
    public void ControlBoolSwitch()
    {
        switch (controlState)
        {
            //Effectively an if statement for 'if using a keyboard/mouse'
            case ControlState.KeyboardAndMouse:
                controlSchemeVisual.keyboardmouseInput = true;
                controlSchemeVisual.touchInput = false;
                controlSchemeVisual.controllerInput.xboxController = false;
                controlSchemeVisual.controllerInput.gamepadController = false;
                if (Keyboard.current != null)
                {
                    if (Keyboard.current.wasUpdatedThisFrame)
                    {
                        controlSchemeVisual.keyboardMostRecently = true;
                        controlSchemeVisual.mouseMostRecently = false;
                    }
                }

                if (Mouse.current != null)
                {
                    if (Mouse.current.wasUpdatedThisFrame)
                    {
                        controlSchemeVisual.keyboardMostRecently = false;
                        controlSchemeVisual.mouseMostRecently = true;
                    }
                }

                break;

            //Effectively an if statement for 'if using a controller'
            case ControlState.Controller:
                controlSchemeVisual.keyboardmouseInput = false;
                controlSchemeVisual.touchInput = false;
                //Specifically for xbox controllers
                if (inputControl.layout == "XInputControllerWindows")
                {
                    controlSchemeVisual.controllerInput.gamepadController = true;
                    controlSchemeVisual.controllerInput.xboxController = true;
                    controlSchemeVisual.keyboardmouseInput = false;
                }

                //Specifically for playstation controllers
                else
                {
                    controlSchemeVisual.controllerInput.xboxController = false;
                    controlSchemeVisual.controllerInput.gamepadController = true;
                    controlSchemeVisual.keyboardmouseInput = false;
                }
                break;

            //Effectively an if statement for 'if using a touch device'
            case ControlState.Touch:
                controlSchemeVisual.touchInput = true;
                controlSchemeVisual.keyboardmouseInput = false;
                controlSchemeVisual.controllerInput.xboxController = false;
                controlSchemeVisual.controllerInput.gamepadController = false;
                break;
            default:
                break;
        }
    }


}




    /// <summary>
    /// The following are classes for the purpose of displaying pleasantly in the Unity Inspector.
    /// </summary>

[System.Serializable]
public class CurrentControlInput
{
    public ControllerInput controllerInput;
    [ReadOnlyInspector]
    [Tooltip("Refers to use on a mobile device. Effectively the same as mouse only")]
    public bool touchInput;
    [ReadOnlyInspector]
    [Tooltip("Default PC layout")]
    public bool keyboardmouseInput;
    [ReadOnlyInspector]
    public bool keyboardMostRecently;
    [ReadOnlyInspector]
    public bool mouseMostRecently;

}

[System.Serializable]
public class ControllerInput
{
    [ReadOnlyInspector]
    [Tooltip("Refers to all controllers that follows XInput")]
    public bool xboxController;
    [Tooltip("Refers to all controllers that follow playstation schemeing")]
    [ReadOnlyInspector] 

    public bool gamepadController;
}














