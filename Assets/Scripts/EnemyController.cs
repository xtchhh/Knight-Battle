using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : EntityController
{
    [Header("Combat")]
    protected float damage = 20f;

    [Header("Movement")]
    protected float walkSpeed = 5f;
    protected float retreatSpeed = 2f;

    [Header("Enemy Collection")]
    protected TeamController[] enemiesArray;
    protected List <TeamController> enemies;
    protected TeamController enemy = null;

    protected void CacheEnemy()
    {
        enemiesArray = FindObjectsByType<TeamController>(FindObjectsSortMode.InstanceID);
        enemies = enemiesArray.ToList();
    }
}