using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.EventSystems;

using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public EventSystem es;
    public FunctionTester functionTester;
    public InputTypeDetection inputTypeDetection;
    [Header("Menu Variables")]
    public MenuButtonsAvailable menuButtonsAvailable;
    public GameMode gameMode;
    public bool canControlTimeScale;


    [Header("Game Variables")]
    public bool onMainMenu;
    public bool gamePaused;
    public int mainMenuInt = 1;
    public int pauseMenuInt = 2;

    [Header("Quality Settings")]
    [SerializeField]
    private Resolutions resolutionScript;
    public string[] qualitySettingNames;
    public int selectedQualityLevel;
    [SerializeField]
    private TextMeshProUGUI qualityText;
    [SerializeField]
    private GameObject qualityTogglePrefab;
    [SerializeField]
    private Transform qualitySettingsHolder;

    [Header("Credits Options")]
    public CreditItem[] creditDetails;
    public CreditReferences creditReferences;
    [SerializeField]
    private bool creditsAutoScroll;
    [SerializeField]
    private float creditsAutoScrollSpeed;
    [SerializeField]
    private float inputCountDown = 2;
    private Vector2 moveAxis;

    [Header("Auto Rotation - Capabilities")]
    [SerializeField]
    [Tooltip("Not to be edited. Is visible in inspector for debugging during development")]
    public bool isCurrentlyVertical;
    private bool previousVerticality;
    private bool hasChanged = false;

    [Header("Tween Options")]
    public TweenScript currentTween;
    [SerializeField]
    private bool canChangePanels;
    float t;

    [Header("Menu Panels")]
    [SerializeField]
    private SceneController sceneController;



    [Header("Auto Rotation Offset - Vertical Layout")]

    public AutoRotationVertical autoRotationVerticalModeOffset;

    [ReadOnlyInspector]
    public bool canShowCursor;
    public ShowMouseInMenu showMouse;

    [Header("Auto Rotation Offset - Horizontal Layout")]
    [SerializeField]
    public AutoRotationHorizontal autoRotationHorizontalModeOffset;

    [Header("Menu Tracker")]
    public PossibleMenu currentMenu;
    [SerializeField]
    public PossibleMenu defaultMenu;
    [SerializeField]
    public UnityEvent onMenuChange;
   // public PossibleMenu previousMenu;
    public SetPauseSettings setPauseSettings;
    public FirstSelectableObject firstSelectableObject;



    //PRIVATE
    private string currentScene;
    [HideInInspector]
    public PossibleMenu tempPossibleMenu;


    public enum GameMode
    {
        Paused,
        [InspectorName("Playing")]
        Active,
        [InspectorName("Game Over")]
        Other //For the instance you need something like a gameover screen
    }
    public enum PossibleMenu
    {
        None,
        PauseMenu,
        MainMenu,
        Credits,
        Options
    }
    public enum OptionPanels
    {
        [InspectorName("General - Specific to game")]
        General,
        Video,
        Audio
    }
    public void Awake()
    {
        //      SetPause();
        //currentMenu = PossibleMenu.PauseMenu;

        //Scene Setup
       
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("PauseMenu"))
        {
            currentScene = "PauseMenu";
        }
       // ChangeCanvasGroup();
        //       currentScene = SceneManager.Ge SceneManager.GetActiveScene().name;
        // ChangeMenuEnum(currentScene);
        CheckMenu();

        //--------QUALITY SETTINGS--------
        //Sets the length in an int and removes 1
        qualitySettingNames = QualitySettings.names;

        //Sets
        int index = 0;
        for (int i = 0; i < qualitySettingNames.Length; i++)
        {
            Instantiate(qualityTogglePrefab, qualitySettingsHolder);
            index++;
            if (index >= qualitySettingNames.Length)
            {

                /////////// qualityScrollController.UpdateLayout();
                ///////////// qualityScrollController.scrollRect.horizontalNormalizedPosition = 0;
            }

        }
        IncreaseQuality();
        DecreaseQuality();

        //----------CREDITS SETTINGS----------
        for (int i = 0; i < creditDetails.Length; i++)
        {
            GameObject localObject = Instantiate(creditReferences.creditPrefab, creditReferences.creditsHolder);
            LocalCreditsController creditObjectController = localObject.GetComponent<LocalCreditsController>();
            creditObjectController.creditsID = i;
            creditObjectController.personText.text = creditDetails[i].person;
            creditObjectController.roleText.text = creditDetails[i].personRole;
        }
        this.enabled = true;

       // tempPossibleMenu = PossibleMenu.None;
        ChangeMenuEnum(defaultMenu.ToString());
       // FindObjectOfType<MenuPanelController>().ChangeMenuPanel();
    }

    private void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(es.currentSelectedGameObject);
        //Debug.Log(es.currentSelectedGameObject);
        //Screen Auto
        CheckRotation();
        CheckScreenRotationChanged();
        CreditScroll();
        CheckMenu();
        if (currentTween != null)
        {
            if (!currentTween.positionCurrentlyMoving || !currentTween.scaleCurrentlyMoving)
            {
                canChangePanels = true;

            }

            else
            {
                canChangePanels = false;

            }
        }

        else
        {
            canChangePanels = true;
        }
        if (t > 0)
        {
            t -= Time.unscaledDeltaTime;
        }

    }

    public void CloseMenuPanel()
    {





        if (currentTween != null)
        {
            t = currentTween.tweenDuration;
        }


        if (t <= 0)
        {
            sceneController.CloseMenu();
        }

    }


    // START OF ROTATION
    public void CheckRotation()
    {
        if (!hasChanged)
        {
            if (Screen.width > Screen.height)
            {
                isCurrentlyVertical = false;
                //  infiniteScroll.SetNewItems();
            }

            else
            {
                isCurrentlyVertical = true;


            }
        }

    }

    public void CheckScreenRotationChanged()
    {

        if (isCurrentlyVertical != previousVerticality)
        {
            previousVerticality = isCurrentlyVertical;
            //Enable and then disable layouts for credits
            resolutionScript.ChangeResolutionButton();

        }

    }

    public void SetCreditScroll()
    {

        creditsAutoScroll = false;
        inputCountDown = 2;

    }

    void CreditScroll()
    {
        //Controls the auto scroll countdown
        if (inputCountDown <= 0 && creditsAutoScroll == false)
        {
            creditsAutoScroll = true;
        }

        else if (inputCountDown > 0)
        {
            inputCountDown -= Time.unscaledDeltaTime;
        }

        //Controls the auto scroll logic
        //If the auto scroll countdown is finished and more than 10 credits exist. USAGE: Ideal for showcases.
        if (creditsAutoScroll && creditDetails.Length > 10)
        {
            creditReferences.creditsScrollGroup.verticalNormalizedPosition -= Time.unscaledDeltaTime * (creditsAutoScrollSpeed / 100);
            if (creditReferences.creditsScrollGroup.verticalNormalizedPosition <= .05f)
            {
                creditReferences.creditsScrollGroup.verticalNormalizedPosition = 1;
            }
        }


        if (moveAxis.y != 0)
        {
            creditReferences.creditsScrollGroup.verticalNormalizedPosition += moveAxis.y * Time.unscaledDeltaTime;
        }

    }

    //Tracks controller/mouse input
    public void OnMove(InputAction.CallbackContext context)
    {
        moveAxis = context.ReadValue<Vector2>();

    }

    public void SendMessage(string text)
    {
        Debug.Log(text);
    }

    public void AllowAutoRotation(bool allowState)
    {
        if (allowState == true)
        {
            Screen.orientation = ScreenOrientation.AutoRotation;

        }

        else
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;

        }


    }
    //END OF ROTATION

    //START OF GRAPHICS SETTINGS
    public void SetGraphicsOption(int selectedQualityInt)
    {
        QualitySettings.SetQualityLevel(selectedQualityInt, true);
    }

    public void UpdateQualityTextDisplay()
    {
        qualityText.text = qualitySettingNames[selectedQualityLevel];
    }
    public void IncreaseQuality()
    {
        //If the next possible number isn't higher than the array
        if (selectedQualityLevel + 1 < qualitySettingNames.Length)
        {
            selectedQualityLevel += 1;
            // SetResolution(resolutions[currentResolutionInt].height, resolutions[currentResolutionInt].width);
            UpdateQualityTextDisplay();
        }
    }

    public void DecreaseQuality()
    {
        //If the next possible number isn't  lower than the array
        if (selectedQualityLevel - 1 >= 0)
        {
            selectedQualityLevel -= 1;
            UpdateQualityTextDisplay();

        }
    }


    //END OF GRAPHICS SETTINGS
    public void QuitGameInDev()
    {
        Application.Quit();
    }
    public void ChangePauseState(bool newBool)
    {
        if (newBool == true)
        {
            gamePaused = true;
            Time.timeScale = 0;
        }

        else
        {
            gamePaused = false;
            Time.timeScale = 1;
        }
    }

    /// <summary>
    /// This changes the cursor state according to 
    /// 
    /// If the control scheme bools are not being changed, make sure this function is being called in 
    /// the unity event in the inspector attached to this script. Alternatively, it can be placed within 
    /// the TrackControlScheme() function (in 'InputTypeDetection' script) within its if statement.
    /// </summary>
    public void ChangeCursorState()
    {
        switch (inputTypeDetection.controlState)
        {
            //Effectively an if statement for 'if using a keyboard/mouse'
            case InputTypeDetection.ControlState.KeyboardAndMouse:
                //If the game is playing
                if (gameMode != GameMode.Paused)
                {
                    canShowCursor = showMouse.canShowCursorInGame;

                }

                else
                {
                    canShowCursor = showMouse.canShowCursorInMenu;

                }
                break;

            //Effectively an if statement for 'if using a controller'
            case InputTypeDetection.ControlState.Controller:

                canShowCursor = false;
                break;

            //Effectively an if statement for 'if using touch'
            case InputTypeDetection.ControlState.Touch:
                canShowCursor = true;
                break;

            default:
                break;
        }
        Cursor.visible = canShowCursor;
    }


    public void ChangeMenuEnum(string menuName)
    {
        Debug.Log("o");
        tempPossibleMenu = (PossibleMenu)System.Enum.Parse(typeof(PossibleMenu), menuName);//THIS IS WHERE THE MENU CHANGE INITIALLY STARTS
        if (currentMenu != tempPossibleMenu)
        {
            Debug.Log("ok dud");
          // previousMenu = currentMenu;
            
            currentMenu = tempPossibleMenu;
            onMenuChange.Invoke();
        }

    }


    public void CheckMenu()
    {

        if (currentMenu != tempPossibleMenu)
        {
           // onMenuChange.Invoke();
        }
#if UNITY_EDITOR
        else if(currentMenu != tempPossibleMenu)
        {
          //  previousMenu = tempPossibleMenu;
        }
#endif
       
    }

 

    public void ChangeToMenuEnum()
    {
        if (currentScene == "MainMenu")
        {
            currentMenu = PossibleMenu.MainMenu;
        }
        else
        {
            currentMenu = PossibleMenu.PauseMenu;

        }
    }

    public void ChangeOptionsPanel(OptionPanels selectedOptionMenu, bool toggleIsOn, CanvasGroup selectedCanvasGroup)
    {
        switch (selectedOptionMenu)
        {
            case OptionPanels.General:
                if (toggleIsOn)
                {
                    selectedCanvasGroup.alpha = 1;
                    selectedCanvasGroup.interactable = true;
                    selectedCanvasGroup.blocksRaycasts = true;
                }

                else
                {
                    selectedCanvasGroup.alpha = 0;
                    selectedCanvasGroup.interactable = false;
                    selectedCanvasGroup.blocksRaycasts = false;

                }
                break;
            case OptionPanels.Video:
                if (toggleIsOn)
                {
                    selectedCanvasGroup.alpha = 1;
                    selectedCanvasGroup.interactable = true;
                    selectedCanvasGroup.blocksRaycasts = true;

                }

                else
                {
                    selectedCanvasGroup.alpha = 0;
                    selectedCanvasGroup.interactable = false;
                    selectedCanvasGroup.blocksRaycasts = false;

                }
                break;
            case OptionPanels.Audio:
                if (toggleIsOn)
                {
                    selectedCanvasGroup.alpha = 1;
                    selectedCanvasGroup.interactable = true;
                    selectedCanvasGroup.blocksRaycasts = true;

                }

                else
                {
                    selectedCanvasGroup.alpha = 0;
                    selectedCanvasGroup.interactable = false;
                    selectedCanvasGroup.blocksRaycasts = false;

                }
                break;
            default:
                break;
        }
    }

    public void SetPauseInput(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
         /////////////   SetPause();
        }
    }

    public void SetPause()
    {
        //previousMenu = possibleMenu;

       // setPauseSettings.toPauseMenu.newMenuName = PossibleMenu.PauseMenu;
      //  setPauseSettings.toNoMenu.newMenuName = PossibleMenu.None;

        
        //SET PAUSE SHIT HERE ASAP
        if (currentMenu == PossibleMenu.None && setPauseSettings.pauseTweenInput != null)
        {
           
            if (currentMenu == PossibleMenu.MainMenu || currentMenu == PossibleMenu.PauseMenu)
            {
                currentMenu = setPauseSettings.toPauseMenu.newMenuName;
                currentMenu = setPauseSettings.toNoMenu.newMenuName;
            }

            else //if(possibleMenu != (PossibleMenu)System.Enum.Parse(typeof(PossibleMenu), menuName))
            {
                //possibleMenu = (PossibleMenu)System.Enum.Parse(typeof(PossibleMenu), menuName);

                setPauseSettings.toPauseMenu.newMenuName = currentMenu;
                setPauseSettings.toNoMenu.newMenuName = currentMenu;
            }
            //el
            setPauseSettings.pauseTweenInput.TriggerPositionTweenInput(setPauseSettings.toPauseMenu.tweenName);
        }

        else if (currentMenu != PossibleMenu.None && setPauseSettings.pauseTweenInput != null)
        {
           currentMenu = setPauseSettings.toPauseMenu.newMenuName;
            currentMenu = setPauseSettings.toNoMenu.newMenuName;
        }

        /* if (possibleMenu.ToString() != previousMenu && possibleMenus.Length != 0)
         {
             int index = 0;
             foreach (var item in collection)
             {
                 if (possibleMenus[index] == possibleMenu)
                 {
                     index++;
                 }
             }


         }

         */


    }

    public void SetSelectedMenuObject(string menuName)
    {
        ///////
        ///SetPause();
        Debug.LogWarning(currentMenu);
        switch ((PossibleMenu)System.Enum.Parse(typeof(PossibleMenu), menuName))
        {
            case PossibleMenu.None:
                if (firstSelectableObject.selectedObjectNoMenu != null)
                {
                    es.SetSelectedGameObject(firstSelectableObject.selectedObjectNoMenu);

                }
                else
                {
                    es.SetSelectedGameObject(null);
                    Debug.LogWarning("Couldn't find a reference for SetSelectedMenuObject on UIManager. Setting the selected object to nothing...");
                }
                break;
            case PossibleMenu.PauseMenu:
                if (firstSelectableObject.selectedObjectPauseOrMainMenu != null)
                {
                    es.SetSelectedGameObject(firstSelectableObject.selectedObjectPauseOrMainMenu);

                }
                else
                {
                    es.SetSelectedGameObject(null);
                    Debug.LogWarning("Couldn't find a reference for SetSelectedMenuObject on UIManager. Setting the selected object to nothing...");
                }
                break;
            case PossibleMenu.MainMenu:
                if (firstSelectableObject.selectedObjectPauseOrMainMenu != null)
                {
                    es.SetSelectedGameObject(firstSelectableObject.selectedObjectPauseOrMainMenu);

                }
                else
                {
                    es.SetSelectedGameObject(null);
                    Debug.LogWarning("Couldn't find a reference for SetSelectedMenuObject on UIManager. Setting the selected object to nothing...");
                }
                break;
            case PossibleMenu.Credits:
                if (firstSelectableObject.selectedObjectCredits != null)
                {
                    es.SetSelectedGameObject(firstSelectableObject.selectedObjectCredits);

                }
                else
                {
                    es.SetSelectedGameObject(null);
                    Debug.LogWarning("Couldn't find a reference for SetSelectedMenuObject on UIManager. Setting the selected object to nothing...");
                }
                break;
            case PossibleMenu.Options:
                if (firstSelectableObject.selectedObjectOptions != null)
                {
                    es.SetSelectedGameObject(firstSelectableObject.selectedObjectOptions);

                }
                else
                {
                    es.SetSelectedGameObject(null);
                    Debug.LogWarning("Couldn't find a reference for SetSelectedMenuObject on UIManager. Setting the selected object to nothing...");
                }
                break;
            default:
                break;
        }
    }

}
[System.Serializable]
public class AutoRotationHorizontal
{
    public int gridLeftOffsetHorizontal;

