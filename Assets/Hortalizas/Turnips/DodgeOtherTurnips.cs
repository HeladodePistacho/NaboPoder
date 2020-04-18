using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeOtherTurnips : MonoBehaviour
{
    public LayerMask layer;
    Rigidbody2D rb;
    public float radius = 1;
    public float force = 10f;
    Vector2 forceToApply = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        forceToApply = Vector2.zero;
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, radius, Vector2.zero, 0, layer);

        for (int i = 0; i < hits.Length; i++)
        {
            Vector2 distance = transform.position - hits[i].transform.position;
            forceToApply += distance;
        }

        rb.AddForce(forceToApply.normalized);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(forceToApply.normalized.x, forceToApply.normalized.y, 0));
    }
}
