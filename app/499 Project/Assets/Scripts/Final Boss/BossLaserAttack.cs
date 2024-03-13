using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaserAttack : MonoBehaviour
{
    public Animator animator;
    public GameObject laser;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    public void OnCollisionEnter2D(Collision2D collision)
    {
        ShootLaser();
    }

    void ShootLaser()
    {
        animator.SetTrigger("ChargeLeft");
        Vector3 laserPos = new Vector3(transform.position.x - 10.0f, transform.position.y, transform.position.z);
        GameObject clone = Instantiate(laser, laserPos, transform.rotation);
        Destroy(clone, 1.0f);
    }
}
