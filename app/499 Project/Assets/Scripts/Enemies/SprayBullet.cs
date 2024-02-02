using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprayBullet : MonoBehaviour
{
    public GameObject impactEffect;
    private Vector2 moveDirection;
    private float moveSpeed;

    void OnCollisionEnter2D(Collision2D collision){
        //Break bullet if colliding with something
        if(collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Breakable") || collision.gameObject.CompareTag("Player") ){
            GameObject clone = Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(clone, 1.0f);

            // Use the pool to return the bullet
            BulletSprayPool.bulletSprayPoolInstanse.ReturnBullet(gameObject);
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

    private void Destroy()
    {
        gameObject.SetActive(false);
    }

}
