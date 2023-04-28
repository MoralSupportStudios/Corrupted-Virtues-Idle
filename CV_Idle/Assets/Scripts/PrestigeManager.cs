using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrestigeManager : MonoBehaviour
{
    public GameManager gameManager;
    public LevelManager levelManager;
    public UIManager uiManager;
    public int prestigeMultiplier = 2;
    public TMP_Text sanctifyPoints;
    public TMP_Text howManySanctify;
    public void Prestige()
    {
        ResetProgress();
        RewardSanctifyPoints();
    }

    private void RewardSanctifyPoints()
    {
        gameManager.sanctifyPoints += levelManager.cycle;

        sanctifyPoints.text = $"Sanctify Points: {gameManager.sanctifyPoints}";
    }

    void HowManySanctify()
    {
        sanctifyPoints.text = $"If you Sanctify Right now you will get {levelManager.cycle} and your progress will reset";
    }

    private void ResetProgress()
    {
        // Reset stage, round, and cycle
        levelManager.round = -1;
        levelManager.stage = 0;
        levelManager.cycle = 0;

        gameManager.enemyManager.CurrentEnemy.GetComponent<Enemy>().Die();

        List<GameObject> heroesToRemove = new List<GameObject>();
        // Reset hero levels and stats
        foreach (GameObject hero in gameManager.heroManager.heroParty)
        {
            Hero heroComponent = hero.GetComponent<Hero>();
            heroComponent.damage = 1;
            heroComponent.attackInterval = 1;
            
            //remove all but first party member
            if (hero != gameManager.heroManager.heroParty[0])
            {
                
                heroesToRemove.Add(heroComponent.gameObject);     
            }
        }

        foreach (GameObject hero in heroesToRemove)
        {
            gameManager.heroManager.heroParty.Remove(hero);
            Destroy(hero);   
        }

        for (int i = 1; i < uiManager.abilityButtonsContainer.childCount; i++)
        {
            GameObject button = uiManager.abilityButtonsContainer.GetChild(i).gameObject;
            Destroy(button);
        }

        for (int i = 0; i < uiManager.ShopPanel.GetComponent<ShopManager>().heroPanels.Count; i++)
        {
            Destroy(uiManager.ShopPanel.GetComponent<ShopManager>().heroPanels[i]);
        }

        uiManager.ShopPanel.GetComponent<ShopManager>().heroPanels.Clear();

        for (int i = 0; i < uiManager.ShopPanel.GetComponent<ShopManager>().availableHeroes.Count; i++)
        {
            GameObject panel = Instantiate(uiManager.ShopPanel.GetComponent<ShopManager>().heroPanelPrefab, uiManager.ShopPanel.GetComponent<ShopManager>().heroPanelsContainer);
            panel.GetComponent<HeroPanel>().Initialize(i, uiManager.ShopPanel.GetComponent<ShopManager>());
            uiManager.ShopPanel.GetComponent<ShopManager>().heroPanels.Add(panel);
        }
        gameManager.virtuePoints = 0;
    }

}
