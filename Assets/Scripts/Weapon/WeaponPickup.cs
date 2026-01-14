using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public string weaponName; // Gun, Shotgun, Knife etc

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Transform weaponHolder = other.transform
            .Find("MainCamera/WeaponRoot/CullingMask/WeaponHolder");

        if (!weaponHolder) return;

        Transform weapon = weaponHolder.Find(weaponName);
        if (!weapon) return;

        WeaponSwitching ws = weaponHolder.GetComponent<WeaponSwitching>();
        if (!ws) return;

        weapon.gameObject.SetActive(true);
        ws.AddWeapon(weapon.gameObject);

        Destroy(gameObject);
    }
}
