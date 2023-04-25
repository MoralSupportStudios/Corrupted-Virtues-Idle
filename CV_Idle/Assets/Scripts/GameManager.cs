using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public HeroManager heroManager;
    public EnemyManager enemyManager;
    public UIManager uiManager;
    
    public int virtuePoints = 0;

    public void Awake()
    {
        heroManager.InitializeHeroes();
    }

    public void Start()
    {
        for (int i = 0; i < heroManager.heroParty.Count; i++)
        {
            heroManager.SpawnHero(heroManager.heroParty[i], i);
        }
        enemyManager.SpawnEnemy();
    }

    void Update()
    {
        // Iterate through the heroParty
        foreach (GameObject hero in heroManager.heroParty)
        {
            // Check if it's time for the current hero to attack
            if (Time.time >= hero.GetComponent<Hero>().nextAttackTime)
            {
                hero.GetComponent<Hero>().Attack(enemyManager.CurrentEnemy.GetComponent<Enemy>());

                // Update the nextAttackTime for the current hero
                hero.GetComponent<Hero>().nextAttackTime = Time.time + hero.GetComponent<Hero>().attackInterval;
            }
        }

        uiManager.UpdateVirtuePointsText(virtuePoints);
    }    
}