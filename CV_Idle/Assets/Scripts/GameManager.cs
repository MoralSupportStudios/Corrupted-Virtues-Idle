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
    public TMP_Text StageCounter;
    public TMP_Text VirtuePoints;
    public GameObject backgroundImage;

    public GameObject currentEnemy;
    public Hero heroScript;
    public Enemy enemyScript;

    public List<Regions> regions;
    public List<Sprite> bossSprites;

    public float nextAttackTime = 0f;
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
        if (round == 0)
        {
            StageCounter.text = $"Stage: {stage} Boss {ToRoman(cycle)}";
        }
        else
        {
            StageCounter.text = $"Stage: {stage+1}-{round} {ToRoman(cycle)}";
        }

        VirtuePoints.text = $"Virtue Points: {VP}";
    }
    public static string ToRoman(int number)
    {
        switch (number)
        {
            case < 0:
                return string.Empty;
            case > 3999:
                throw new ArgumentOutOfRangeException("insert value betwheen 1 and 3999");
            case < 1:
                return string.Empty;
            case >= 1000:
                return "M" + ToRoman(number - 1000);
            case >= 900:
                return "CM" + ToRoman(number - 900);
            case >= 500:
                return "D" + ToRoman(number - 500);
            case >= 400:
                return "CD" + ToRoman(number - 400);
            case >= 100:
                return "C" + ToRoman(number - 100);
            case >= 90:
                return "XC" + ToRoman(number - 90);
            case >= 50:
                return "L" + ToRoman(number - 50);
            case >= 40:
                return "XL" + ToRoman(number - 40);
            case >= 10:
                return "X" + ToRoman(number - 10);
            case >= 9:
                return "IX" + ToRoman(number - 9);
            case >= 5:
                return "V" + ToRoman(number - 5);
            case >= 4:
                return "IV" + ToRoman(number - 4);
            case >= 1:
                return "I" + ToRoman(number - 1);
            default:
                throw new InvalidOperationException("Impossible state reached");
        }
    }
    private void SpawnEnemy()
    {
        // Instantiate a new enemy and get its script
        currentEnemy = Instantiate(enemyPrefab);
        enemyScript = currentEnemy.GetComponent<Enemy>();
        // Increase the enemy's health
        enemyScript.health += enemyHealthIncrement;
        enemyHealthIncrement += 10;
        round++;
        VP++;

        if (stage >= regions.Count)
        {
            round = 0;
            stage = 0;
            cycle++;
        }

        if (round < regions[stage].sprites.Count)
        {
            backgroundImage.GetComponent<Image>().sprite = regions[stage].background;
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
    }

    public void OnEnemyDeath()
    {
        // Unsubscribe from the enemy's death event to prevent memory leaks
        enemyScript.OnEnemyDeath -= OnEnemyDeath;

        // Spawn a new enemy
        SpawnEnemy();
    }
}