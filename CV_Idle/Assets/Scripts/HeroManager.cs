using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroManager : MonoBehaviour
{
    public List<GameObject> heroParty;
    public Transform heroSpawn;
    public UIManager uiManager;

    public void InitializeHeroes()
    {
        foreach (GameObject hero in heroParty)
        {
            hero.GetComponent<Hero>().nextAttackTime = 0;
        }
    }
    public void SpawnHero(GameObject heroPrefab, int heroIndex)
    {
        Vector2[] heroPositions = new Vector2[] {
            new Vector2(-3, -0.5f),
            new Vector2(-5, -1.5f),
            new Vector2(-5, 0.5f),
            new Vector2(-7, -1.5f),
            new Vector2(-7f, 0.5f)
        };

        GameObject hero = Instantiate(heroPrefab, heroPositions[heroIndex], Quaternion.identity);
        hero.transform.SetParent(heroSpawn, false);
        heroParty[heroIndex] = hero;
        hero.transform.localScale = new Vector3(2, 2, 1);
        uiManager.CreateAbilityButton(hero.GetComponent<Hero>().abilityButtonPrefab, hero.GetComponent<Hero>(), heroIndex);
    }
}
