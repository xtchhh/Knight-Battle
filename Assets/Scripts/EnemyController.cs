using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private GameObject player;
    public float moveSpeed;
    private float walkSpeed = 5.0f;
    private float runSpeed = 5.0f;
    private NavMeshAgent enemy;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");
        enemy = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveToPlayer();
    }

    void MoveToPlayer()
    {
        enemy.SetDestination(player.transform.position);
    }
}
