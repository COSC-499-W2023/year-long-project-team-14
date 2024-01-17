using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyMovement1 : MonoBehaviour
{
    Animator animator;
    public Vector2 targetPosition;

    public float movementSpeed = 4;
    public float nextWaypointDistance = 1;
    
    Path path;
    int currentWaypoint = 0;

    Seeker seeker; 
    Rigidbody2D rb;

    public float waitTime = 0;
    float timer = 0;

    void Start()
    {
        //Get components and start pathfinding
        animator = GetComponent<Animator>();
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

        //Wait for timer before moving
        if(timer >= waitTime)
        {
            if(path == null)
                return;

            //Set new target after reaching target position
            if(currentWaypoint >= path.vectorPath.Count)
            {
                NewTarget();
                return;
            }

            //Moves the enemy
            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * movementSpeed * 2500 * Time.deltaTime;
            rb.AddForce(force);

            //Increment waypoint
            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
            if(distance < nextWaypointDistance)
            {
                currentWaypoint++;
            }

            // Smoothly interpolate the animation parameters
            float x = Mathf.Lerp(animator.GetFloat("X"), direction.x, 0.1f);
            float y = Mathf.Lerp(animator.GetFloat("Y"), direction.y, 0.1f);

            if (direction.x != 0 || direction.y != 0)
            {
                animator.SetFloat("X", x);
                animator.SetFloat("Y", y);

                animator.SetBool("IsWalking", true);
            }
        }

        //Increment wait timer
        if(timer < waitTime)
            timer += Time.deltaTime;
    }

    //Set random target position and waitTime
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

    //Set specific target position, used for unit testing. 
    public void NewTarget(float x, float y)
    {
        targetPosition = new Vector2(x,y);
        waitTime = 0;
    }

    //If enemy collides with player or walls, find new target position 
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Player"))
        {
            currentWaypoint = path.vectorPath.Count;

        }
    }
    
}
