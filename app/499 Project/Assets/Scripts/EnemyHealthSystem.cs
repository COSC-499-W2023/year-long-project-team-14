using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private EnemyAttack ea; 
    private int enemyHealth = 2;
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
    }
}
