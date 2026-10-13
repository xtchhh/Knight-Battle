using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class Skeleton : EnemyController
{
    [Header("Combat")]
    private float attackTimer;

    [Header("Movement")]
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

    // Update is called once per frame

    private TeamController ClosestEnemy()
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

    void Update()
    {
        Move();
        Rotation();
        Attack();
        SelfDestruct();
        Anims();
    }

    void Move()
    {
        enemyAgent.stoppingDistance = 3f;

        if (health > 0f)
        {
            enemyAgent.speed = walkSpeed;
            enemyAgent.destination = ClosestEnemy().transform.position; //once entire enemy team is deleted from memory, mrc error
                                                                        //will happen because engine is pointing to objects that dont exist
        }
 
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

                //enemy.health -= damage;
                attackTimer = 0f;
            }
        }
    }

    void Anims()
    {
        if (enemyAgent.velocity != Vector3.zero && !IsDead())
        {
            animator.Play("Skeleton_Rig|Run");
        }
        else if (enemyAgent.velocity == Vector3.zero && !IsDead())
        {
            animator.Play("Skeleton_Rig|Idle");
        }

        /*if (IsDead())
        {
            animator.Play("Death");
        }*/
    }

    void SelfDestruct()
    {
        if (IsDead())
        {
            Destroy(this.gameObject);
        }
    }
}
