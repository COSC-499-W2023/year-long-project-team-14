using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    //public Transform player;
    public Transform[] potentialTargets;
    public GameObject bulletPrefab;
    public float shootInterval = 2.0f;
    public float bulletSpeed = 10.0f;

    private float lastShootTime;

    void Update()
    {
        // Check if it's time to shoot again
        if (Time.time - lastShootTime >= shootInterval)
        {
            // Loop through potential targets
            foreach (Transform target in potentialTargets)
            {
                // Perform raycast from enemy to target
                RaycastHit2D hit = Physics2D.Raycast(transform.position, (target.position - transform.position).normalized);

                if (hit.collider != null && hit.collider.CompareTag("Player"))
                {
                    // The ray hit a player, so we can shoot
                    Shoot(target);
                    break;  // Exit the loop after shooting to avoid shooting at multiple targets.
                }
            }
        }
    }

    void Shoot(Transform target)
    {
        // Instantiate a bullet and set its direction towards the target
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Vector2 direction = (target.position - transform.position).normalized;
        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

        lastShootTime = Time.time;
    }
}
