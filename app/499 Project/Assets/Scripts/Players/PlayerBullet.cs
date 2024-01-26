using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public int bounces = 1;
    public GameObject impactEffect;

    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Breakable")){
            if(bounces <= 0)
            {
                Destroy(gameObject);
                GameObject clone = Instantiate(impactEffect, transform.position, transform.rotation);
                Destroy(clone, 1.0f);
            }
            bounces--;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
                Destroy(gameObject);
                GameObject clone = Instantiate(impactEffect, transform.position, transform.rotation);
                Destroy(clone, 1.0f);
        }
    }
    
}
