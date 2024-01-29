using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public GameObject explosionPrefab;

    //Delete itself and spawn in explosion when colliding with an object
    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(explosion, 0.55f);
        Destroy(gameObject); 
    }
    
}
