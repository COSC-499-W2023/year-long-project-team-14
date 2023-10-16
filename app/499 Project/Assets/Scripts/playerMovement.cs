using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed = 10f;
    public InputAction playerMovements;

    Vector2 moveDirection = Vector2.zero;

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
        
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }
}
