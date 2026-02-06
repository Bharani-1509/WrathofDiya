using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI")]
    public Image healthBarFill;
    public TextMeshProUGUI healthText;

    [Header("Animation")]
    public float barSpeed = 5f;
    private float displayPercent; // 0–1 value

    private bool isDead;

    [Header("Respawn")]
    public Transform spawnPoint;
    public float respawnDelay = 2f;

    private PlayerMovement movement;
    private CharacterController characterController;
    private Rigidbody rb;

    void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        currentHealth = maxHealth;
        displayPercent = 1f;
        SetupBar();
        UpdateText();
    }

    void Update()
    {
        if (healthBarFill == null) return;

        float targetPercent = (float)currentHealth / maxHealth;
        displayPercent = Mathf.MoveTowards(
            displayPercent,
            targetPercent,
            barSpeed * Time.deltaTime
        );

        healthBarFill.fillAmount = displayPercent;
    }

    void SetupBar()
    {
        if (healthBarFill == null) return;

        healthBarFill.type = Image.Type.Filled;
        healthBarFill.fillMethod = Image.FillMethod.Horizontal;
        healthBarFill.fillOrigin = (int)Image.OriginHorizontal.Left;
        healthBarFill.fillAmount = 1f;
    }

    // ================= DAMAGE =================
    public void TakeDamage(int dmg)
    {
        if (isDead) return;

        currentHealth -= dmg;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateText();

        if (currentHealth <= 0)
            Die();
    }

    // ================= HEAL =================
    public void Heal(int amount)
    {
        if (isDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // FIXED: Force bar to immediately move UP (snap or faster catch-up)
        float targetPercent = (float)currentHealth / maxHealth;
        displayPercent = Mathf.Max(displayPercent, targetPercent);

        UpdateText();
    }

    void UpdateText()
    {
        if (healthText != null)
            healthText.text = $"{currentHealth} / {maxHealth}";
    }

    // ================= DEATH =================
    void Die()
    {
        isDead = true;

        if (movement != null)
            movement.enabled = false;

        Invoke(nameof(Respawn), respawnDelay);
    }

    void Respawn()
    {
        if (characterController != null)
            characterController.enabled = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (spawnPoint != null)
            transform.position = spawnPoint.position;

        currentHealth = maxHealth;
        displayPercent = 1f;
        isDead = false;

        UpdateText();

        if (characterController != null)
            characterController.enabled = true;

        if (movement != null)
            movement.enabled = true;
    }
}