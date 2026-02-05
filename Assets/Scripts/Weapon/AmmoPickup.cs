using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 15;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (Gun.ActiveGun == null)
        {
            Debug.LogError("AmmoPickup: No ActiveGun found!");
            return;
        }

        Gun.ActiveGun.AddAmmo(ammoAmount);
        Destroy(gameObject);
    }
}
