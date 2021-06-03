using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlDisplayMenus : MonoBehaviour
{
    [SerializeField] 
    private TweenScript controlFooter;
    [SerializeField]
    private UIManager uiManager;
    [SerializeField]
    private string footerTweenID;
    public MenuControlsAppearIn menuControlsMenu;
    public FooterPreset footerPresetGroups;

    public void ChangeControlBarState()
    {
        switch (uiManager.currentMenu)
        {
            case UIManager.PossibleMenu.None:
                if(menuControlsMenu.noMenu)
                {
                    controlFooter.moveTowardsEnd = true;
                    controlFooter.TriggerPositionTween(footerTweenID);
                }

                else
                {
                    controlFooter.moveTowardsEnd = false;
                    controlFooter.TriggerPositionTween(footerTweenID);
                }
                footerPresetGroups.genericGroup.SetActive(false);
                footerPresetGroups.creditGroup.SetActive(false);
                footerPresetGroups.optionsGroup.SetActive(false);
                footerPresetGroups.gameplayGroup.SetActive(true);

                break;
            case UIManager.PossibleMenu.PauseMenu:
                if (menuControlsMenu.pauseMenu)
                {
                    controlFooter.moveTowardsEnd = true;
                    controlFooter.TriggerPositionTween(footerTweenID);
                }

                else
                {
                    controlFooter.moveTowardsEnd = false;
                    controlFooter.TriggerPositionTween(footerTweenID);
                }
                footerPresetGroups.genericGroup.SetActive(true);
                footerPresetGroups.creditGroup.SetActive(false);
                footerPresetGroups.optionsGroup.SetActive(false);
                footerPresetGroups.gameplayGroup.SetActive(false);

                break;
            case UIManager.PossibleMenu.MainMenu:
                if (menuControlsMenu.mainMenu)
                {
                    controlFooter.moveTowardsEnd = true;
                    controlFooter.TriggerPositionTween(footerTweenID);

                }

                else
                {
                    controlFooter.moveTowardsEnd = false;
                    controlFooter.TriggerPositionTween(footerTweenID);
                }
                footerPresetGroups.genericGroup.SetActive(true);
                footerPresetGroups.creditGroup.SetActive(false);
                footerPresetGroups.optionsGroup.SetActive(false);
                footerPresetGroups.gameplayGroup.SetActive(false);

                break;
            case UIManager.PossibleMenu.Credits:
                if (menuControlsMenu.creditsMenu)
                {
                    controlFooter.moveTowardsEnd = true;
                    controlFooter.TriggerPositionTween(footerTweenID);
                }

                else
                {
                    controlFooter.moveTowardsEnd = false;
                    controlFooter.TriggerPositionTween(footerTweenID);
                }
                footerPresetGroups.genericGroup.SetActive(false);
                footerPresetGroups.creditGroup.SetActive(true);
                footerPresetGroups.optionsGroup.SetActive(false);
                footerPresetGroups.gameplayGroup.SetActive(false);

                break;
            case UIManager.PossibleMenu.Options:
                if (menuControlsMenu.optionsMenu)
                {
                    controlFooter.moveTowardsEnd = true;
                    controlFooter.TriggerPositionTween(footerTweenID);
                }

                else
                {
                    controlFooter.moveTowardsEnd = false;
                    controlFooter.TriggerPositionTween(footerTweenID);
                }
                footerPresetGroups.genericGroup.SetActive(false);
                footerPresetGroups.creditGroup.SetActive(false);
                footerPresetGroups.optionsGroup.SetActive(true);
                footerPresetGroups.gameplayGroup.SetActive(false);

                break;
            default:
                break;
        }
    }
}
[System.Serializable]
public class MenuControlsAppearIn
{
    [LabelOverride("Show in Game")]
    public bool noMenu;
    [LabelOverride("Show in Pause Menu")]
    public bool pauseMenu;
    [LabelOverride("Show in Main Menu")]
    public bool mainMenu;
    [LabelOverride("Show in Credits Menu")]
    public bool creditsMenu;
    [LabelOverride("Show in Pause Menu")]
    public bool optionsMenu;
}

[System.Serializable]
public class FooterPreset
{
    public GameObject genericGroup;
    public GameObject creditGroup;
    public GameObject optionsGroup;
    public GameObject gameplayGroup;
}
