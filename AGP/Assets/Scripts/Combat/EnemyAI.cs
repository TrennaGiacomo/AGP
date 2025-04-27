using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 20f;
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private float maxRange = 20f;
    [SerializeField] private int damageToPlayer = 10;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 1.5f;

    private bool active = false;
    private NavMeshAgent agent;
    private bool returningToSpawn = false;
    private Transform player;
    private Animator NPCAnimator;
    private Vector3 spawnPosition;
    private float attackTimer = 0f;

    //Optimization stuff
    private float sleepDistance = 17f;
    private float wakeDistance = 13f;
    private bool isSleeping = false;
    private Collider col;

    private void Start()
    {
        EnemyManager.Instance.RegisterEnemy(this);
        NPCAnimator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        col = GetComponent<Collider>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        spawnPosition = transform.position;

        if (agent != null)
        {
            agent.speed = moveSpeed;
            agent.stoppingDistance = attackRange;
        }
    }

    private void OnDestroy()
    {
        if(EnemyManager.Instance != null) EnemyManager.Instance.UnregisterEnemy(this);
    }

    public void CheckSleepState(Vector3 playerPos)
    {
        var distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if(!isSleeping && distanceToPlayer > sleepDistance) Sleep();
        else if (isSleeping && distanceToPlayer < wakeDistance) WakeUp();
    }

    private void Update()
    {      
        if(!isSleeping) RunAI();        
    }

    private void Sleep()
    {
        if(returningToSpawn) return;

        isSleeping = true;
        NPCAnimator.enabled = false;
        agent.enabled = false;
        col.enabled = false;
    }

    private void WakeUp()
    {
        isSleeping = false;
        NPCAnimator.enabled = true;
        agent.enabled = true;
        col.enabled = true;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRadius)
        {
            Activate();
        }
    }

    private void RunAI()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (!active && !returningToSpawn && distanceToPlayer <= detectionRadius)
            Activate();
        else if (active && distanceToPlayer >= maxRange)
            Deactivate();

        if (active)
        {
            if (distanceToPlayer > attackRange)
                MoveTowardsPlayer();
            else
                AttackPlayer();
        }
        else if (returningToSpawn)
            ReturnToSpawn();

        if (attackTimer > 0f)
            attackTimer -= Time.deltaTime;
    }

    public void Activate()
    {
        active = true;
        returningToSpawn = false;
        NPCAnimator?.SetBool("isActive", active);
    }

    private void Deactivate()
    {
        active = false;
        returningToSpawn = true;
    }

    private void MoveTowardsPlayer()
    {
        if (agent.isActiveAndEnabled)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
    }

    private void AttackPlayer()
    {
        if (attackTimer <= 0f)
        {
            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageToPlayer);
            }

            attackTimer = attackCooldown;
        }

        if (agent.isActiveAndEnabled)
            agent.isStopped = true;

        Vector3 lookDirection = (player.position - transform.position).normalized;
        lookDirection.y = 0f;
        if (lookDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void ReturnToSpawn()
    {
        if (agent.isActiveAndEnabled)
        {
            agent.isStopped = false;
            agent.SetDestination(spawnPosition);

            if (Vector3.Distance(transform.position, spawnPosition) < 1.5f)
            {
                returningToSpawn = false;
                active = false;
                NPCAnimator?.SetBool("isActive", active);
                agent.ResetPath();
            }
        }
    }
}
