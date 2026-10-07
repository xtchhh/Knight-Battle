using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Components")]
    private Rigidbody rb;

    [Header("Input")]
    private Camera cam;
    private Vector3 input;
    private Keyboard keyboardInput;

    [Header("Movement")]
    public float moveSpeed;
    private float walkSpeed = 5f;
    private float runSpeed = 10f;
    private Vector3 cameraRelativeDirection;
    private Vector3 velocity;
    private Quaternion lookRot = Quaternion.identity;
    private Quaternion combatRot;
    private bool canGiveInput;

    [Header("Checks")]
    private bool moveRequested;
    private bool jumpRequested;

    [Header("Combat")]
    private float distanceToEnemy;
    private float damage = 20f;
    private bool isInCombat = false;

    [Header("Enemy")]
    private EnemyController[] enemiesArray;
    private List<EnemyController> enemies;
    private EnemyController enemy = null;

    [Header("Health")]
    private float health = 100f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();
        enemiesArray = FindObjectsByType<EnemyController>(FindObjectsSortMode.InstanceID);
        enemies = enemiesArray.ToList();
        canGiveInput = true;
    }

    // Update is called once per frame
    void Update()
    {
        Input();
        JumpCheck();
        MoveCheck();
        PlayerRotation();
        Attack();
    }
    void FixedUpdate()
    {
        Movement();
        //Jump();
        jumpRequested = false; //check full every physics tick, every 0.02 seconds

        return;
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

            float statcDistanceToEnemy = Vector3.Distance(this.transform.position, enemies[i].transform.position);

            if (distanceToEnemy < closest)
            {
                closest = distanceToEnemy;
                enemy = enemies[i];
            }
        }
        return enemy;
    }

    void Input()
    {
        keyboardInput = Keyboard.current;
        input = Vector3.zero;

        if (keyboardInput.wKey.isPressed)
        {
            input += Vector3.forward;
        }

        if (keyboardInput.sKey.isPressed)
        {
            input += Vector3.back;
        }

        if (keyboardInput.aKey.isPressed)
        {
            input += Vector3.left;
        }

        if (keyboardInput.dKey.isPressed)
        {
            input += Vector3.right;
        }

        if (keyboardInput.spaceKey.wasPressedThisFrame)
        {
            input += Vector3.up;
        }

        MovementDirection(ref input);
    }

    void MovementDirection(ref Vector3 playerInput)
    {
        Vector3 forwardDirection = cam.transform.forward * playerInput.z;
        //forwardDirection = forwardDirection.normalized;
        forwardDirection.y = 0;

        Vector3 rightDirection = cam.transform.right * playerInput.x;
        //rightDirection = rightDirection.normalized;
        rightDirection.y = 0;

        cameraRelativeDirection = (rightDirection + forwardDirection).normalized;
    }

    void Movement()
    {
        velocity = new Vector3(0, rb.linearVelocity.y, 0);
        //Debug.Log(rb.linearVelocity);

        if (moveRequested == true)
        {
            moveSpeed = walkSpeed;

            rb.linearVelocity = (cameraRelativeDirection * moveSpeed) + velocity;// cameraRelativeDirection * moveSpeed;
        }
    }

    void Jump()
    {
        if (jumpRequested == true && IsGrounded() && rb.linearVelocity.y == 0)
        {
            rb.AddForce(Vector3.up * 5.0f);
        }
    }

    void Attack()
    {
        distanceToEnemy = Vector3.Distance(this.transform.position, ClosestEnemy().transform.position);

        if (distanceToEnemy < 10f && Mouse.current.leftButton.wasPressedThisFrame)
        {
            isInCombat = true;
            canGiveInput = false;
        }

        if( Mouse.current.leftButton.wasPressedThisFrame && isInCombat)
        {
            canGiveInput = true;
            isInCombat = false;
        }
    }

    void PlayerRotation()
    {
        Vector3 directionToEnemy = (ClosestEnemy().transform.position - this.transform.position).normalized;
        Quaternion standardRot = Quaternion.LookRotation(cameraRelativeDirection);
        Quaternion combatRot = Quaternion.LookRotation(directionToEnemy);

        if (input.sqrMagnitude > 0.1 && canGiveInput)
        {
            rb.rotation = standardRot;
        }

        else if (isInCombat)
        {
            rb.rotation = combatRot;
        }
    }

    void JumpCheck()
    {
        keyboardInput = Keyboard.current;

        if (keyboardInput.spaceKey.wasPressedThisFrame)
        {
            jumpRequested = true;
        }
    }

    void MoveCheck()
    {
        if (input.sqrMagnitude > 0.1)
        {
            moveRequested = true;
        }
        else
        {
            moveRequested = false;
        }
    }

    bool IsGrounded()
    {
        if (Physics.Raycast(this.transform.position + (Vector3.up * 0.3f), Vector3.down, 0.4f))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
