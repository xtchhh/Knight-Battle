using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class TeamController : MonoBehaviour
{
    [Header("Combat")]
    public float health = 100f;
    private float damage = 20f;
    private float attackTimer;

    [Header("Movement")]
    private float walkSpeed = 4f;
    private float retreatSpeed = 2f;
    private float distanceToEnemy;
    private Vector3 directionToEnemy;

    [Header("Enemy Collection")]
    private EnemyController[] enemiesArray;
    private List <EnemyController> enemies;
    private EnemyController enemy = null;

    [Header("Agent")]
    private NavMeshAgent enemyAgent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyAgent = gameObject.AddComponent<NavMeshAgent>();
        enemyAgent.autoBraking = false;
        
        enemiesArray = FindObjectsByType<EnemyController>(FindObjectsSortMode.InstanceID);
        enemies = enemiesArray.ToList();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Rotation();
        Attack();
        ClosestEnemy();

        if (health <= 0f)
        {
            Destroy(this.gameObject);
        }

        Debug.Log(enemies.Count);
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

    void Move()
    {
        enemyAgent.stoppingDistance = 3f;

        if (health > 0f)
        {
            enemyAgent.speed = walkSpeed;
            enemyAgent.destination = ClosestEnemy().transform.position;
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
                Debug.Log("Attacking Enemy");

                enemy.health -= damage;

                /*
                if (enemy.health <= 0)
                {
                    Destroy(enemy.gameObject);
                }
                */
                attackTimer = 0f;
            }
        }
    }
}