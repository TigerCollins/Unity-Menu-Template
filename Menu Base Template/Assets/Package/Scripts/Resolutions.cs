using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class Resolutions : MonoBehaviour
{
    [Header("Saving details")]
    [SerializeField]
    private PlayerData playerData;

    [Header("Script References")]
    [SerializeField]
    private UIManager uIManager;
   

    [Tooltip("Every resolutions supported by this device")]
    [SerializeField]
    public Resolution[] resolutions;
    [SerializeField]
    private bool platformAllowsForChange; //If the platform doesnt let the play change the res.

    [Header("UI Elements")]
    [SerializeField]
    private TextMeshProUGUI resolutionDisplay;

    [Header("Settings")]
    [SerializeField]
    private bool fullscreen;
    [SerializeField]
    private int currentResolutionInt;
    public int selectedResolutionInt;

    [Header("Resolution Scroll")]
    [SerializeField]
    private GameObject resolutionTogglePrefab;
    [SerializeField]
    private Transform resolutionPrefabHolder;
    [Space(4)]

    [SerializeField]
    private RectTransform contentPanel;
    [SerializeField]
    private ScrollRect scrollRect;
    private RectTransform currentScrollTarget;

    [SerializeField]
    private float timeOfTravel;
    private float currentTime;
    private float normalizedValue;

    public int screenResLength;

    private bool canChangeRes;
    private int timesResChange = 0;
    private bool changingScenes;


    public int valueChangeCount = 0;

    private float previousWidth;
    private float previousHeight;
    [SerializeField]
    private UnityEvent onResolutionChange;




    public enum DisplayModes { Unknown, Fullscreen, Borderless, Windowed }

    public void Awake()
    {
        //Grabs possible resolutions
        resolutions = Screen.resolutions;


        //Counts how many resolutions can be used
        screenResLength = resolutions.Length -1;

        //If the resolution has been set previously
        if (playerData.resolutionHeight != 0 || playerData.resolutionWidth != 0)
        {
            //checks each resolution
            for (int i = 0; i < resolutions.Length; i++)
            {
               
                //set the resolution to be the first one that was last used
                if (resolutions[i].height == playerData.resolutionHeight && resolutions[i].width == playerData.resolutionWidth)
                {
                    
                    currentResolutionInt = i;
                    selectedResolutionInt=i;
                    resolutionDisplay.text = resolutions[currentResolutionInt].width + " x " + resolutions[currentResolutionInt].height + " @" + resolutions[currentResolutionInt].refreshRate + "Hz";
                    SetResolution(playerData.resolutionWidth, playerData.resolutionHeight);
                    break;
                }


            }
        }


        int index = 0;
        for (int i = 0; i < resolutions.Length ; i++)
        {
            Instantiate(resolutionTogglePrefab, resolutionPrefabHolder);
            index++;
            if(index >= resolutions.Length)
            {
              //  resolutionScrollController.startInfo.index = currentResolutionInt;
               // resolutionScrollController.UpdateLayout();
               // resolutionScrollController.scrollRect.horizontalNormalizedPosition = 0;
            }
        }
        currentScrollTarget = contentPanel.GetChild(currentResolutionInt).GetComponent<RectTransform>();
        IncreaseResolution();
        DecreaseResolution();
    }

    public void SetFullscreen(bool newBool)
    {
        fullscreen = newBool;
        SetResolution(Screen.width, Screen.height);
    }

    public void UpdateTextDisplay()
    {
        //Updates the text
        resolutionDisplay.text = resolutions[selectedResolutionInt].width + " x " + resolutions[selectedResolutionInt].height + " @" + resolutions[selectedResolutionInt].refreshRate + "Hz";
   
    }

    public void IncreaseResolution()
    {
        //If the next possible number isn't higher than the array
        if(selectedResolutionInt + 1 < resolutions.Length)
        {
            selectedResolutionInt += 1;
            
            UpdateTextDisplay();

        }
    }



    public void DecreaseResolution()
    {
        //If the next possible number isn't  lower than the array
        if (selectedResolutionInt - 1 >= 0)
        {
            selectedResolutionInt -= 1;
    
            UpdateTextDisplay();


        }
    }
    public void ChangeResolutionButton()
    {
        playerData.resolutionHeight = resolutions[selectedResolutionInt].height;
        playerData.resolutionWidth = resolutions[selectedResolutionInt].width;
        if (uIManager.isCurrentlyVertical == true)
        {
            SetResolution(playerData.resolutionHeight, playerData.resolutionWidth);
          
        }

        else
        {
            SetResolution(playerData.resolutionWidth, playerData.resolutionHeight);

        }

    }
    public void SetResolution(int resolutionX, int resolutionY)
    {


       // if (canChangeRes)
        //{
    
                Screen.SetResolution(resolutionX, resolutionY, fullscreen);
                playerData.resolutionWidth = resolutionX;
                playerData.resolutionHeight = resolutionY;
                //("On the scene called " + SceneManager.GetActiveScene().name + " the resolution changed");

        // }

    }

    string ResToString(Resolution res)
    {
        return res.width + " x " + res.height + " @ " + res.refreshRate + "Hz";
    }

    public void IsChangingScenes()
    {
        if (!changingScenes)
        {
            changingScenes = true;
        }

        else
        {
            changingScenes = false;
        }
    }

    public void Update()
    {
        if(Screen.width != previousWidth || Screen.height != previousHeight)
        {
            onResolutionChange.Invoke();
            previousWidth = Screen.width;
            previousHeight = Screen.height;
        }
    }


}


