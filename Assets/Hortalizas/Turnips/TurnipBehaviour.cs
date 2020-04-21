using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public enum TurnipState
{
    IDLE,
    GOING_TO,
    FOLLOWING_PLAYER,
    WAITING_FOR_PLAYER
}


public class TurnipBehaviour : MonoBehaviour
{
    public LayerMask layerForWaitingForPlayer;

    public float maxDistanceToPlayer = 10f;

    public TurnipState turnipState = TurnipState.IDLE;

    public HortalizaPathfinding pathfinding;

    Camera main;
    //PossibleTargets
    Transform player;

    //Animations
    public Animator anim;
    [HideInInspector]
    public bool selected = false;
    public GameObject deadParticles;

    // Start is called before the first frame update
    void Start()
    {
        //TODO: search for player
        main = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player").transform;        
    }

    // Update is called once per frame
    void Update()
    {
         switch (turnipState)
         {
             case TurnipState.WAITING_FOR_PLAYER:
                Update_WaitingForPlayer();
                 break;
         }

        
    }

    public void SetState_FollowPlayer()
    {
        turnipState = TurnipState.FOLLOWING_PLAYER;
        pathfinding.SetTarget(player, OnPlayerReached);       
        anim.SetBool("Moving", true);
        gameObject.layer = LayerMask.NameToLayer("Turnips_FollowingPlayer");
    }

    void OnPlayerReached()
    {
       // Debug.Log("reachedPlayer");
        turnipState = TurnipState.WAITING_FOR_PLAYER;
        anim.SetBool("Moving", false);
    }

    void Update_WaitingForPlayer()
    {
        if(Vector3.Distance(transform.position, player.position)> maxDistanceToPlayer)
        {
            Vector2 direction = player.position - transform.position;

            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction.normalized, 0.3f, layerForWaitingForPlayer);
            if(hits.Length <= 1)
                SetState_FollowPlayer();
        }
    }

    public void SetTargetPosition(Vector3 _targetPos)
    {
        turnipState = TurnipState.GOING_TO;
        pathfinding.SetTarget(_targetPos, OnTargetPositionReached);

        anim.SetBool("Moving", true);
        gameObject.layer = LayerMask.NameToLayer("Turnips_GoingTo");

    }

    public void SetTarget(Transform _target)
    {
        turnipState = TurnipState.GOING_TO;
        pathfinding.SetTarget(_target, SetState_FollowPlayer);

        anim.SetBool("Moving", true);
        gameObject.layer = LayerMask.NameToLayer("Turnips_GoingTo");
    }

    void OnTargetPositionReached()
    {
        turnipState = TurnipState.IDLE;
        anim.SetBool("Moving", false);
        gameObject.layer = LayerMask.NameToLayer("Turnips_Idle");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, maxDistanceToPlayer);
        Gizmos.color = Color.white;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            GameObject p = Instantiate(deadParticles, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(p, 3);
        }

        if (turnipState == TurnipState.FOLLOWING_PLAYER && collision.collider.CompareTag("Turnip"))
        {
            TurnipBehaviour tb = collision.gameObject.GetComponent<TurnipBehaviour>();
            
            if(tb.turnipState == TurnipState.WAITING_FOR_PLAYER)
            {
               // Debug.Log("forceStop");
                pathfinding.Stop();
            }
        }
    }


}
