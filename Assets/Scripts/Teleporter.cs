using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour
{
    public Teleporter targetTeleporter;
    public float cooldown = 0.5f;

    private bool canTeleport = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!canTeleport) return;

        if (other.CompareTag("Player"))
        {
            if (targetTeleporter == null)
            {
                Debug.LogError("Target Teleporter NOT assigned on " + gameObject.name);
                return;
            }

            StartCoroutine(Teleport(other));
        }
    }

    IEnumerator Teleport(Collider player)
    {
        canTeleport = false;
        targetTeleporter.canTeleport = false;

        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        player.transform.position = targetTeleporter.transform.position;

        yield return null;

        if (cc != null) cc.enabled = true;

        yield return new WaitForSeconds(cooldown);

        canTeleport = true;
        targetTeleporter.canTeleport = true;
    }
}
