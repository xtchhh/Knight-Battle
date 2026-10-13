using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;

public class Manager : MonoBehaviour
{
    private EnemyController[] enemyArray;
    private TeamController[] teamArray;
    public List<EnemyController> enemyList;
    public List<TeamController> teamList;

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
        CheckPlayerCount();
        EndGame();
    }

    void CheckPlayerCount()
    {
        for (int i = teamList.Count - 1; i >= 0; i--)
        {
            if (teamList[i] == null)
            {
                teamList.RemoveAt(i);
            }
        }

        for (int i = enemyList.Count - 1; i >= 0; i--)
        {
            if (enemyList[i] == null)
            {
                enemyList.RemoveAt(i);
            }
        }
    }

    void EndGame()
    {
        if (teamList.Count == 0 || enemyList.Count == 0)
        {
            Debug.Log("GAME OVER");
        }
    }
}
