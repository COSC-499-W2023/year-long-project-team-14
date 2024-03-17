using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public GameObject[] players;
    public GameObject targetPlayer;
    public GameObject bulletPrefab;
    public float shootInterval = 2.0f;
    public float bulletSpeed = 10.0f;
    public float lastShootTime;

    public List<Vector3> Points1;
    public List<Vector3> Points2;
    public List<Vector3> Points3;
    Vector2 direction1 = Vector2.zero;
    Vector2 direction2 = Vector2.zero;
    Vector2 direction3 = Vector2.zero;
    public LineRenderer lineRenderer1;
    public LineRenderer lineRenderer2;
    public LineRenderer lineRenderer3;
    public LayerMask LayerDetection;
    float radius = 0.2f;
    int reflections1 = 0;
    int reflections2 = 0;
    public int maxReflections = 1;
    public float rotationSpeed;

    [SerializeField] private AudioSource shootSound;

    int diff = 1;

    void Start()
    {
        //Create list of points for each line renderer
        Points1 = new List<Vector3>();
        Points2 = new List<Vector3>();
        Points3 = new List<Vector3>();

        //Get difficulty
        diff = PlayerPrefs.GetInt("difficulty");

        //Set enemy fire rate
        if(diff == 1) 
            shootInterval /= 1f;
        else if(diff == 2)
            shootInterval /= 1.5f;
        else if(diff == 3)
            shootInterval /= 2f;
        else if(diff == 4)
            shootInterval /= 2.5f;
        
        //Set enemy bullet speed
        if(diff == 1) 
            bulletSpeed *= 1f;
        else if(diff == 2)
            bulletSpeed *= 1.5f;
        else if(diff == 3)
            bulletSpeed *= 2f;
        else if(diff == 4)
            bulletSpeed *= 2.5f;

        //Prevent enemies from shooting at the start of a level
        lastShootTime = Time.time + Random.Range(0, shootInterval/2);
    }

    void Update()
    {
        //Get players
        players = GameObject.FindGameObjectsWithTag("Player");

        if(players.Length > 0)
        {
            //Find closest player and aim at them
            FindClosestPlayer();
            AimAtPlayer(lineRenderer3);

            //Once enemy can shoot, aim other 2 line renderers at player
            if(Time.time - lastShootTime >= shootInterval - 0.1f && Time.time - lastShootTime < shootInterval)
            {
                AimAtPlayer(lineRenderer1);
                AimAtPlayer(lineRenderer2);
            }

            //Rotate 2 of the line renderers in oposite directions to check every possible angle
            if(Time.time - lastShootTime >= shootInterval)
            {
                lineRenderer1.transform.Rotate(rotationSpeed * Vector3.forward * Time.deltaTime);
                lineRenderer2.transform.Rotate(-rotationSpeed * Vector3.forward * Time.deltaTime);
            }

            //Get the direction of each line renderer
            direction1 = lineRenderer1.transform.right;
            direction2 = lineRenderer2.transform.right;
            direction3 = lineRenderer3.transform.right;

            //Shoot a raycast on each line renderer
            RaycastHit2D hitData1 = Physics2D.CircleCast(lineRenderer1.transform.position, radius, direction1, 100, LayerDetection);
            RaycastHit2D hitData2 = Physics2D.CircleCast(lineRenderer2.transform.position, radius, direction2, 100, LayerDetection);
            RaycastHit2D hitData3 = Physics2D.CircleCast(lineRenderer3.transform.position, radius, direction3, 100, LayerDetection);

            //Reset reflections
            reflections1 = 0;
            reflections2 = 0;

            //Reset points
            Points1.Clear();
            Points2.Clear();
            Points3.Clear();

            //Add line renderer positions to points
            Points1.Add(lineRenderer1.transform.position);
            Points2.Add(lineRenderer2.transform.position);
            Points3.Add(lineRenderer3.transform.position);

            //Check raycast collisions
            CheckCollision(direction1, hitData1, Points1, reflections1, lineRenderer1);
            CheckCollision(direction2, hitData2, Points2, reflections2, lineRenderer2);
            CheckCollision(direction3, hitData3, Points3, maxReflections, lineRenderer3);

            //Set line renderer position count to be point count
            lineRenderer1.positionCount = Points1.Count;
            lineRenderer2.positionCount = Points2.Count;
            lineRenderer3.positionCount = Points3.Count;

            //Draw the line for each line renderer 
            lineRenderer1.SetPositions(Points1.ToArray());
            lineRenderer2.SetPositions(Points2.ToArray());
            lineRenderer3.SetPositions(Points3.ToArray());
        }     
    }

    public void Shoot(LineRenderer lr) //Shoot a bullet in the direction of the line renderer
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Vector2 direction = lr.transform.right;
        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
        bullet.GetComponent<EnemyBullet>().bounces = maxReflections;
        lastShootTime = Time.time + Random.Range(-1f / diff, 1f / diff);;
        //play shoot sound
        shootSound.Play();
    }

    void CheckCollision(Vector2 direction, RaycastHit2D hitData, List<Vector3> Points, int reflections, LineRenderer lr)
    {
        if(hitData.collider != null)
        {
            if(hitData.collider.CompareTag("Wall")) //If raycast hits wall, reflect and continue casting
            {
                if(reflections < maxReflections)
                    Reflect(direction, hitData, Points, reflections, lr);
                else
                    Points.Add(hitData.centroid);
            }
            else if(hitData.collider.CompareTag("Player")) //If raycast hits player, shoot in that direction
            {
                Points.Add(hitData.centroid);

                if(Time.time - lastShootTime >= shootInterval)
                    Shoot(lr);
            }
        }
    }

    void Reflect(Vector2 inDirection, RaycastHit2D hitData, List<Vector3> Points, int reflections, LineRenderer lr) //Reflect raycast and line renderer off of a surface
    {
        Points.Add(hitData.centroid);
        reflections++;
        Vector2 newDirection = Vector2.Reflect(inDirection, hitData.normal);
        RaycastHit2D newHitData = Physics2D.CircleCast(hitData.centroid, radius, newDirection, 100, LayerDetection);
        CheckCollision(newDirection, newHitData, Points, reflections, lr);
        
    }

    void FindClosestPlayer() //Sets target to be the player closest to the enemy
    {
        if(players.Length > 1)
        {
            float distance1 = Vector3.Distance(gameObject.transform.position, players[0].transform.position);
            float distance2 = Vector3.Distance(gameObject.transform.position, players[1].transform.position);
            if(distance2 < distance1)
                targetPlayer = players[1];
            else
                targetPlayer = players[0];
        }
        else
            targetPlayer = players[0];
    }

    void AimAtPlayer(LineRenderer lr) //Aims line renderer directly at player
    {
        Vector3 aimDirection = targetPlayer.transform.position - lr.transform.position;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        lr.transform.rotation = Quaternion.Slerp(lr.transform.rotation, rotation, 1000 * Time.deltaTime);
    }
}
