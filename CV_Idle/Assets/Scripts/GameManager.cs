using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject hero;
    public GameObject enemyPrefab;
    public UIManager uiManager;

    public GameObject CurrentEnemy { get; private set; }
    public Hero heroScript;
    public Enemy enemyScript;

    public List<Regions> regions;
    public List<Sprite> bossSprites;

    public float nextAttackTime = 2f;
    public int enemyHealthIncrement = 10;

    public int stage = 0;
    public int round = 0;
    public int cycle = 0;
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
        uiManager.UpdateVirtuePointsText(VP);
    }

    public void UpdateStageDisplay()
    {
        uiManager.UpdateStageDisplay(enemyScript.isBoss, stage, round, cycle);

    }
    public void SpawnEnemy()
    {

        // Instantiate a new enemy and get its script
        CurrentEnemy = Instantiate(enemyPrefab);
        enemyScript = CurrentEnemy.GetComponent<Enemy>();

        // Set the position of the instantiated enemyPrefab
        CurrentEnemy.transform.position = new Vector3(5, CurrentEnemy.transform.position.y, CurrentEnemy.transform.position.z);

        // Set the scale of the instantiated enemyPrefab
        CurrentEnemy.transform.localScale = new Vector3(3, 3, 1);

        // Increase the enemy's health
        enemyScript.health += enemyHealthIncrement;

        if (round < regions[stage].sprites.Count)  
        {
            uiManager.UpdateBackgroundImage(regions[stage].background);
            enemyScript.GetComponent<SpriteRenderer>().sprite = regions[stage].sprites[round];
        }
        else
        {
            enemyScript.GetComponent<SpriteRenderer>().sprite = bossSprites[stage];
            enemyScript.isBoss = true;
        }

        UpdateStageDisplay();

        // Subscribe to the enemy's death event
        enemyScript.OnEnemyDeath += OnEnemyDeath;
    }

    public void OnEnemyDeath()
    {
        // Unsubscribe from the enemy's death event to prevent memory leaks
        enemyScript.OnEnemyDeath -= OnEnemyDeath;

        enemyHealthIncrement += 10;

        if (round > regions[stage].sprites.Count)
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

        VP++;

        // Spawn a new enemy
        SpawnEnemy();
    }
}