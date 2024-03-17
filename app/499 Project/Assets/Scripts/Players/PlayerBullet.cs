using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public int bounces = 1;
    public GameObject impactEffect;

    void OnCollisionEnter2D(Collision2D collision){
        //Destroy bullet if bounces <= 0 and colliding with an object
        if(collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Breakable")){
            if(bounces <= 0)
            {
                Destroy(gameObject);
                GameObject clone = Instantiate(impactEffect, transform.position, transform.rotation);
                Destroy(clone, 1.0f);
            }
            bounces--;
        }

        //Break bullet if colliding with enemy
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Boss"))
        {
                Destroy(gameObject);
                GameObject clone = Instantiate(impactEffect, transform.position, transform.rotation);
                Destroy(clone, 1.0f);
        }
    }
    
}
