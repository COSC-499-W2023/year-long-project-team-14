using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyMovement : MonoBehaviour
{
    Animator animator;
    public Vector2 targetPosition;

    public float movementSpeed = 4;
    public float nextWaypointDistance = 1;
    
    Path path;
    int currentWaypoint = 0;

    Seeker seeker; 
    Rigidbody2D rb;

    public float chargeDuration = 0;
    public float waitTime = 0;
    public float timer = 0;

    public GameObject targetPlayer;
    public GameObject[] players;
    public bool followPlayer = false;
    public bool chargePlayer = false;
    public bool noWait = false;

    void Start()
    {
        //Get components and start pathfinding
        animator = GetComponent<Animator>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        players = GameObject.FindGameObjectsWithTag("Player");

        //Find target
        if(!followPlayer && !chargePlayer)
            NewTarget();

        //Set starting wait time and charge duration
        if(chargePlayer)
        {
            ResetWaitTime(0.5f, 1f);
            chargeDuration = 0.75f;
        }

        //Get difficulty
        int diff = PlayerPrefs.GetInt("difficulty");

        //Set slime move speed
        if(followPlayer) 
            movementSpeed *= diff; 
        else if(diff == 1) //Set other enemy move speed
            movementSpeed *= 1f;
        else if(diff == 2)
            movementSpeed *= 1.5f;
        else if(diff == 3)
            movementSpeed *= 2f;
        else if(diff == 4)
            movementSpeed *= 2.5f;
        
        //Start pathfinding
        InvokeRepeating("UpdatePath", 0f, 0.1f);
    }

    void Update()
    {
        //Sets target position to be the closest player
        if(followPlayer || chargePlayer)
        {
            FindClosestPlayer();
            if(targetPlayer != null)
                SetTargetPosition();
        }
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

        //Wait for timer before moving unless followPlayer = true
        if((timer >= waitTime || followPlayer) && (targetPlayer != null || (!followPlayer && !chargePlayer)) || noWait)
        {
            if(path == null)
                return;

            //Set new target after reaching target position
            if(currentWaypoint >= path.vectorPath.Count)
            {
                if(!followPlayer && !chargePlayer)
                {
                    NewTarget();
                    ResetWaitTime(-2f, 2f);
                    return;
                }
            }

            if(currentWaypoint < path.vectorPath.Count)
            {
                //Move the enemy
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
        }

        //Increment wait timer
        timer += Time.deltaTime;

        //Reset wait time and charge duration
        if(chargePlayer && timer >= waitTime + chargeDuration)
        {
            ResetWaitTime(1f, 2f);
            chargeDuration = 0.75f;
        }
    }

    //Set random target position and waitTime
    public void NewTarget()
    {
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

    //If enemy collides with something, find new target position
    void OnCollisionEnter2D(Collision2D collision)
    {   
        if(collision.gameObject.CompareTag("Player") && (chargePlayer || followPlayer))
        {     
            ResetWaitTime(1f, 2f);
            chargeDuration = 0.75f;
        }
        else if((collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("BrokenWall") || collision.gameObject.CompareTag("Breakable") || collision.gameObject.CompareTag("Player")) && !chargePlayer && !followPlayer)
        {
            currentWaypoint = path.vectorPath.Count;
        }
    }

    public void FindClosestPlayer()
    {
        if(players.Length > 1)
        {
            //Target alive player if other is dead
            if(!players[0].GetComponent<healthSystem>().dead && players[1].GetComponent<healthSystem>().dead) //If player 2 is dead, target player 1
                targetPlayer = players[0];
            else if(players[0].GetComponent<healthSystem>().dead && !players[1].GetComponent<healthSystem>().dead) //If player 1 is dead, target player 2
                targetPlayer = players[1];
            else if(!players[0].GetComponent<healthSystem>().dead && !players[1].GetComponent<healthSystem>().dead) //Target closest player
            {
                float distance1 = Vector3.Distance(gameObject.transform.position, players[0].transform.position);
                float distance2 = Vector3.Distance(gameObject.transform.position, players[1].transform.position);
                if(distance2 < distance1)
                    targetPlayer = players[1];
                else
                    targetPlayer = players[0];
            }
            else targetPlayer = null;
        }
        else if(!players[0].GetComponent<healthSystem>().dead)
            targetPlayer = players[0]; //Target player 1 if only 1 player and not dead
        else targetPlayer = null;
    }

    //Set new wait time before moving again
    public void ResetWaitTime(float x, float y)
    {
        waitTime = Random.Range(x, y);
        if(waitTime < 0) waitTime = 0;
        timer = 0;
    }

    //Sets the pathfinding target position to the target players position
    public void SetTargetPosition() 
    {
        targetPosition.x = targetPlayer.transform.position.x;
        targetPosition.y = targetPlayer.transform.position.y;
    }
}
