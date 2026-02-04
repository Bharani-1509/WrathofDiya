using TMPro;
using UnityEngine;

public class PatternRewardPickupTrigger : MonoBehaviour
{
    [Header("UI (Scene TMP object name)")]
    public string pickupTextObjectName = "PickupText";

    [Header("Pickup Key")]
    public KeyCode pickupKey = KeyCode.E;

    private TMP_Text pickupText;
    private bool playerInside = false;
    private bool collected = false;

    void Awake()
    {
        // Ensure Rigidbody exists (trigger needs it)
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody>();

        rb.isKinematic = true;
        rb.useGravity = false;

        // Ensure collider exists and is trigger
        Collider col = GetComponent<Collider>();
        if (col == null)
        {
            SphereCollider sc = gameObject.AddComponent<SphereCollider>();
            sc.isTrigger = true;
            sc.radius = 1.5f;
        }
        else
        {
            col.isTrigger = true;
        }
    }

    void Start()
    {
        // Auto-find TMP text in scene (works with spawned prefab)
        GameObject t = GameObject.Find(pickupTextObjectName);

        if (t != null)
            pickupText = t.GetComponent<TMP_Text>();

        if (pickupText == null)
        {
            Debug.LogError($"❌ TMP Text not found. Create TMP text named: {pickupTextObjectName}");
            return;
        }

        pickupText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (collected) return;

        if (playerInside && Input.GetKeyDown(pickupKey))
        {
            CollectReward();
        }
    }

    void CollectReward()
    {
        collected = true;

        Debug.Log("🎁 Pattern Puzzle Reward Collected!");

        if (pickupText != null)
            pickupText.gameObject.SetActive(false);

        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (collected) return;

        if (other.CompareTag("Player"))
        {
            playerInside = true;

            if (pickupText != null)
                pickupText.gameObject.SetActive(true);

            Debug.Log("✅ Player entered Pattern Reward trigger");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (collected) return;

        if (other.CompareTag("Player"))
        {
            playerInside = false;

            if (pickupText != null)
                pickupText.gameObject.SetActive(false);

            Debug.Log("⬅ Player left Pattern Reward trigger");
        }
    }
}
