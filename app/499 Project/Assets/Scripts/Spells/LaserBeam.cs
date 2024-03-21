using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    public int maxReflections = 5;
    public LayerMask obstacleLayer;
    public GameObject laserSegmentPrefab;
    public float animationSpeed = 5f; // Speed of the laser beam animation

    private void Start()
    {
        DrawLaser(transform.position, transform.right, 0);
    }

    private void DrawLaser(Vector2 origin, Vector2 direction, int reflections)
    {
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, Mathf.Infinity, obstacleLayer);

        Vector2 endPoint = hit ? hit.point : origin + direction * 100f; // If no hit, extend laser beam to a distant point
        float segmentLength = Vector2.Distance(origin, endPoint);

        // Instantiate laser segment at the origin
        GameObject laserSegment = Instantiate(laserSegmentPrefab, origin, Quaternion.identity);

        // Calculate rotation of the laser segment
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        laserSegment.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Set laser segment scale to represent its length
        laserSegment.transform.localScale = new Vector2(segmentLength, 1f);

        // Start coroutine to animate the laser segment
        StartCoroutine(AnimateLaserSegment(laserSegment, segmentLength));

        if (hit && reflections < maxReflections)
        {
            Vector2 reflectDirection = Vector2.Reflect(direction, hit.normal);
            DrawLaser(hit.point + reflectDirection * 0.01f, reflectDirection, reflections + 1); // Offset origin slightly to prevent hitting the same point again
        }
    }

    private System.Collections.IEnumerator AnimateLaserSegment(GameObject laserSegment, float segmentLength)
    {
        float elapsedTime = 0f;
        Vector2 initialScale = new Vector2(0f, 1f);
        Vector2 targetScale = new Vector2(segmentLength, 1f);

        while (elapsedTime < segmentLength / animationSpeed)
        {
            float scale = Mathf.Lerp(0f, segmentLength, elapsedTime / (segmentLength / animationSpeed));
            laserSegment.transform.localScale = new Vector2(scale, 1f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        laserSegment.transform.localScale = targetScale; // Ensure the scale is set to full length
    }
}
