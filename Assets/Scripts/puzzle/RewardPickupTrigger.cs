using UnityEngine;
using TMPro;

public class RewardPickupTrigger : MonoBehaviour
{
    [Header("Healing")]
    [SerializeField] private int healAmount = 25;

    [Header("UI Prompt")]
    [SerializeField] private TMP_Text pickupPromptText;   // ← Drag your "Press E" text here!

    private bool playerInside = false;
    private PlayerHealth playerHealthRef;
    private bool hasBeenCollected = false;

    void Awake()
    {
        // Ensure we have a trigger collider
        Collider col = GetComponent<Collider>();
        if (col == null)
        {
            SphereCollider sphere = gameObject.AddComponent<SphereCollider>();
            sphere.isTrigger = true;
            sphere.radius = 1.8f;   // adjust to your pickup size
        }
        else
        {
            col.isTrigger = true;
        }
    }

    void Start()
    {
        if (pickupPromptText != null)
        {
            pickupPromptText.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning($"PickupPromptText is not assigned on {gameObject.name}");
        }
    }

    void Update()
    {
        if (!playerInside || hasBeenCollected) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (playerHealthRef != null)
            {
                playerHealthRef.Heal(healAmount);
            }

            hasBeenCollected = true;

            if (pickupPromptText != null)
                pickupPromptText.gameObject.SetActive(false);

            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasBeenCollected) return;

        if (other.CompareTag("Player"))
        {
            playerInside = true;
            playerHealthRef = other.GetComponent<PlayerHealth>();

            if (pickupPromptText != null)
                pickupPromptText.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            playerHealthRef = null;

            if (pickupPromptText != null)
                pickupPromptText.gameObject.SetActive(false);
        }
    }
}