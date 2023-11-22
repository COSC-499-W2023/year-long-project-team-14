using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthSystem : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject[] hearts;
    private Animator animator;
    private CircleCollider2D cc;
    private int life;
    public bool dead;

    private void Start()
    {
        life = hearts.Length;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();
        
    }

    void Update()
    {
    
    }

    public void takeDamage()
    {
        if (life >=1)
        {
            life --;
            hearts[life].SetActive(false);
            if (life < 1)
            {
                Die();
            }
        }
    }

    void Die()
    {
        rb.bodyType = RigidbodyType2D.Static;
        animator.SetTrigger("Death");
        cc.enabled = false;
    }
}
