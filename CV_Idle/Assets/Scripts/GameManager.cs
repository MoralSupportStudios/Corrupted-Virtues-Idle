using System;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> heroParty;
    public GameObject enemyPrefab;
    public UIManager uiManager;
    public Transform heroSpawn;
    public GameObject CurrentEnemy { get; private set; }

    public GameObject ShopPanel;
    public List<Regions> regions;
    public List<Sprite> bossSprites;

    public int enemyHealthIncrement = 10;

    public int stage = 0;
    public int round = 0;
    public int cycle = 0;
    public int virtuePoints = 0;
    public void Awake()
    {
        InitializeHeroes();
    }

    public void InitializeHeroes()
    {
        foreach (GameObject hero in heroParty)
        {
            hero.GetComponent<Hero>().nextAttackTime = 0;
        }
    }
    public void Start()
    {
        SpawnHeroes();
        SpawnEnemy();
    }
    public void SpawnHeroes()
    {
        Vector2[] heroPositions = new Vector2[] {
        new Vector2(-8, -1),
        new Vector2(-7, -1),
        new Vector2(-6, 0),
        new Vector2(-7, 1),
        new Vector2(-8, 1)
    };

        for (int i = 0; i < heroParty.Count; i++)
        {
            GameObject hero = Instantiate(heroParty[i], heroPositions[i], Quaternion.identity);
            hero.transform.SetParent(heroSpawn, false);
            heroParty[i] = hero;
            // Set the scale of the instantiated enemyPrefab
            hero.transform.localScale = new Vector3(3, 3, 1);

        }
    }
    void Update()
    {
        // Iterate through the heroParty
        foreach (GameObject hero in heroParty)
        {
            // Check if it's time for the current hero to attack
            if (Time.time >= hero.GetComponent<Hero>().nextAttackTime)
            {
                hero.GetComponent<Hero>().Attack(CurrentEnemy.GetComponent<Enemy>());

                // Update the nextAttackTime for the current hero
                hero.GetComponent<Hero>().nextAttackTime = Time.time + hero.GetComponent<Hero>().attackInterval;
            }
        }

        uiManager.UpdateVirtuePointsText(virtuePoints);
    }

    public void UpdateStageDisplay()
    {
        uiManager.UpdateStageDisplay(CurrentEnemy.GetComponent<Enemy>().isBoss, stage, round, cycle);

    }
    public void SpawnEnemy()
    {
        
        // Instantiate a new enemy and get its script
        CurrentEnemy = Instantiate(enemyPrefab);
        CurrentEnemy.SetActive(!ShopPanel.activeSelf);
        // Set the Canvas reference
        CurrentEnemy.GetComponent<Enemy>().canvas = uiManager.mainCanvas;
        // Set the parent of the instantiated enemyPrefab to the EnemiesContainer
        CurrentEnemy.transform.SetParent(uiManager.enemySpawn, false);
        // Set the position of the instantiated enemyPrefab
        CurrentEnemy.transform.position = new Vector3(6, CurrentEnemy.transform.position.y, CurrentEnemy.transform.position.z);

        // Set the scale of the instantiated enemyPrefab
        CurrentEnemy.transform.localScale = new Vector3(3, 3, 1);


        // Increase the enemy's health
        CurrentEnemy.GetComponent<Enemy>().health += enemyHealthIncrement;
        CurrentEnemy.GetComponent<Enemy>().maxHealth = CurrentEnemy.GetComponent<Enemy>().health;
        CurrentEnemy.GetComponent<Enemy>().healthBarPrefab = uiManager.healthBarPrefab;

        CurrentEnemy.GetComponent<Enemy>().CreateHealthBar();
        CurrentEnemy.GetComponent<Enemy>().healthBarInstance.gameObject.SetActive(!ShopPanel.activeSelf);
        if (round < regions[stage].sprites.Count)
        {
            uiManager.UpdateBackgroundImage(regions[stage].background);
            CurrentEnemy.GetComponent<Enemy>().GetComponent<SpriteRenderer>().sprite = regions[stage].sprites[round];
            CurrentEnemy.GetComponent<Enemy>().isBoss = false;
        }
        else
        {
            CurrentEnemy.GetComponent<Enemy>().GetComponent<SpriteRenderer>().sprite = bossSprites[stage];
            CurrentEnemy.GetComponent<Enemy>().isBoss = true;
        }

        UpdateStageDisplay();

        // Subscribe to the enemy's death event
        CurrentEnemy.GetComponent<Enemy>().OnEnemyDeath += OnEnemyDeath;
    }

    public void OnEnemyDeath()
    {
        // Unsubscribe from the enemy's death event to prevent memory leaks
        CurrentEnemy.GetComponent<Enemy>().OnEnemyDeath -= OnEnemyDeath;
        if(CurrentEnemy.GetComponent<Enemy>().isBoss)
        {
            enemyHealthIncrement += (int)((100 * Math.Pow(2, stage - 1)) + enemyHealthIncrement + (round * 10));
        }
        else
        {
            enemyHealthIncrement += (int)((10 * Math.Pow(2, stage - 1)) + enemyHealthIncrement + (round * 10));
        }
        

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

        virtuePoints++;

        // Spawn a new enemy
        SpawnEnemy();
    }
}