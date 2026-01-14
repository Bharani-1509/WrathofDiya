using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    public List<GameObject> ownedWeapons = new List<GameObject>();
    public int selectedWeapon = 0;

    void Start()
    {
        // Add all active weapons at start (Knife)
        foreach (Transform weapon in transform)
        {
            if (weapon.gameObject.activeSelf)
                ownedWeapons.Add(weapon.gameObject);
            else
                weapon.gameObject.SetActive(false);
        }

        SelectWeapon();
    }

    void Update()
    {
        if (ownedWeapons.Count <= 1) return;

        int previous = selectedWeapon;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            selectedWeapon = (selectedWeapon + 1) % ownedWeapons.Count;

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            selectedWeapon = (selectedWeapon - 1 + ownedWeapons.Count) % ownedWeapons.Count;

        if (previous != selectedWeapon)
            SelectWeapon();
    }

    void SelectWeapon()
    {
        for (int i = 0; i < ownedWeapons.Count; i++)
        {
            ownedWeapons[i].SetActive(i == selectedWeapon);
        }
    }

    public void AddWeapon(GameObject newWeapon)
    {
        if (ownedWeapons.Contains(newWeapon)) return;

        ownedWeapons.Add(newWeapon);
        selectedWeapon = ownedWeapons.Count - 1; // auto-equip new weapon
        SelectWeapon();
    }
}
