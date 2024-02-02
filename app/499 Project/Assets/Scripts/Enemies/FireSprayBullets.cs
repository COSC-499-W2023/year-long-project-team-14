using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSprayBullets : MonoBehaviour
{
    [SerializeField]
    private float rateOfFire = 10f;

    [SerializeField]
    private float waitTime = 1f; // pause delay between bursts

    int totalBurstsToFire = 5; // Set the number of bursts you want to fire
    
    [SerializeField]
    private int bulletsAmount = 10;

    [SerializeField]
    private float startAngle = 0, endAngle = 360f;
    private Vector2 bulletMoveDirection;

    public bool firingEnabled = true;

    // Start is called before the first frame update
     void Start()
    {
        InvokeRepeating("Fire", 0f, 1f / rateOfFire);
    }

    private void Fire()
    {
        
        float angleStep = (endAngle - startAngle) / bulletsAmount;
        float angle = startAngle;

        for (int i = 0; i < bulletsAmount + 1; i++)
        {
            GameObject bul = BulletSprayPool.Instance.GetBullet();

            float bulDirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
            float bulDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

            Vector3 bulMoveVector = new Vector3(bulDirX, bulDirY, 0f);
            Vector2 bulDir = (bulMoveVector - transform.position).normalized; 
            
            bul.transform.position = transform.position;
            bul.transform.rotation = transform.rotation;
            bul.SetActive(true);
            bul.GetComponent<SprayBullet>().SetMoveDirection(bulDir);

            angle += angleStep;
        }
    }
}
