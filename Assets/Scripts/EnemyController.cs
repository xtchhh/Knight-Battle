using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private GameObject player;
    public float moveSpeed;
    public float health = 5f;
    private float damage;
    private float walkSpeed = 5.0f;
    private float runSpeed = 5.0f;
    private float distanceToEnemy;
    private float attackTimer;
    private NavMeshAgent enemyAgent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");
        enemyAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Rotation();
        Attack();
    }

    void Move()
    {
        distanceToEnemy = Vector3.Distance(this.transform.position, player.transform.position);

        //Debug.Log(distanceToEnemy);
        if (health > 3)
        {
            enemyAgent.SetDestination(player.transform.position);

            if (distanceToEnemy < 3f)
            {
                enemyAgent.stoppingDistance = 3f;
            }
            else
            {
                enemyAgent.stoppingDistance = 0f;
            }
        }
        /*
        else
        {
            enemyAgent.SetDestination(-this.transform.position);
        }
        */
    }

    void Rotation()
    {
        Vector3 directionToEnemy = (player.transform.position - this.transform.position).normalized; // why do i have to subtract from target instead of opposite to create direction?
        this.transform.rotation = Quaternion.LookRotation(directionToEnemy);
    }

    void Attack()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer > 2f && distanceToEnemy < 3f)
        {
            Debug.Log("Attacking Enemy");

            /* playerHealth -= damage; 
            if (playerHealth <= 0)
            {
                enemyDeathRequested = true;
            }
            */
            attackTimer = 0f;
        }
    }

}
