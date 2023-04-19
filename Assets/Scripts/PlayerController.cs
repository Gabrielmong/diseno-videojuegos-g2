using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movement Components
    CharacterController controller;
    Animator animator;

    private float moveSpeed = 4F;

    [Header("Movement System")]
    public float walkSpeed = 4F;
    public float runSpeed = 8F;

    // Start is called before the first frame update
    void Start()
    {
        // Get movements components
        controller= GetComponent<CharacterController>();
        animator= GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        // Runs is called once per frame
        Move();
    }

    public void Move()
    {
        // Is the sprint key pressed down?
        if (Input.GetButton("Sprint"))
        {
            // Set the animaion to run and increases our movespeed
            moveSpeed = runSpeed;
            animator.SetBool("Run", true);
        }
        else
        {
            // Set the animaion to walk and increases our movespeed
            moveSpeed = walkSpeed;
            animator.SetBool("Run", false);
        }

        // Get the horizontal and vertical inputs as a number
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Direction in a normalized vector
        Vector3 dir = new Vector3(horizontal, 0.0F, vertical).normalized;
        Vector3 velocity = moveSpeed * Time.deltaTime * dir;

        // Check if there is movement
        if (dir.magnitude >= 0.1F)
        {
            // Look towards that direction smoothly
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 0.15F);
            

            // Move
            controller.Move(velocity);
        }

        // Animator speed parameter
        animator.SetFloat("Speed", dir.magnitude);
    }
}
