using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBehaviour : MonoBehaviour
{
    public Animator animator;
    private Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision involves a player GameObject.
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("SwingSword");
        }
    }
}
