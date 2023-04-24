using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    public Image buttonForeground;
    public Image buttonBust;
    public float cooldownDuration = 5.0f;
    private float cooldownTimeLeft;

    private Hero hero;
    private GameManager gameManager;
    private int abilityDamage = 1000;

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
            buttonForeground.fillAmount = (cooldownTimeLeft / cooldownDuration);
        }
    }

    public void OnAbilityButtonClicked()
    {
        if (cooldownTimeLeft <= 0)
        {
            // Activate ability and deal damage
            if (gameManager.CurrentEnemy != null)
            {
                gameManager.CurrentEnemy.GetComponent<Enemy>().TakeDamage(abilityDamage);
            }

            // Start cooldown
            cooldownTimeLeft = cooldownDuration;
            buttonForeground.fillAmount = 1;
        }
    }

}
