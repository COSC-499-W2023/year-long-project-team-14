using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    [SerializeField] private float attackCharge = 0;
    [SerializeField] private float attackChargeSpeed = 0.5f;
    [SerializeField] private float attackChargeMax = 3;
    [SerializeField] private float attackCost = 1;
    [SerializeField] private float bulletForce = 250;

    public GameObject bulletPrefab;


    void Start()
    {
        
    }

    void Update()
    {
        if(attackCharge < attackChargeMax)
        {
            attackCharge += Time.deltaTime * attackChargeSpeed;
        }

        if((Input.GetMouseButtonDown(0) || Input.GetButtonDown("joystick button 14")) && attackCharge >= attackCost)
        {
            attackCharge -= attackCost;
            Shoot();
        }
    }

    public void Shoot()
    {
        //get the direction to launch the bullet in
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 direction = (Vector2)((worldMousePos - transform.position));
		direction.Normalize();

        //spawn the bullet and launch it forward
        GameObject bullet = Instantiate(bulletPrefab, transform.position + (Vector3)(direction * 0.75f), Quaternion.identity);
        Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
        bulletRB.AddForce(direction * bulletForce);
        Destroy(bullet, 30);
    }
}
