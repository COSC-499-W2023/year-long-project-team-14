using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthSystem : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    public GameObject[] hearts;
    public Animator animator;
    private CircleCollider2D cc;
    public int life;
    public int maxLife = 3;
    public bool dead;
    public GameOverMenu gameOverMenu;


    private void Start()
    {
        life = maxLife;
        GameObject canvas = GameObject.FindWithTag("Canvas");
        if(canvas != null)
            gameOverMenu = canvas.GetComponent<GameOverMenu>();
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        StartCoroutine(Transparent());
        cc.enabled = false;
        
        PlayerController p = GetComponent<PlayerController>();
        GameObject bullet = transform.Find("Center").gameObject;
        if (p!= null)
        {
            Destroy(p);
            Destroy(bullet);
        }
        dead = true;
        spriteRenderer.sortingOrder = 8;

        if(gameOverMenu != null)
            gameOverMenu.playercount--;
    }

    IEnumerator Transparent()
    {
        yield return new WaitForSeconds(0.5f);
        Color currentColor = spriteRenderer.color; // Set the transparency (alpha) value
        currentColor.a = 0.5f; // Adjust the alpha value as needed
        spriteRenderer.color = currentColor;
    }

    public void SetHeartsActive()
    {
        for(int i = 0; i < life; i++)
        hearts[i].SetActive(true);
    }
}
