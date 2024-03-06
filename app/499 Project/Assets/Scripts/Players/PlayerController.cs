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
    public bool player1 = false;

    public bool aimingInWall = false;

    public GameObject bulletPrefab;

    public PlayerInput playerInput;
    public Rigidbody2D rb;
    public Animator animator;
    public healthSystem hs;

    public Transform gunFollow;
    public GameObject playerCenter; 

    Vector2 moveDirection = Vector2.zero;
    Vector2 aimDirection = Vector2.zero;

    [SerializeField] private LineRenderer lineRenderer;
    float maxAimDistance = 50;
    [SerializeField] private LayerMask layerDetection;

    GameMaster gameMaster;
    PauseMenu pauseMenu;
    GameOverMenu gameOverMenu;
    ControlMenu controlMenu;
    WinMenu winMenu;

    [HideInInspector] public bool unitTest = false;
    [HideInInspector] public bool unitTest2 = false;

    [SerializeField] private AudioSource shootSound;
    [SerializeField] private AudioSource dashSound;
    public AudioSource buttonClick;

    //dash cooldown
    public float dashCooldown = 1;
    
    //dash cool down timer 
    public float dashCDT = 1;

    public GameObject dashPrefab; 

    public GameObject interactable;

    void Start()
    {
        //Get objects and components

        GameObject canvas = GameObject.FindWithTag("Canvas");
        if(canvas != null)
        {
            pauseMenu = canvas.GetComponent<PauseMenu>();
            gameOverMenu = canvas.GetComponent<GameOverMenu>();
            controlMenu = canvas.GetComponent<ControlMenu>();
            winMenu = canvas.GetComponent<WinMenu>();
        }

        GameObject gm = GameObject.FindWithTag("GameMaster");
        if(gm != null)
            gameMaster = gm.GetComponent<GameMaster>();
    }

    void Update()
    {
        if (!unitTest2) //If running a unit test, do not run this code
        {
            if (animator != null && !hs.dead && !PauseMenu.GameIsPaused)
            {
                //Increase attack charge and update bullet UI
                if (attackCharge < attackChargeMax)
                {
                    attackCharge += Time.deltaTime * attackChargeSpeed;
                    bulletUI bullets = GetComponent<bulletUI>();
                    if (bullets != null)
                    {
                        bullets.setCharge((int) attackCharge);
                    }
                }
                
                //Play animations based on movement
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

            if (playerInput != null && !hs.dead && !PauseMenu.GameIsPaused)
            {
                if (!unitTest && (gameMaster == null || !gameMaster.unitTest))
                {
                    //Set aim direction based on user input
                    if(aimDirection.x != 0 && aimDirection.y != 0)
                    {
                        Quaternion rotation = Quaternion.LookRotation(aimDirection, playerCenter.transform.TransformDirection(Vector3.forward));
                        playerCenter.transform.rotation = Quaternion.Slerp(playerCenter.transform.rotation, new Quaternion(0, 0, rotation.z, rotation.w), 50 * Time.deltaTime);
                    }
                    else if(player1)
                    {
                        var mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                        Quaternion rotation = Quaternion.LookRotation(mousePos - playerCenter.transform.position, playerCenter.transform.TransformDirection(Vector3.forward));
                        playerCenter.transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
                    }

                    //Create line renderer in direction the player is aiming in
                    lineRenderer.positionCount = 1;
                    lineRenderer.SetPosition(0, gunFollow.transform.position);
                    RaycastHit2D hit = Physics2D.Raycast(gunFollow.transform.position, -gunFollow.up, maxAimDistance, layerDetection);

                    lineRenderer.positionCount += 1;
                    if (hit.collider != null && !aimingInWall)
                    {
                        lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                    }
                    else
                    {
                        lineRenderer.SetPosition(lineRenderer.positionCount - 1, gunFollow.transform.position);
                    }
                }
            }
        }

        //Used for the dash cooldown (its the timer)
        dashCDT += Time.deltaTime;

    }

    private void FixedUpdate()
    {
        //Moves player
        if(rb != null && !hs.dead)
            rb.velocity = new Vector2(moveDirection.x * 0.5f * moveSpeed, moveDirection.y * 0.5f * moveSpeed);
    }

    //Gets the direction the player is moving in from input system
    public void Move(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>().normalized;
    }

    //Gets the direction the player is aiming in from input system
    public void Aim(InputAction.CallbackContext context)
    {
        aimDirection = context.ReadValue<Vector2>().normalized;
    }

    //Detects if player presses shoot button
    public void Fire(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            Shoot();
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            Dash();
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            Interact();
        }
    }

    public void Back(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(controlMenu.controlMenu)
            {
                controlMenu.Back();
                buttonClick.Play();
            }
        }   
    }

    //Pauses the game
    public void Pause(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(pauseMenu.pauseMenu)
            {
                pauseMenu.Resume();
            }
            else if(!winMenu.winMenu &&!controlMenu.controlMenu && !gameOverMenu.GameIsOver)   //TODO: add win menu to this when its done
            {
                pauseMenu.Pause();
            }
        }
    }

    //Shoots a bullet in the direction the player is aiming in if they can shoot
    public void Shoot()
    {
        if(attackCharge >= attackCost && !hs.dead && !PauseMenu.GameIsPaused)
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

            //shoot sound effect
            shootSound.Play();

            PlayerBullet playerBullet = bullet.GetComponent<PlayerBullet>();
            playerBullet.bounces = bulletBounces;

            Destroy(bullet, 30);
        }
    }

    public void Dash(){
        //If the dash is off cooldown, the player is alive and the game is not paused.
        if(dashCDT >= dashCooldown && !hs.dead && !PauseMenu.GameIsPaused){
            dashSound.Play();
            GameObject dashSmoke = Instantiate(dashPrefab, transform.position, transform.rotation);
            //Add force in the direction the player is moving
            rb.AddForce(GetMoveDirection()*60000);
            Destroy(dashSmoke, 0.2f);
            //Make the player invincible through the duration of the dash.
            hs.dashHs();
            // reset the dash cool down.
            dashCDT = 0;
             // Start a coroutine for the second part of the dash after a delay.
            //StartCoroutine(SecondDashEffect());
        }
    }

    private IEnumerator SecondDashEffect(){
        // Wait for new player position after dash
        yield return new WaitForSeconds(0.15f);

        // Instantiate the second dash smoke at the updated position.
        GameObject dashSmoke2 = Instantiate(dashPrefab, transform.position, transform.rotation);
        Destroy(dashSmoke2, 0.2f);
    }

    //Calls a function depending on what object you are interacting with
    public void Interact()
    {
        if(interactable != null)
        {
            string tag = interactable.tag;
            if(tag == "Ladder")
            {
                interactable.GetComponent<Ladder>().Interact();
            }
            else if(tag == "Portal")
            {
                interactable.GetComponent<Portal>().Interact();
            }
            else if(tag == "lightning")
            {
                interactable.GetComponent<LightningPickup>().Interact();
            }
            else if (tag == "Fireball")
            {
                interactable.GetComponent<FireballPickup>().Interact();
            }
            else if (tag == "SeekingOrb")
            {
                interactable.GetComponent<SeekingOrbPickup>().Interact();
            }
            else if (tag == "summonChad")
            {
                interactable.GetComponent<SummonChadPickup>().Interact();
            }
        }
    }

    public void OnTriggerStay2D(Collider2D collider)
    {
        interactable = collider.gameObject;
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        interactable = null;
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
 