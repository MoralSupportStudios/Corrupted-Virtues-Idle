using UnityEngine;

public class Hero : MonoBehaviour
{
    public int damage = 100;
    public int abilityDamage = 1000;
    public float cooldownDuration = 60.0f;
    public float attackInterval = 1.0f;
    public GameObject abilityButtonPrefab;
    public Sprite bust;
    public int purchaseCost = 1;
    public float nextAttackTime { get; set; }

    public void Attack(Enemy enemyScript)
    {
        enemyScript.TakeDamage(damage);
    }
}

