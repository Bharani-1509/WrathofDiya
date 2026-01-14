using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public string weaponName;   // "Gun", "Shotgun", "Knife"

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Transform weaponHolder = other.transform
            .Find("MainCamera/WeaponRoot/CullingMask/WeaponHolder");

        if (weaponHolder == null) return;

        Transform weapon = weaponHolder.Find(weaponName);

        if (weapon == null) return;

        // If already owned
        if (weapon.gameObject.activeSelf)
            return;

        weapon.gameObject.SetActive(true);

        // Auto-equip
        WeaponSwitching ws = weaponHolder.GetComponent<WeaponSwitching>();
        ws.selectedWeapon = weapon.GetSiblingIndex();

        Destroy(gameObject);
    }
}
