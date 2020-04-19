using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    // Variables
    public int life = 10;
    public int nabos = 0;
    public int seeds = 0;
    public bool dead = false;
    public TextMeshProUGUI UIhp;
    public TextMeshProUGUI UIturnips;
    public TextMeshProUGUI UIseeds;

    public void Update()
    {
        UIhp.SetText("HP: " + life);
        UIturnips.SetText("Turnips: " + nabos);
        UIseeds.SetText("Seeds: " + seeds);
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

        // Restart game
        //SceneHandler.GetInstance().GotoInGame();
    }
}
