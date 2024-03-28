using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterShotBullet : MonoBehaviour
{
    public GameObject impactEffect;
    private Vector2 moveDirection;
    public float moveSpeed;
    public int bounces = 5;
    private Rigidbody2D rb;

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Destroy bullet if bounces <= 0 and colliding with an object
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Breakable"))
        {
            if (bounces <= 0)
            {
                Destroy(gameObject);
                GameObject clone = Instantiate(impactEffect, transform.position, transform.rotation);
                Destroy(clone, 1.0f);
            }
            else
            {
                // Get the contact normal (direction the bullet hits the wall)
                Vector2 normal = collision.GetContact(0).normal;

                // Reflect the bullet's velocity off the wall
                Vector2 reflectedVelocity = Vector2.Reflect(rb.velocity, normal);

                // Update the bullet's velocity to the reflected velocity
                rb.velocity = reflectedVelocity;

                bounces--;
            }
        }

        //Break bullet if colliding with enemy
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Boss"))
        {
            Destroy(gameObject);
            GameObject clone = Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(clone, 1.0f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 5f;
        rb = GetComponent<Rigidbody2D>();
        // Set initial velocity
        rb.velocity = moveDirection * moveSpeed *5;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    public void SetMoveDirection(Vector2 dir)
    {
        moveDirection = dir;
    }

    private void DestroyBullet()
    {
        gameObject.SetActive(false);
    }

}
