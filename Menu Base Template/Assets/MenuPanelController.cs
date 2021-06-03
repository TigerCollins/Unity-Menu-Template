using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanelController : MonoBehaviour
{
    [ReadOnlyInspector]
    [SerializeField]
    private UIManager.PossibleMenu currentMenu;
    public MenuPanelDetails[] menuPanelDetails;
    [SerializeField]
    private UIManager uiManager;
    [SerializeField]
    private MenuLinker menuLinker;
    

    public void Awake()
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
    }


    public void AddListerFunction()
    {
        //uiManager.onMenuChange.AddListener(ChangeCurrentMenu); //important 

    }

    public void ChangeCurrentMenu()
    {
        currentMenu = uiManager.currentMenu;
    }

    private void Update()
    {
    }

    public void ChangeMenuPanel()
    {
        ChangeCurrentMenu();
        Debug.LogWarning(currentMenu);
        for (int i = 0; i < menuPanelDetails.Length; i++)
        {

            if (menuPanelDetails[i].menuPanelTweenScript.tweenLibrary != null)
            {
           
                switch (currentMenu)
                {

                    case UIManager.PossibleMenu.None:
                        if (menuPanelDetails[i].showPanelInMenus.showInNoMenu == true)
                        {
                            menuPanelDetails[i].menuPanelTweenScript.moveTowardsEnd = true;
                            menuPanelDetails[i].menuPanelTweenScript.TriggerPositionTween(menuPanelDetails[i].tweenIDName);
                            menuPanelDetails[i].canvasGroup.interactable = true;
                        }

                        else// if (menuPanelDetails[i].menuPanelTweenScript.moveTowardsEnd != false)
                        {
                            menuPanelDetails[i].menuPanelTweenScript.moveTowardsEnd = false;
                            menuPanelDetails[i].menuPanelTweenScript.TriggerPositionTween(menuPanelDetails[i].tweenIDName);
                            menuPanelDetails[i].canvasGroup.interactable = false;
                        }

                        break;
                    case UIManager.PossibleMenu.PauseMenu:
                        if (menuPanelDetails[i].showPanelInMenus.showInPauseMenu == true)
                        {
                            menuPanelDetails[i].menuPanelTweenScript.moveTowardsEnd = true;
                            menuPanelDetails[i].menuPanelTweenScript.TriggerPositionTween(menuPanelDetails[i].tweenIDName);
                            menuPanelDetails[i].canvasGroup.interactable = true;
                        }

                        else if (menuPanelDetails[i].menuPanelTweenScript.moveTowardsEnd != false)
                        {
                            menuPanelDetails[i].menuPanelTweenScript.moveTowardsEnd = false;
                            menuPanelDetails[i].menuPanelTweenScript.TriggerPositionTween(menuPanelDetails[i].tweenIDName);
                            menuPanelDetails[i].canvasGroup.interactable = false;
                        }

                        break;
                    case UIManager.PossibleMenu.MainMenu:
                        if (menuPanelDetails[i].showPanelInMenus.showInMainMenu == true)
                        {
                            menuPanelDetails[i].menuPanelTweenScript.moveTowardsEnd = true;
                            menuPanelDetails[i].menuPanelTweenScript.TriggerPositionTween(menuPanelDetails[i].tweenIDName);
                            menuPanelDetails[i].canvasGroup.interactable = true;
                        }

                        else if (menuPanelDetails[i].menuPanelTweenScript.moveTowardsEnd != false)
                        {
                            menuPanelDetails[i].menuPanelTweenScript.moveTowardsEnd = false;
                            menuPanelDetails[i].menuPanelTweenScript.TriggerPositionTween(menuPanelDetails[i].tweenIDName);
                            menuPanelDetails[i].canvasGroup.interactable = false;
                        }
                        break;
                    case UIManager.PossibleMenu.Credits:
                        Debug.LogWarning(currentMenu);
                        if (menuPanelDetails[i].showPanelInMenus.showInCreditsMenu == true)
                        {
                            menuPanelDetails[i].menuPanelTweenScript.moveTowardsEnd = true;
                            menuPanelDetails[i].menuPanelTweenScript.TriggerPositionTween(menuPanelDetails[i].tweenIDName);
                            menuPanelDetails[i].canvasGroup.interactable = true;
                        }

                        else if (menuPanelDetails[i].menuPanelTweenScript.moveTowardsEnd != false)
                        {
                            menuPanelDetails[i].menuPanelTweenScript.moveTowardsEnd = false;
                            menuPanelDetails[i].menuPanelTweenScript.TriggerPositionTween(menuPanelDetails[i].tweenIDName);
                            menuPanelDetails[i].canvasGroup.interactable = false;
                        }
                        break;
                    case UIManager.PossibleMenu.Options:
                        if (menuPanelDetails[i].showPanelInMenus.showInOptionsMenu == true)
                        {
                            menuPanelDetails[i].menuPanelTweenScript.moveTowardsEnd = true;
                            menuPanelDetails[i].menuPanelTweenScript.TriggerPositionTween(menuPanelDetails[i].tweenIDName);
                            menuPanelDetails[i].canvasGroup.interactable = true;
                        }

                        else if (menuPanelDetails[i].menuPanelTweenScript.moveTowardsEnd != false)
                        {
                            menuPanelDetails[i].menuPanelTweenScript.moveTowardsEnd = false;
                            menuPanelDetails[i].menuPanelTweenScript.TriggerPositionTween(menuPanelDetails[i].tweenIDName);
                            menuPanelDetails[i].canvasGroup.interactable = false;
                        }

                        break;
                    default:
                        break;
                }
   
            }

            else
            {
                Debug.LogError("Missing the Tween Library on " + menuPanelDetails[i].menuPanelTweenScript.gameObject.name);
            }

            
        }
    }

}



[System.Serializable]
public class MenuPanelDetails
{
    public string name;
    public string tweenIDName;
    public MenuPanelBools showPanelInMenus;
    public TweenScript menuPanelTweenScript;
    public CanvasGroup canvasGroup;

  
}

[System.Serializable]
public class MenuPanelBools
{
    public bool showInNoMenu;
    public bool showInPauseMenu;
    public bool showInMainMenu;
    public bool showInGameMenu;
    public bool showInOptionsMenu;
    public bool showInCreditsMenu;
}

