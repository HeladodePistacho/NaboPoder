using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public enum EnemyState
{
    IDLE,
    BACK_TO_INIT_POS,
    FOLLOWING_PLAYER,
    ATTACK_NEXUS,
    HIT_PLAYER
}

public class EnemyBehaviour : MonoBehaviour
{
    // private variables
    private EnemyStats enemyStats;
    private Vector3 initPosition;
    private Vector3 nexusPos;
    
    // public variables
    public EnemyState enemyState = EnemyState.IDLE;
    public HortalizaPathfinding pathfinding;
    

    Camera main;
    Transform player;

    //Animations
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        nexusPos = GameObject.FindGameObjectWithTag("Nexus").transform.position;
        initPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        main = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyStats = GetComponent<EnemyStats>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (enemyState)
        {
            case EnemyState.FOLLOWING_PLAYER:
                Update_FollowingPlayer();
                break;
            case EnemyState.BACK_TO_INIT_POS:
                Update_BackToInitialPos();
                break;
            case EnemyState.IDLE:
                Update_Idle();
                break;
            //case EnemyState.ATTACK_NEXUS:

        }
    }
    public void SetState_FollowPlayer()
    {
        enemyState = EnemyState.FOLLOWING_PLAYER;
        pathfinding.SetTarget(player, SetState_FollowPlayer);
        anim.SetBool("Moving", true);
    }
    public void SetState_BackToInitPos()
    {
        enemyState = EnemyState.BACK_TO_INIT_POS;
        pathfinding.SetTarget(initPosition, SetState_Idle);
        anim.SetBool("Moving", true);
    }
    public void SetState_Idle()
    {
        enemyState = EnemyState.IDLE;
        anim.SetBool("Moving", false);
    }
    void Update_FollowingPlayer()
    {
        if (Vector3.Distance(transform.position, player.position) > enemyStats.sightRange)
        {
            // Lose sight of the player and go back to init pos
            SetState_BackToInitPos();
        }
    }
    void Update_BackToInitialPos()
    {
        if (Vector3.Distance(transform.position, player.position) < enemyStats.sightRange)
        {
            SetState_FollowPlayer();
        }
    }
    void Update_Idle()
    {
        if (Vector3.Distance(transform.position, player.position) < enemyStats.sightRange)
        {
            SetState_FollowPlayer();
        }
    }
}
