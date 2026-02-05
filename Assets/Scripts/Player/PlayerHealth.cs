using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;
    private bool isDead;

    [Header("UI - Image Fill Style")]
    public Image healthBarFill;           // Drag the FILL Image (Image.Type = Filled)
    public TextMeshProUGUI healthText;    // Optional numbers display

    [Header("Respawn Settings")]
    public Transform spawnPoint;          // ← Drag your SpawnPoint GameObject here
    public float respawnDelay = 2f;       // Time before respawn (seconds)

    private Vector3 initialPosition;      // Fallback if no spawn point assigned
    private Quaternion initialRotation;

    void Awake()
    {
        // Remember starting position/rotation as fallback
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    void Start()
    {
        currentHealth = maxHealth;

        // Setup fill image properties once
        if (healthBarFill != null)
        {
            healthBarFill.type = Image.Type.Filled;
            healthBarFill.fillMethod = Image.FillMethod.Horizontal;
            healthBarFill.fillOrigin = (int)Image.OriginHorizontal.Left;
            healthBarFill.fillAmount = 1f;
        }

        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBarFill == null) return;

        float percent = (float)currentHealth / maxHealth;
        healthBarFill.fillAmount = percent;

        // Color transition
        if (percent > 0.6f)
            healthBarFill.color = new Color(0.2f, 0.8f, 0.2f);      // green
        else if (percent > 0.3f)
            healthBarFill.color = new Color(1f, 0.8f, 0.1f);        // yellow
        else
            healthBarFill.color = new Color(0.9f, 0.15f, 0.15f);    // red

        if (healthText != null)
        {
            healthText.text = $"{currentHealth} / {maxHealth}";
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("PLAYER DEAD → respawning in " + respawnDelay + " seconds");

        // Disable movement
        var movement = GetComponent<PlayerMovement>();
        if (movement != null)
        {
            movement.enabled = false;
        }

        // Visual feedback: empty bar
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = 0f;
        }

        // Schedule respawn
        Invoke(nameof(Respawn), respawnDelay);
    }

    private void Respawn()
    {
        Debug.Log("Respawn() executed");

        // Reset health & state
        currentHealth = maxHealth;
        isDead = false;
        UpdateHealthBar();

        // Move to spawn point (with fallback)
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;
            Debug.Log($"Respawned at spawn point: {spawnPoint.position}");
        }
        else
        {
            // Fallback to initial position
            transform.position = initialPosition;
            transform.rotation = initialRotation;
            Debug.LogWarning("No spawn point assigned → respawned at initial position");
        }

        // Re-enable movement
        var movement = GetComponent<PlayerMovement>();
        if (movement != null)
        {
            movement.enabled = true;
            Debug.Log("Player movement re-enabled");
        }
    }

    // Optional: public method to change spawn point (for checkpoints later)
    public void SetSpawnPoint(Transform newSpawn)
    {
        if (newSpawn != null)
        {
            spawnPoint = newSpawn;
            Debug.Log("Spawn point updated to: " + newSpawn.position);
        }
    }
}