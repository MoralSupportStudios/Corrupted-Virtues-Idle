using System.Collections;
using NUnit.Framework;
using UnityEngine;
using TMPro;
using UnityEngine.TestTools;

[TestFixture]
public class GameManagerTests
{
    private GameManager gameManager;
    private GameObject gameManagerObject;
    private GameObject hero;
    private GameObject enemyPrefab;
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

        StageCounter = new GameObject().AddComponent<TextMeshProUGUI>();
        VirtuePoints = new GameObject().AddComponent<TextMeshProUGUI>();

        gameManager.hero = hero;
        gameManager.enemyPrefab = enemyPrefab;
        gameManager.StageCounter = StageCounter;
        gameManager.VirtuePoints = VirtuePoints;
    }

    [TearDown]
    public void Teardown()
    {
        // Clean up game objects after each test
        Object.DestroyImmediate(gameManagerObject);
        Object.DestroyImmediate(hero);
        Object.DestroyImmediate(enemyPrefab);
        Object.DestroyImmediate(StageCounter.gameObject);
        Object.DestroyImmediate(VirtuePoints.gameObject);
    }

    [UnityTest]
    public IEnumerator TestEnemySpawn()
    {
        gameManager.Start();

        yield return null;

        Assert.IsNotNull(gameManager.currentEnemy, "Enemy should be spawned at the start.");
    }
    [UnityTest]
    public IEnumerator TestEnemyDeath()
    {
        gameManager.Start();

        yield return null;

        int initialRound = gameManager.round;
        int initialVP = gameManager.VP;
        GameObject initialEnemy = gameManager.currentEnemy;
        gameManager.OnEnemyDeath();

        Assert.AreNotEqual(initialEnemy, gameManager.currentEnemy, "A new enemy should be spawned after the previous enemy's death.");
        Assert.AreEqual(initialRound + 1, gameManager.round, "Round should be incremented after an enemy's death.");
        Assert.AreEqual(initialVP + 1, gameManager.VP, "VP should be incremented after an enemy's death.");
    }
}
