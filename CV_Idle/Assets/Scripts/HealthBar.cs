using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image foregroundImage;
    public TMP_Text healthText;

    public void SetHealth(int health, int maxHealth)
    {
        float healthPercentage = (float)health / (float)maxHealth;
        foregroundImage.fillAmount = healthPercentage;
        healthText.text = $"{health} / {maxHealth}";
    }
}
