using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInput playerInput;
    public Rigidbody2D rb;
    public float moveSpeed = 10f;
    public Animator animator;

    [SerializeField] Transform gunFollow;

    Vector2 moveDirection = Vector2.zero;
    Vector2 aimDirection = Vector2.zero;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(moveDirection.x != 0 || moveDirection.y != 0){
            animator.SetFloat("X", moveDirection.x);
            animator.SetFloat("Y", moveDirection.y);

            animator.SetBool("IsWalking", true);
        } else {
            animator.SetBool("IsWalking", false);
        }


        if (playerInput.currentControlScheme == "Keyboard&Mouse")
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Quaternion rotation = Quaternion.LookRotation(mousePos - gunFollow.transform.position, gunFollow.transform.TransformDirection(Vector3.forward));
            gunFollow.transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
        }
        else
        {
            Quaternion rotation = Quaternion.LookRotation(aimDirection, gunFollow.transform.TransformDirection(Vector3.forward));
            gunFollow.transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
        }

    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>().normalized;
    }

    public void Aim(InputAction.CallbackContext context)
    {
        aimDirection = context.ReadValue<Vector2>();
    }
}
