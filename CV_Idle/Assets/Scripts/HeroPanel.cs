using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroPanel : MonoBehaviour
{
    public Image heroImage;
    public TMP_Text heroNameText;
    public TMP_Text heroStatsText;
    public Button increaseDamageButton;
    public Button decreaseAttackIntervalButton;
    public Button buyHeroButton;
    public GameObject Hero { get; private set; }

    private ShopManager shopManager;

    public void UpdateHeroStats()
    {
        Hero heroComponent = Hero.GetComponent<Hero>();
        heroStatsText.text = $"Damage Per Attack: {heroComponent.damage}" +
                             $"\r\nAttacks Per Second: {heroComponent.attackInterval}" +
                             $"\r\nAbility Damage: {heroComponent.abilityDamage}";

        increaseDamageButton.GetComponentInChildren<TMP_Text>().text = $"Increase Damage {heroComponent.damage} \r\nCost {shopManager.damageUpgradeCost} VP";
        decreaseAttackIntervalButton.GetComponentInChildren<TMP_Text>().text = $"Buy Hero {heroComponent.attackInterval} \r\nCost {shopManager.attackIntervalUpgradeCost} VP";
    }
    public void Initialize(int heroIndex, ShopManager shopManager)
    {
        GameObject hero = shopManager.availableHeroes[heroIndex];
        Hero = hero;
        bool isInParty = shopManager.gameManager.heroManager.heroParty.Exists(h => h.name.StartsWith(hero.name));

        this.shopManager = shopManager; 

        heroNameText.text = hero.name;
        heroImage.sprite = hero.GetComponent<Hero>().bust;
        heroStatsText.text = "";
        increaseDamageButton.gameObject.SetActive(isInParty);
        decreaseAttackIntervalButton.gameObject.SetActive(isInParty);
        buyHeroButton.gameObject.SetActive(!isInParty);

        if (isInParty)
        {
            // Store the associated hero game object in the new Hero property
            Hero = shopManager.gameManager.heroManager.heroParty.Find(h => h.name.StartsWith(hero.name));

            // Pass 'this' (the current HeroPanel object) to the methods instead of the partyIndex
            increaseDamageButton.onClick.AddListener(() => shopManager.IncreaseDamage(this));
            increaseDamageButton.GetComponentInChildren<TMP_Text>().text = $"Increase Damage {Hero.GetComponent<Hero>().damage} \r\nCost {shopManager.damageUpgradeCost} VP";
            decreaseAttackIntervalButton.onClick.AddListener(() => shopManager.DecreaseAttackInterval(this));
            decreaseAttackIntervalButton.GetComponentInChildren<TMP_Text>().text = $"Buy Hero {Hero.GetComponent<Hero>().attackInterval} \r\nCost {shopManager.attackIntervalUpgradeCost} VP";
        }
        else
        {
            buyHeroButton.GetComponentInChildren<TMP_Text>().text = $"Buy Hero {hero.GetComponent<Hero>().purchaseCost} VP";
            buyHeroButton.onClick.AddListener(() =>
            {
                shopManager.PurchaseHero(heroIndex, this);
                Initialize(heroIndex, shopManager);
            });
        }
    }
}
