using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NexusStats : MonoBehaviour
{
    public int hp = 20;
    private SceneHandler sceneHandler;

    private void Start()
    {
        sceneHandler = GameObject.FindGameObjectWithTag("SceneHandler").GetComponent<SceneHandler>();
    }
    void Update()
    {
        if(hp <= 0)
        {
            Destroy(gameObject);
            sceneHandler.GoToNexusDeadScene();
        }
    }
}
