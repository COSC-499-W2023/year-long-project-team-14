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

    PauseMenu pauseMenu;

    [HideInInspector] public bool unitTest = false;
    [HideInInspector] public bool unitTest2 = false;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        GameObject canvas = GameObject.FindWithTag("Canvas");
        if(canvas != null)
            pauseMenu = canvas.GetComponent<PauseMenu>();
    }


    // This function is called when a collision is detected.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy")){
            healthSystem playerHealth = GetComponent<healthSystem>();

            if(playerHealth != null){
                playerHealth.takeDamage();
            }
            animator.SetTrigger("isHit");
        }
    }

    void Update()
    {
        if (!unitTest2)
        {
            if (animator != null && !PauseMenu.GameIsPaused)
            {
                if (attackCharge < attackChargeMax)
                {
                    attackCharge += Time.deltaTime * attackChargeSpeed;
                    bulletUI bullets = GetComponent<bulletUI>();
                    if (bullets != null)
                    {
                        bullets.setCharge((int) attackCharge);
                    }
                }

                if (moveDirection.x != 0 || moveDirection.y != 0)
                {
                    animator.SetFloat("X", moveDirection.x);
                    animator.SetFloat("Y", moveDirection.y);

                    animator.SetBool("IsWalking", true);
                }
                else
                {
                    animator.SetBool("IsWalking", false);
                }
            }



            if (playerInput != null && !PauseMenu.GameIsPaused)
            {
                if (!unitTest)
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
                if (hit.collider != null)
                {
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            rb.velocity = new Vector2(moveDirection.x * 0.5f * moveSpeed, moveDirection.y * 0.5f * moveSpeed);
        }
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
        if(context.performed && !PauseMenu.GameIsPaused)
        {
            Shoot();
        }
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(PauseMenu.GameIsPaused)
            {
                pauseMenu.Resume();
            }
            else
            {
                pauseMenu.Pause();
            }
        }
    }

    public void Shoot()
    {
        if(attackCharge >= attackCost)
        {
            bulletUI bullets = GetComponent<bulletUI>();
            if (bullets != null)
            {
                bullets.oneLessShot();
            }
            attackCharge -= attackCost;

            GameObject bullet = Instantiate(bulletPrefab, gunFollow.position, Quaternion.identity);
            Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
            bulletRB.AddForce(-gunFollow.up * 50 * bulletForce);

            PlayerBullet playerBullet = bullet.GetComponent<PlayerBullet>();
            playerBullet.bounces = bulletBounces;

            Destroy(bullet, 30);
        }
    }

    public Vector2 GetMoveDirection()
    {
        return moveDirection;
    }

    public void SetMoveDirection(Vector2 direction)
    {
        moveDirection = direction;
    }

    public Vector2 GetAimDirection()
    {
        return aimDirection;
    }

    public void SetAimDirection(Vector2 direction)
    {
        aimDirection = direction;
    }


}
 