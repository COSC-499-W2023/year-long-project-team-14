using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SeekingOrb : MonoBehaviour
{
    public Vector2 targetPosition;

    public float movementSpeed = 4;
    public float nextWaypointDistance = 1;
    
    Path path;
    int currentWaypoint = 0;

    Seeker seeker; 
    Rigidbody2D rb;

    public GameObject target;
    public GameObject[] enemies;

    public GameObject explosionPrefab;

    void Start()
    {
        //Get components
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        //Get array of enemies and find closest one
        StartCoroutine(FindRandomEnemy());

        //Start pathfinding
        InvokeRepeating("UpdatePath", 0f, 0.1f);
    }

    void Update()
    {
        //Update target position
        if(target != null)
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

    //If orb collides with enemy... KABOOM!!!
    void OnCollisionEnter2D(Collision2D collision)
    {   
        if(collision.gameObject.CompareTag("Enemy"))
        {     
            EnemyHealthSystem enemyHealthSystem = collision.gameObject.GetComponent<EnemyHealthSystem>();
            if(enemyHealthSystem != null)
                enemyHealthSystem.takeDamage(2);
            else
            {
                MiniBossHealthSystem miniBossHealthSystem = collision.gameObject.GetComponent<MiniBossHealthSystem>();
                if(miniBossHealthSystem != null)
                    miniBossHealthSystem.takeDamage(2);
            }
            Explode();
        }
    }

    //Find random enemy and sets them as the target
    public IEnumerator FindRandomEnemy() 
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        List<GameObject> aliveEnemies = new List<GameObject>();

        for(int i = 0; i < enemies.Length; i++)
        {
            EnemyHealthSystem enemyHealthSystem = enemies[i].GetComponent<EnemyHealthSystem>();
            if(enemyHealthSystem != null)
            {
                if(enemyHealthSystem.enemyHealth > 0)
                    aliveEnemies.Add(enemies[i]);
            }
            else
            {
                MiniBossHealthSystem miniBossHealthSystem = enemies[i].GetComponent<MiniBossHealthSystem>();
                if(miniBossHealthSystem != null)
                {
                    if(miniBossHealthSystem.enemyHealth > 0)
                        aliveEnemies.Add(enemies[i]);
                }
            }
        }

        if(aliveEnemies.Count > 0)
        {
            target = aliveEnemies[Random.Range(0, aliveEnemies.Count)];
        }
        
        yield return new WaitForSeconds(0.25f);
        if(target == null)
            Explode();

        
    }

    //Sets the pathfinding target position to the target enemies position
    public void SetTargetPosition() 
    {
        EnemyHealthSystem enemyHealthSystem = target.GetComponent<EnemyHealthSystem>();
        if(enemyHealthSystem != null)
        {
            if(enemyHealthSystem.enemyHealth > 0)
            {
                targetPosition.x = target.transform.position.x;
                targetPosition.y = target.transform.position.y;
            }
            else if(target != null) Explode();
        }
        else
        {
            MiniBossHealthSystem miniBossHealthSystem = target.GetComponent<MiniBossHealthSystem>();
            if(miniBossHealthSystem != null)
            {
                if(miniBossHealthSystem.enemyHealth > 0)
                {
                    targetPosition.x = target.transform.position.x;
                    targetPosition.y = target.transform.position.y;
                }
                else if(target != null) Explode();
            }
        }
    }

    public void Explode()
    {
        GameObject dashSmoke2 = Instantiate(explosionPrefab, transform.position, transform.rotation);
        Destroy(dashSmoke2, 0.55f);
        Destroy(gameObject);
    }
}
