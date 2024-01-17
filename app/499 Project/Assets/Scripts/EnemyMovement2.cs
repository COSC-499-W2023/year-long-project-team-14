using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyMovement2 : MonoBehaviour
{
    Animator animator;
    public Vector2 targetPosition;

    public float movementSpeed = 4;
    public float nextWaypointDistance = 1;
    
    Path path;
    int currentWaypoint = 0;

    Seeker seeker; 
    Rigidbody2D rb;

    public GameObject targetPlayer;
    public GameObject[] players;

    void Start()
    {
        //Get components and players, and start pathfinding
        animator = GetComponent<Animator>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        players = GameObject.FindGameObjectsWithTag("Player");
        InvokeRepeating("UpdatePath", 0f, 0.25f);
        
    }

    void Update()
    {
        //Sets target position to always be the closest player
        FindClosestPlayer();
        if(targetPlayer != null)
            SetTargetPosition();
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
        
        if(path == null)
            return;

        //Set new target after reaching target position
        if(currentWaypoint >= path.vectorPath.Count)
        {
            FindClosestPlayer();
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

        //Smoothly interpolate the animation parameters
        float x = Mathf.Lerp(animator.GetFloat("X"), direction.x, 0.1f);
        float y = Mathf.Lerp(animator.GetFloat("Y"), direction.y, 0.1f);

        if (direction.x != 0 || direction.y != 0)
        {
            animator.SetFloat("X", x);
            animator.SetFloat("Y", y);

            animator.SetBool("IsWalking", true);
        }
    }

    public void FindClosestPlayer()
    {
        
        if(players.Length > 1)
        {
            if(!players[0].GetComponent<healthSystem>().dead && players[1].GetComponent<healthSystem>().dead) //If player 2 is dead, target player 1
                targetPlayer = players[0];
            if(players[0].GetComponent<healthSystem>().dead && !players[1].GetComponent<healthSystem>().dead) //If player 1 is dead, target player 2
                targetPlayer = players[1];
            else 
            {
                //Target closest player
                float distance1 = Vector3.Distance(gameObject.transform.position, players[0].transform.position);
                float distance2 = Vector3.Distance(gameObject.transform.position, players[1].transform.position);
                if(distance2 < distance1)
                    targetPlayer = players[1];
                else
                    targetPlayer = players[0];
            }
        }
        else
            targetPlayer = players[0]; //Target player 1 if there is only 1 player
    }

    public void SetTargetPosition() //Sets the pathfinding target position to the target players position
    {
        targetPosition.x = targetPlayer.transform.position.x;
        targetPosition.y = targetPlayer.transform.position.y;
    }
}
