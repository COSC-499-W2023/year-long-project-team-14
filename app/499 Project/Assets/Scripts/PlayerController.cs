using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerController : MonoBehaviour
{
    public float attackCharge;
    public float attackChargeSpeed;
    public float attackChargeMax;
    public float attackCost;
    public float bulletForce;
    public float moveSpeed;
    public int bulletBounces;

    public GameObject bulletPrefab;

    PlayerInput playerInput;
    Rigidbody2D rb;
    Animator animator;

    [SerializeField] Transform gunFollow;
    [SerializeField] Transform playerCenter;

    Vector2 moveDirection = Vector2.zero;
    Vector2 aimDirection = Vector2.zero;

    [SerializeField] private LineRenderer lineRenderer;
    float maxAimDistance = 50;
    [SerializeField] private LayerMask layerDetection;

    [HideInInspector] public bool unitTest = false;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
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

        if(!unitTest)
            if (playerInput.currentControlScheme == "Keyboard&Mouse")
            {
                var mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                Quaternion rotation = Quaternion.LookRotation(mousePos - playerCenter.transform.position, playerCenter.transform.TransformDirection(Vector3.forward));
                playerCenter.transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
            }
            else
            {
                Quaternion rotation = Quaternion.LookRotation(aimDirection, playerCenter.transform.TransformDirection(Vector3.forward));
                playerCenter.transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
            }

        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, gunFollow.transform.position);
        RaycastHit2D hit = Physics2D.Raycast(gunFollow.transform.position, -gunFollow.up, maxAimDistance, layerDetection);

        lineRenderer.positionCount += 1;
        if(hit.collider != null)
        {
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
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

    public void Fire(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        if(attackCharge >= attackCost)
        {
            attackCharge -= attackCost;

            GameObject bullet = Instantiate(bulletPrefab, gunFollow.position, Quaternion.identity);
            Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
            bulletRB.AddForce(-gunFollow.up * bulletForce);

            PlayerBullet playerBullet = bullet.GetComponent<PlayerBullet>();
            playerBullet.bounces = bulletBounces;

            Destroy(bullet, 30);
        }
    }

}
