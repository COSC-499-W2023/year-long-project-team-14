using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthSystem : MonoBehaviour
{
    public GameObject[] hearts;
    private int life;
    public bool dead;

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

    public void takeDamage()
    {
        if (life >=1)
        {
            life --;
            hearts[life].SetActive(false);
            if (life < 1)
            {
                dead = true;
            }
        }
    }
}
