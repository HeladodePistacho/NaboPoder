using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    // Variables
    [Header("Player variables")]
    public int life = 10;
    public int nabos = 0;
    public int seeds = 0;
    public bool dead = false;
    
    [Header("UI stuff")]
    public TextMeshProUGUI UIhp;
    public TextMeshProUGUI UIturnips;
    public TextMeshProUGUI UIseeds;

    private SceneHandler sceneHandler;
    private float damageDelayTimer = 5f;

    private Rigidbody2D rb;

    private void Start()
    {
        sceneHandler = GameObject.FindGameObjectWithTag("SceneHandler").GetComponent<SceneHandler>();
        rb = GetComponent<Rigidbody2D>();
    }
    public void Update()
    {
        damageDelayTimer += Time.deltaTime;
        UIhp.SetText("HP: " + life);
        UIturnips.SetText("Turnips: " + nabos);
        UIseeds.SetText("Seeds: " + seeds);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy") && damageDelayTimer >= 0.2)
        {
            damageDelayTimer = 0f;

            Vector2 direction = transform.position - collision.transform.position;
            GetComponent<PlayerController>().Damage(direction.normalized);
            TakeDamage(collision.gameObject.GetComponent<EnemyStats>().damage);
        }
    }
    // Getters
    public int GetLife()
    {
        return life;
    }
    public int GetNabos()
    {
        return nabos;
    }
    public int GetSeeds()
    {
        return seeds;
    }

    // Setters
    public void SetLife(int l)
    {
        life = l;
    }
    public void SetNabos(int n)
    {
        nabos = n;
    }
    public void SetSeeds(int s)
    {
        seeds = s;
    }

    // Takers
    public int TakeHeal(int heal)
    {
        life += heal;
        return life;
    }
    public int TakeDamage(int dmg)
    {
        life -= dmg;
        
        if (life <= 0)
            KillPlayer();
        return life;
    }
    public int TakeNabos(int n)
    {
        nabos += n;
        return nabos;
    }
    public int EraseSeeds(int s)
    {
        seeds -= s;
        return seeds;
    }
    public int AddSeeds(int s)
    {
        seeds += s;
        return seeds;
    }

    public void KillPlayer()
    {
        // Kill player
        // Show dead msg
        sceneHandler.GoToDeadScene();
        // Restart game
        //SceneHandler.GetInstance().GotoInGame();
    }
}
