using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    [SerializeField]
    float walkFactor = 5.0F;

    [SerializeField]
    float runFactor = 8.5F;

    [SerializeField]
    float rotationFactor = 20.0F;

    [Header("Jump")]
    [SerializeField]
    float jumpForce = 8.0F;

    [SerializeField]
    int maximumNumberOfJumps = 2;

    [SerializeField]
    float gravityMultiplier = 3.0F;

    CharacterController character;

    Vector3 direction;

    float inputX;
    float inputZ;
    float magnitude;
    float gravityY;
    float velocityY;

    bool isRunning;
    bool isMovePressed;
    bool isJumpPressed;

    int numberOfJumps;

    void Awake()
    {
        character = GetComponent<CharacterController>();

        gravityY = Physics.gravity.y;
    }

    void Update()
    {
        HandleInputs();
        HandleGravity();
        HandleMove();
        HandleRotation();
    }

    void HandleInputs()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputZ = Input.GetAxisRaw("Vertical");

        isMovePressed = inputX != 0.0F || inputZ != 0.0F;
        isRunning = Input.GetButton("Fire3");
        isJumpPressed = Input.GetButtonDown("Jump");
    }

    void HandleGravity()
    {
        bool isGrounded = IsGrounded();
        if (isGrounded)
        {
            if (velocityY < 1.0F)
            {
                velocityY = -1.0F;
            }

            if (isJumpPressed)
            {
                HandleJump();
                StartCoroutine(WaitForGroundedCoroutine());
            }
        }
        else
        {
            if (isJumpPressed && maximumNumberOfJumps > 1)
            {
                HandleJump();
            }
            velocityY += gravityY * gravityMultiplier * Time.deltaTime;
        }
    }

    void HandleJump()
    {
        if (numberOfJumps > maximumNumberOfJumps)
        {
            return;
        }

        numberOfJumps++;
        velocityY = jumpForce / numberOfJumps;
    }

    void HandleMove()
    {
        direction = new Vector3(inputX, 0.0F, inputZ);
        magnitude = Mathf.Clamp01(direction.magnitude);
        direction.Normalize();

        Vector3 velocity = 
            direction * magnitude *
                (isRunning
                    ? runFactor
                    : walkFactor);

        velocity.y = velocityY;
        character.Move(velocity * Time.deltaTime);
    }

    void HandleRotation()
    {
        if (isMovePressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = 
                Quaternion.Lerp
                    (transform.rotation, targetRotation, rotationFactor * Time.deltaTime);
        }
    }
    IEnumerator WaitForGroundedCoroutine()
    {
        yield return new WaitUntil(() => !IsGrounded());
        yield return new WaitUntil(() => IsGrounded());
        numberOfJumps = 0;
    }

    bool IsGrounded()
    {
        return character.isGrounded;
    }
}
