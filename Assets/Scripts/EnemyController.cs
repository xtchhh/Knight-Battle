using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Combat")]
    public float health = 100f;
    protected float damage = 20f;

    [Header("Movement")]
    protected float walkSpeed = 4f;
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

    protected void SelfDestruct()
    {
        if (health <= 0f)
        {
            Destroy(this.gameObject);
        }
    }
}