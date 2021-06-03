using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// CODE BY TIGER COLLINS https://tigercollins.myportfolio.com/ , 
/// https://twitter.com/_TigerCollins , https://www.linkedin.com/in/tiger-collins-2b8619120/
/// Scripts used for the Menu Template package.
/// Feel free to modify or expand upon this script. Please credit myself if you redistribute this
/// script or use this asset in a project.
/// </summary>

/// <summary>
/// This script is to work as a passthrough for UnityEvents so multiple variables can be passed into
/// fucntions and other variable types such as enums can be used. This specific component targets
/// toggle groups triggering canvas groups.
/// </summary>

[RequireComponent(typeof(Toggle))]
public class TogglePanelComponent : MonoBehaviour
{
    /// <summary>
    /// Visible to the inspector
    /// </summary>
    
    [Header("MENU TEMPLATE - Choose which options menu to open")]

    [Space(20)]

    public UIManager.OptionPanels optionPanels;
    [SerializeField]
    [LabelOverride("Target Canvas Group")]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private ColourOptions colourOptions;



    /// <summary>
    /// Hidden to the inspector
    /// </summary>

    private UIManager uiManager;
    private Toggle toggle;


    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
        toggle = GetComponent<Toggle>();
        ChangePanel();
    }

    public void ChangePanel()
    {
        if(uiManager !=null)
        {
            uiManager.ChangeOptionsPanel(optionPanels,toggle.isOn,canvasGroup);

            if(toggle.isOn)
            {
                toggle.targetGraphic.color = colourOptions.selectedColour;
            }

            else
            {
                toggle.targetGraphic.color = colourOptions.unselectedColour;
            }
        }

        else
        {
            Debug.LogError("Could not find UI Manager for TogglePanelComponent. Add UIManager component to scene to resolve error");
        }
    }

}

[System.Serializable]
public class ColourOptions
{
    public Color selectedColour = Color.black;
    public Color unselectedColour = Color.white;
}