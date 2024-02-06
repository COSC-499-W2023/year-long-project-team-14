using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSprayBullets : MonoBehaviour
{
    [SerializeField]
    private float rateOfFire = 10f; // Set the rate of fire

    [SerializeField]
    private float waitTime = 3f; // Sets pause delay between bursts

    [SerializeField] 
    int totalShotsPerBurst = 2; // Sets the number of bursts you want to fire
    
    [SerializeField]
    private int bulletsAmount = 10; // Sets the total number of bullets in the spread

    [SerializeField]
    private float startAngle = 0, endAngle = 360f; // Sets the start and end angle of the burst
    private Vector2 bulletMoveDirection;

    public bool firingEnabled = true;

    // Start is called before the first frame update
     void Start()
    {
        StartCoroutine(FireBursts());
        //InvokeRepeating("Fire", 0f, 1f / rateOfFire);
    }
    private IEnumerator FireBursts() // Continuous bursts mode
    {
        yield return new WaitForSeconds(1f); // 1 second delay before first shots fired

        while(firingEnabled)
        {
            for (int burstCount = 0; burstCount < totalShotsPerBurst; burstCount++)
            {
                Fire(); // Fire a burst

                yield return new WaitForSeconds(1f / rateOfFire); // rate of fire between shots of a single burst
            }

             yield return new WaitForSeconds(waitTime); // Wait for the specified time, the break time between burst of 3
        }
    }

    private IEnumerator FireSingleBurst() 
    {
        if(firingEnabled)
        {
            for (int burstCount = 0; burstCount < totalShotsPerBurst; burstCount++)
            {
                Fire(); // Fire a burst

                yield return new WaitForSeconds(1f / rateOfFire); // rate of fire between shots of a single burst
            }
        }
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
