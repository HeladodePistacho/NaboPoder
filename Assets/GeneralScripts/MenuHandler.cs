using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuHandler : MonoBehaviour
{
    public enum GoToScene
    {
        MENU,
        GAME_SCENE
    }

    private SceneHandler sceneHandler;
    public TextMeshProUGUI anyKeyText;
    public GoToScene nextScene;
    public int waitSeconds = 4;

    private bool readyToChangeScene = false;
    private float timeLerp = 0f;

    private void Start()
    {
        sceneHandler = GameObject.FindGameObjectWithTag("SceneHandler").GetComponent<SceneHandler>();
        timeLerp = 0f;
        StartCoroutine(FadeText());
    }
    void Update()
    {
        timeLerp += Time.deltaTime*0.2f;
        anyKeyText.color = Color.Lerp(Color.black, Color.white, timeLerp);

        if (Input.anyKey && readyToChangeScene)
        {
            if (nextScene == GoToScene.MENU)
                sceneHandler.GotoMenu();
            else if (nextScene == GoToScene.GAME_SCENE)
                sceneHandler.GotoInGame();
        }
    }
    IEnumerator FadeText()
    {
        yield return new WaitForSeconds(waitSeconds);
        readyToChangeScene = true;
    }
}
