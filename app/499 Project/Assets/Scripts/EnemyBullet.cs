using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
  
    public int bounces = 1;
    public GameObject impactEffect;
    void Start()
    {
        // Ignore collisions with broken walls
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), GameObject.FindGameObjectWithTag("BrokenWall").GetComponent<CompositeCollider2D>());
    }

    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Wall") ){
            if(bounces <= 0){
                Destroy(gameObject);
                Instantiate(impactEffect, transform.position, transform.rotation);
            }
            bounces--;
        }

        if (collision.gameObject.CompareTag("Breakable"))
        {
            Destroy(collision.gameObject);
            if (bounces <= 0)
            {
                Destroy(gameObject);
                Instantiate(impactEffect, transform.position, transform.rotation);
            }
            bounces--;
        }
    }
    
}
