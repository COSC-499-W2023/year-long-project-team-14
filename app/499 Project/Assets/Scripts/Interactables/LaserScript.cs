using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    public GameObject laserStart;
    public GameObject laserMiddle;
    public GameObject laserEnd;

    private GameObject start;
    private GameObject middle;
    private GameObject end;

    public float rotationSpeed;

    // Update is called once per frame
    void Update()
    {
        // Create the laser start from the prefab
        if (start == null)
        {
            start = Instantiate(laserStart, transform);
            start.transform.localPosition = Vector2.zero;
        }

        // Laser middle
        if (middle == null)
        {
            middle = Instantiate(laserMiddle, transform);
            middle.transform.localPosition = Vector2.zero;
        }

        // Define max laser size
        float maxLaserSize = 30f;

        // Raycast to detect hits
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, maxLaserSize);

        if (hit.collider != null)
        {
            // Adjust current laser size
            float currentLaserSize = Vector2.Distance(hit.point, transform.position);

            // Create the end sprite if not created
            if (end == null)
            {
                end = Instantiate(laserEnd, transform);
            }

            // Set the position and scale of the middle sprite
            middle.transform.localPosition = new Vector2(currentLaserSize / 2f, 0f);
            middle.transform.localScale = new Vector3(currentLaserSize, 1f, 1f);

            // Set the position of the end sprite
            end.transform.localPosition = new Vector2(currentLaserSize, 0f);

            // Check if the object hit has the "Player" tag and apply damage
            if (hit.collider.CompareTag("Player"))
            {
                healthSystem playerHealth = hit.collider.GetComponent<healthSystem>();
                if (playerHealth != null)
                {
                    playerHealth.takeDamage();
                }
            }
        }
        else
        {
            // If no hit, set laser to maximum size
            middle.transform.localScale = new Vector3(maxLaserSize, 1f, 1f);
            middle.transform.localPosition = new Vector2(maxLaserSize / 2f, 0f);

            if (end != null)
            {
                end.transform.localPosition = new Vector2(maxLaserSize, 0f);
            }
        }
        transform.Rotate(rotationSpeed * Vector3.forward * Time.deltaTime);
    }
}
