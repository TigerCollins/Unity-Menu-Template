using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MenuLinker : MonoBehaviour
{
    [ReadOnlyInspector]
    [SerializeField]
    private UIManager.PossibleMenu lastMenu;
    [ReadOnlyInspector]
    [SerializeField]
    private UIManager.PossibleMenu currentMenu;
    [SerializeField]
    private DesiredMenus desiredMenus;
    [SerializeField]
    private UIManager uiManager;

    [SerializeField]
    private GameObject targetObject;

    public string newScene;
    public WorkableMenus workableMenus;
    public bool canHideMenu;
    public bool useDefaultSelectable;


    private MouselessGeneralControl mouselessGeneralControl;

    private bool canToggleMenu;
    // Start is called before the first frame update
    void Awake()
    {
        if (uiManager == null)
        {
            uiManager = FindObjectOfType<UIManager>();
            if (uiManager == null)
            {
                Debug.LogError("Could not find UIManager for Return Setting on " + gameObject.name + ". Add UIManager script to scene or add reference.");
            }
            else
            {
                AddListerFunction();
            }
        }

        else
        {
            AddListerFunction();
        }
        
        mouselessGeneralControl = FindObjectOfType<MouselessGeneralControl>();
        if (mouselessGeneralControl == null)
        {
            Debug.LogError("Couldn't find MouselessGeneralControl as part of ReturnSetting. Add script to scene to resolve error.");
        }

    }


    private void Update()
    {
        ChangeCurrentMenu();
        
        
    }

    public void AddListerFunction()
    {
        uiManager.onMenuChange.AddListener(ChangeCurrentMenu); //important 

    }


    //MUST BE CALLED BEFORE THE UI MANAGER CHANGES ITS PREVIOUS MENU
    public void ChangeCurrentMenu()
    {
       // lastMenu = uiManager.previousMenu;
           

    }


    public void ReturnMenu()
    {
        currentMenu = uiManager.currentMenu;
        //Debug.Log((UIManager.PossibleMenu)System.Enum.Parse(typeof(UIManager.PossibleMenu), uiManager.currentMenu.ToString()));
        //  currentMenu = (UIManager.PossibleMenu)System.Enum.Parse(typeof(UIManager.PossibleMenu), uiManager.currentMenu.ToString());
        if (mouselessGeneralControl != null && uiManager != null)
        {
            switch (currentMenu)
            {
                case UIManager.PossibleMenu.None:
                    if (workableMenus.noMenu)
                    {
                        uiManager.ChangeMenuEnum(desiredMenus.noMenuTarget.ToString());
                        uiManager.SetSelectedMenuObject(desiredMenus.noMenuTarget.ToString());
                    }
                    break;
                case UIManager.PossibleMenu.PauseMenu:
                    if (workableMenus.mainOrPauseMenu)
                    {
                        uiManager.ChangeMenuEnum(desiredMenus.pauseOrGameMenuTarget.ToString());
                        uiManager.SetSelectedMenuObject(desiredMenus.pauseOrGameMenuTarget.ToString());
                    }
                    break;
                case UIManager.PossibleMenu.MainMenu:
                    if (workableMenus.mainOrPauseMenu)
                    {
                        uiManager.ChangeMenuEnum(desiredMenus.pauseOrGameMenuTarget.ToString());
                        uiManager.SetSelectedMenuObject(desiredMenus.pauseOrGameMenuTarget.ToString());
                    }
                    break;
                case UIManager.PossibleMenu.Credits:
                    if (workableMenus.CreditsMenu)
                    {
                        uiManager.ChangeMenuEnum(desiredMenus.creditsMenuTarget.ToString());
                        uiManager.SetSelectedMenuObject(desiredMenus.creditsMenuTarget.ToString());
                    }
                    break;
                case UIManager.PossibleMenu.Options:
                    if (workableMenus.OptionsMenu)
                    {
                        uiManager.ChangeMenuEnum(desiredMenus.optionsMenuTarget.ToString());
                        uiManager.SetSelectedMenuObject(desiredMenus.optionsMenuTarget.ToString());
                    }


                    break;
                default:
                    Debug.LogError("FUUUUUUUUUCK");
                    break;
            }

          }
      //  Debug.Log(lastMenu);
  
    }


    public void ReturnMenuInput(InputAction.CallbackContext callbackContext)
    {
        if (mouselessGeneralControl != null && uiManager != null && callbackContext.performed) //&& TryGetComponent(out Button button) 
        {
            ReturnMenu();
        }
    }


}

[System.Serializable]
public class WorkableMenus
{
    public bool noMenu;
    public bool mainOrPauseMenu;
    public bool OptionsMenu;
    public bool CreditsMenu;
}

[System.Serializable]
public class DesiredMenus
{
    public UIManager.PossibleMenu noMenuTarget;
    public UIManager.PossibleMenu pauseOrGameMenuTarget;
    public UIManager.PossibleMenu optionsMenuTarget;
    public UIManager.PossibleMenu creditsMenuTarget;
}

