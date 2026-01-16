using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float sightRange = 8f;
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;

    NavMeshAgent agent;
    Animator animator;
    float nextAttackTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }
    }

    void Update()
    {
        if (player == null || agent == null || animator == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= sightRange && distance > attackRange)
        {
            // CHASE
            agent.isStopped = false;
            agent.SetDestination(player.position);

            animator.SetBool("IsChasing", true);
            animator.SetBool("IsAttacking", false);
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }
        else if (distance <= attackRange)
        {
            // ATTACK
            Attack();
        }
        else
        {
            // IDLE
            agent.isStopped = true;
            animator.SetBool("IsChasing", false);
            animator.SetBool("IsAttacking", false);
            animator.SetFloat("Speed", 0f);
        }
    }

    void Attack()
    {
        agent.isStopped = true;
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f; // prevent vertical rotation

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }

        if (Time.time >= nextAttackTime)
        {
            animator.SetBool("IsAttacking", true);
            nextAttackTime = Time.time + attackCooldown;
        }
    }
}
