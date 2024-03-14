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
            DestroyBullet();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 2.5f;

        //Get difficulty
        int diff = PlayerPrefs.GetInt("difficulty");
        
        //Set bullet speed
        if(diff == 1) 
            moveSpeed *= 1f;
        else if(diff == 2)
            moveSpeed *= 1.5f;
        else if(diff == 3)
            moveSpeed *= 2f;
        else if(diff == 4)
            moveSpeed *= 2.5f;
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
