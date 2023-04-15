using UnityEngine;

public class Hero : MonoBehaviour
{
    public int damage = 100;
    public float attackInterval = 1.0f;
    public float nextAttackTime { get; set; }

    public void Attack(Enemy enemyScript)
    {
        enemyScript.TakeDamage(damage);
    }
}

