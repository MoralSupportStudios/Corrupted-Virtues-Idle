using UnityEngine;

public class Hero : MonoBehaviour
{
    public int damage = 10;

    public void Attack(Enemy enemyScript)
    {
        enemyScript.TakeDamage(damage);
    }
}
