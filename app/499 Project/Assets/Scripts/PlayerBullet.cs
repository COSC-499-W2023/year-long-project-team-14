using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{

    public int bounces = 1;

    void Start()
    {
        // Ignore collisions with objects in the "Player" layer
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Player"));
    }
    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Wall") ){
            if(bounces <= 0){
                Destroy(gameObject);
            }
            bounces--;
        }

        if(collision.gameObject.CompareTag("EnemyBullet")){
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }

        // ^ if a player bullet collidies with an enemy bullet than delete both.
    }
    
}
