using TMPro;
using UnityEngine;

public class RewardPickupTrigger : MonoBehaviour
{
    [Header("UI Text Object Name (in scene)")]
    public string pickupTextObjectName = "PickupText";

    private TMP_Text pickupText;

    private bool playerInside = false;
    private bool collected = false;

    void Start()
    {
        // Find TMP in the SCENE at runtime (works for spawned prefabs)
        GameObject t = GameObject.Find(pickupTextObjectName);

        if (t != null)
            pickupText = t.GetComponent<TMP_Text>();

        if (pickupText == null)
            Debug.LogError($"❌ TMP Text not found. Create TMP object named: {pickupTextObjectName}");

        if (pickupText != null)
            pickupText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (collected) return;

        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            CollectReward();
        }
    }

    void CollectReward()
    {
        collected = true;

        Debug.Log("🎁 Player collected the reward!");

        if (pickupText != null)
            pickupText.gameObject.SetActive(false);

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collected) return;

        if (other.CompareTag("Player"))
        {
            playerInside = true;

            if (pickupText != null)
                pickupText.gameObject.SetActive(true);

            Debug.Log("✅ Player entered reward trigger");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (collected) return;

        if (other.CompareTag("Player"))
        {
            playerInside = false;

            if (pickupText != null)
                pickupText.gameObject.SetActive(false);

            Debug.Log("⬅ Player left reward trigger");
        }
    }
}
