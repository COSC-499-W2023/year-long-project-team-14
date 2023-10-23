using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{

    public int bounces = 1;


    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Wall") ){
            if(bounces <= 0){
                Destroy(gameObject);
            }
            bounces--;
        }
    }
    
}