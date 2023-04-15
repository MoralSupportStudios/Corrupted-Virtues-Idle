using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> heroParty;
    public GameObject enemyPrefab;
    public UIManager uiManager;

    public GameObject CurrentEnemy { get; private set; }

    public List<Regions> regions;
    public List<Sprite> bossSprites;

    public GameObject healthBarPrefab;
    public int enemyHealthIncrement = 10;

    public int stage = 0;
    public int round = 0;
    public int cycle = 0;
    public int VP = 0;
    private void Awake()
    {
        foreach (GameObject hero in heroParty)
        {
            // Check if it's time for the current hero to attack
            hero.GetComponent<Hero>().nextAttackTime = 0;
            
        }
    }
    public void Start()
    {
        // Spawn the first enemy
        SpawnEnemy();
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

        //update UI
        uiManager.UpdateVirtuePointsText(VP);
    }

    public void UpdateStageDisplay()
    {
        uiManager.UpdateStageDisplay(CurrentEnemy.GetComponent<Enemy>().isBoss, stage, round, cycle);

    }
    public void SpawnEnemy()
    {

        // Instantiate a new enemy and get its script
        CurrentEnemy = Instantiate(enemyPrefab);

        // Set the position of the instantiated enemyPrefab
        CurrentEnemy.transform.position = new Vector3(5, CurrentEnemy.transform.position.y, CurrentEnemy.transform.position.z);

        // Set the scale of the instantiated enemyPrefab
        CurrentEnemy.transform.localScale = new Vector3(3, 3, 1);

        // Increase the enemy's health
        CurrentEnemy.GetComponent<Enemy>().health += enemyHealthIncrement;
        CurrentEnemy.GetComponent<Enemy>().maxHealth = CurrentEnemy.GetComponent<Enemy>().health;

        // Instantiate a new health bar for the enemy
        GameObject healthBar = Instantiate(healthBarPrefab);
        healthBar.transform.SetParent(CurrentEnemy.transform, false);

        // Set the health bar's position above the enemy
        healthBar.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 150);

        // Set the reference to the Enemy component in the EnemyHealthBar script
        healthBar.GetComponent<EnemyHealthBar>().SetEnemy(CurrentEnemy.GetComponent<Enemy>());

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

        enemyHealthIncrement += 10;

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

        VP++;

        // Spawn a new enemy
        SpawnEnemy();
    }
}