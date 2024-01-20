using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class movingWall : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    private int index = 0;

    [SerializeField] private float speed = 2f;

    void Start()
    {
        StartCoroutine(UpdateGrid());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            healthSystem playerHealth = collision.gameObject.GetComponent<healthSystem>();
            
            if (playerHealth != null)
            {
                playerHealth.takeDamage();
            }
            playerHealth.animator.SetTrigger("isHit");
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHealthSystem enemyHealth = collision.gameObject.GetComponent<EnemyHealthSystem>();

            if (enemyHealth != null)
            {
                enemyHealth.takeDamage();
            }
            enemyHealth.animator.SetTrigger("isHit");
        }
    }

    // Update is called once per frame
    private void Update()
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

    //Updates pathfinding grid so enemies know where the wall is as it moves
    public IEnumerator UpdateGrid()
    {
        GraphUpdateObject guo = new GraphUpdateObject(transform.parent.gameObject.GetComponent<Collider2D>().bounds);
        guo.updatePhysics = true;
        AstarPath.active.UpdateGraphs(guo);
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(UpdateGrid());
    }
}
