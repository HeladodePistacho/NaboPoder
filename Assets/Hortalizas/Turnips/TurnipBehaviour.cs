using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public enum TurnipState
{
    IDLE,
    MOVING,
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
    }

    void OnPlayerReached()
    {
        turnipState = TurnipState.WAITING_FOR_PLAYER;
    }

    void Update_WaitingForPlayer()
    {
        if(Vector3.Distance(transform.position, player.position)> maxDistanceToPlayer)
        {
            SetState_FollowPlayer();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, maxDistanceToPlayer);
        Gizmos.color = Color.white;
    }


}
