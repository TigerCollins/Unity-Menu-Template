using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private int inputTriggered;
    public string menuSceneName;

    public void Awake()
    {
        menuSceneName = SceneManager.GetActiveScene().ToString();
    }

    public void ChangeMenuScene(string SceneName)
    {
        inputTriggered++;
        if (inputTriggered >= 3 && SceneManager.GetSceneByName(SceneName).isLoaded==false)
        {
            SceneManager.LoadScene(SceneName, LoadSceneMode.Additive);
            menuSceneName = SceneName;
        }
    }
    public void ChangeGameScene(string SceneName)
    {
        inputTriggered++;
        SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
        Debug.Log("Loading " + SceneName + " currently...");
    }

    public void CloseMenu()
    {
        SceneManager.UnloadSceneAsync("Main Menu");
    }

    public static void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif

    }
}