    public int gridRightOffsetHorizontal;

    public int gridTopOffsetHorizontal;

    public int gridBottomOffsetHorizontal;
}

[System.Serializable]
public class AutoRotationVertical
{
    public int gridLeftOffsetVertical;
    public int gridRightOffsetVertical;
    public int gridTopOffsetVertical;
    public int gridBottomOffsetVertical;
}

[System.Serializable]
public class CreditItem
{
    public string person;
    public string personRole;
}

[System.Serializable]
public class MenuButtonsAvailable
{
    public bool showEmptySpaceObjects;
    [Space(5)]
    public bool playGame;
    public bool creditsMenu;
    public bool optionsMenu;
    public bool quitGame;
    public MenuOptionsAvailable OptionsAvailable;
}


[System.Serializable]
public class MenuOptionsAvailable
{
    public ToggleGroupOptionsAvailable togglesAvailable;
    public VideoOptionsAvailable videoOptionsAvailable;
    public AudioOptionsAvailable audioOptionsAvailable;

}

[System.Serializable]
public class AudioOptionsAvailable
{
    public bool masterVolume;
    public bool gameVolume;
    public bool menuVolume;
    public bool musicVolume;
}

[System.Serializable]
public class ToggleGroupOptionsAvailable
{
    public bool generalOptions = false;
    public bool videoOptions = true;
    public bool audioOptions = true;
}

