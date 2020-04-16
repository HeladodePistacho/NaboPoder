using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    // Variables
    public static int life = 10;
    public static int nabos = 0;
    public static int seeds = 0;
    public static bool dead = false;
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
    public static int GetLife()
    {
        return life;
    }
    public static int GetNabos()
    {
        return nabos;
    }
    public static int GetSeeds()
    {
        return seeds;
    }

    // Setters
    public static void SetLife(int l)
    {
        life = l;
    }
    public static void SetNabos(int n)
    {
        nabos = n;
    }
    public static void SetSeeds(int s)
    {
        seeds = s;
    }

    // Takers
    public static int TakeHeal(int heal)
    {
        life += heal;
        return life;
    }
    public static int TakeDamage(int dmg)
    {
        life -= dmg;
        if (life <= 0)
            KillPlayer();
        return life;
    }
    public static int TakeNabos(int n)
    {
        nabos += n;
        return nabos;
    }
    public static int EraseSeeds(int s)
    {
        seeds -= s;
        return seeds;
    }
    public static int AddSeeds(int s)
    {
        seeds += s;
        return seeds;
    }

    public static void KillPlayer()
    {
        // Kill player
        // Show dead msg

        // Restart game
        SceneHandler.GetInstance().GotoInGame();
    }
}
