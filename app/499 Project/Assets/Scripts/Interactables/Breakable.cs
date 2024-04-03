using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public Animator animator;
    public GameObject ladder;
    public bool unlocked = false;
    public bool slimes = false;
    public bool boss = false;
    public int brokenWalls = 0;
    public Breakable masterBreakable;
    public GameObject justin;
    public Collider2D justinsCollider;
    public FireSprayBullets justinsAttack;
    public GameObject[] breakables;
    public GameObject slimePrefab;

    public int health = 6;
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player_bullet") || collision.gameObject.CompareTag("GiantBullet"))
        {
            health--;

            if(health <= 0 && !unlocked)
            {
                //Unlock secret
                StartCoroutine(Unlock());
            }
        } 
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("FireballExplosion"))
        {
            health -= 3;

            if(health <= 0)
            {
                //Unlock secret
                StartCoroutine(Unlock());
            }
        } 
    }

    public IEnumerator Unlock()
    {
        unlocked = true;
        animator.Play("WallFadeOut");

        if(boss)
        {
            masterBreakable.brokenWalls++;

            if(masterBreakable.brokenWalls == 4 && masterBreakable == this)
            {
                justin.SetActive(true);
                yield return new WaitForSeconds(0.6f);
                
                for(int i = 0; i < breakables.Length; i++)
                    Destroy(breakables[i]);

                justinsCollider.enabled = true;
                justinsAttack.enabled = true;
            }
            else
                StartCoroutine(CheckWalls());
        }

        if(ladder != null)
            ladder.SetActive(true);

        if(slimes)
        {
            for(int i = 0; i < 10; i++)
                Instantiate(slimePrefab, new Vector3((0.5f * i) - 2.5f, -0.25f, 0), Quaternion.identity);
            for(int i = 0; i < 10; i++)
                Instantiate(slimePrefab, new Vector3((0.5f * i) - 2.5f, -0.75f, 0), Quaternion.identity);
        }

        yield return new WaitForSeconds(0.5f);

        //Destroy walls
        if(!boss)
            Destroy(gameObject);
    }

    public IEnumerator CheckWalls()
    {
        yield return new WaitForSeconds(2.5f);

        if(masterBreakable.brokenWalls < 4)
        {
            masterBreakable.brokenWalls--;
            health = 1;
            unlocked = false;
            animator.Play("WallFadeIn");
        }
    }
}
