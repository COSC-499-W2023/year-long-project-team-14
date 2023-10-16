using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed = 10f;
    public InputAction playerMovements;
    public Animator animator;

    Vector2 moveDirection = Vector2.zero;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        playerMovements.Enable();
    }

    private void OnDisable()
    {
        playerMovements.Disable();
    }

    // Update is called once per frame
    void Update()
    {

        moveDirection = playerMovements.ReadValue<Vector2>().normalized;

        if(moveDirection.x != 0 || moveDirection.y != 0){
            animator.SetFloat("X", moveDirection.x);
            animator.SetFloat("Y", moveDirection.y);

            animator.SetBool("IsWalking", true);
        } else {
            animator.SetBool("IsWalking", false);
        }
        
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }
}
