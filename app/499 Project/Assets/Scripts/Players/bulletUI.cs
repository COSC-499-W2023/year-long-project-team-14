using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletUI : MonoBehaviour
{
    public GameObject[] bullets;
    private int charge;

    private void Start()
    {
        charge = bullets.Length;
    }

    public void oneLessShot()
    {
        if (charge >= 1)
        {
            charge--;
            bullets[charge].SetActive(false);
        }
    }

    public void setCharge(int attackCharge)
    {
        charge = attackCharge;

        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i].SetActive(i < charge);
        }
    }
}
