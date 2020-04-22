using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NexusStats : MonoBehaviour
{
    public int hp = 20;
    int initialHP;
    public Image lifebarUI;
    private SceneHandler sceneHandler;

    private void Start()
    {
        initialHP = hp;
        sceneHandler = GameObject.FindGameObjectWithTag("SceneHandler").GetComponent<SceneHandler>();
    }
    void Update()
    {
        lifebarUI.fillAmount = (1 / (float)initialHP) * (float)hp;

        if (hp <= 0)
        {
            Destroy(gameObject);
            sceneHandler.GoToNexusDeadScene();
        }
    }
}
