using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    // Variables
    public static SceneHandler instance = null;
    private static string sceneToLoad = "";

    void Awake()
    {
        Debug.Log("AWAKE");
        DontDestroyOnLoad(this.gameObject);

        if (instance != null)
            Destroy(gameObject);
        else
            instance = new SceneHandler();
        if (instance != null)
            Debug.Log("INSTANCE CREATED");
    }

    // Getters
    public static SceneHandler GetInstance()
    {
        return instance;
    }

    public string GetSceneToLoad()
    {
        return sceneToLoad;
    }

    // Save scene to load
    public void GotoMenu()
    {
        sceneToLoad = "Menu";
        LoadingScene();
    }
    public void GotoInGame()
    {
        sceneToLoad = "InGame";
        LoadingScene();
    }
    public void GoToDeadScene()
    {
        sceneToLoad = "DeadScene";
        LoadingScene();
    }
    public void GoToNexusDeadScene()
    {
        sceneToLoad = "NexusDeadScene";
        LoadingScene();
    }
    // Load saved scene
    private void LoadingScene()
    {
        SceneManager.LoadScene("LoadScene");
    }
}
