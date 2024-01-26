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

    [SerializeField] private AudioSource hitSound;
    [SerializeField] private AudioSource deathSound;

    private void Start()
    {
        life = maxLife;
        GameObject canvas = GameObject.FindWithTag("Canvas");
        if(canvas != null)
            gameOverMenu = canvas.GetComponent<GameOverMenu>();
    }

    public void takeDamage()
    {
        //Check if the player has >= health and if the player is temporarly invincible
        if (life >=1 && isInvic == false)
        {
            life--;
            hearts[life].SetActive(false);
           
           //Make the player invicible 
           isInvic = true;

            if (life < 1)
            {
                Die();
            }
            else
            {
                //Signal the isHit anamation and play the invincible anamation
                animator.SetTrigger("isHit");
                StartCoroutine(Transparent2());
                //play hit sound
                hitSound.Play();
            }
        }
    }

    void Die()
    {
        rb.bodyType = RigidbodyType2D.Static;
        animator.SetBool("IsWalking", false);
        animator.SetTrigger("Death");

        //play death sound
        deathSound.Play();

        StartCoroutine(Transparent());
        
        gameObject.layer = LayerMask.NameToLayer("NoCollide");
        playerController.playerCenter.SetActive(false);
        
        dead = true;
        spriteRenderer.sortingOrder = 8;

        if(gameOverMenu != null)
            gameOverMenu.playercount--;
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

    public void SetHeartsActive()
    {
        for(int i = 0; i < life; i++)
        hearts[i].SetActive(true);
    }
}
