using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 10;
    public int health = 10;
    public event Action OnEnemyDeath;
    public bool isBoss = false;
    public Canvas canvas;
    public HealthBar healthBarPrefab;

    public HealthBar healthBarInstance;

    public void CreateHealthBar()
    {
        // Instantiate the health bar prefab as a child of the Canvas
        healthBarInstance = Instantiate(healthBarPrefab, canvas.transform);

        healthBarInstance.transform.position = new Vector3(6, 4, healthBarInstance.transform.position.z);

        healthBarInstance.SetHealth(health, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBarInstance.SetHealth(health, maxHealth);

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        OnEnemyDeath?.Invoke();

        if (healthBarInstance != null)
        {
            Destroy(healthBarInstance.gameObject);
        }
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }
}
