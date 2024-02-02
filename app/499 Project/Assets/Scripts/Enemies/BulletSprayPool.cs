using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSprayPool : MonoBehaviour
{
    public static BulletSprayPool Instance;
    [SerializeField]
    private int poolSize = 30;

    [SerializeField]
    private GameObject pooledBullet;
    private List<GameObject> sprayBullets;

    private void Awake()
    {
        Instance = this;
        InitializeBulletPool();
    }

    private void InitializeBulletPool()
    {   
        sprayBullets = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(pooledBullet);
            bullet.SetActive(false);
            sprayBullets.Add(bullet);
        }
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
    }

    public GameObject GetBullet()
    {
        // Search for an inactive bullet in the pool
        for (int i = 0; i < sprayBullets.Count; i++)
        {
            if (!sprayBullets[i].activeInHierarchy)
            {
                return sprayBullets[i];
            }
        }

        // If no inactive bullet is found, create a new one
        GameObject newBullet = Instantiate(pooledBullet);
        newBullet.SetActive(false);
        sprayBullets.Add(newBullet);

        return newBullet;
    }
}
