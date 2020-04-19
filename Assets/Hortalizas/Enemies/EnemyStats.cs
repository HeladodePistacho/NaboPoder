using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [HideInInspector]
    public Vector3 initPos;
    // Gameplay variables
    public float speed = 0f;
    public float sightRange = 0f;
    public float hp = 0f;
    public float damage = 0f;

    public void Start()
    {
        initPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
    }
}
