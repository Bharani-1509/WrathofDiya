using UnityEngine;
using System.Collections;
using TMPro;

public class Gun : MonoBehaviour
{
    // 🔥 GLOBAL ACCESS (FIXES PICKUP ISSUES)
    public static Gun ActiveGun;

    [Header("Gun Stats")]
    public float damage = 10f;
    public float range = 100f;
    public float impactForce = 30f;
    public float fireRate = 15f;

    [Header("Ammo")]
    public int maxAmmo = 10;
    public int ammoReserve = 30;
    public int maxAmmoReserve = 90;
    private int currentAmmo;

    [Header("Reload")]
    public float reloadTime = 1f;
    private bool isReloading;

    [Header("Effects")]
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public Animator animator;
    public Camera fpsCam;

    [Header("UI")]
    public TextMeshProUGUI ammoText;

    private float nextTimeToFire;
    private LayerMask shootMask;

    void Awake()
    {
        // Register gun early
        ActiveGun = this;
    }

    void OnEnable()
    {
        ActiveGun = this;
        isReloading = false;

        if (animator != null)
            animator.SetBool("Reloading", false);

        UpdateAmmoUI();
    }

    void Start()
    {
        currentAmmo = maxAmmo;
        shootMask = ~LayerMask.GetMask("Player", "Weapon");
        UpdateAmmoUI();
    }

    void Update()
    {
        if (isReloading) return;

        // Auto reload
        if (currentAmmo <= 0 && ammoReserve > 0)
        {
            StartCoroutine(Reload());
            return;
        }

        // Shoot
        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire && currentAmmo > 0)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    IEnumerator Reload()
    {
        if (ammoReserve <= 0) yield break;

        isReloading = true;
        if (animator != null)
            animator.SetBool("Reloading", true);

        yield return new WaitForSeconds(reloadTime);

        int needed = maxAmmo - currentAmmo;
        int take = Mathf.Min(needed, ammoReserve);

        currentAmmo += take;
        ammoReserve -= take;

        if (animator != null)
            animator.SetBool("Reloading", false);

        isReloading = false;
        UpdateAmmoUI();
    }

    void Shoot()
    {
        currentAmmo--;
        UpdateAmmoUI();

        if (muzzleFlash != null)
            muzzleFlash.Play();

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, shootMask))
        {
            Enemy enemy = hit.transform.GetComponentInParent<Enemy>();
            if (enemy != null)
                enemy.TakeDamage((int)damage);

            if (hit.rigidbody != null)
                hit.rigidbody.AddForce(-hit.normal * impactForce);

            if (impactEffect != null)
            {
                GameObject impact = Instantiate(
                    impactEffect,
                    hit.point,
                    Quaternion.LookRotation(hit.normal)
                );
                Destroy(impact, 1f);
            }

            hit.collider.SendMessage("OnShot", SendMessageOptions.DontRequireReceiver);
        }
    }

    // ✅ AMMO PICKUP ENTRY POINT
    public void AddAmmo(int amount)
    {
        ammoReserve = Mathf.Clamp(ammoReserve + amount, 0, maxAmmoReserve);
        UpdateAmmoUI();
    }

    void UpdateAmmoUI()
    {
        if (ammoText == null) return;
        ammoText.text = currentAmmo + " | " + ammoReserve;
    }
}
