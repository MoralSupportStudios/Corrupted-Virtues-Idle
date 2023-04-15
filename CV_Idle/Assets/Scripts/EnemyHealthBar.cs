using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] public Slider healthSlider;
    public Enemy enemy;

    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        healthSlider.maxValue = enemy.maxHealth;
        healthSlider.value = enemy.health;
    }

    void Update()
    {
        healthSlider.value = enemy.health;
    }

    public void SetEnemy(Enemy newEnemy)
    {
        enemy = newEnemy;
        healthSlider.maxValue = enemy.maxHealth;
        healthSlider.value = enemy.health;
    }
}