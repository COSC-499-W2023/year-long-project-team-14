using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laser : MonoBehaviour
{

    public LineRenderer LineOfSight;

    public int reflections;
    public float MaxRayDistance;
    public LayerMask LayerDetection;
    public float rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        Physics2D.queriesStartInColliders = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationSpeed * Vector3.forward * Time.deltaTime);

        LineOfSight.positionCount = 1;
        LineOfSight.SetPosition(0, transform.position);

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right, MaxRayDistance, LayerDetection);
        // Ray
        Ray2D ray = new Ray2D(transform.position, transform.right);

        bool isMirror = false;
        Vector2 mirrorHitPoint = Vector2.zero;
        Vector2 mirrorHitNormal = Vector2.zero;


        for (int i = 0; i < reflections; i++)
        {
            LineOfSight.positionCount += 1;

            if (hitInfo.collider != null)
            {
                LineOfSight.SetPosition(LineOfSight.positionCount - 1, hitInfo.point - ray.direction * -0.1f);

                isMirror = false;
                if (hitInfo.collider.CompareTag("Wall"))
                {
                    mirrorHitPoint = (Vector2)hitInfo.point;
                    mirrorHitNormal = (Vector2)hitInfo.normal;
                    hitInfo = Physics2D.Raycast((Vector2)hitInfo.point - ray.direction * -0.1f, Vector2.Reflect(hitInfo.point - ray.direction * -0.1f, hitInfo.normal), MaxRayDistance, LayerDetection);
                    isMirror = true;
                }
                else
                    break;
            }
            else
            {
                if (isMirror)
                {
                    LineOfSight.SetPosition(LineOfSight.positionCount - 1, mirrorHitPoint + Vector2.Reflect(mirrorHitPoint, mirrorHitNormal) * MaxRayDistance);
                    break;
                }
                else
                {
                    LineOfSight.SetPosition(LineOfSight.positionCount - 1, transform.position + transform.right * MaxRayDistance);
                    break;
                }
            }
        }

    }
}