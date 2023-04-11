using UnityEngine;

    public class Hero : MonoBehaviour
    {
        public int damage = 10;
        public float attackInterval = 2.0f;

        public void Attack(Enemy enemyScript)
        {
            enemyScript.TakeDamage(damage);
        }
    }

