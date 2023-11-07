using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public GameObject[] players;
    public GameObject targetPlayer;

    public List<Vector3> Points;
    Vector2 direction = Vector2.zero;
    public LineRenderer lineRenderer;
    public LayerMask LayerDetection;
    float radius = 0.18f;
    

    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        Points = new List<Vector3>();
    }

    void Update()
    {
        if(players.Length > 0)
        {
            FindClosestPlayer();
            AimAtPlayer();

            direction = lineRenderer.transform.right;
            RaycastHit2D hitData = Physics2D.CircleCast(lineRenderer.transform.position + (Vector3)direction, radius, direction, 100, LayerDetection);
            
            Points.Clear();
            Points.Add(lineRenderer.transform.position);

            if(hitData.collider != null)
            {
                Points.Add(hitData.centroid);

                if(hitData.collider.CompareTag("Player"))
                {
                    //shoot at player
                }
            }

            lineRenderer.positionCount = Points.Count;
            lineRenderer.SetPositions(Points.ToArray());
        }
    }

    public void FindClosestPlayer()
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
    public void AimAtPlayer()
    {
        Vector3 aimDirection = targetPlayer.transform.position - lineRenderer.transform.position;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        lineRenderer.transform.rotation = Quaternion.Slerp(lineRenderer.transform.rotation, rotation, 1000 * Time.deltaTime);
    }
}
