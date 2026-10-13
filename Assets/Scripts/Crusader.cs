using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AI;

public class Crusader : TeamController
{
    [Header("Combat")]
    private float attackTimer;

    [Header("Movement")]
    private float walkSpeed = 4f;
    private float retreatSpeed = 2f;
    private float distanceToEnemy;
    private Vector3 directionToEnemy;

    [Header("Agent")]
    private NavMeshAgent enemyAgent;

    [Header("Animation")]
    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyAgent = gameObject.AddComponent<NavMeshAgent>();
        enemyAgent.autoBraking = false;
        enemyAgent.updateRotation = false;

        animator = GetComponent<Animator>();

        CacheEnemy();
    }

    private EnemyController ClosestEnemy()
    {
        float closest = Mathf.Infinity;

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null)
            {
                continue;
            }

            distanceToEnemy = Vector3.Distance(this.transform.position, enemies[i].transform.position);

            if (distanceToEnemy < closest)
            {
                closest = distanceToEnemy;
                enemy = enemies[i];
            }
        }
        return enemy;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Attack();
        SelfDestruct();
        Rotation();
        Anims();
    }

    void Move()
    {
        enemyAgent.stoppingDistance = 3f;

        if (health > 0f)
        {
            enemyAgent.speed = walkSpeed;
            enemyAgent.destination = ClosestEnemy().transform.position;//once entire enemy team is deleted from memory, mrc error
                                                                       //will happen because engine is pointing to objects that dont exist
        }
        /*
        else
        {
            enemyAgent.speed = retreatSpeed;
            enemyAgent.destination = test.transform.position;
        }
        */
    }

    void Rotation()
    {
        directionToEnemy = (ClosestEnemy().transform.position - this.transform.position).normalized; // why do i have to subtract from target instead of opposite to create direction?
        this.transform.rotation = Quaternion.LookRotation(directionToEnemy);
    }

    void Attack()
    {
        attackTimer += Time.deltaTime;
        float randomAttackTime = Random.Range(1, 4);
        float updatedDistanceToEnemy = Vector3.Distance(this.transform.position, ClosestEnemy().transform.position);

        if (updatedDistanceToEnemy <= 4f)
        {
            if (attackTimer > randomAttackTime)
            {
                animator.SetTrigger("isAttacking");
                attackTimer = 0f;
            }
        }
    }

    void Anims()
    {
        if (enemyAgent.velocity != Vector3.zero)
        {
            animator.Play("Run");
        }
        else if (enemyAgent.velocity == Vector3.zero)
        {
            animator.Play("Idle");
        }
    }
}
