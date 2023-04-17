using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroPanel : MonoBehaviour
{
    public Image heroImage;
    public TMP_Text heroNameText;
    public Button increaseDamageButton;
    public Button decreaseAttackIntervalButton;

    public void Initialize(int heroIndex, ShopManager shopManager)
    {
        heroNameText.text = shopManager.gameManager.heroParty[heroIndex].name;
        heroImage.sprite = shopManager.gameManager.heroParty[heroIndex].GetComponent<Hero>().bust;

        increaseDamageButton.onClick.AddListener(() => shopManager.IncreaseDamage(heroIndex));
        decreaseAttackIntervalButton.onClick.AddListener(() => shopManager.DecreaseAttackInterval(heroIndex));
    }
}
