using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SeekerBullet : MonoBehaviour
{
    public bool allowMovement = false;
    public bool chasePlayer = false;
    public Vector2 targetPosition;

    public float movementSpeed = 4;
    public float nextWaypointDistance = 1;
    
    Path path;
    int currentWaypoint = 0;

    Seeker seeker; 
    Rigidbody2D rb;
    public Animator anim;

    public GameObject target;
    public GameObject[] players;

    public GameObject explosionPrefab;

    int diff = 0;

    void Start()
    {
        movementSpeed = 2.5f;

        //Get components
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        //Get difficulty
        diff = PlayerPrefs.GetInt("difficulty");

        //Set bullet speed
        if(diff == 1) 
            movementSpeed *= 1f;
        else if(diff == 2)
            movementSpeed *= 1.33f;
        else if(diff == 3)
            movementSpeed *= 1.67f;
        else if(diff == 4)
            movementSpeed *= 2f;

        //Start pathfinding
        InvokeRepeating("UpdatePath", 0f, 0.1f);
    }

    void Update()
    {
        //Get array of players and find closest one
        StartCoroutine(FindClosestPlayer());
        
        //Update target position
        if(target != null)
            SetTargetPosition();

        if(allowMovement && !chasePlayer)
        {
            rb.velocity = transform.right * movementSpeed * 2;
        }

        if(allowMovement && chasePlayer)
            allowMovement = false;
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
        if(chasePlayer && !allowMovement)
        {
            if(path == null)
                return;

            //Reached target position
            if(currentWaypoint >= path.vectorPath.Count)
                return;

            if(currentWaypoint < path.vectorPath.Count)
            {
                //Move the orb
                Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
                Vector2 force = direction * movementSpeed * 250 * Time.deltaTime;
                rb.AddForce(force);
            
                //Increment waypoint
                float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
                if(distance < nextWaypointDistance)
                {
                    currentWaypoint++;
                }   
            }
        }
    }

    //If orb collides with enemy... KABOOM!!!
    void OnCollisionEnter2D(Collision2D collision)
    {   
        if(collision.gameObject.CompareTag("Player"))
        {     
            healthSystem hs = collision.gameObject.GetComponent<healthSystem>();
            if(hs != null)
                hs.takeDamage();
            Explode();
        }

        if(collision.gameObject.CompareTag("Player_bullet") || collision.gameObject.CompareTag("GiantBullet"))
        {     
            Explode();
        }

        if(collision.gameObject.CompareTag("Wall"))
        {     
            StartCoroutine(DelayBeforeChasing());
        }
    }

    //Find random enemy and sets them as the target
    public IEnumerator FindClosestPlayer() 
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        List<GameObject> alivePlayers = new List<GameObject>();

        for(int i = 0; i < players.Length; i++)
        {
            healthSystem hs = players[i].GetComponent<healthSystem>();
            if(hs != null)
            {
                if(hs.life > 0 && !hs.chad)
                    alivePlayers.Add(players[i]);
            }
        }

        if(alivePlayers.Count > 1)
        {
            float distance1 = Vector3.Distance(gameObject.transform.position, alivePlayers[0].transform.position);
            float distance2 = Vector3.Distance(gameObject.transform.position, alivePlayers[1].transform.position);
            if(distance2 < distance1)
                target = alivePlayers[1];
            else
                target = alivePlayers[0];
        }
        else if(alivePlayers.Count > 0)
            target = alivePlayers[0];
        
        yield return new WaitForSeconds(0.25f);
        if(target == null)
            Explode();

        
    }

    //Sets the pathfinding target position to the target enemies position
    public void SetTargetPosition() 
    {
        healthSystem hs = target.GetComponent<healthSystem>();
        if(hs != null)
        {
            if(hs.life > 0)
            {
                targetPosition.x = target.transform.position.x;
                targetPosition.y = target.transform.position.y;
            }
            else if(target != null) Explode();
        }
    }

    public void Explode()
    {
        GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
        Destroy(explosion, 0.5f);
        Destroy(gameObject);
    }

    public IEnumerator DelayBeforeChasing() 
    {
        allowMovement = false;
            
        anim.Play("RedSeeker");
        gameObject.layer = LayerMask.NameToLayer("BossSeekerRed");

        yield return new WaitForSeconds(0.5f);

        chasePlayer = true;
    }
}
