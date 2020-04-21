using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyStats : MonoBehaviour
{
    [HideInInspector]
    public Vector3 initPos;
    // Gameplay variables
    public float speed = 0f;
    public float sightRange = 0f;
    public float loseSightRange = 4f;
    public int hp = 0;
    int initialHP;
    public int damage = 0;
    public GameObject deadParticles;

    public Image lifebarUI;
    
    void Start()
    {
        initialHP = hp;
        initPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
    }

    void Update()
    {
        if (hp <= 0)
        {
            Destroy(gameObject);
            GameObject p = Instantiate(deadParticles, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(p, 3);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Turnip"))
        {
            //Todo: hacer que el enemigo recule
            hp--;
            lifebarUI.fillAmount = (1 / (float)initialHP) * (float)hp;
        }
        else if (collision.collider.CompareTag("Nexus"))
        {
            GameObject.FindGameObjectWithTag("Nexus").GetComponent<NexusStats>().hp -= damage;
            hp = 0;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.white;
    }
}
