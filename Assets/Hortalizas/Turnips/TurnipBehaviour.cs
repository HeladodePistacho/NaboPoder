using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TurnipState
{
    IDLE,
    GO_TO,
    FOLLOW_PLAYER
}


public class TurnipBehaviour : MonoBehaviour
{
    public float maxDistanceToPlayer = 10f;

    public TurnipState turnipState = TurnipState.GO_TO;
   
    Camera main;
    //PossibleTargets
    Transform player;
    public Vector2 target;

    // Start is called before the first frame update
    void Start()
    {
        //TODO: search for player
        main = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        target = main.ScreenToWorldPoint(Input.mousePosition);

        switch (turnipState)
        {
            case TurnipState.FOLLOW_PLAYER:
                break;
            case TurnipState.IDLE:
                Update_IDLE();
                break;
            case TurnipState.GO_TO:
                Update_GO_TO();
                break;

        }
    }

    void Update_IDLE()
    {
        if (player == null)
            return;

        /*if(Vector2.Distance(transform.position, player.transform.position) > maxDistanceToPlayer)
        {
            turnipState = TurnipState.FOLLOW_PLAYER;
        }*/
    }

    bool reached = false;
    void Update_GO_TO()
    {
        if(reached == false)
        {
            reached = MoveTurnip(target);
        }
        else if (StopTurnip() == true)
        {
            turnipState = TurnipState.IDLE;
        }
    }

    void Update_FOLLOW_PLAYER()
    {

    }

    public void SetTarget(Vector2 point) //Object or position
    {
        turnipState = TurnipState.GO_TO;
        target = point;
    }

    #region Movement Functions
    Vector2 currentMovementSpeed = Vector2.zero;
    public float maxMovementSpeed = 10f;
    public float acceleration = 3;
    public float friction = 3;

    bool MoveTurnip(Vector3 t)
    {
        Vector2 direction = (t - transform.position).normalized;

        currentMovementSpeed += direction * acceleration  * Time.deltaTime;

        currentMovementSpeed.x = Mathf.Clamp(currentMovementSpeed.x, -maxMovementSpeed, maxMovementSpeed);
        currentMovementSpeed.y = Mathf.Clamp(currentMovementSpeed.y, -maxMovementSpeed, maxMovementSpeed);

        //Target is reached
      //  if (Vector2.Distance(transform.position, t) < 3)
       //     return true;

        transform.Translate(currentMovementSpeed * Time.deltaTime);
        return false;
    }

    bool StopTurnip()
    {
        currentMovementSpeed.x -= Mathf.Sign(currentMovementSpeed.x) * friction * Time.deltaTime;
        currentMovementSpeed.y -= Mathf.Sign(currentMovementSpeed.y) * friction * Time.deltaTime;


        if (Mathf.Abs(currentMovementSpeed.x) < 0.5 && Mathf.Abs(currentMovementSpeed.y) < 0.5)
        {
            currentMovementSpeed = Vector2.zero;
            return true;
        }

        return false;
    }
    #endregion
}
