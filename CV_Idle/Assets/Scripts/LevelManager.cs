using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameManager gameManager;
    public List<Regions> regions;
    public int enemyHealthIncrement = 10;

    public int stage = 0;
    public int round = 0;
    public int cycle = 0;

    public void UpdateStageDisplay()
    {
        gameManager.uiManager.UpdateStageDisplay(gameManager.enemyManager.CurrentEnemy.GetComponent<Enemy>().isBoss, stage, round, cycle);
    }

    public void IncreaseDifficulty()
    {
        if (round > regions[stage].sprites.Count - 1)
        {
            if (stage == regions.Count - 1)
            {
                stage = 0;
                cycle++;
            }
            else
            {
                stage++;
            }
            round = 0;
        }
        else
        {
            round++;
        }
    }

    public int CalculateEnemyHealth()
    {
        // Determine if the enemy is a boss
        bool isBoss = round >= regions[stage].sprites.Count;

        // Calculate health increment
        int roundHealthIncrement = 5 * round;
        int stageHealthIncrement = 100 * stage;
        int cycleHealthIncrement = 1000 * cycle;

        // Set a base health and a boss health bonus
        int baseHealth = 100;
        int bossHealthBonus = isBoss ? 150 : 0;

        // Calculate total health
        int enemyHealth = baseHealth + roundHealthIncrement + stageHealthIncrement + cycleHealthIncrement + bossHealthBonus;

        return enemyHealth;
    }

    public void RewardVirtuePoints()
    {
        bool isBoss = round >= regions[stage].sprites.Count;

        if(isBoss)
        {
            gameManager.virtuePoints += 10 * (cycle + 1);
        }
        else
        {
            gameManager.virtuePoints += 1 * (cycle + 1);
        }
    }
}