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
    [SerializeField]
    private GameObject bossBullet;
    private List<GameObject> sprayBullets;
    private List<GameObject> bossBullets;

    private void Awake()
    {
        Instance = this;
        InitializeBulletPool();
    }

    private void InitializeBulletPool()
    {   
        sprayBullets = new List<GameObject>();
        bossBullets = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(pooledBullet);
            bullet.SetActive(false);
            sprayBullets.Add(bullet);

            GameObject bullet2 = Instantiate(bossBullet);
            bullet2.SetActive(false);
            bossBullets.Add(bullet2);
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

    public GameObject GetBossBullet()
    {
        // Search for an inactive bullet in the pool
        for (int i = 0; i < bossBullets.Count; i++)
        {
            if (!bossBullets[i].activeInHierarchy)
            {
                return bossBullets[i];
            }
        }

        // If no inactive bullet is found, create a new one
        GameObject newBullet = Instantiate(bossBullet);
        newBullet.SetActive(false);
        bossBullets.Add(newBullet);

        return newBullet;
    }
}
