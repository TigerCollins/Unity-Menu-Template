using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SetSelectedObject : MonoBehaviour
{
    public bool dontSelectObject;

    [SerializeField]
    private GameObject newSelectedObject;
    private MouselessGeneralControl mouselessGeneralControl;
    // Start is called before the first frame update
    void Awake()
    {
        mouselessGeneralControl = FindObjectOfType<MouselessGeneralControl>();
    }

    // Update is called once per frame


    /// <summary>
    /// This function is for setting the SelectedGameObject through Unity Event - It simply changes the 
    /// selected object by calling the <see cref="NavigationControl(MoveDirection)"/> and changing 
    /// the <see cref="latestSelectedObject"/> is changed to this functions GameObject argument. 
    /// </summary>
    public void SetSelectedGameObject()
    {
        if (mouselessGeneralControl != null)
        {
            if (newSelectedObject != null && mouselessGeneralControl.canActivate == true || !dontSelectObject && mouselessGeneralControl.canActivate == true)
            {
                mouselessGeneralControl.latestSelectedObject = newSelectedObject;
                mouselessGeneralControl.NavigationControl(MoveDirection.None);
            }

            else if (mouselessGeneralControl.canActivate == true)
            {
                mouselessGeneralControl.latestSelectedObject = null;
                mouselessGeneralControl.NavigationControl(MoveDirection.None);
                Debug.LogWarning("Couldn't find a GameObject for SetSelectedGameObject on " + gameObject + ". Nothing will be selected...");
            }
        }

        else
        {
            Debug.LogError("Couldn't find MouselessGeneralControl script on " + gameObject + ". Please add required script to scene.");

        }

    }
}
