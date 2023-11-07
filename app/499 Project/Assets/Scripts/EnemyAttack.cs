using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Transform player;
    public GameObject bulletPrefab;
    public float shootInterval = 2.0f;
    public float bulletSpeed = 10.0f;

    private float lastShootTime;

    void Update()
    {
        // Check if it's time to shoot again
        if (Time.time - lastShootTime >= shootInterval)
        {
            // Perform raycast from enemy to player
            RaycastHit2D hit = Physics2D.Raycast(transform.position, (player.position - transform.position).normalized);

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                // The ray hit the player, so we can shoot
                Shoot();
            }
        }
    }

    void Shoot()
    {
        // Instantiate a bullet and set its direction towards the player
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Vector2 direction = (player.position - transform.position).normalized;
        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

        lastShootTime = Time.time;
    }
}
