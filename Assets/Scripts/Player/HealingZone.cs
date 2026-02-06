using UnityEngine;

public class HealingZone : MonoBehaviour
{
    [Header("Healing Settings")]
    [SerializeField] private int healAmount = 20;           // How much health to restore per tick
    [SerializeField] private float healInterval = 0.5f;     // How often to heal (in seconds)

    [Header("Feedback (optional)")]
    [SerializeField] private bool showDebugMessages = true;

    private float nextHealTime;

    private void Awake()
    {
        // Make sure this object has a trigger collider
        Collider col = GetComponent<Collider>();
        if (col == null)
        {
            Debug.LogWarning("HealingZone needs a Collider! Adding SphereCollider automatically.");
            SphereCollider sphere = gameObject.AddComponent<SphereCollider>();
            sphere.isTrigger = true;
            sphere.radius = 2f; // ← you can change this in Inspector later
        }
        else
        {
            col.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (showDebugMessages)
            {
                Debug.Log("Player entered healing zone → healing started");
            }

            // Optional: Heal once immediately when entering
            HealPlayer(other);

            // Reset timer so healing starts right away
            nextHealTime = Time.time + healInterval;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Heal periodically while staying inside
        if (Time.time >= nextHealTime)
        {
            HealPlayer(other);
            nextHealTime = Time.time + healInterval;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (showDebugMessages)
            {
                Debug.Log("Player left healing zone");
            }
        }
    }

    private void HealPlayer(Collider playerCollider)
    {
        PlayerHealth health = playerCollider.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.Heal(healAmount);

            if (showDebugMessages)
            {
                Debug.Log($"Healed player for {healAmount}. Current HP: {health.currentHealth}");
            }
        }
    }
}