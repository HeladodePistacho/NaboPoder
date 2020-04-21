﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NexusStats : MonoBehaviour
{
    public int hp = 20;
    public SceneHandler sceneHandler;

    void Update()
    {
        if(hp <= 0)
        {
            Destroy(gameObject);
            sceneHandler.GoToNexusDeadScene();
        }
    }
}
