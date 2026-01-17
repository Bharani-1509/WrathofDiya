using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    int currentHealth;
    Animator animator;
    bool isDead;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponentInChildren<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        animator.SetTrigger("Hit");

        Debug.Log($"[ENEMY HIT] {gameObject.name} | HP Left: {currentHealth}");

        // Removed ResetAttackCooldown() call

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");
        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        GetComponent<Collider>().enabled = false;

        // Disable AI after death
        EnemyAI enemyAI = GetComponentInChildren<EnemyAI>();
        if (enemyAI != null)
        {
            enemyAI.enabled = false;
        }

        Destroy(gameObject, 5f);
    }
}
    