using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float sightRange = 8f;
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    public int attackDamage = 10;

    private NavMeshAgent agent;
    private Animator animator;
    private float nextAttackTime;
    private bool playerSpotted = false;

    void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (!agent.enabled || player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // SEE PLAYER
        if (distance <= sightRange)
            playerSpotted = true;

        if (playerSpotted)
        {
            if (distance > attackRange)
            {
                ChasePlayer();
            }
            else
            {
                AttackPlayer();
            }
        }
        else
        {
            Idle();
        }
    }

    void ChasePlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);

        animator.SetBool("IsChasing", true);
        animator.SetBool("IsAttacking", false);
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    void AttackPlayer()
    {
        agent.isStopped = true;

        // Rotate towards player
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }

        if (Time.time >= nextAttackTime)
        {
            animator.SetBool("IsAttacking", true);
            nextAttackTime = Time.time + attackCooldown;
            Invoke(nameof(ResetAttackBool), 0.1f);
        }
    }

    void ResetAttackBool()
    {
        animator.SetBool("IsAttacking", false);
    }

    void Idle()
    {
        agent.isStopped = true;
        animator.SetBool("IsChasing", false);
        animator.SetBool("IsAttacking", false);
        animator.SetFloat("Speed", 0f);
    }

    public void DealDamage()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= attackRange)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }
            Debug.Log("Enemy hit player!");
        }
    }
}
