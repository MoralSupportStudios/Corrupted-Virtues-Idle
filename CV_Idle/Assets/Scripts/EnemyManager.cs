using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameManager gameManager;
    public LevelManager levelManager;
    public UIManager uiManager;
    public GameObject CurrentEnemy { get; private set; }

    public void SpawnEnemy()
    {
        // Instantiate a new enemy and get its script
        CurrentEnemy = Instantiate(enemyPrefab);
        CurrentEnemy.SetActive(!uiManager.ShopPanel.activeSelf);

        // Set the Canvas reference
        CurrentEnemy.GetComponent<Enemy>().canvas = gameManager.uiManager.mainCanvas;

        // Set the parent of the instantiated enemyPrefab to the EnemiesContainer
        CurrentEnemy.transform.SetParent(gameManager.uiManager.enemySpawn, false);

        // Set the position of the instantiated enemyPrefab
        CurrentEnemy.transform.position = new Vector3(6, CurrentEnemy.transform.position.y, CurrentEnemy.transform.position.z);

        // Set the scale of the instantiated enemyPrefab
        CurrentEnemy.transform.localScale = new Vector3(3, 3, 1);

        if (levelManager.round < levelManager.regions[levelManager.stage].sprites.Count)
        {
            gameManager.uiManager.UpdateBackgroundImage(levelManager.regions[levelManager.stage].background);
            CurrentEnemy.GetComponent<Enemy>().GetComponent<SpriteRenderer>().sprite = levelManager.regions[levelManager.stage].sprites[levelManager.round];
            CurrentEnemy.GetComponent<Enemy>().isBoss = false;
        }
        else
        {
            CurrentEnemy.GetComponent<Enemy>().GetComponent<SpriteRenderer>().sprite = levelManager.regions[levelManager.stage].bossSprite;
            CurrentEnemy.GetComponent<Enemy>().isBoss = true;
        }

        // Calculate and set the enemy's health
        int enemyHealth = levelManager.CalculateEnemyHealth();
        CurrentEnemy.GetComponent<Enemy>().health = enemyHealth;
        CurrentEnemy.GetComponent<Enemy>().maxHealth = enemyHealth;

        // Set the health bar prefab
        CurrentEnemy.GetComponent<Enemy>().healthBarPrefab = gameManager.uiManager.healthBarPrefab;

        // Create and set up the health bar
        CurrentEnemy.GetComponent<Enemy>().CreateHealthBar();
        CurrentEnemy.GetComponent<Enemy>().healthBarInstance.gameObject.SetActive(!uiManager.ShopPanel.activeSelf);

        // Update the stage display
        levelManager.UpdateStageDisplay();

        // Subscribe to the enemy's death event
        CurrentEnemy.GetComponent<Enemy>().OnEnemyDeath += OnEnemyDeath;
    }

    public void OnEnemyDeath()
    {
        // Unsubscribe from the enemy's death event to prevent memory leaks
        CurrentEnemy.GetComponent<Enemy>().OnEnemyDeath -= OnEnemyDeath;

        // Increase the difficulty
        levelManager.IncreaseDifficulty();

        // Increase virtue points
        gameManager.virtuePoints += 3;

        // Spawn a new enemy
        SpawnEnemy();
    }
}
