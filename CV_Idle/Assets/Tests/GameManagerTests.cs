using System.Collections;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class GameManagerTests
{
    private GameManager gameManager;
    private GameObject gameManagerObject;
    private GameObject hero;
    private GameObject enemyPrefab;
    private UIManager uiManager;
    private TMP_Text StageCounter;
    private TMP_Text VirtuePoints;

    [SetUp]
    public void Setup()
    {
        // Create and set up required game objects and components
        gameManagerObject = new GameObject();
        gameManager = gameManagerObject.AddComponent<GameManager>();

        hero = new GameObject();
        hero.AddComponent<Hero>();

        enemyPrefab = new GameObject();
        enemyPrefab.AddComponent<Enemy>();

        GameObject uiManagerObject = new GameObject();
        uiManager = uiManagerObject.AddComponent<UIManager>();

        // Set up the UI components
        StageCounter = new GameObject().AddComponent<TextMeshProUGUI>();
        VirtuePoints = new GameObject().AddComponent<TextMeshProUGUI>();
        uiManager.stageCounter = StageCounter;
        uiManager.virtuePoints = VirtuePoints;

        gameManager.enemyPrefab = enemyPrefab;
        gameManager.uiManager = uiManager;

        // Set up the UIManager
        //uiManager.Setup();
    }


    [TearDown]
    public void Teardown()
    {
        // Clean up game objects after each test
        Object.DestroyImmediate(gameManagerObject);
        Object.DestroyImmediate(hero);
        Object.DestroyImmediate(enemyPrefab);
        Object.DestroyImmediate(uiManager.gameObject);
    }

    [UnityTest]
    public IEnumerator TestEnemySpawn()
    {
        gameManager.Start();

        yield return null;

        Assert.IsNotNull(gameManager.CurrentEnemy, "Enemy should be spawned at the start.");
    }
    [UnityTest]
    public IEnumerator TestEnemyDeath()
    {
        gameManager.Start();

        yield return null;

        int initialRound = gameManager.round;
        int initialVP = gameManager.virtuePoints;
        GameObject initialEnemy = gameManager.CurrentEnemy;
        gameManager.OnEnemyDeath();

        Assert.AreNotEqual(initialEnemy, gameManager.CurrentEnemy, "A new enemy should be spawned after the previous enemy's death.");
        Assert.AreEqual(initialRound + 1, gameManager.round, "Round should be incremented after an enemy's death.");
        Assert.AreEqual(initialVP + 1, gameManager.virtuePoints, "VP should be incremented after an enemy's death.");
    }
}
