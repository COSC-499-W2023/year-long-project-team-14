using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float attackCharge;
    [SerializeField] private float attackChargeSpeed;
    [SerializeField] private float attackChargeMax;
    [SerializeField] private float attackCost;
    [SerializeField] private float bulletForce;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int bulletBounces;

    public GameObject bulletPrefab;

    public PlayerInput playerInput;
    public Rigidbody2D rb;
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
        if(attackCharge < attackChargeMax)
            attackCharge += Time.deltaTime * attackChargeSpeed;

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

    public void Shoot(InputAction.CallbackContext context)
    {
        if(context.performed && attackCharge >= attackCost)
        {
            attackCharge -= attackCost;

            GameObject bullet = Instantiate(bulletPrefab, gunFollow.position + (Vector3)(-gunFollow.up), new Quaternion(0, 0, 180, transform.rotation.w));
            Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
            bulletRB.AddForce(-gunFollow.up * bulletForce);

            PlayerBullet playerBullet = bullet.GetComponent<PlayerBullet>();
            playerBullet.bounces = bulletBounces;

            Destroy(bullet, 30);
        }
    }
}
