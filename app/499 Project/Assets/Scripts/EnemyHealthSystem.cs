using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    public EnemyAttack ea; 
    public EnemyMovement1 enemyMovement; 
    public int enemyHealth = 2;

    public CircleCollider2D enemyCollider;
    public Ladder ladder;
    public Portal portal;

    private void Start()
    {
        // Add the enemy to the list of allEnemies when it's instantiated
        GameObject port = GameObject.FindWithTag("Portal");
        if (port != null)
        {   portal = port.GetComponent<Portal>();
            portal.allEnemies.Add(gameObject);
        }
        else
        {
            GameObject lad = GameObject.FindWithTag("Ladder");
            if(lad != null)
            {
                ladder = lad.GetComponent<Ladder>();
                ladder.allEnemies.Add(gameObject);
            }
        }
        
        enemyCollider = GetComponent<CircleCollider2D>();
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        ea = GetComponent<EnemyAttack>();
        enemyMovement = GetComponent<EnemyMovement1>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }
     // This function is called when a collision is detected.
    public void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision involves a player bullet GameObject.
        if (collision.gameObject.CompareTag("Player_bullet"))
        {
            if (enemyHealth > 1)
            {
                takeDamage();
                animator.SetTrigger("isHit");
            }
            else
            {
                Die();
            }
            
            Destroy(collision.gameObject);

        }
    }

    public void takeDamage()
    {
        enemyHealth--;
    }
    
    public void Die()
    {
        enemyMovement.enabled = false;
        ea.enabled = false;
        enemyCollider.enabled = false;

        animator.SetTrigger("Death");

        StartCoroutine(Transparent());

        if (portal != null)
        {
            portal.allEnemies.Remove(gameObject);

            if (portal.allEnemies.Count == 0)
            {
                EndLevel();
            }
        }
        else
        {
            if (ladder != null)
            {
                ladder.allEnemies.Remove(gameObject);

                if (ladder.allEnemies.Count == 0)
                {
                    EndLevel();
                }
            }
        }
    }

    IEnumerator Transparent()
    {
        yield return new WaitForSeconds(0.5f);
        Color currentColor = spriteRenderer.color; // Set the transparency (alpha) value
        currentColor.a = 0.5f; // Adjust the alpha value as needed
        spriteRenderer.color = currentColor;
    }

    void EndLevel()
    {
        if (portal != null)
        {
            portal.SetPortalActive(true);
        }
        else
        {
            if (ladder != null)
            {
                ladder.SetLadderActive(true);
            }
        }
    }
}
