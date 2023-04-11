using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject hero;
    public GameObject enemyPrefab;
    public TMP_Text StageCounter;
    public TMP_Text VirtuePoints;

    public GameObject currentEnemy;
    public Hero heroScript;
    public Enemy enemyScript;

    public List<Regions> regions;
    public List<Sprite> bossSprites;

    public float nextAttackTime = 0f;
    public int enemyHealthIncrement = 10;

    public int stage = 0;
    public int round = 0;
    public int VP = 0;

    public void Start()
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
            nextAttackTime = Time.time + heroScript.attackInterval;
        }

        //update UI
        StageCounter.text = $"Stage: {stage}-{round}";
        VirtuePoints.text = $"Virtue Points: {VP}";
    }

    private void SpawnEnemy()
    {
        // Instantiate a new enemy and get its script
        currentEnemy = Instantiate(enemyPrefab);
        enemyScript = currentEnemy.GetComponent<Enemy>();

        if (round < regions[stage].sprites.Count)
        {
            enemyScript.GetComponent<SpriteRenderer>().sprite = regions[stage].sprites[round];
        }
        else
        {
            enemyScript.GetComponent<SpriteRenderer>().sprite = bossSprites[stage];
            round = 0;
            stage++;
        }

        // Subscribe to the enemy's death event
        enemyScript.OnEnemyDeath += OnEnemyDeath;

        // Increase the enemy's health
        enemyScript.health += enemyHealthIncrement;
        enemyHealthIncrement += 10;
        round++;
        VP++;
    }

    public void OnEnemyDeath()
    {
        // Unsubscribe from the enemy's death event to prevent memory leaks
        enemyScript.OnEnemyDeath -= OnEnemyDeath;

        // Spawn a new enemy
        SpawnEnemy();
    }
}
