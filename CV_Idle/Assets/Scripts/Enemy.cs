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

    private HealthBar healthBarInstance;

    public void CreateHealthBar()
    {
        // Instantiate the health bar prefab as a child of the Canvas
        healthBarInstance = Instantiate(healthBarPrefab, canvas.transform);

        healthBarInstance.transform.position = new Vector3(6, 4, healthBarInstance.transform.position.z);


        // Initialize the health bar with the max health value
        healthBarInstance.SetMaxHealth(maxHealth);
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

    private void Die()
    {
        // Handle the enemy's death, e.g. play animation
        Debug.Log("Enemy died!");

        // Notify GameManager that the enemy has died
        OnEnemyDeath?.Invoke();

        // Destroy the health bar instance if it exists
        if (healthBarInstance != null)
        {
            Destroy(healthBarInstance.gameObject);
        }

        // Destroy the enemy game object if it exists
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }
}
