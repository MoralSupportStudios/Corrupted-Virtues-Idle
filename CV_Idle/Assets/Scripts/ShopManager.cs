using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameManager gameManager;
    public TMP_Text virtuePointsText;
    public GameObject heroPanelPrefab;
    public Transform heroPanelsContainer;
    public List<GameObject> availableHeroes;

    public int damageUpgradeCost = 5;
    public int attackIntervalUpgradeCost = 10;
    public int heroPurchaseCost = 100;

    private List<GameObject> heroPanels = new List<GameObject>();

    private void Start()
    {
        CreateHeroPanels();
    }

    private void Update()
    {
        virtuePointsText.text = $"VP: {gameManager.virtuePoints}";
    }

    public void ToggleShop()
    {
        gameObject.SetActive(!gameObject.activeSelf);

        gameManager.CurrentEnemy.SetActive(!gameObject.activeSelf);
        gameManager.CurrentEnemy.GetComponent<Enemy>().healthBarInstance.gameObject.SetActive(!gameObject.activeSelf);

        ToggleHero();
    }

    private void ToggleHero()
    {
        foreach (GameObject hero in gameManager.heroParty)
        {
            hero.SetActive(!gameObject.activeSelf);
        }
    }

    private void CreateHeroPanels()
    {
        for (int i = 0; i < availableHeroes.Count; i++)
        {
            GameObject panel = Instantiate(heroPanelPrefab, heroPanelsContainer);
            panel.GetComponent<HeroPanel>().Initialize(i, this);
            heroPanels.Add(panel);
        }
    }

    public void IncreaseDamage(int heroIndex)
    {
        Hero selectedHero = gameManager.heroParty[heroIndex].GetComponent<Hero>();

        if (gameManager.virtuePoints >= damageUpgradeCost)
        {
            selectedHero.damage += 10;
            gameManager.virtuePoints -= damageUpgradeCost;
        }
    }

    public void DecreaseAttackInterval(int heroIndex)
    {
        Hero selectedHero = gameManager.heroParty[heroIndex].GetComponent<Hero>();

        if (gameManager.virtuePoints >= attackIntervalUpgradeCost && selectedHero.attackInterval > 0.1f)
        {
            selectedHero.attackInterval -= 0.1f;
            gameManager.virtuePoints -= attackIntervalUpgradeCost;
        }
    }


    public void PurchaseHero(int heroIndex, HeroPanel heroPanel)
    {
        if (gameManager.virtuePoints >= heroPurchaseCost)
        {
            GameObject hero = availableHeroes[heroIndex];
            if (!gameManager.heroParty.Exists(h => h.name.StartsWith(hero.name)))
            {
                gameManager.heroParty.Add(hero);
                gameManager.virtuePoints -= heroPurchaseCost;

                heroPanel.Initialize(heroIndex, this);

                gameManager.SpawnHero(hero, gameManager.heroParty.Count - 1);
                ToggleHero();
            }
        }
    }

}
