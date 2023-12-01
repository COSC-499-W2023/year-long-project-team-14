using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private EnemyAttack ea; 
    private EnemyMovement1 enemyMovement; 
    public int enemyHealth = 2;

    private CircleCollider2D enemyCollider;
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
        
    }
     // This function is called when a collision is detected.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision involves a player bullet GameObject.
        if (collision.gameObject.CompareTag("Player_bullet"))
        {
            if (enemyHealth > 1)
            {
                enemyHealth--;
                animator.SetTrigger("isHit");
            }
            else
            {
                Die();
            }
            
            Destroy(collision.gameObject);

        }
    }

    void Die()
    {
        enemyMovement.enabled = false;
        ea.enabled = false;
        enemyCollider.enabled = false;

        animator.SetTrigger("Death");

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
