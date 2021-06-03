using UnityEditor;
using UnityEngine;

//CONTROLS USABLE MENU ELEMENTS. ONLY IN THE UNITYEDITOR.
[ExecuteInEditMode]
public class MenuElementController : MonoBehaviour
{


    [SerializeField]
    [LabelOverride("UI Manager Script")]
    private UIManager uiManager;
    [Space(10)]
    [SerializeField]
    private GameObject controlDisplay;


    [Header("Menu Groupings")]
    [SerializeField]
    private GameObject playGame;
    [SerializeField]
    private GameObject[] optionGroupObject;
    [SerializeField]
    private GameObject[] creditsGroupObject;
    [SerializeField]
    private GameObject[] emptySpaceObject;
    [SerializeField]
    private GameObject quitButton;

    [Header("Option Groupings")]
  
    public OptionGroups optionGrouping;
    

    #if UNITY_EDITOR
    void Update()
    {
        //Only do if the game is not running
        if(!EditorApplication.isPlaying)
        {
            ChangeMenuGroups();
            ChangeAudioGroups();
            ChangeOptionsGroups();
            ChangeToggleHeaderGroups();
        }

    }

    public void ChangeAudioGroups()
    {
        //Game volume
        if (uiManager.menuButtonsAvailable.OptionsAvailable.audioOptionsAvailable.gameVolume == false && optionGrouping.audioGroups.gameVolumeObject.activeInHierarchy == true)
        {
            optionGrouping.audioGroups.gameVolumeObject.SetActive(false);
        }

        else if (uiManager.menuButtonsAvailable.OptionsAvailable.audioOptionsAvailable.gameVolume == true)
        {
            optionGrouping.audioGroups.gameVolumeObject.SetActive(true);
        }

         //Master volume
        if (uiManager.menuButtonsAvailable.OptionsAvailable.audioOptionsAvailable.masterVolume == false && optionGrouping.audioGroups.masterVolumeObject.activeInHierarchy == true)
        {
            optionGrouping.audioGroups.masterVolumeObject.SetActive(false);
        }

        else if (uiManager.menuButtonsAvailable.OptionsAvailable.audioOptionsAvailable.masterVolume == true)
        {
            optionGrouping.audioGroups.masterVolumeObject.SetActive(true);
        }

         //Music volume
        if (uiManager.menuButtonsAvailable.OptionsAvailable.audioOptionsAvailable.musicVolume == false && optionGrouping.audioGroups.musicVolumeObject.activeInHierarchy == true)
        {
            optionGrouping.audioGroups.musicVolumeObject.SetActive(false);
        }

        else if (uiManager.menuButtonsAvailable.OptionsAvailable.audioOptionsAvailable.musicVolume == true)
        {
            optionGrouping.audioGroups.musicVolumeObject.SetActive(true);
        }

         //Menu volume
        if (uiManager.menuButtonsAvailable.OptionsAvailable.audioOptionsAvailable.menuVolume == false && optionGrouping.audioGroups.menuVolumeObject.activeInHierarchy == true)
        {
            optionGrouping.audioGroups.menuVolumeObject.SetActive(false);
        }

        else if (uiManager.menuButtonsAvailable.OptionsAvailable.audioOptionsAvailable.menuVolume == true)
        {
            optionGrouping.audioGroups.menuVolumeObject.SetActive(true);
        }
    }
    public void ChangeMenuGroups()
    {
        //Play game or resume button
        if (uiManager.menuButtonsAvailable.playGame == false && playGame.activeInHierarchy == true)
        {
            playGame.SetActive(false);
        }

        else if(uiManager.menuButtonsAvailable.playGame == true)
        {
            playGame.SetActive(true);
        }

        //Options Group and button
        if (optionGroupObject.Length != 0)
        {
            foreach (GameObject option in optionGroupObject)
            {
                if(option !=null)
                {
                    if (uiManager.menuButtonsAvailable.optionsMenu == false && option.activeInHierarchy == true)
                    {
                        option.SetActive(false);
                    }

                    else if (uiManager.menuButtonsAvailable.optionsMenu == true)
                    {
                        option.SetActive(true);
                    }
                }
               
            }
        }
        

        //Empty objects
        if (emptySpaceObject.Length != 0)
        {
            foreach (GameObject spacer in emptySpaceObject)
            {
                if(spacer != null)
                {
                    if (uiManager.menuButtonsAvailable.showEmptySpaceObjects == false && spacer.activeInHierarchy == true)
                    {
                        spacer.SetActive(false);
                    }

                    else if (uiManager.menuButtonsAvailable.showEmptySpaceObjects == true)
                    {
                        spacer.SetActive(true);
                    }
                }
                
            }
        }
       

        //Credits Group and button
        if(creditsGroupObject.Length != 0)
        {
            foreach (GameObject credit in creditsGroupObject)
            {
                if(credit != null)
                {
                    if (uiManager.menuButtonsAvailable.creditsMenu == false && credit.activeInHierarchy == true)
                    {
                        credit.SetActive(false);
                    }

                    else if (uiManager.menuButtonsAvailable.creditsMenu == true)
                    {
                        credit.SetActive(true);
                    }
                }
               
            }
        }
        

        //Quit game or to menu button
        if (uiManager.menuButtonsAvailable.quitGame == false && quitButton.activeInHierarchy == true)
        {
            quitButton.SetActive(false);
        }

        else if(uiManager.menuButtonsAvailable.quitGame == true)
        {
            quitButton.SetActive(true);
        }
    }

