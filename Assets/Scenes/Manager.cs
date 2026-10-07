using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;

public class Manager : MonoBehaviour
{
    private EnemyController[] enemyArray;
    private TeamController[] teamArray;
    List<EnemyController> enemyList;
    List<TeamController> teamList;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyArray = FindObjectsByType<EnemyController>(FindObjectsSortMode.InstanceID);
        enemyList = enemyArray.ToList();

        teamArray = FindObjectsByType<TeamController>(FindObjectsSortMode.InstanceID);
        teamList = teamArray.ToList();
    }

    // Update is called once per frame
    void Update()
    {
        //CheckPlayerCount();
    }

    void CheckPlayerCount()
    {
        //teamList.RemoveAll(null);
        for (int i = 0; i < teamList.Count; i++)
        {
            if (teamList[i] == null)
            {
                
                teamList.Remove(teamList[i]);
            }
        }

        for (int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i] == null)
            {
                enemyList.Remove(enemyList[i]);
            }
        }
    }
}
