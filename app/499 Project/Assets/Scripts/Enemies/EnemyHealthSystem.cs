using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour
{
    public Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    public EnemyAttack ea;
    public EnemyTripleShot et;
    public EnemyMovement enemyMovement; 
    public int enemyHealth = 3;

    public CircleCollider2D enemyCollider;
    public Ladder ladder;
    public Portal portal;

    [SerializeField] private AudioSource hitSound;
    [SerializeField] private AudioSource deathSound;

    private void Start()
    {
        // Add the enemy to the list of allEnemies when it's instantiated
        GameObject port = GameObject.FindWithTag("Portal");
        if (port != null)
        {   portal = port.GetComponent<Portal>();
            portal.allEnemies.Add(gameObject);
        }
        else
        {
            GameObject lad = GameObject.FindWithTag("Ladder");
            if(lad != null)
            {
                ladder = lad.GetComponent<Ladder>();
                ladder.allEnemies.Add(gameObject);
            }
        }

        //Get difficulty
        int diff = PlayerPrefs.GetInt("difficulty");

        //Set enemy health
        if(diff == 1) 
            enemyHealth = (int)Mathf.Round(enemyHealth * 1.5f);
        else if(diff == 2)
            enemyHealth = (int)Mathf.Round(enemyHealth * 2f);
        else if(diff == 3)
            enemyHealth = (int)Mathf.Round(enemyHealth * 2.5f);
        else if(diff == 4)
            enemyHealth = (int)Mathf.Round(enemyHealth * 3f);
        
        enemyCollider = GetComponent<CircleCollider2D>();
    }
    private void Awake() //Get components
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        if(GetComponent<EnemyAttack>() == true){
            ea = GetComponent<EnemyAttack>();
        }else if(GetComponent<EnemyTripleShot>() == true){
            et = GetComponent<EnemyTripleShot>();
        }
        enemyMovement = GetComponent<EnemyMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }
     // Damage enemy if colliding with bullet
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player_bullet"))
        {
            takeDamage();
        }
    }

    // Damage enemy if colliding with fireball
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("FireballExplosion"))
        {
            takeDamage(4);
        }
    }

    //Decrease health by 1 and kill enemy if health <= 0
    public void takeDamage()
    {
        if(enemyHealth > 0)
        {
            enemyHealth--;

            if (enemyHealth <= 0)
            {
                Die();
            }
            else
            {
                //play hit animation
                animator.SetTrigger("isHit");

                //play hit sound
                hitSound.Play();
            }
        }
    }

    //Decreases health and kill enemy if health <= 0
    public void takeDamage(int damage)
    {
        if(enemyHealth > 0)
        {
            enemyHealth -= damage;

            if (enemyHealth <= 0)
            {
                Die();
            }
            else
            {
                //play hit animation
                animator.SetTrigger("isHit");

                //play hit sound
                hitSound.Play();
            }
        }
    }
    
    //Kills enemy
    public void Die()
    {
        enemyHealth = -1;
        enemyMovement.enabled = false;
        if(ea != null){
            ea.enabled = false;
        }else if( et != null){
            et.enabled = false;
        }
        enemyCollider.enabled = false;

        animator.SetBool("IsWalking", false);
        
        animator.SetBool("IsDead", true);

        //play death sound
        deathSound.Play();

        StartCoroutine(Transparent());

        //If last enemy, end level
        if (portal != null)
        {
            portal.allEnemies.Remove(gameObject);

            if (portal.allEnemies.Count == 0)
            {
                EndLevel();
            }
        }
        else
        {
            if (ladder != null)
            {
                ladder.allEnemies.Remove(gameObject);

                if (ladder.allEnemies.Count == 0)
                {
                    EndLevel();
                }
            }
        }
    }

    IEnumerator Transparent()
    {
        yield return new WaitForSeconds(0.5f);
        Color currentColor = spriteRenderer.color; // Set the transparency (alpha) value
        currentColor.a = 0.2f; // Adjust the alpha value as needed
        spriteRenderer.color = currentColor;
    }

    void EndLevel()
    {
        if (portal != null)
        {
            portal.SetPortalActive(true);
        }
        else
        {
            if (ladder != null)
            {
                ladder.SetLadderActive(true);
            }
        }
    }
}
