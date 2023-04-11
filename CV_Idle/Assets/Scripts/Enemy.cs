using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 10;
    public event Action OnEnemyDeath;
    public Sprite sprite;

    public void TakeDamage(int damage)
    {
        health -= damage;
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

        // Destroy the enemy game object
        Destroy(gameObject);
    }
}
