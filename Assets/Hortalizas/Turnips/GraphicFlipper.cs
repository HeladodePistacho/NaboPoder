using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class GraphicFlipper : MonoBehaviour
{
    public AIPath aiPath;
    Vector3 left = new Vector3(1f, 1f, 1f);
    Vector3 right = new Vector3(-1f, 1f, 1f);

    // Update is called once per frame
    void Update()
    {
        if(aiPath.desiredVelocity.x >= 0.05)
        {
            transform.localScale = right;
        }
        else if (aiPath.desiredVelocity.x <= -0.05)
        {
            transform.localScale = left;
        }
    }
}
