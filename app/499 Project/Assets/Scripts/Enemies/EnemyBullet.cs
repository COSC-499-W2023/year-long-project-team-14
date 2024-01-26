using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
  
    public int bounces = 1;
    public GameObject impactEffect;

    [SerializeField] private AudioSource wallBreakSound;

    void Start()
    {
        // Ignore collisions with broken walls
        if(GameObject.FindGameObjectWithTag("BrokenWall") == true){
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), GameObject.FindGameObjectWithTag("BrokenWall").GetComponent<CompositeCollider2D>());}
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
            //play wall break sound
            wallBreakSound.Play();
            if (bounces <= 0)
            {
                Destroy(gameObject);
                GameObject clone = Instantiate(impactEffect, transform.position, transform.rotation);
                Destroy(clone, 1.0f);
            }
            bounces--;
        }

        if(collision.gameObject.CompareTag("Player") ){
                Destroy(gameObject);
                GameObject clone = Instantiate(impactEffect, transform.position, transform.rotation);
                Destroy(clone, 1.0f);
        }


    }
    
}
