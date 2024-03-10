using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthSystem : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D rb;
    public GameObject[] hearts;
    public Animator animator;
    public CircleCollider2D cc;
    public int life;
    public int maxLife = 3;
    public bool dead;
    public GameOverMenu gameOverMenu;
    public PlayerController playerController;
    public bool isInvic = false;
    public bool chad = false;

    [SerializeField] private AudioSource hitSound;
    [SerializeField] private AudioSource deathSound;

    public string ChadAttack = "chadAttack";

    private void Start()
    {
        life = maxLife;
        GameObject canvas = GameObject.FindWithTag("Canvas");
        if(canvas != null)
            gameOverMenu = canvas.GetComponent<GameOverMenu>();
    }

    // Damage player if colliding with enemy or bullet
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyBullet") ){
            takeDamage();
        }
    }

    //Continue to take damage if still touching enemy
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy")){
            takeDamage();
            
        }
    }

    //Used to check collision with the spikes while still allowing the player to walk through them.
    private void OnTriggerStay2D(Collider2D collision){
        if(collision.gameObject.CompareTag("Spike")){
            takeDamage();
        }
    }

    public void takeDamage()
    {
        //Check if the player has >= health and if the player is temporarly invincible
        if(life >= 1 && isInvic == false)
        {
            //Decrease health by 1
            life--;
            hearts[life].SetActive(false);
           
           //Make the player invicible 
           isInvic = true;

            if (life <= 0) //Kill player if no life
            {
                Die();
            }
            else
            {
                //Signal the isHit anamation and play the invincible anamation
                animator.SetTrigger("isHit");

                //Make player flash while invincible
                StartCoroutine(Transparent2());

                //play hit sound
                hitSound.Play();
            }
        }
    }

    public void Die()
    {
        //Play death animation
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsDead", true);

        //Play death sound
        deathSound.Play();

        //Make player transparent
        StartCoroutine(Transparent());
        
        //Make player not able to interact or do anything while dead
        rb.bodyType = RigidbodyType2D.Static;
        gameObject.layer = LayerMask.NameToLayer("NoCollide");
        playerController.playerCenter.SetActive(false);
        Component component = GetComponent(ChadAttack);
        if (component != null)
        {
            (component as Behaviour).enabled = false;
        }
        
        dead = true;
        spriteRenderer.sortingOrder = 8;

        if(gameOverMenu != null && !chad)
            gameOverMenu.playercount--;
    }

    public void dashHs(){
        // Set the player to invincible during the dash
        isInvic = true;
        gameObject.layer = LayerMask.NameToLayer("NoCollide");
        //Start the transparent anamation for the dash
        StartCoroutine(Transparent3());
    }

    IEnumerator Transparent()
    {
        yield return new WaitForSeconds(0.5f);
        Color currentColor = spriteRenderer.color; // Set the transparency (alpha) value
        currentColor.a = 0.5f; // Adjust the alpha value as needed
        spriteRenderer.color = currentColor;
    }

    //This is used to show when the player is invicible 
    IEnumerator Transparent2()
    {
        //Make the player transparent
        yield return new WaitForSeconds(0f);
        Color currentColor = spriteRenderer.color; // Set the transparency (alpha) value
        currentColor.a = 0.5f; // Adjust the alpha value as needed
        spriteRenderer.color = currentColor;

        //Make the player invisible 
        yield return new WaitForSeconds(0.15f);
         currentColor = spriteRenderer.color; // Set the transparency (alpha) value
        currentColor.a = 0f; // Adjust the alpha value as needed
        spriteRenderer.color = currentColor;

        //Repeate the steps above multiple times 
        yield return new WaitForSeconds(0.15f);
         currentColor = spriteRenderer.color; // Set the transparency (alpha) value
        currentColor.a = 0.5f; // Adjust the alpha value as needed
        spriteRenderer.color = currentColor;

        yield return new WaitForSeconds(0.15f);
         currentColor = spriteRenderer.color; // Set the transparency (alpha) value
        currentColor.a = 0f; // Adjust the alpha value as needed
        spriteRenderer.color = currentColor;

        yield return new WaitForSeconds(0.15f);
         currentColor = spriteRenderer.color; // Set the transparency (alpha) value
        currentColor.a = .5f; // Adjust the alpha value as needed
        spriteRenderer.color = currentColor;

        yield return new WaitForSeconds(0.15f);
         currentColor = spriteRenderer.color; // Set the transparency (alpha) value
        currentColor.a = 0f; // Adjust the alpha value as needed
        spriteRenderer.color = currentColor;

        yield return new WaitForSeconds(0.15f);
         currentColor = spriteRenderer.color; // Set the transparency (alpha) value
        currentColor.a = 0.5f; // Adjust the alpha value as needed
        spriteRenderer.color = currentColor;

        yield return new WaitForSeconds(0.15f);
         currentColor = spriteRenderer.color; // Set the transparency (alpha) value
        currentColor.a = 1f; // Adjust the alpha value as needed
        spriteRenderer.color = currentColor;

        //Make the player able to take damage again
        isInvic = false;
    }

    //This is used during the dash to make the player transparent and resets invincibility 
     IEnumerator Transparent3()
    {
        //Make the player transparent
        yield return new WaitForSeconds(0f);
        Color currentColor = spriteRenderer.color; // Set the transparency (alpha) value
        currentColor.a = 0.5f; // Adjust the alpha value as needed
        spriteRenderer.color = currentColor;

        //Make the player fully visible  
        yield return new WaitForSeconds(0.13f);
         currentColor = spriteRenderer.color; // Set the transparency (alpha) value
        currentColor.a = 1f; // Adjust the alpha value as needed
        spriteRenderer.color = currentColor;

        //reset invincibility. 
        isInvic = false;
        gameObject.layer = LayerMask.NameToLayer("Player");

    }

    //Update heart UI
    public void SetHeartsActive()
    {
        for(int i = 0; i < life; i++)
        hearts[i].SetActive(true);
    }
}
