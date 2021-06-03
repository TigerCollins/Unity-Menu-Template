using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

/// <summary>
/// CODE BY TIGER COLLINS https://tigercollins.myportfolio.com/ , 
/// https://twitter.com/_TigerCollins , https://www.linkedin.com/in/tiger-collins-2b8619120/
/// Scripts used for the Menu Template package.
/// Feel free to modify or expand upon this script. Please credit myself if you redistribute this
/// script or use this asset in a project.
/// </summary>

/// <summary>
/// This script is to work as an extension to the Unity <see cref="EventSystem"/> to better support 
/// controller and keyboard support and work well with different use cases. It's main function is
/// tracking the compatible gameobjects selected with keyboard or controller and returning to those 
/// when swapping from mouse to keyboard or controller.
/// </summary>

public class MouselessGeneralControl : MonoBehaviour
{
    /// <summary>
    /// Public to inspector.
    /// </summary>
    ///  

    [Header("MENU TEMPLATE - Track keyboard and mouse input")]

    [Space(20)]

    [SerializeField]
    private InputTypeDetection inputTypeDetection;
    [Header("Mouseless Input Tracking")]
    [ReadOnlyInspector]
    //This is for the last selected object but display only
    public GameObject latestSelectedObject;
    [SerializeField]
    [ReadOnlyInspector]
    //This is for the last selected input but display only
    private NavigationDirections lastNavigation = NavigationDirections.None;
    [SerializeField]
    [Range(.5f, 10f)]
    //Resets Last Navigation to None
    private float resetNavigationTimer = 1;
    [Header("Set Selected Object Details")]
    [SerializeField]
    private MouselessToggleControl mouselessToggleControl;

    [Space(10)]

    [SerializeField]
    private UnityEvent onNewSelectedObject;

    /// <summary>
    /// Invisible to inpsector.
    /// </summary>

    private EventSystem eventSystem;
    private Coroutine currentCoroutine;

    private float currentCountdown =.01f;
    [HideInInspector]
    public bool canActivate;
    private bool deselectOnBackgroud;

    public enum NavigationDirections
    {
        North,
        West,
        South,
        East,
        None
    }

    private void Awake()
    {
        if (eventSystem == null)
        {
            eventSystem = FindObjectOfType<EventSystem>();
            if (eventSystem == null)
            {
                Debug.LogError("Could not find EventSystem for MouselessGeneralControl. Add EventSystem component " +
                    "to the scene to resolve the Null Reference");
            }
        }

        if (mouselessToggleControl == null)
        {
            mouselessToggleControl = FindObjectOfType<MouselessToggleControl>();
            if (mouselessToggleControl == null)
            {
                Debug.LogError("Could not find MouselessToggleControl for MouselessGeneralControl. Add MouselessToggleControl component " +
                    "to the scene to resolve the Null Reference");
            }
        }

        if (eventSystem.TryGetComponent(out InputSystemUIInputModule inputModule))
        {
            deselectOnBackgroud = inputModule.deselectOnBackgroundClick;
         }

    }

    private void Start()
    {
         if (mouselessToggleControl != null && mouselessToggleControl.selectableToggles.Count != 0)
        {
            int index = 0;
            foreach (Toggle toggleItem in mouselessToggleControl.selectableToggles)
            {
                toggleItem.onValueChanged.AddListener((x) => Invoke("SetSelectedForToggle",0));
                index++;
            }
        }
    }

    void Update()
    {
        NewSelectedObjectCheck();
        if(currentCountdown >= 0 && !canActivate)
        {
            currentCountdown -= Time.unscaledDeltaTime;
        }

        else
        {
            canActivate = true;
        }

         //   Debug.Log(eventSystem.currentSelectedGameObject);

    }

