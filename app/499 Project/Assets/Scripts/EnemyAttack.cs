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
    private float lastShootTime;

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
    
    void Start()
    {
        Points1 = new List<Vector3>();
        Points2 = new List<Vector3>();
        Points3 = new List<Vector3>();
    }

    void Update()
    {
        if(players.Length > 0)
        {
            FindClosestPlayer();
            AimAtPlayer(lineRenderer3);

            if(Time.time - lastShootTime >= shootInterval - 0.1f && Time.time - lastShootTime < shootInterval)
            {
                AimAtPlayer(lineRenderer1);
                AimAtPlayer(lineRenderer2);
            }

            if(Time.time - lastShootTime >= shootInterval)
            {
                lineRenderer1.transform.Rotate(rotationSpeed * Vector3.forward * Time.deltaTime);
                lineRenderer2.transform.Rotate(-rotationSpeed * Vector3.forward * Time.deltaTime);
            }

            direction1 = lineRenderer1.transform.right;
            direction2 = lineRenderer2.transform.right;
            direction3 = lineRenderer3.transform.right;

            RaycastHit2D hitData1 = Physics2D.CircleCast(lineRenderer1.transform.position + (Vector3)direction1, radius, direction1, 100, LayerDetection);
            RaycastHit2D hitData2 = Physics2D.CircleCast(lineRenderer2.transform.position + (Vector3)direction2, radius, direction2, 100, LayerDetection);
            RaycastHit2D hitData3 = Physics2D.CircleCast(lineRenderer3.transform.position + (Vector3)direction3, radius, direction3, 100, LayerDetection);

            reflections1 = 0;
            reflections2 = 0;

            Points1.Clear();
            Points2.Clear();
            Points3.Clear();

            Points1.Add(lineRenderer1.transform.position);
            Points2.Add(lineRenderer2.transform.position);
            Points3.Add(lineRenderer3.transform.position);

            CheckCollision(direction1, hitData1, Points1, reflections1, lineRenderer1);
            CheckCollision(direction2, hitData2, Points2, reflections2, lineRenderer2);
            CheckCollision(direction3, hitData3, Points3, maxReflections, lineRenderer3);

            lineRenderer1.positionCount = Points1.Count;
            lineRenderer2.positionCount = Points2.Count;
            lineRenderer3.positionCount = Points3.Count;

            lineRenderer1.SetPositions(Points1.ToArray());
            lineRenderer2.SetPositions(Points2.ToArray());
            lineRenderer3.SetPositions(Points3.ToArray());
        }
        else
            players = GameObject.FindGameObjectsWithTag("Player");
    }

    void Shoot(LineRenderer lr)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Vector2 direction = lr.transform.right;
        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
        bullet.GetComponent<EnemyBullet>().bounces = maxReflections;
        lastShootTime = Time.time;
    }

    void CheckCollision(Vector2 direction, RaycastHit2D hitData, List<Vector3> Points, int reflections, LineRenderer lr)
    {
        if(hitData.collider != null)
        {
            if(hitData.collider.CompareTag("Wall"))
            {
                if(reflections < maxReflections)
                    Reflect(direction, hitData, Points, reflections, lr);
                else
                    Points.Add(hitData.centroid);
            }
            else if(hitData.collider.CompareTag("Player"))
            {
                Points.Add(hitData.centroid);

                if(Time.time - lastShootTime >= shootInterval)
                    Shoot(lr);
            }
        }
    }

    void Reflect(Vector2 inDirection, RaycastHit2D hitData, List<Vector3> Points, int reflections, LineRenderer lr)
    {
        Points.Add(hitData.centroid);
        reflections++;
        Vector2 newDirection = Vector2.Reflect(inDirection, hitData.normal);
        RaycastHit2D newHitData = Physics2D.CircleCast(hitData.centroid, radius, newDirection, 100, LayerDetection);
        CheckCollision(newDirection, newHitData, Points, reflections, lr);
        
    }

    void FindClosestPlayer()
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

    void AimAtPlayer(LineRenderer lr)
    {
        Vector3 aimDirection = targetPlayer.transform.position - lr.transform.position;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        lr.transform.rotation = Quaternion.Slerp(lr.transform.rotation, rotation, 1000 * Time.deltaTime);
    }
}
