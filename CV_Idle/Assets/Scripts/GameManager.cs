using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject hero;
    public GameObject enemyPrefab;
    private GameObject currentEnemy;
    private Hero heroScript;
    private Enemy enemyScript;
    private float attackInterval = 2.0f;
    private float nextAttackTime = 0f;
    private int enemyHealthIncrement = 10;

    void Start()
    {
        // Get the Hero script
        heroScript = hero.GetComponent<Hero>();

        // Spawn the first enemy
        SpawnEnemy();
    }

    void Update()
    {
        // Check if it's time to attack
        if (Time.time >= nextAttackTime)
        {
            heroScript.Attack(enemyScript);
            nextAttackTime = Time.time + attackInterval;
        }
    }

    private void SpawnEnemy()
    {
        // Instantiate a new enemy and get its script
        currentEnemy = Instantiate(enemyPrefab);
        enemyScript = currentEnemy.GetComponent<Enemy>();

        // Subscribe to the enemy's death event
        enemyScript.OnEnemyDeath += OnEnemyDeath;

        // Increase the enemy's health
        enemyScript.health += enemyHealthIncrement;
        enemyHealthIncrement += 10;
    }

    private void OnEnemyDeath()
    {
        // Unsubscribe from the enemy's death event to prevent memory leaks
        enemyScript.OnEnemyDeath -= OnEnemyDeath;

        // Spawn a new enemy
        SpawnEnemy();
    }
}