using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public enum EnemyState
{
    IDLE,
    BACK_TO_INIT_POS,
    FOLLOWING_PLAYER,
    HIT_PLAYER
}

public class EnemyBehaviour : MonoBehaviour
{
    private EnemyStats enemyStats;
    private Vector3 initPosition;

    public EnemyState enemyState = EnemyState.IDLE;
    public HortalizaPathfinding pathfinding;

    Camera main;
    Transform player;

    //Animations
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
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
            case EnemyState.HIT_PLAYER:
                Update_HitPlayer();
                break;
        }
    }
    public void SetState_FollowPlayer()
    {
        enemyState = EnemyState.FOLLOWING_PLAYER;
        pathfinding.SetTarget(player, SetState_Attack);
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
    public void SetState_Attack()
    {
        enemyState = EnemyState.HIT_PLAYER;
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
        if (Vector3.Distance(transform.position, player.position) > enemyStats.sightRange)
        {
            SetState_FollowPlayer();
        }
    }
    void Update_HitPlayer()
    {
        // TODO: Hit player
        if (Vector3.Distance(transform.position, player.position) < enemyStats.sightRange)
        {
            SetState_FollowPlayer();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (enemyState == EnemyState.FOLLOWING_PLAYER && collision.collider.CompareTag("Player"))
        {
            // Hit player
        }
    }
}
