using UnityEngine;

public class DoorMovement : MonoBehaviour
{
    public Animator animator;
    private BoxCollider boxCollider;
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        animator = GetComponent<Animator>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("isOpen", true);
            other.GetComponent<BoxCollider>().enabled = false;
        }
    }
        void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("isClose", true);
            other.GetComponent<BoxCollider>().enabled = true;
        }
    }
}
