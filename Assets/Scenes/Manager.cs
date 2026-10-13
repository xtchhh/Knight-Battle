using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class Manager : MonoBehaviour
{
    private EnemyController[] enemyArray;
    private TeamController[] teamArray;
    public List<EnemyController> enemyList;
    public List<TeamController> teamList;
    private Player player;
    private TextMeshProUGUI healthText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyArray = FindObjectsByType<EnemyController>(FindObjectsSortMode.InstanceID);
        enemyList = enemyArray.ToList();

        teamArray = FindObjectsByType<TeamController>(FindObjectsSortMode.InstanceID);
        teamList = teamArray.ToList();

        healthText = FindFirstObjectByType<TextMeshProUGUI>();

        player = FindFirstObjectByType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerCount();
        EndGame();
        healthText.text = $"Your health is {player.health}";
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
        if (teamList.Count == 0 || enemyList.Count == 0 || player == null)
        {
            StartCoroutine("SwitchScene");
        }
    }

    IEnumerator SwitchScene()
    {
        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene("Menu");
    }
}