    /// <summary>
    /// This function is a event invoke check for <see cref="onNewSelectedObject"/>.
    /// It works by checking if a UI element is different to the previous one and setting the 
    /// <see cref="latestSelectedObject"/> to the new element if it can return a compatible component
    /// type and it's navigation type is not <see cref="Navigation.Mode.None"/>.
    /// 
    /// As of Unity 2019.2, Unity had introduced <see cref="GameObject.TryGetComponent(System.Type, out Component)"/>
    /// which is effectively the same as <see cref="GameObject.GetComponent(System.Type)"/> but
    /// doesn't assign anything to Inspector and hence is performance cost effecient. If you're
    /// using a earlier version of Unity, change the below if statements to the following;
    /// 
    /// <code>If(GameObject.GetComponent<Type>() != null)</code>
    /// </summary>
    void NewSelectedObjectCheck()
    {
        if(canActivate && latestSelectedObject !=null)
        {
            if (eventSystem.currentSelectedGameObject != latestSelectedObject && eventSystem.currentSelectedGameObject != null && latestSelectedObject.activeInHierarchy)
            {
                if (eventSystem.currentSelectedGameObject.TryGetComponent(out Toggle toggle))
                {
                    if (toggle.navigation.mode != Navigation.Mode.None)
                    {
                        InvokeEvent();
                    }
                }

                if (eventSystem.currentSelectedGameObject.TryGetComponent(out Slider slider))
                {
                    if (slider.navigation.mode != Navigation.Mode.None)
                    {
                        InvokeEvent();
                    }
                }

                if (eventSystem.currentSelectedGameObject.TryGetComponent(out Button button))
                {
                    if (button.navigation.mode != Navigation.Mode.None )
                    {
                        InvokeEvent();
                    }
                }

                if (eventSystem.currentSelectedGameObject.TryGetComponent(out Scrollbar scrollBar))
                {
                    if (scrollBar.navigation.mode != Navigation.Mode.None)
                    {
                        InvokeEvent();
                    }
                }

                if (eventSystem.currentSelectedGameObject.TryGetComponent(out Selectable selectable))
                {
                    if (selectable.navigation.mode != Navigation.Mode.None)
                    {
                        InvokeEvent();
                    }
                }

                if (eventSystem.currentSelectedGameObject.TryGetComponent(out Dropdown dropdown))
                {
                    if (dropdown.navigation.mode != Navigation.Mode.None)
                    {
                        InvokeEvent();
                    }
                }

                if (eventSystem.currentSelectedGameObject.TryGetComponent(out InputField inputField))
                {
                    if (inputField.navigation.mode != Navigation.Mode.None)
                    {
                        InvokeEvent();
                    }
                }

            }
        }
        
    }


    /// <summary>
    /// This Vector2 is a Set that interpretes the Vector2 from a Input and converts it to
    /// cardinal directions and assigns it to the  <see cref="NavigationDirections"/> enum.
    /// 
    /// <see cref="ChangeLastInput(InputAction.CallbackContext)"/> takes advantage of the
    /// New <see cref="InputSystem"/> bug where the different input states are all called 
    /// on a <see cref="Button"/> input - it does it by checking calling the <see cref="DominantAxis"/>
    /// Set when perf <see cref="InputAction.CallbackContext.performed"/> is called and
    /// when the <see cref="TriggerDominantAxis"/> coroutine is finished. TriggerDominantAxis
    /// is called  by <see cref="InputAction.CallbackContext.canceled"/> and the coroutine 
    /// waits for <see cref="resetNavigationTimerwhen"/> to complete.
    /// </summary>
    Vector2 DominantAxis
    {
        set
        {
            if (Mathf.Abs(value.x) > Mathf.Abs(value.y))
            {
                if (value.x > 0)
                {
                    lastNavigation = NavigationDirections.East;
                }

                else
                {
                    lastNavigation = NavigationDirections.West;
                }
            }

            else if (Mathf.Abs(value.x) < Mathf.Abs(value.y))
            {
                if (value.y > 0)
                {
                    lastNavigation = NavigationDirections.North;
                }

                else
                {
                    lastNavigation = NavigationDirections.South;
                }
            }

            else
            {
                lastNavigation = NavigationDirections.None;
            }
        }
    }