    public void ChangeOptionsGroups()
    {
        //Full Screen
        if (uiManager.menuButtonsAvailable.OptionsAvailable.videoOptionsAvailable.fullScreen == false && optionGrouping.videoGroups.fullScreenObject.activeInHierarchy == true)
        {
            optionGrouping.videoGroups.fullScreenObject.SetActive(false);
        }

       else if (uiManager.menuButtonsAvailable.OptionsAvailable.videoOptionsAvailable.fullScreen == true)
        {
            optionGrouping.videoGroups.fullScreenObject.SetActive(true);
        }

        //Screen Rotation
        if (uiManager.menuButtonsAvailable.OptionsAvailable.videoOptionsAvailable.screenRotation == false && optionGrouping.videoGroups.autoRotateObject.activeInHierarchy == true)
        {
            optionGrouping.videoGroups.autoRotateObject.SetActive(false);
        }

        else if (uiManager.menuButtonsAvailable.OptionsAvailable.videoOptionsAvailable.screenRotation == true)
        {
            optionGrouping.videoGroups.autoRotateObject.SetActive(true);
        }

        //Resolution
        if (uiManager.menuButtonsAvailable.OptionsAvailable.videoOptionsAvailable.resolution == false && optionGrouping.videoGroups.resolutionObject.activeInHierarchy == true)
        {
            optionGrouping.videoGroups.resolutionObject.SetActive(false);
        }

        else if (uiManager.menuButtonsAvailable.OptionsAvailable.videoOptionsAvailable.resolution == true)
        {
            optionGrouping.videoGroups.resolutionObject.SetActive(true);
        }

        //Graphics Preset
        if (uiManager.menuButtonsAvailable.OptionsAvailable.videoOptionsAvailable.graphicsPreset == false && optionGrouping.videoGroups.graphicsPresetObject.activeInHierarchy == true)
        {
            optionGrouping.videoGroups.graphicsPresetObject.SetActive(false);
        }

        else if (uiManager.menuButtonsAvailable.OptionsAvailable.videoOptionsAvailable.graphicsPreset == true)
        {
            optionGrouping.videoGroups.graphicsPresetObject.SetActive(true);
        }
    }

    public void ChangeToggleHeaderGroups()
    {
        //General
        if (uiManager.menuButtonsAvailable.OptionsAvailable.togglesAvailable.generalOptions == false && optionGrouping.toggleHeaders.generalOptionsToggle.activeInHierarchy == true)
        {
            optionGrouping.toggleHeaders.generalOptionsToggle.SetActive(false);
        }

        else if (uiManager.menuButtonsAvailable.OptionsAvailable.togglesAvailable.generalOptions == true)
        {
            optionGrouping.toggleHeaders.generalOptionsToggle.SetActive(true);
        }

        //Video
        if (uiManager.menuButtonsAvailable.OptionsAvailable.togglesAvailable.videoOptions == false && optionGrouping.toggleHeaders.videoOptionsToggle.activeInHierarchy == true)
        {
            optionGrouping.toggleHeaders.videoOptionsToggle.SetActive(false);
        }

        else if (uiManager.menuButtonsAvailable.OptionsAvailable.togglesAvailable.videoOptions == true)
        {
            optionGrouping.toggleHeaders.videoOptionsToggle.SetActive(true);
        }

        //Audio
        if (uiManager.menuButtonsAvailable.OptionsAvailable.togglesAvailable.audioOptions == false && optionGrouping.toggleHeaders.audioOptionsToggle.activeInHierarchy == true)
        {
            optionGrouping.toggleHeaders.audioOptionsToggle.SetActive(false);
        }

        else if (uiManager.menuButtonsAvailable.OptionsAvailable.togglesAvailable.audioOptions == true)
        {
            optionGrouping.toggleHeaders.audioOptionsToggle.SetActive(true);
        }

    }
#endif
}

[System.Serializable]
public class OptionGroups
{
    public VideoOptions videoGroups;
    public ToggleHeaders toggleHeaders;
    public AudioGroups audioGroups;
}

[System.Serializable]
public class ToggleHeaders
{
    public GameObject generalOptionsToggle;
    public GameObject videoOptionsToggle;
    public GameObject audioOptionsToggle;
}

[System.Serializable]
public class VideoOptions
{
    public GameObject fullScreenObject;
    public GameObject autoRotateObject;
    public GameObject resolutionObject;
    public GameObject graphicsPresetObject;

}

[System.Serializable]
public class AudioGroups
{
    public GameObject masterVolumeObject;
    public GameObject gameVolumeObject;
    public GameObject musicVolumeObject;
    public GameObject menuVolumeObject;
}







