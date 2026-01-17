using UnityEngine;

public class KnifeHit : MonoBehaviour
{
    public Transform hitPoint;
    public float range = 2f;
    public int damage = 25;
    public LayerMask hitLayers;

    public void DoKnifeHit()
    {
        RaycastHit hit;

        if (Physics.Raycast(hitPoint.position, hitPoint.forward, out hit, range, hitLayers))
        {
            Debug.Log("Hit: " + hit.collider.name);

            // DAMAGE ENEMY
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}

