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
    protected float damage = 20f;
   
    [Header("Enemy Collection")]
    protected EnemyController[] enemiesArray;
    protected List <EnemyController> enemies;
    protected EnemyController enemy = null;

    protected void CacheEnemy()
    {
        enemiesArray = FindObjectsByType<EnemyController>(FindObjectsSortMode.InstanceID);
        enemies = enemiesArray.ToList();
    }

    protected void SelfDestruct()
    {
        if (health <= 0f)
        {
            Destroy(this.gameObject);
        }
    }
}