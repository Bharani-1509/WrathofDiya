using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    bool isDead;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        Debug.Log(
            $"[PLAYER HIT] Damage: {damage} | HP Left: {currentHealth}"
        );

        if (currentHealth <= 0)
        {
            Debug.Log("[PLAYER DEAD]");
            Die();
        }
    }


    void Die()
    {
        isDead = true;
        Debug.Log("Player Dead");

        // Disable movement
        GetComponent<PlayerMovement>().enabled = false;

        // Optional: death animation / UI / restart
    }
}
