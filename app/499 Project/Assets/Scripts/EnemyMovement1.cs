using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyMovement1 : MonoBehaviour
{
    Animator animator;
    public Vector2 targetPosition;
    public float movementSpeed = 10;
    public float nextWaypointDistance = 1;
    
    Path path;
    int currentWaypoint = 0;
    //bool reachedEndOfPath = false;

    Seeker seeker; 
    Rigidbody2D rb;

    float waitTime = 0;
    float timer = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        NewTarget();
        InvokeRepeating("UpdatePath", 0f, 0.25f);
        
    }

    void UpdatePath()
    {
        if(seeker.IsDone())
            seeker.StartPath(rb.position, targetPosition, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        animator.SetBool("IsWalking", false);
        if(timer >= waitTime)
        {
            if(path == null)
                return;

            if(currentWaypoint >= path.vectorPath.Count)
            {
                //reachedEndOfPath = true;
                NewTarget();
                return;
            }
            else
            {
                //reachedEndOfPath = false;
            }

            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * movementSpeed * 5000 * Time.deltaTime;
            rb.AddForce(force);

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

            if(distance < nextWaypointDistance)
            {
                currentWaypoint++;
            }

            if (direction.x != 0 || direction.y != 0)
            {
                animator.SetFloat("X", direction.x);
                animator.SetFloat("Y", direction.y);

                animator.SetBool("IsWalking", true);
            }
        }

        if(timer < waitTime)
            timer += Time.deltaTime;
    }

    public void NewTarget()
    {
        float t = Random.Range(-2f, 2f);
        if(t < 0)
            t = 0;
        waitTime = t;
        timer = 0;
        
        float x = Random.Range(-12f, 12f);
        float y = Random.Range(-8f, 7f);
        targetPosition = new Vector2(x, y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player"))
        {
            currentWaypoint = path.vectorPath.Count;

        }
    }
}
