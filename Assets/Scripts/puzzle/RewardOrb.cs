using UnityEngine;

public class RewardOrb : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("🎁 Reward orb collected!");
            Destroy(gameObject);
        }
    }
}
