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

        gameManager.enemyManager.CurrentEnemy.SetActive(!gameObject.activeSelf);
        gameManager.enemyManager.CurrentEnemy.GetComponent<Enemy>().healthBarInstance.gameObject.SetActive(!gameObject.activeSelf);

        ToggleHero();

        //Update hero panel
        UpdateHeroPanelHeroStat();
    }

    private void UpdateHeroPanelHeroStat()
    {
        if (gameObject.activeSelf)
        {
            int index = 0;
            foreach (GameObject go in heroPanels)
            {

                if (gameManager.heroManager.heroParty.Exists(h => h.name.StartsWith(go.GetComponent<HeroPanel>().heroNameText.text)))
                {

                    go.GetComponent<HeroPanel>().heroStatsText.text = $"Damage Per Attack: {gameManager.heroManager.heroParty[index].GetComponent<Hero>().damage}" +
                                         $"\r\nAttacks Per Second: {gameManager.heroManager.heroParty[index].GetComponent<Hero>().attackInterval}" +
                                         $"\r\nAbility Damage: {gameManager.heroManager.heroParty[index].GetComponent<Hero>().abilityDamage}";
                }
                index++;
            }
        }
    }

    private void ToggleHero()
    {
        foreach (GameObject hero in gameManager.heroManager.heroParty)
        {
            hero.SetActive(!gameObject.activeSelf);
        }
        gameManager.uiManager.abilityButtonsContainer.gameObject.SetActive(!gameObject.activeSelf);
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
        Hero selectedHero = gameManager.heroManager.heroParty[heroIndex].GetComponent<Hero>();

        if (gameManager.virtuePoints >= damageUpgradeCost)
        {
            selectedHero.damage += 10;
            gameManager.virtuePoints -= damageUpgradeCost;

            UpdateHeroPanelHeroStat();
        }
    }

    public void DecreaseAttackInterval(int heroIndex)
    {
        Hero selectedHero = gameManager.heroManager.heroParty[heroIndex].GetComponent<Hero>();

        if (gameManager.virtuePoints >= attackIntervalUpgradeCost && selectedHero.attackInterval > 0.1f)
        {
            selectedHero.attackInterval -= 0.1f;
            gameManager.virtuePoints -= attackIntervalUpgradeCost;

            UpdateHeroPanelHeroStat();
        }
    }


    public void PurchaseHero(int heroIndex, HeroPanel heroPanel)
    {
        if (gameManager.virtuePoints >= heroPurchaseCost)
        {
            GameObject hero = availableHeroes[heroIndex];
            if (!gameManager.heroManager.heroParty.Exists(h => h.name.StartsWith(hero.name)))
            {
                gameManager.heroManager.heroParty.Add(hero);
                gameManager.virtuePoints -= heroPurchaseCost;

                heroPanel.Initialize(heroIndex, this);

                gameManager.heroManager.SpawnHero(hero, gameManager.heroManager.heroParty.Count - 1);
                ToggleHero();
            }
        }
    }

}
