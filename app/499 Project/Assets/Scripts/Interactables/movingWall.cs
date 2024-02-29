using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingWall : MonoBehaviour
{
    [SerializeField] public GameObject[] waypoints;
    private int index = 0;
    public float speed = 2f;

    //Damages enemies and players if they get crushed by the wall
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            healthSystem playerHealth = collision.gameObject.GetComponent<healthSystem>();
            
            if (playerHealth != null)
            {
                playerHealth.takeDamage();
            }
        }

        if(collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHealthSystem enemyHealth = collision.gameObject.GetComponent<EnemyHealthSystem>();

            if (enemyHealth != null)
            {
                enemyHealth.takeDamage();
            }
        }
    }

    //Moves the wall
    public void Update()
    {
        if (Vector2.Distance(transform.position, waypoints[index].transform.position) < .01f)
        {
            index++;
            if(index >= waypoints.Length)
            {
                index = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, waypoints[index].transform.position, Time.deltaTime * speed);
    }
}
