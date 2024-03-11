using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSprayBullets : MonoBehaviour
{    
    [SerializeField]
    private float rateOfFire = 1f; // Set the rate of fire for circular burst
    
    [SerializeField]
    private float spiralRateOfFire = 8f; // Set the rate of fire for spiral shooting

    [SerializeField]
    private float fireAtPlayerRateOfFire = 4f; // Set the rate of fire for firing at player

    [SerializeField]
    private float fireAtPlayerAmount = 40f; // Amount of bullets to shoot at the player

    [SerializeField]
    private float waitTime = 3f; // Sets pause delay between bursts

    [SerializeField] 
    int totalShotsPerBurst = 2; // Sets the number of bursts you want to fire
    
    [SerializeField]
    private float bulletsAmount = 10; // Sets the total number of bullets in the spread

    [SerializeField]
    private float spiralAngleIncrease = 10; // Sets the angle that the spiral attack increases by

    [SerializeField]
    private float startAngle = 0, endAngle = 360f; // Sets the start and end angle of the burst
    private Vector2 bulletMoveDirection;

    public bool firingEnabled = true;
    
    [SerializeField]
    float totalSpirals = 1f; 

    int diff = 1;

    // Start is called before the first frame update
     void Start()
    {
        StartCoroutine(AlternatingShooting());
        //StartCoroutine(FireBursts());
        //InvokeRepeating("Fire", 0f, 1f / rateOfFire);
        //InvokeRepeating("FireDoubleSpiral", 0f, 1f / spiralRateOfFire);
        //StartCoroutine(FireDoubleSpiral()); 

        //Get difficulty
        diff = PlayerPrefs.GetInt("difficulty");
        
        if(diff == 1) 
        {
            rateOfFire /= 1f;
            waitTime  /= 1f;
            bulletsAmount *= 1f;

            spiralRateOfFire /= 1f;
            spiralAngleIncrease /= 1f;
            totalSpirals *= 1f;

            fireAtPlayerRateOfFire /= 1f;
            fireAtPlayerAmount /= 1f;
        }
        else if(diff == 2)
        {
            rateOfFire /= 1.5f;
            waitTime /= 1.33f;
            bulletsAmount *= 1.33f;

            spiralRateOfFire /= 1.78f;
            spiralAngleIncrease /= 1.33f;
            totalSpirals *= 1.33f;

            fireAtPlayerRateOfFire /= 2f;
            fireAtPlayerAmount *= 2f;
        }
        else if(diff == 3)
        {
            rateOfFire /= 2f;
            waitTime /= 1.67f;
            bulletsAmount *= 1.67f;

            spiralRateOfFire /= 2.78f;
            spiralAngleIncrease /= 1.67f;
            totalSpirals *= 1.67f;

            fireAtPlayerRateOfFire /= 3f;
            fireAtPlayerAmount *= 3f;
        }
        else if(diff == 4)
        {
            rateOfFire /= 2.5f;
            waitTime /= 2f;
            bulletsAmount *= 2f;

            spiralRateOfFire /= 4f;
            spiralAngleIncrease /= 2f;
            totalSpirals *= 2f;

            fireAtPlayerRateOfFire /= 4f;
            fireAtPlayerAmount *= 4f;
        }

        totalShotsPerBurst = 1 + diff;
    }

    private IEnumerator AlternatingShooting()
    {
        while (firingEnabled == true) 
        {
            yield return new WaitForSeconds(3f / (((diff - 1) / 2) + 1)); // delay between attacks
            int rand = Random.Range(0, 3);
            if(rand == 0)
            {
                yield return StartCoroutine(FireSingleBurst());
                yield return new WaitForSeconds(1f); // delay between attacks
            }
            else if(rand == 1)
            {
                yield return StartCoroutine(FireDoubleSpiral());
                yield return new WaitForSeconds(1f); // delay between attacks
            }
            else if(rand == 2)
            {
                yield return StartCoroutine(FireAtPlayer());
                yield return new WaitForSeconds(1f); // delay between attacks
            }
        }
    }

    private IEnumerator FireBursts() // Continuous bursts mode
    {
        yield return new WaitForSeconds(1f); // 1 second delay before first shots fired

        while(firingEnabled)
        {
            for (int burstCount = 0; burstCount < totalShotsPerBurst; burstCount++)
            {
                Fire(); // Fire a burst

                yield return new WaitForSeconds(rateOfFire); // rate of fire between shots of a single burst
            }

             yield return new WaitForSeconds(waitTime); // Wait for the specified time, the break time between burst of 3
        }
    }

    private IEnumerator FireSingleBurst() 
    {
        for (int burstCount = 0; burstCount < totalShotsPerBurst; burstCount++)
        {
            Fire(); // Fire a burst

            yield return new WaitForSeconds(rateOfFire); // rate of fire between shots of a single burst
        }
    }

    private void Fire()
    {
        
        float angleStep = (endAngle - startAngle) / bulletsAmount;
        float angle = startAngle;

        if(firingEnabled)
        {
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

    private IEnumerator FireDoubleSpiral()
    {
        float spiralAngle = 0f;

        for (float i = 0; i < 360 * totalSpirals; i+= spiralAngleIncrease)
        {
            if(firingEnabled == true)
            {
                for (int j = 0; j <= 1; j++)
                {
                    GameObject bul = BulletSprayPool.Instance.GetBullet();

                    float bulDirX = transform.position.x + Mathf.Sin(((spiralAngle + 180f * j) * Mathf.PI) / 180f);
                    float bulDirY = transform.position.y + Mathf.Cos(((spiralAngle + 180f * j) * Mathf.PI) / 180f);

                    Vector3 bulMoveVector = new Vector3(bulDirX, bulDirY, 0f);
                    Vector2 bulDir = (bulMoveVector - transform.position).normalized; 
                    
                    bul.transform.position = transform.position;
                    bul.transform.rotation = transform.rotation;
                    bul.SetActive(true);
                    bul.GetComponent<SprayBullet>().SetMoveDirection(bulDir);

                    spiralAngle += spiralAngleIncrease; 
                    yield return new WaitForSeconds(spiralRateOfFire);                
                }
            }
        }
    }

    private IEnumerator FireAtPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject targetPlayer = null;

        if(players.Length > 1)
        {
            if(Random.Range(0, 2) == 0)
                targetPlayer = players[0];
            else
                targetPlayer = players[1];
        }
        else if(players.Length == 1)
            targetPlayer = players[0];
        

        for (int i = 0; i < fireAtPlayerAmount; i++)
        {
            if(firingEnabled == true)
            {
                GameObject bul = BulletSprayPool.Instance.GetBullet();

                float rand = Random.Range(0, 360);

                Quaternion rotation = Quaternion.Euler(0, 0, rand);

                Vector3 bulMoveVector = (rotation * (gameObject.transform.position - targetPlayer.transform.position)).normalized;
                    
                bul.transform.position = transform.position;
                bul.transform.rotation = transform.rotation;
                bul.SetActive(true);
                bul.GetComponent<SprayBullet>().SetMoveDirection(-bulMoveVector);

                yield return new WaitForSeconds(fireAtPlayerRateOfFire);                
            }
        }
    }
}
