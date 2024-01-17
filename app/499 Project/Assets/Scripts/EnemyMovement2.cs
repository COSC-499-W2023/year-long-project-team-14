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

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        players = GameObject.FindGameObjectsWithTag("Player");
        FindClosestPlayer();
        InvokeRepeating("UpdatePath", 0f, 0.25f);
        
    }

    void Update()
    {
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

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * movementSpeed * 2500 * Time.deltaTime;
        rb.AddForce(force);

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

    public void FindClosestPlayer()
    {
        
        if(players.Length > 1)
        {
            if(!players[0].GetComponent<healthSystem>().dead && players[1].GetComponent<healthSystem>().dead)
                targetPlayer = players[0];
            if(players[0].GetComponent<healthSystem>().dead && !players[1].GetComponent<healthSystem>().dead)
                targetPlayer = players[1];
            else
            {
                float distance1 = Vector3.Distance(gameObject.transform.position, players[0].transform.position);
                float distance2 = Vector3.Distance(gameObject.transform.position, players[1].transform.position);
                if(distance2 < distance1)
                    targetPlayer = players[1];
                else
                    targetPlayer = players[0];
            }
        }
        else
            targetPlayer = players[0];
    }

    public void SetTargetPosition()
    {
        targetPosition.x = targetPlayer.transform.position.x;
        targetPosition.y = targetPlayer.transform.position.y;
    }
}
