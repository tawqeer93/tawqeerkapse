using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 6f; 
    public float rotationSpeed = 450f; 
    public MainCamera mainCamera; 

    [Header("Animation Settings")]
    public Animator animator; 

    [Header("Collision & Gravity Settings")]
    public CharacterController characterController; 
    public float gravity = -9.81f; 
    public float groundCheckRadius = 0.1f; 
    public Vector3 groundCheckOffset; 
    public LayerMask groundMask; 
    bool isGrounded; 
    float verticalSpeed; 

    private void Update()
    {
        HandleGravity(); 
        HandleMovement(); 
        CheckGround(); 
        Debug.Log("Is Player Grounded: " + isGrounded);
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal"); 
        float vertical = Input.GetAxis("Vertical"); 

        Vector3 moveDirection = (new Vector3(horizontal, 0, vertical)).normalized; 

        moveDirection = mainCamera.flatRotation * moveDirection; 

        characterController.Move(moveDirection * speed * Time.deltaTime); 

        if (moveDirection.magnitude > 0) 
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection); 
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime); 
        }

        animator.SetFloat("Speed", moveDirection.magnitude);
        /*animator.SetFloat("movementValue", movementAmount, 0.2f, Time.deltaTime);*/
    }

    void HandleGravity()
    {
        if (isGrounded) 
        {
            verticalSpeed = 0.5f; 
        }
        else 
        {
            verticalSpeed += gravity * Time.deltaTime; 
        }

        Vector3 gravityVector = Vector3.up * verticalSpeed; 
        characterController.Move(gravityVector * Time.deltaTime); 
    }

    void CheckGround()
    {
        isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundMask); 
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius); 
    }
}
