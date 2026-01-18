using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public EnemyStats stats;

    private int currentHealth;
    private Animator animator;
    private bool isDead;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => stats.maxHealth;
    public GameObject deathVFXPrefab;

    void Start()
    {
        currentHealth = stats.maxHealth;
        animator = GetComponentInChildren<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        animator.SetTrigger("Hit");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");

        // Spawn death particles
        if (deathVFXPrefab != null)
            Instantiate(deathVFXPrefab, transform.position + Vector3.up * 1f, Quaternion.identity);

        // Disable components
        var agent = GetComponent<NavMeshAgent>();
        if (agent != null) agent.enabled = false;

        var collider = GetComponent<Collider>();
        if (collider != null) collider.enabled = false;

        var enemyAI = GetComponent<EnemyAI>();
        if (enemyAI != null) enemyAI.enabled = false;

        Destroy(gameObject, 0.5f);
    }
}
