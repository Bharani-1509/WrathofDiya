using UnityEngine;

public class SlidingDoorTrigger : MonoBehaviour
{
    [Header("Door Reference")]
    public Transform door;   // Drag your door asset here

    [Header("Movement Settings")]
    public float slideHeight = 3f;
    public float slideSpeed = 2f;

    private Vector3 closedPos;
    private Vector3 openPos;
    private bool playerInside;

    void Start()
    {
        if (door == null)
        {
            Debug.LogError("Door not assigned in SlidingDoorTrigger");
            enabled = false;
            return;
        }

        closedPos = door.position;
        openPos = closedPos + Vector3.up * slideHeight;
    }

    void Update()
    {
        Vector3 target = playerInside ? openPos : closedPos;

        door.position = Vector3.MoveTowards(
            door.position,
            target,
            slideSpeed * Time.deltaTime
        );
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }
}
