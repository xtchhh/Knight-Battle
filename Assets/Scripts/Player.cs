using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : TeamController
{
    [Header("Input")]
    private Camera cam;
    private Keyboard keyboardInput;
    private Vector3 input;
    private bool canGiveInput;

    [Header("Movement")]
    public float moveSpeed;
    private float walkSpeed = 8f;
    private float runSpeed = 10f;
    private Vector3 cameraRelativeDirection;
    private Vector3 velocity;
    private Rigidbody rb;

    [Header("Checks")]
    private bool moveRequested;
    private bool jumpRequested;
    private bool runRequested;

    [Header("Combat")]
    private float distanceToEnemy;
    private bool isLocked;

    [Header("Animations")]
    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();

        animator = GetComponent<Animator>();

        keyboardInput = Keyboard.current;

        Cursor.lockState = CursorLockMode.Locked;

        canGiveInput = true;
        isLocked = false;

        CacheEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        Input();
        //JumpCheck();
        MoveCheck();
        PlayerRotation();
        Attack();
        SelfDestruct();
        Anims();
    }
    void FixedUpdate()
    {
        Movement();
        //Jump();
        //jumpRequested = false; //check full every physics tick, every 0.02 seconds

        return;
    }

    private EnemyController ClosestEnemy()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null) //continue loop despite missingReferencException
            {
                continue;
            }

            Vector3 staticDirectionToEnemy = (enemies[i].transform.position - this.transform.position).normalized;

            float dot = Vector3.Dot(cam.transform.forward, staticDirectionToEnemy);

            if (dot > 0.5f)
            {
                enemy = enemies[i];
            }
        }
        return enemy;
    }

    void Input()
    {
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

        if (moveRequested && IsGrounded())
        {
            moveSpeed = walkSpeed;

            rb.linearVelocity = (cameraRelativeDirection * moveSpeed) + velocity;// cameraRelativeDirection * moveSpeed;
        }
    }

    void Jump()
    {
        if (jumpRequested && IsGrounded())
        {
            rb.AddForce(Vector3.up * 5.0f);
        }
    }

    void Attack()
    {
        if (keyboardInput.gKey.wasPressedThisFrame && !isLocked)
        {
            isLocked = true;
            canGiveInput = false;
        }
        
        else if (keyboardInput.gKey.wasPressedThisFrame && isLocked)
        {
            isLocked = false;
            canGiveInput = true;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            animator.SetTrigger("isAttacking");
        }
    }

    void PlayerRotation()
    {
        if (input.sqrMagnitude > 0.1f && !isLocked && canGiveInput)
        {
            rb.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(cameraRelativeDirection), 20f * Time.deltaTime);
        }

        try
        {
            Vector3 directionToEnemy = (ClosestEnemy().transform.position - this.transform.position).normalized; // variable which didn't include anything you were using the other distance assigning closest enemy

            if (isLocked && !canGiveInput)
            {
                rb.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(directionToEnemy), 20f * Time.deltaTime);
            }
        }
        catch (NullReferenceException ex)
        {
            Debug.Log($"You are getting this error: {ex}");
        }          
    }

    void JumpCheck()
    {
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
        if (Physics.Raycast(this.transform.position + (Vector3.up * 0.3f), Vector3.down, 0.5f))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Anims()
    {
        if (input.sqrMagnitude > 0.1f)
        {
            animator.Play("Run");
        }
        else
        {
            animator.Play("Idle");
        }
    }

    void SelfDestruct()
    {
        if (IsDead())
        {
            StartCoroutine("StartDeath");
        }
    }

    IEnumerator StartDeath()
    {
        yield return new WaitForSeconds(1f);

        Destroy(this.gameObject);
    }
}
