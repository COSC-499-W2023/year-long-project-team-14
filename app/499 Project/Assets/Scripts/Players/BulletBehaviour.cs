using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    private Animator animator;


    //THIS COLLISION IS NOW IN THE HEALTHSYSTEM SCRIPT AND THIS SCRIPT IS NO LONGER USED

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
     // This function is called when a collision is detected.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision involves a player bullet GameObject.
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            healthSystem playerHealth = GetComponent<healthSystem>();
            if (playerHealth != null)
            {
                playerHealth.takeDamage();
            }
            animator.SetTrigger("isHit");
            Destroy(collision.gameObject);

        }
    }
}