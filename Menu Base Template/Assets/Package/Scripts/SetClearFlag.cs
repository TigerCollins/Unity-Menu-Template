using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetClearFlag : MonoBehaviour
{
    [SerializeField]
    private Camera pauseCamera;

    void Awake()
    {
        //If the scene is loaded as an additive scene and not by itself
        if (SceneManager.sceneCount > 1)
        {
            //Hides background
            pauseCamera.clearFlags = CameraClearFlags.Nothing;
        }
        
    }

}
