using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyNexusStats : MonoBehaviour
{

    public int hp = 20;
    int initialHP;

    private SceneHandler sceneHandler;
    private PlayerStats playerStats;
    public Image lifebarUI;
    public GameObject enemyToSpawn;

    public float threshold1, threshold2, threshold3;
    public float ratio0, ratio1, ratio2, ratio3;

    float timer;
    private void Start()
    {
        timer = 0;
        initialHP = hp;
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        sceneHandler = GameObject.FindGameObjectWithTag("SceneHandler").GetComponent<SceneHandler>();
    }
    void Update()
    {
        if (playerStats.GetNabos() < threshold1)
        {
            if (timer >= ratio0)
                SpawnEnemy();
        }
        else if (playerStats.GetNabos() < threshold2)
        {
            if (timer >= ratio1)
                SpawnEnemy();
        }
        else if (playerStats.GetNabos() < threshold3)
        {
            if (timer >= ratio2)
                SpawnEnemy();
        }
        else if (timer >= ratio3)
            SpawnEnemy();

        timer += Time.deltaTime;

        if (hp <= 0)
        {
            Destroy(gameObject);
            sceneHandler.WinScene();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Turnip"))
        {
            hp--;
            lifebarUI.fillAmount = (1 / (float)initialHP) * (float)hp;
        }
    }

    void SpawnEnemy()
    {
        Instantiate(enemyToSpawn, gameObject.transform.position, gameObject.transform.rotation);
        timer = 0;
    }

}
