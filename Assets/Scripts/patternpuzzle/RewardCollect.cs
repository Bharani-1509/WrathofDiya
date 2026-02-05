using UnityEngine;

public class RewardCollect : MonoBehaviour
{
    private bool playerInRange = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Press E to collect reward");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            CollectReward();
        }
    }

    void CollectReward()
    {
        Debug.Log("Reward collected!");
        // Optional: play sound, add score, particles, etc. here
        Destroy(gameObject);
    }
}