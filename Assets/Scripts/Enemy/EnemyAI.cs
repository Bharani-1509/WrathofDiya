using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public EnemyStats stats;

    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private float nextAttackTime;
    private bool playerSpotted;
    private Transform root;
    private float startY;

    void Start()
    {
        // NavMeshAgent is on the same GameObject (Enemy)
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent missing on Enemy!");
            enabled = false;
            return;
        }

        // Animator is on child model
        animator = GetComponentInChildren<Animator>();
        if (animator == null)
            Debug.LogError("Animator missing on EnemyModel child!");

        root = transform; // root is the object with NavMeshAgent
        startY = root.position.y;

        // Find player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogError("Player not found in scene with tag 'Player'");

        // Apply stats
        agent.stoppingDistance = stats.attackRange;
    }

    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Hit"))
        {
            agent.isStopped = true;
            return;
        }
        if (!agent.enabled || player == null) return;
        agent.updateRotation = false;

        float distance = Vector3.Distance(root.position, player.position);

        if (distance <= stats.sightRange)
            playerSpotted = true;

        if (!playerSpotted)
        {
            Idle();
            LockRootY();
            return;
        }

        if (distance > stats.attackRange)
            ChasePlayer();
        else
            AttackPlayer();

        LockRootY();
    }

    void Idle()
    {
        agent.isStopped = true;
        animator.SetBool("IsChasing", false);
        animator.SetBool("IsAttacking", false);
        animator.SetFloat("Speed", 0f);
    }
    void ChasePlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);

        RotateTowardsPlayer(); // 🔥 ADD THIS

        animator.SetBool("IsChasing", true);
        animator.SetBool("IsAttacking", false);
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    void AttackPlayer()
    {
        agent.isStopped = true;
        RotateTowardsPlayer();

        animator.SetBool("IsChasing", false);
        animator.SetBool("IsAttacking", true);
        animator.SetFloat("Speed", 0f);

        if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") &&
            Time.time >= nextAttackTime)
        {
            animator.SetTrigger("Attack");
            nextAttackTime = Time.time + stats.attackCooldown;
        }
    }

    void LockRootY()
    {
        Vector3 pos = root.position;
        pos.y = startY; // keep enemy from moving up/down
        root.position = pos;
    }

    void RotateTowardsPlayer()
    {
        Vector3 dir = agent.velocity;
        dir.y = 0;

        if (dir.sqrMagnitude > 0.01f)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            root.rotation = Quaternion.Slerp(root.rotation, rot, Time.deltaTime * 12f);
        }
    }
    public void EndHit()
    {
        animator.SetBool("IsChasing", false);
        animator.SetBool("IsAttacking", false);
    }
    // Animation Event
    public void DealDamage()
    {
        if (player == null) return;

        float distance = Vector3.Distance(root.position, player.position);
        if (distance <= stats.attackRange)
        {
            PlayerHealth health = player.GetComponent<PlayerHealth>();
            if (health != null)
                health.TakeDamage(stats.attackDamage);
        }
    }
}
