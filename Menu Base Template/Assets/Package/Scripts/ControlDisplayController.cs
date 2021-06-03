using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEditor;


////TODO: have updateBindingUIEvent receive a control path string, too (in addition to the device layout name)


/// <summary>
/// This is an example for how to override the default display behavior of bindings. The component
/// hooks into <see cref="RebindActionUI.updateBindingUIEvent"/> which is triggered when UI display
/// of a binding should be refreshed. It then checks whether we have an icon for the current binding
/// and if so, replaces the default text display with an icon.
/// </summary>


public class ControlDisplayController : MonoBehaviour
{
    public InputTypeDetection inputType;
    public GamepadIcons ps4Icons;
    public GamepadIcons xboxIcons;

    [Header("Control Scheme Footer")]
    [Tooltip("Empty variables in this list are removed at runtime")]
    public List<ControlDisplayLocal> controlIconScripts;
    //  private 
    private void Awake()
    {
        //If the input type hasn't been assigned, assigns it for you. 
        //Better to be previously assigned for performance.
        if (inputType == null)
        {
            inputType = FindObjectOfType<InputTypeDetection>();
        }
    }

    public void UpdateControlDisplay()
    {

        if (controlIconScripts.Count != 0)
        {
            for (int i = controlIconScripts.Count - 1; i >= 0; i--)
            {
                if (controlIconScripts[i] != null)
                {
                    //Do stuff
                }
                else
                {
                    controlIconScripts.RemoveAt(i);
                }
            }
        
        foreach (ControlDisplayLocal item in controlIconScripts)
            {
                if (item.isActiveAndEnabled)
                {


                    item.UpdateBindingDisplay();
                    item.SetState();
                    if (item.controlType == ControlDisplayLocal.ControlType.controller)
                    {
                        OnUpdateBindingDisplay(item.controlPath, item.deviceLayout, item.controllerImage);
                    }
                    else
                    {

                    }
                }
            }
        }
    }

    void OnUpdateBindingDisplay(string controlPath, string deviceLayoutName, Image imageComponent)
    {
        if (string.IsNullOrEmpty(deviceLayoutName) || string.IsNullOrEmpty(controlPath))
            return;

        var icon = default(Sprite);
        if (InputSystem.IsFirstLayoutBasedOnSecond(deviceLayoutName, "DualShockGamepad"))
        {
            icon = ps4Icons.GetSprite(controlPath);
        }

        else if (InputSystem.IsFirstLayoutBasedOnSecond(deviceLayoutName, "Gamepad"))
        {
            icon = xboxIcons.GetSprite(controlPath);
        }

        if (icon != null)
        {
            imageComponent.sprite = icon;
        }
        else
        {
            Debug.LogError("Couldn't find a controller sprite to display");
        }
    }


}

[System.Serializable]
public struct GamepadIcons
{
    public Sprite buttonSouth;
    public Sprite buttonNorth;
    public Sprite buttonEast;
    public Sprite buttonWest;
    public Sprite startButton;
    public Sprite selectButton;
    public Sprite leftTrigger;
    public Sprite rightTrigger;
    public Sprite leftShoulder;
    public Sprite rightShoulder;
    public Sprite dpad;
    public Sprite dpadUp;
    public Sprite dpadDown;
    public Sprite dpadLeft;
    public Sprite dpadRight;
    public Sprite leftStick;
    public Sprite rightStick;
    public Sprite leftStickPress;
    public Sprite rightStickPress;

    public Sprite GetSprite(string controlPath)
    {
        switch (controlPath)
        {
            case "buttonSouth": return buttonSouth;
            case "buttonNorth": return buttonNorth;
            case "buttonEast": return buttonEast;
            case "buttonWest": return buttonWest;
            case "start": return startButton;
            case "select": return selectButton;
            case "leftTrigger": return leftTrigger;
            case "rightTrigger": return rightTrigger;
            case "leftShoulder": return leftShoulder;
            case "rightShoulder": return rightShoulder;
            case "dpad": return dpad;
            case "dpad/up": return dpadUp;
            case "dpad/down": return dpadDown;
            case "dpad/left": return dpadLeft;
            case "dpad/right": return dpadRight;
            case "leftStick": return leftStick;
            case "rightStick": return rightStick;
            case "leftStickPress": return leftStickPress;
            case "rightStickPress": return rightStickPress;
        }
        return null;
    }
}


