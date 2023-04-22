using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroPanel : MonoBehaviour
{
    public Image heroImage;
    public TMP_Text heroNameText;
    public Button increaseDamageButton;
    public Button decreaseAttackIntervalButton;
    public Button buyHeroButton;

    public void Initialize(int heroIndex, ShopManager shopManager)
    {
        GameObject hero = shopManager.availableHeroes[heroIndex];
        bool isInParty = shopManager.gameManager.heroParty.Exists(h => h.name.StartsWith(hero.name));

        heroNameText.text = hero.name;
        heroImage.sprite = hero.GetComponent<Hero>().bust;

        increaseDamageButton.gameObject.SetActive(isInParty);
        decreaseAttackIntervalButton.gameObject.SetActive(isInParty);
        buyHeroButton.gameObject.SetActive(!isInParty);

        if (isInParty)
        {
            int partyIndex = shopManager.gameManager.heroParty.FindIndex(h => h.name.StartsWith(hero.name));
            increaseDamageButton.onClick.AddListener(() => shopManager.IncreaseDamage(partyIndex));
            decreaseAttackIntervalButton.onClick.AddListener(() => shopManager.DecreaseAttackInterval(partyIndex));
        }
        else
        {
            buyHeroButton.onClick.AddListener(() => {
                shopManager.PurchaseHero(heroIndex, this);
                Initialize(heroIndex, shopManager);
            });
        }
    }

}