[System.Serializable]
public class VideoOptionsAvailable
{
    public bool graphicsPreset;
    public bool resolution;
    public bool fullScreen;
    public bool screenRotation;
}

[System.Serializable]
public class CreditReferences
{
    
    public GameObject creditPrefab;
    public Transform creditsHolder;
    [Space(10)]
    public ScrollRect creditsScrollGroup;
   
}

[System.Serializable]
public class GameSettings
{

    public GameObject creditPrefab;
    public Transform creditsHolder;
    [Space(10)]
    public ScrollRect creditsScrollGroup;

}
[System.Serializable]
public class ShowMouseInMenu
{
    public bool canShowCursorInGame;
    public bool canShowCursorInMenu;
}



[System.Serializable]
public class SetPauseSettings
{
    public TweenScript pauseTweenInput;
    public NoMenu toNoMenu;
    public Pause toPauseMenu;

}

[System.Serializable]
public class Pause
{
    public MenuLinker returnSetting;


    public UIManager.PossibleMenu newMenuName = UIManager.PossibleMenu.None;
    public string tweenName;

}

[System.Serializable]
public class NoMenu
{
    public MenuLinker returnSetting;
    [ReadOnlyInspector]
    public UIManager.PossibleMenu newMenuName;
    public string tweenName;

}

[System.Serializable]
public class FirstSelectableObject
{
    public GameObject selectedObjectNoMenu;
    public GameObject selectedObjectPauseOrMainMenu;
    public GameObject selectedObjectOptions;
    public GameObject selectedObjectCredits;

}