    public void ChangeLastInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DominantAxis = context.ReadValue<Vector2>();
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
        }
        if (context.canceled)
        {
            currentCoroutine = StartCoroutine(TriggerDominantAxis());
        }

    }
    public IEnumerator TriggerDominantAxis()
    {
        yield return new WaitForSeconds(resetNavigationTimer);
        DominantAxis = new Vector2(0, 0);
    }

    /// <summary>
    /// This is interpretation of <see cref="EventSystem.SetSelectedGameObject(GameObject)"/> that works
    /// really well for this scripts purpose or if you want to adapt your your input system. Its function
    /// is literally SetSelectedGameObject but takes into account navigation.
    /// 
    /// The idea is that if a keyboard or controller <see cref="Navigation"/> input is called, it'll move
    /// from the last selected GameObject. If another input is called, then the previous gameobject will be
    /// called.
    /// 
    /// The function argument for <see cref="MoveDirection"/> determines the input direction and is determined
    /// by <see cref="OnInputTypeChange"/> which is called by the <see cref="InputTypeDetection.onControlSchemeChange"/>  
    /// Unity Event. It uses a switch statement that goes through the MoveDirections and 
    /// <see cref="InputTypeDetection.ControlState"/> enums, which then called <see cref="NavigationControl(MoveDirection)"/> 
    /// </summary>
    public void NavigationControl(MoveDirection moveDirection)
    {
        AxisEventData data = new AxisEventData(eventSystem);
        data.moveDir = moveDirection;
        data.selectedObject = latestSelectedObject;
        ExecuteEvents.Execute(data.selectedObject, data, ExecuteEvents.moveHandler);
    }

    public void InvokeEvent()
    {
        onNewSelectedObject.Invoke();
        latestSelectedObject = eventSystem.currentSelectedGameObject;
    }

    public void OnInputTypeChange()
    {
        
        if(canActivate && deselectOnBackgroud == true)
        {
            
            switch (lastNavigation)
            {
                case NavigationDirections.North:
                    switch (inputTypeDetection.controlState)
                    {
                        case InputTypeDetection.ControlState.KeyboardAndMouse:
                            if (inputTypeDetection.controlSchemeVisual.keyboardMostRecently && eventSystem.currentSelectedGameObject != null)
                            {
                                NavigationControl(MoveDirection.None);
                            }
                            break;
                        case InputTypeDetection.ControlState.Controller:
                            NavigationControl(MoveDirection.None);
                            break;
                        case InputTypeDetection.ControlState.Touch:
                            break;
                        default:
                            break;
                    }
                    break;
                case NavigationDirections.West:
                    switch (inputTypeDetection.controlState)
                    {
                        case InputTypeDetection.ControlState.KeyboardAndMouse:
                            if (inputTypeDetection.controlSchemeVisual.keyboardMostRecently && eventSystem.currentSelectedGameObject != null)
                            {
                                NavigationControl(MoveDirection.None);
                            }
                            break;
                        case InputTypeDetection.ControlState.Controller:
                            NavigationControl(MoveDirection.None);
                            break;
                        case InputTypeDetection.ControlState.Touch:
                            break;
                        default:
                            break;
                    }
                    break;
                case NavigationDirections.South:
                    switch (inputTypeDetection.controlState)
                    {
                        case InputTypeDetection.ControlState.KeyboardAndMouse:
                            if (inputTypeDetection.controlSchemeVisual.keyboardMostRecently && eventSystem.currentSelectedGameObject != null)
                            {
                                NavigationControl(MoveDirection.None);
                            }
                            break;
                        case InputTypeDetection.ControlState.Controller:
                            NavigationControl(MoveDirection.None);
                            break;
                        case InputTypeDetection.ControlState.Touch:
                            break;
                        default:
                            break;
                    }
                    break;
                case NavigationDirections.East:
                    switch (inputTypeDetection.controlState)
                    {
                        case InputTypeDetection.ControlState.KeyboardAndMouse:
                            if (inputTypeDetection.controlSchemeVisual.keyboardMostRecently && eventSystem.currentSelectedGameObject != null)
                            {

                                NavigationControl(MoveDirection.None);
                            }
                            break;
                        case InputTypeDetection.ControlState.Controller:
                            NavigationControl(MoveDirection.None);
                            break;
                        case InputTypeDetection.ControlState.Touch:
                            break;
                        default:
                            break;
                    }
                    break;
                case NavigationDirections.None:
                    switch (inputTypeDetection.controlState)
                    {
                        case InputTypeDetection.ControlState.KeyboardAndMouse:
                            
                            if (inputTypeDetection.controlSchemeVisual.keyboardMostRecently && eventSystem.currentSelectedGameObject != null)
                            {
                              //  eventSystem.SetSelectedGameObject(latestSelectedObject);
                                NavigationControl(MoveDirection.None);
                            }
                            break;
                        case InputTypeDetection.ControlState.Controller:
                            eventSystem.SetSelectedGameObject(latestSelectedObject);
                            NavigationControl(MoveDirection.None);
                            break;
                        case InputTypeDetection.ControlState.Touch:
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
             
        }

    }

  

    /// <summary>
    /// This function is for setting the SelectedGameObject through Unity Event - It simply changes the 
    /// selected object by calling running the Submit event on a predefined object. If the template is being
    /// modified, this may need to be expanded (along with the <see cref="FirstSelected"/> class) as this
    /// function is not automatically expandable.
    /// </summary>
    public void SetSelectedForToggle()
    {
        
       
        if (mouselessToggleControl.selectableToggles.Count != 0 && canActivate == true)
        {
            int index = 0;
            foreach (Toggle toggleItem in mouselessToggleControl.selectableToggles)
            {
                if (toggleItem.isOn && toggleItem.TryGetComponent(out TogglePanelComponent togglePanel))
                {
                    if(togglePanel.optionPanels == UIManager.OptionPanels.General)
                    {
                        if (togglePanel.TryGetComponent(out SetSelectedObject setSelectedObject))
                        {
                            setSelectedObject.SetSelectedGameObject();
                            return;
                        }
                        
                    }

                    else if(togglePanel.optionPanels == UIManager.OptionPanels.Video)
                    {
                        if (togglePanel.TryGetComponent(out SetSelectedObject setSelectedObject))
                        {
                            setSelectedObject.SetSelectedGameObject();
                            return;
                        }
                    }

                    else if(togglePanel.optionPanels == UIManager.OptionPanels.Audio)
                    {
                        if (togglePanel.TryGetComponent(out SetSelectedObject setSelectedObject))
                        {
                            setSelectedObject.SetSelectedGameObject();
                            return;
                        }
                    }

                    else if(!togglePanel.TryGetComponent(out SetSelectedObject setSelectedObject))
                    {
                        Debug.LogWarning("Could not find SetSelectedObject on Toggle. Please add script to " + togglePanel.name);
                    }

                    else
                    {
                        Debug.LogWarning("Could not find a enum to match the toggle with. Please adjust this script to reflect possible enums");

                    }


                }
                index++;
            }
        }
    }

}

