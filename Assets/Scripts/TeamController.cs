using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TeamController : EntityController
{
    [Header("Combat")]
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
}