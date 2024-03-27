using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public LineRenderer LineOfSight;
    public float MaxRayDistance;
    public LayerMask LayerDetection;
    public float rotationSpeed;

    private PolygonCollider2D polygonCollider;
    private BoxCollider2D[] segmentColliders;

    void Start()
    {
        Physics2D.queriesStartInColliders = false;

        // Check if LineOfSight is null and create it if needed
        if (LineOfSight == null)
        {
            LineOfSight = gameObject.AddComponent<LineRenderer>();
            // You might want to set other properties of the LineRenderer here (material, width, etc.)
        }

        // Set the LineRenderer's position count to 2
        LineOfSight.positionCount = 2;

        // Create and attach PolygonCollider2D
        polygonCollider = gameObject.AddComponent<PolygonCollider2D>();

        // Create an array to hold individual segment colliders
        segmentColliders = new BoxCollider2D[LineOfSight.positionCount - 1];

        // Add BoxCollider2D to each segment of the line renderer
        for (int i = 0; i < segmentColliders.Length; i++)
        {
            GameObject segmentColliderObject = new GameObject("SegmentCollider" + i);
            segmentColliderObject.transform.parent = transform;
            BoxCollider2D segmentCollider = segmentColliderObject.AddComponent<BoxCollider2D>();
            segmentColliders[i] = segmentCollider;
        }

        UpdateCollider();
    }

    void Update()
    {
        transform.Rotate(rotationSpeed * Vector3.forward * Time.deltaTime);

        LineOfSight.SetPosition(0, transform.position);

        RaycastHit2D hitinfo = Physics2D.Raycast(transform.position, transform.right, MaxRayDistance, LayerDetection);
        bool isWall = false;
        Vector2 wallHitPoint = Vector2.zero;
        Vector2 wallHitNormal = Vector2.zero;

        if (hitinfo.collider != null)
        {
            LineOfSight.SetPosition(LineOfSight.positionCount - 1, hitinfo.point);
            isWall = false;
            if (hitinfo.collider.CompareTag("Wall"))
            {
                wallHitPoint = (Vector2)hitinfo.point;
                wallHitNormal = (Vector2)hitinfo.normal;
                hitinfo = Physics2D.Raycast((Vector2)hitinfo.point, Vector2.Reflect(hitinfo.point, hitinfo.normal), MaxRayDistance, LayerDetection);
                isWall = true;
            }
        }
        else
        {
            if (isWall)
            {
                LineOfSight.SetPosition(LineOfSight.positionCount - 1, wallHitPoint + Vector2.Reflect(wallHitPoint, wallHitNormal) * MaxRayDistance);
            }
            else
            {
                LineOfSight.SetPosition(LineOfSight.positionCount - 1, transform.position + transform.right * MaxRayDistance);
            }
        }

        // Update colliders to match the LineRenderer's shape
        UpdateCollider();
    }

    void UpdateCollider()
    {
        // Ensure segmentColliders is not null
        if (segmentColliders == null)
        {
            return;
        }

        // Iterate through each segment of the LineRenderer and update colliders
        for (int i = 0; i < segmentColliders.Length; i++)
        {
            // Check if LineRenderer's position count is sufficient
            if (i >= LineOfSight.positionCount - 1)
            {
                break;
            }

            Vector3 segmentStart = LineOfSight.GetPosition(i);
            Vector3 segmentEnd = LineOfSight.GetPosition(i + 1);

            // Calculate midpoint and distance for the collider size
            Vector3 midpoint = (segmentStart + segmentEnd) / 2f;
            float distance = Vector3.Distance(segmentStart, segmentEnd);
            
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, segmentEnd - segmentStart);

            // Update collider position, rotation, and scale
            segmentColliders[i].transform.position = midpoint;
            segmentColliders[i].size = new Vector2(distance, 0.1f); // Adjust the height as needed
            segmentColliders[i].size = new Vector2(distance, 0.1f); // Adjust the height as needed
            // Calculate the local position of the collider relative to the segment
            Vector3 localColliderPos = new Vector3(0, 0f, 0f);

            // Transform the local position to world space
            Vector3 worldColliderPos = segmentColliders[i].transform.TransformPoint(localColliderPos);

            // Update the collider's position
            segmentColliders[i].transform.position = worldColliderPos;
            segmentColliders[i].isTrigger = true;
            segmentColliders[i].gameObject.tag = "Laser";
        }

        // Update PolygonCollider2D with LineRenderer's points
        if (polygonCollider != null && LineOfSight.positionCount >= 2)
        {
            Vector2[] colliderPoints = new Vector2[LineOfSight.positionCount];
            for (int i = 0; i < LineOfSight.positionCount; i++)
            {
                colliderPoints[i] = LineOfSight.GetPosition(i);
            }
            polygonCollider.points = colliderPoints;
        }
    }



}
