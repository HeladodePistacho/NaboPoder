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

    public float maxDistanceToPlayer = 10f;

    public TurnipState turnipState = TurnipState.IDLE;

    public HortalizaPathfinding pathfinding;

    Camera main;
    //PossibleTargets
    Transform player;

    //Animations
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        //TODO: search for player
        main = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        SetState_FollowPlayer();
    }

    // Update is called once per frame
    void Update()
    {
         switch (turnipState)
         {
             case TurnipState.IDLE:

                 break;
             case TurnipState.WAITING_FOR_PLAYER:
                Update_WaitingForPlayer();
                 break;

         }
    }

    void SetState_FollowPlayer()
    {
        turnipState = TurnipState.FOLLOWING_PLAYER;
        pathfinding.SetTarget(player, OnPlayerReached);

        anim.SetBool("Moving", true);
    }

    void OnPlayerReached()
    {
        turnipState = TurnipState.WAITING_FOR_PLAYER;
        anim.SetBool("Moving", false);
    }

    void Update_WaitingForPlayer()
    {
        if(Vector3.Distance(transform.position, player.position)> maxDistanceToPlayer)
        {
            SetState_FollowPlayer();
        }
    }

    public void SetTargetPosition(Vector3 _targetPos)
    {
        turnipState = TurnipState.GOING_TO;
        pathfinding.SetTarget(_targetPos, SetState_FollowPlayer);

        anim.SetBool("Moving", true);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, maxDistanceToPlayer);
        Gizmos.color = Color.white;
    }


}
