using System.IO;
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

    [Header("Checks")]
    private bool moveRequested;
    private bool jumpRequested;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Input();
        JumpCheck();
        MoveCheck();
        PlayerRotation();
    }
    void FixedUpdate()
    {
        Movement();
        Jump();
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
        forwardDirection = forwardDirection.normalized;
        forwardDirection.y = 0;

        Vector3 rightDirection = cam.transform.right * playerInput.x;
        rightDirection = rightDirection.normalized;
        rightDirection.y = 0;

        cameraRelativeDirection = rightDirection + forwardDirection;
    }

    void Movement()
    {
        if (moveRequested == true)
        {
            moveSpeed = walkSpeed;
            rb.linearVelocity = cameraRelativeDirection * moveSpeed;
        }
    }

    void Jump()
    {
        if (jumpRequested == true && IsGrounded())
        {
            rb.AddForce(Vector3.up * 5.0f, ForceMode.Impulse);
        }
    }

    void PlayerRotation()
    {
        if (input.sqrMagnitude > 0.1)
        {
            rb.rotation = Quaternion.LookRotation(cameraRelativeDirection);
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

    void JumpCheck()
    {
        keyboardInput = Keyboard.current;

        if (keyboardInput.spaceKey.wasPressedThisFrame)
        {
            jumpRequested = true;
        }
        else
        {
            jumpRequested = false; ;
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
}
