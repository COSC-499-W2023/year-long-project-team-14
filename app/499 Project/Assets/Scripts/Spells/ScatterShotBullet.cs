using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterShotBullet : MonoBehaviour
{
    public GameObject impactEffect;
    private Vector2 moveDirection;
    private float moveSpeed;
    public int bounces = 5;

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

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 5f;
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
