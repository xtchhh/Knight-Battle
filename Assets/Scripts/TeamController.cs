using System;
using System.Collections.Generic;
using System.Linq;
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

    [Header("Agent")]
    private NavMeshAgent enemyAgent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
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
    }

    private EnemyController ClosestEnemy()
    {
        EnemyController enemy = null;
        float closest = Mathf.Infinity;

        for (int i = 0; i < enemies.Count; i++)
        {
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
        if (health > 40f)
        {
            enemyAgent.speed = walkSpeed;
            enemyAgent.destination = ClosestEnemy().transform.position;
            enemyAgent.stoppingDistance = 3f;
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

        if (attackTimer > 2f && distanceToEnemy < 4f)
        {
            Debug.Log("Attacking Enemy");

            ClosestEnemy().health -= damage;
            if (ClosestEnemy().health <= 0)
            {
                Destroy(ClosestEnemy().gameObject);
            }
            attackTimer = 0f;
        }
    }
}