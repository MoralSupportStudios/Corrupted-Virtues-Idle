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

    public int damageUpgradeCost = 10;
    public int attackIntervalUpgradeCost = 100;

    public List<GameObject> heroPanels = new List<GameObject>();

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

        UpdateHeroPanelHeroStat();
        //This works right now of the heirchy structure of the canvas. If the heirchy changes, this will break.
        gameManager.uiManager.ButtonPanel.SetActive(false);
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

    public void IncreaseDamage(HeroPanel heroPanel)
    {
        Hero selectedHero = heroPanel.Hero.GetComponent<Hero>();

        if (gameManager.virtuePoints >= damageUpgradeCost)
        {
            selectedHero.damage += 1;
            gameManager.virtuePoints -= damageUpgradeCost;

            heroPanel.UpdateHeroStats(); // Update the hero stats in the panel
        }
    }

    public void DecreaseAttackInterval(HeroPanel heroPanel)
    {
        Hero selectedHero = heroPanel.Hero.GetComponent<Hero>();

        if (gameManager.virtuePoints >= attackIntervalUpgradeCost && selectedHero.attackInterval > 0.1f)
        {
            selectedHero.attackInterval -= 0.1f;
            gameManager.virtuePoints -= attackIntervalUpgradeCost;

            heroPanel.UpdateHeroStats(); // Update the hero stats in the panel
        }
    }


    public void PurchaseHero(int heroIndex, HeroPanel heroPanel)
    {
        GameObject hero = availableHeroes[heroIndex];
        if (gameManager.virtuePoints >= hero.GetComponent<Hero>().purchaseCost)
        {
            if (!gameManager.heroManager.heroParty.Exists(h => h.name.StartsWith(hero.name)))
            {
                gameManager.heroManager.heroParty.Add(hero);
                gameManager.virtuePoints -= hero.GetComponent<Hero>().purchaseCost;

                heroPanel.Initialize(heroIndex, this);

                gameManager.heroManager.SpawnHero(hero, gameManager.heroManager.heroParty.Count - 1);
                ToggleHero();
            }
        }
    }

}
