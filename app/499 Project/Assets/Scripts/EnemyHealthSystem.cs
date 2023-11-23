using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private EnemyAttack ea; 
    public int enemyHealth = 2;

    private CircleCollider2D enemyCollider;
    public Portal portal;
    private void Start()
    {
        // Add the enemy to the list of allEnemies when it's instantiated
        GameObject p = GameObject.FindWithTag("Portal");
        if(p != null)
        {
            portal = p.GetComponent<Portal>();
            portal.allEnemies.Add(gameObject);
        }
        enemyCollider = GetComponent<CircleCollider2D>();
        
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        ea = GetComponent<EnemyAttack>();
        
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
        rb.bodyType = RigidbodyType2D.Static;
        ea.enabled = false;
        animator.SetTrigger("Death");

        enemyCollider.enabled = false;
        portal.allEnemies.Remove(gameObject);

        if (portal.allEnemies.Count == 0)
        {
            EndLevel();
        }
    }

    void EndLevel()
    {
        portal.SetPortalActive(true);
    }
}
