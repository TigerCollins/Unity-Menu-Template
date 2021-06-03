using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GraphicsPresetToggle : MonoBehaviour
{
    [SerializeField]
    private Toggle toggle;
    [SerializeField]
    private bool canAdjustNavigation;
    public bool loopScrollGroup;
    public CanvasGroup canvasGroup;

    [SerializeField]
    private UIManager uiScript;
    [SerializeField]
    private int selectedID;
    [SerializeField]
    private TextMeshProUGUI textMesh;
    [SerializeField]
    private ControllerSnapInstantiated controllerSnapInstantiated;
    // private NavigationObjects navigationObjects;

    private Navigation newNavigation = new Navigation();
    public Button tempButton;

    // Start is called before the first frame update
    void Start()
    {
        if (transform.parent.parent.parent.TryGetComponent(out ToggleGroup toggleGroup))
        {
            toggle.group = toggleGroup;
        }
        uiScript = FindObjectOfType<UIManager>();
        selectedID = transform.GetSiblingIndex();
        textMesh.text = uiScript.qualitySettingNames[selectedID];
        FindObjectOfType<UIManager>().onMenuChange.AddListener(ChangeNavigation);
    }

    public void ChangeNavigation()
    {

        if (controllerSnapInstantiated != null)
        {
            int siblingCount = transform.parent.childCount - 1;
            int thisSiblingNumber = transform.GetSiblingIndex();

            if (thisSiblingNumber == 0)
            {
                CreateButton();
                if (tempButton != null)
                {
                    if (controllerSnapInstantiated.orientation == ControllerScrollSnap.Orientation.vertical)
                    {
                        newNavigation.mode = Navigation.Mode.Explicit;
                        newNavigation.selectOnLeft = tempButton.FindSelectableOnLeft();
                        if (transform.parent.GetChild(1).TryGetComponent(out Toggle newToggle))
                        {
                            newNavigation.selectOnDown = newToggle;
                        }
                        newNavigation.selectOnRight = tempButton.FindSelectableOnRight();
                        if (toggle.navigation.wrapAround == true && toggle.navigation.mode == Navigation.Mode.Vertical || loopScrollGroup == true)
                        {
                            newNavigation.selectOnUp = transform.parent.GetChild(thisSiblingNumber).GetComponent<Toggle>();
                        }
                        toggle.navigation = newNavigation;
                    }

                    else
                    {
                        newNavigation.mode = Navigation.Mode.Explicit;

                        newNavigation.selectOnUp = tempButton.FindSelectableOnUp();
                        newNavigation.selectOnDown = tempButton.FindSelectableOnDown();
                        if (transform.parent.GetChild(1).TryGetComponent(out Toggle newToggle))
                        {
                            newNavigation.selectOnRight = newToggle;
                        }
                        if (toggle.navigation.wrapAround == true && toggle.navigation.mode == Navigation.Mode.Horizontal || loopScrollGroup == true)
                        {
                            newNavigation.selectOnLeft = transform.parent.GetChild(thisSiblingNumber).GetComponent<Toggle>();
                        }
                        toggle.navigation = newNavigation;
                    }
                }
            }


            else if (thisSiblingNumber == siblingCount)
            {
                CreateButton();
                if (tempButton != null)
                {
                    if (controllerSnapInstantiated.orientation == ControllerScrollSnap.Orientation.vertical)
                    {
                        newNavigation.mode = Navigation.Mode.Explicit;
                        newNavigation.selectOnLeft = tempButton.FindSelectableOnLeft();
                        if (transform.parent.GetChild(siblingCount - 1).TryGetComponent(out Toggle newToggle))
                        {
                            newNavigation.selectOnUp = newToggle;
                        }
                        newNavigation.selectOnRight = tempButton.FindSelectableOnRight();
                        if (toggle.navigation.wrapAround == true && toggle.navigation.mode == Navigation.Mode.Vertical || loopScrollGroup == true)
                        {
                            newNavigation.selectOnDown = transform.parent.GetChild(0).GetComponent<Toggle>();
                        }
                        toggle.navigation = newNavigation;
                    }

                    else
                    {
                        newNavigation.mode = Navigation.Mode.Explicit;

                        newNavigation.selectOnUp = tempButton.FindSelectableOnUp();
                        newNavigation.selectOnDown = tempButton.FindSelectableOnDown();
                        if (transform.parent.GetChild(siblingCount - 1).TryGetComponent(out Toggle newToggle))
                        {
                            newNavigation.selectOnLeft = newToggle;
                        }
                        if (toggle.navigation.wrapAround == true && toggle.navigation.mode == Navigation.Mode.Horizontal || loopScrollGroup == true)
                        {
                            newNavigation.selectOnRight = transform.parent.GetChild(0).GetComponent<Toggle>();
                        }
                        toggle.navigation = newNavigation;
                    }
                }
            }
        }

        else if (transform.parent.TryGetComponent(out ControllerScrollSnap scrollSnap))
        {
            int siblingCount = transform.parent.childCount - 1;
            int thisSiblingNumber = transform.GetSiblingIndex();

            if (thisSiblingNumber == 0)
            {
                CreateButton();
                if (tempButton != null)
                {
                    if (scrollSnap.scrollOrientation == ControllerScrollSnap.Orientation.vertical)
                    {
                        newNavigation.mode = Navigation.Mode.Explicit;
                        newNavigation.selectOnLeft = tempButton.FindSelectableOnLeft();
                        if (transform.parent.GetChild(1).TryGetComponent(out Toggle newToggle))
                        {
                            newNavigation.selectOnDown = newToggle;
                        }
                        newNavigation.selectOnRight = tempButton.FindSelectableOnRight();
                        if (toggle.navigation.wrapAround == true && toggle.navigation.mode == Navigation.Mode.Vertical || loopScrollGroup == true)
                        {
                            newNavigation.selectOnUp = transform.parent.GetChild(thisSiblingNumber).GetComponent<Toggle>();
                        }
                        toggle.navigation = newNavigation;
                    }

                    else
                    {
                        newNavigation.mode = Navigation.Mode.Explicit;

                        newNavigation.selectOnUp = tempButton.FindSelectableOnUp();
                        newNavigation.selectOnDown = tempButton.FindSelectableOnDown();
                        if (transform.parent.GetChild(1).TryGetComponent(out Toggle newToggle))
                        {
                            newNavigation.selectOnRight = newToggle;
                        }
                        if (toggle.navigation.wrapAround == true && toggle.navigation.mode == Navigation.Mode.Horizontal || loopScrollGroup == true)
                        {
                            newNavigation.selectOnLeft = transform.parent.GetChild(thisSiblingNumber).GetComponent<Toggle>();
                        }
                        toggle.navigation = newNavigation;
                    }
                }
            }


            else if (thisSiblingNumber == siblingCount)
            {
                CreateButton();
                if (tempButton != null)
                {
                    if (scrollSnap.scrollOrientation == ControllerScrollSnap.Orientation.vertical)
                    {
                        newNavigation.mode = Navigation.Mode.Explicit;
                        newNavigation.selectOnLeft = tempButton.FindSelectableOnLeft();
                        if (transform.parent.GetChild(siblingCount - 1).TryGetComponent(out Toggle newToggle))
                        {
                            newNavigation.selectOnUp = newToggle;
                        }
                        newNavigation.selectOnRight = tempButton.FindSelectableOnRight();
                        if (toggle.navigation.wrapAround == true && toggle.navigation.mode == Navigation.Mode.Vertical || loopScrollGroup == true)
                        {
                            newNavigation.selectOnDown = transform.parent.GetChild(0).GetComponent<Toggle>();
                        }
                        toggle.navigation = newNavigation;
                    }

                    else
                    {
                        newNavigation.mode = Navigation.Mode.Explicit;

                        newNavigation.selectOnUp = tempButton.FindSelectableOnUp();
                        newNavigation.selectOnDown = tempButton.FindSelectableOnDown();
                        if (transform.parent.GetChild(siblingCount - 1).TryGetComponent(out Toggle newToggle))
                        {
                            newNavigation.selectOnLeft = newToggle;
                        }
                        if (toggle.navigation.wrapAround == true && toggle.navigation.mode == Navigation.Mode.Horizontal || loopScrollGroup == true)
                        {
                            newNavigation.selectOnRight = transform.parent.GetChild(0).GetComponent<Toggle>();
                        }
                        toggle.navigation = newNavigation;
                    }
                }
            }
        }




        else
        {
            Debug.LogError("Could not find ControllerScrollSnap or ControllerSnapInstantiated components. Please assign missing variables.");
        }



        if (transform.parent.parent.parent.TryGetComponent(out Button button))
        {
            Destroy(button);
        }

    }


    // Update is called once per frame
    public void UpdateSelectedID()
    {
        if(uiScript !=null)
        {
            uiScript.selectedQualityLevel = selectedID;

        }

    }

    public void CreateButton()
    {
        if (transform.parent.parent.parent.TryGetComponent(out Button button))
        {
            tempButton = button;
        }
        else
        {
            tempButton = transform.parent.parent.parent.gameObject.AddComponent<Button>();
        }
        tempButton.interactable = false;
    }
}




