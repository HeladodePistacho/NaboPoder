using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AsyncLoadScript : MonoBehaviour
{
    // Variables
    private int frameCounter = 0;

    void Start()
    {
        // Clean Garbage Colector
        System.GC.Collect();
        StartCoroutine(LoadAsyncScene());
    }
    void Update()
    {
        Debug.Log(frameCounter++);
    }
    IEnumerator LoadAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneHandler.GetInstance().GetSceneToLoad());

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
