using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.Events;

public class HortalizaPathfinding : MonoBehaviour
{
    public Transform target;
    public float acceleration = 200f;
    public float maxSpeed = 10f;
    public float nextWaypointDistance = 3f;
    public float refreshPathRate = 0.5f;

    Vector2 currentMovementSpeed = Vector2.zero;

    public Transform graphics;
    Vector3 left = new Vector3(1f, 1f, 1f);
    Vector3 right = new Vector3(-1f, 1f, 1f);

    Path path;
    int currentWaypoint = 0;
    bool reachedEnd = false;

    Seeker seeker;
    Rigidbody2D rb;

    UnityAction onReachedEnd;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetTarget(Transform _target, UnityAction onEnd)
    {
        if(path != null)
            CancelInvoke("UpdatePath");

        onReachedEnd = onEnd;
        target = _target;
        InvokeRepeating("UpdatePath", 0f, refreshPathRate);
    }

    public void Stop()
    {
        rb.velocity = Vector2.zero;
        reachedEnd = true;
        CancelInvoke("UpdatePath");
        onReachedEnd.Invoke();
        path = null;
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathGenerationComplete);
        }

    }

    void OnPathGenerationComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;           
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            Stop();
            return;
        }
        else reachedEnd = false;

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * acceleration * Time.deltaTime;

        rb.AddForce(force);

        currentMovementSpeed.x = Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed);
        currentMovementSpeed.y = Mathf.Clamp(rb.velocity.y, -maxSpeed, maxSpeed);

        rb.velocity = currentMovementSpeed;
        
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
            currentWaypoint++;

        if (force.x >= 0.05)
        {
            graphics.localScale = right;
        }
        else if (force.x <= -0.05)
        {
            graphics.localScale = left;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, nextWaypointDistance);
        Gizmos.color = Color.white;
    }
}
