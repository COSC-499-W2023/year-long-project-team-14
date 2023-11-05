using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthSystem : MonoBehaviour
{
    public GameObject[] hearts;
    private int life;
    private bool dead;

    private void Start()
    {
        life = hearts.Length;
    }
    // Update is called once per frame
    void Update()
    {
        if (dead == true)
        {
            //death
        }
    }

    public void takeDamage(int damage)
    {
        if (life >=1)
        {
            life -= damage;
            Destroy(hearts[life].gameObject);
            if (life < 1)
            {
                dead = true;
            }
        }
    }
}
