using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{

    public int bounces = 1;
    public GameObject impactEffect;

    void Start()
    {
        // Ignore collisions with objects in the "Player" layer and broken walls
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Player"));
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), GameObject.FindGameObjectWithTag("BrokenWall").GetComponent<CompositeCollider2D>());
    }
    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Wall") ){
            if(bounces <= 0){
                Destroy(gameObject);
                GameObject clone = Instantiate(impactEffect, transform.position, transform.rotation);
                Destroy(clone, 1.0f);
            }
            bounces--;
        }

        if (collision.gameObject.CompareTag("Breakable"))
        {
            Destroy(collision.gameObject);
            if (bounces <= 0)
            {
                Destroy(gameObject);
                GameObject clone = Instantiate(impactEffect, transform.position, transform.rotation);
                Destroy(clone, 1.0f);
            }
            bounces--;
        }

        if (collision.gameObject.CompareTag("EnemyBullet")){
            Destroy(collision.gameObject);
            Destroy(gameObject);
            GameObject clone = Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(clone, 1.0f);
        }

        // ^ if a player bullet collidies with an enemy bullet than delete both.
    }
    
}
