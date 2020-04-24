using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllahuAkbarEnemyBehaviour : MonoBehaviour
{
    // private variables
    private EnemyStats enemyStats;
    private GameObject nexus;

    // public variables
    public HortalizaPathfinding pathfinding;

    Camera main;
    Transform player;

    //Animations
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        nexus = GameObject.FindGameObjectWithTag("Nexus");
        pathfinding.SetTarget(nexus.transform, AttackNexus);
        anim.SetBool("Moving", true);
    }

    void AttackNexus()
    {
        // Deprecated but needed to SetTarget method
        return;
    }
}
