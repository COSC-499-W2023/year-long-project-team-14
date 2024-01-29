using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    public GameObject breakEffect;
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player_bullet") || collision.gameObject.CompareTag("EnemyBullet"))
        {
            //Destroy wall
            Destroy(gameObject);

            //Play break effect
            GameObject effect = Instantiate(breakEffect, transform.position, transform.rotation);
            Destroy(effect, 1.0f);
        } 
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("FireballExplosion"))
        {
            //Destroy wall
            Destroy(gameObject);

            //Play break effect
            GameObject effect = Instantiate(breakEffect, transform.position, transform.rotation);
            Destroy(effect, 1.0f);
        } 
    }
}
