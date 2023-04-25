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
        int healthMultiplier = isBoss ? 11 : 10;
        enemyHealthIncrement = (int)((healthMultiplier * Mathf.Pow(1 + 0.05f, (round + 1) * (stage + 1) * (cycle + 1))) + 101);

        return enemyHealthIncrement;
    }
}
