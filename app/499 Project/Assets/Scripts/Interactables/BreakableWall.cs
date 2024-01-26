using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    [SerializeField] private AudioSource wallBreakSound;
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player_bullet") || collision.gameObject.CompareTag("EnemyBullet"))
        {
            //Play wall break sound
            wallBreakSound.Play();

            //Destroy wall
            Destroy(gameObject);

            //TODO: instantiate breakable wall effect here
        } 
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("FireballExplosion"))
        {
            //Play wall break sound
            wallBreakSound.Play();

            //Destroy wall
            Destroy(gameObject);

            //TODO: instantiate breakable wall effect here
        } 
    }
}
