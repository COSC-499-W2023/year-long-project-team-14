using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSprayPool : MonoBehaviour
{
    public static BulletSprayPool bulletSprayPoolInstanse;

    [SerializeField]
    private GameObject pooledBullet;
    private List<GameObject> sprayBullets;

    private void Awake()
    {
        bulletSprayPoolInstanse = this;
    }

    void Start()
    {
        sprayBullets = new List<GameObject>();
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
    }

    public GameObject GetBullet()
    {
        GameObject bul = Instantiate(pooledBullet);
        bul.SetActive(false);
        sprayBullets.Add(bul);
        return bul;
    }
}
