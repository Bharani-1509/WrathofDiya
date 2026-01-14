using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 15;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Gun gun = other.GetComponentInChildren<Gun>();

        if (gun == null)
        {
            Debug.Log("No Gun found on player");
            return;
        }
        if (gun.ammoReserve >= gun.maxAmmoReserve)
        {
            Debug.Log("Ammo already full. Pickup blocked.");
            return;
        }
            

        int spaceLeft = gun.maxAmmoReserve - gun.ammoReserve;
        int ammoToGive = Mathf.Min(spaceLeft, ammoAmount);

        gun.ammoReserve += ammoToGive;

        Destroy(gameObject);
    }
}
