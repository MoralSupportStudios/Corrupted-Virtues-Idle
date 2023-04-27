using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    public Image buttonForeground;
    public Image buttonBust;
    private float cooldownTimeLeft;
    private Hero hero;
    private GameManager gameManager;

    public void Initialize(Hero hero, GameManager gameManager)
    {
        this.hero = hero;
        this.gameManager = gameManager;
        buttonBust.sprite = hero.bust;
    }

    private void Start()
    {
        buttonForeground.fillAmount = 0;
        cooldownTimeLeft = 0;
    }

    private void Update()
    {
        if (cooldownTimeLeft > 0)
        {
            cooldownTimeLeft -= Time.deltaTime;
            buttonForeground.fillAmount = (cooldownTimeLeft / hero.cooldownDuration);
        }
    }

    public void OnAbilityButtonClicked()
    {
        if (cooldownTimeLeft <= 0)
        {
            // Activate ability and deal damage
            if (gameManager.enemyManager.CurrentEnemy != null)
            {
                gameManager.enemyManager.CurrentEnemy.GetComponent<Enemy>().TakeDamage(hero.abilityDamage);
            }

            // Start cooldown
            cooldownTimeLeft = hero.cooldownDuration;
            buttonForeground.fillAmount = 1;
        }
    }

}
