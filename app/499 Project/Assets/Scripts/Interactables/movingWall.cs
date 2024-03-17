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
                StartCoroutine(StunPlayer(collision.gameObject));
            }
        }

        if(collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHealthSystem enemyHealth = collision.gameObject.GetComponent<EnemyHealthSystem>();

            if (enemyHealth != null)
            {
                enemyHealth.takeDamage();
                StartCoroutine(StunEnemy(collision.gameObject));
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

    public IEnumerator StunPlayer(GameObject player)
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.moveSpeed /= 1000;
        player.layer = 17;

        yield return new WaitForSeconds(0.5f);

        playerController.moveSpeed *= 1000;
        player.layer = 7;
    }
    public IEnumerator StunEnemy(GameObject enemy)
    {
        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
        enemyMovement.movementSpeed /= 1000;
        enemy.layer = 17;

        yield return new WaitForSeconds(0.5f);

        enemyMovement.movementSpeed *= 1000;
        enemy.layer = 9;
    }
}
