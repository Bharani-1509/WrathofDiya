using Unity.Mathematics;
using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float impactForce = 30f;
    public float fireRate = 15f;



    public int bulletsUsed = 0;
    public int maxAmmo = 10;
    public int ammoReserve = 30;
    public int maxAmmoReserve = 90;
    private int currentAmmo;
    public float reloadTime = 1f;
    private bool isReloading = false;



    LayerMask shootMask;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    private float nextTimeToFire = 0f;


    public Animator animator;


    public Camera fpsCam;


    void Start()
    {
        currentAmmo = maxAmmo;
        shootMask = ~LayerMask.GetMask("Player", "Weapon");
    }

    void OnEnable()
    {
        isReloading = false;
        animator.SetBool("Reloading", false);
    }


    void Update()
    {
        if (isReloading)
            return;
        if (currentAmmo <= 0 && ammoReserve > 0)
        {
            StartCoroutine(Reload());
            return;
        }
        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire && currentAmmo > 0)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    IEnumerator Reload()
    {
        if (ammoReserve <= 0)
            yield break;

        isReloading = true;
        animator.SetBool("Reloading", true);

        yield return new WaitForSeconds(reloadTime);

        int bulletsNeeded = maxAmmo - currentAmmo;
        int bulletsToLoad = Mathf.Min(bulletsNeeded, ammoReserve);

        currentAmmo += bulletsToLoad;
        ammoReserve -= bulletsToLoad;

        animator.SetBool("Reloading", false);
        isReloading = false;
    }


    void Shoot()
    {
        
    if (currentAmmo <= 0)
        return;
        muzzleFlash.Play();


        currentAmmo--;
        bulletsUsed++;
        RaycastHit hit;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, shootMask))
        {
            Debug.Log(hit.transform.name);
            Debug.Log("Bullets used: " + bulletsUsed);
            Enemy target = hit.transform.GetComponent<Enemy>();

            if (target != null)
            {
                target.TakeDamage(damage);
            }
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            GameObject ImpactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(ImpactGO, 1f);
        }
    }
}
