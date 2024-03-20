using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaserAttack : MonoBehaviour
{   
    public Animator animator;
    public GameObject laser;
    public GameObject slime;
    public EnemyMovement em;
    public MiniBossHealthSystem hs;
    public float slimeSpeed = 50.0f;
    
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
        animator = GetComponent<Animator>();
        em = GetComponent<EnemyMovement>();
        hs = GetComponent<MiniBossHealthSystem>();

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
            rand = 2;

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
                yield return StartCoroutine(ShootSlimeLeft());
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

    public GameObject clone;
    public GameObject clone2;
    public GameObject clone3;
    public GameObject clone4;
    public bool phase2 = true;
    private IEnumerator ShootLaser()
    {
         if(firingEnabled)
        {
            em.enabled = false;
            animator.SetTrigger("ChargeLeft");
            Vector3 laserPos = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);
            clone = Instantiate(laser, laserPos, transform.rotation);

            BoxCollider2D laserHitbox;
            laserHitbox = clone.GetComponent<BoxCollider2D>();
            
            yield return new WaitForSeconds(0.6f);  
            laserHitbox.enabled = true;

            yield return new WaitForSeconds(5.0f);
            
            if(hs.enemyHealth > 0)
            {
                em.enabled = true;
                laserHitbox.enabled = false;
                Destroy(clone);
            }
        
        }
    }

    private IEnumerator ShootFourLasers()
    {
        if(firingEnabled)
        {
            em.enabled = false;

            //laser 1
            animator.SetTrigger("ChargeLeft");
            Vector3 laserPos = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);
            clone = Instantiate(laser, laserPos, transform.rotation);
            BoxCollider2D laserHitbox;
            laserHitbox = clone.GetComponent<BoxCollider2D>();
            
             //laser 2
            Vector3 laserPos2 = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);
            clone2 = Instantiate(laser, laserPos2, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, 90.0f)));
            BoxCollider2D laserHitbox2;
            laserHitbox2 = clone2.GetComponent<BoxCollider2D>();

             //laser 3
            Vector3 laserPos3 = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);
            clone3 = Instantiate(laser, laserPos3, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, 180.0f)));
            BoxCollider2D laserHitbox3;
            laserHitbox3 = clone3.GetComponent<BoxCollider2D>();
            
            //laser 4
            Vector3 laserPos4 = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);
            clone4 = Instantiate(laser, laserPos4, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, 270.0f)));
            BoxCollider2D laserHitbox4;
            laserHitbox4 = clone4.GetComponent<BoxCollider2D>();
            
            yield return new WaitForSeconds(0.6f);  
            laserHitbox.enabled = true;
            laserHitbox2.enabled = true; 
            laserHitbox3.enabled = true;
            laserHitbox4.enabled = true;
            yield return new WaitForSeconds(5.0f);

            if(hs.enemyHealth > 0)
            {
                em.enabled = true;
                laserHitbox.enabled = false;
                laserHitbox2.enabled = false;
                laserHitbox3.enabled = false;
                laserHitbox4.enabled = false;
                Destroy(clone);
                Destroy(clone2);
                Destroy(clone3);
                Destroy(clone4);
            }
        
        }
    }

    private IEnumerator ShootSlimeLeft()
    {
         if(firingEnabled)
        {
            em.enabled = false;
            animator.SetTrigger("ShootLeft");

            GameObject slimeClone = Instantiate(slime, transform.position, Quaternion.identity);
            Vector2 direction = -transform.right;
            slimeClone.GetComponent<Rigidbody2D>().AddForce(direction * slimeSpeed);

        }
        yield return null;
    }

    void Update()
    {
        if(clone != null)
        {
            clone.transform.Rotate(new Vector3(0,0,100*Time.deltaTime));
        }

        if(clone2 != null && phase2 == true)
        {
            clone2.transform.Rotate(new Vector3(0,0,100*Time.deltaTime));
        }

        if(clone3 != null && phase2 == true)
        {
            clone3.transform.Rotate(new Vector3(0,0,100*Time.deltaTime));
        }

        if(clone4 != null && phase2 == true)
        {
            clone4.transform.Rotate(new Vector3(0,0,100*Time.deltaTime));
        }

        if(hs.enemyHealth <= 0)
        {
            Destroy(clone);
            Destroy(clone2);
            Destroy(clone3);
            Destroy(clone4);
        }
       
    }
}


    

