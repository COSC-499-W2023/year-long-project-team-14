using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaserAttack : MonoBehaviour
{   
    public GameObject bossBulletPrefab;
    public Animator animator;
    public GameObject laser;
    public GameObject quadLaser;
    public GameObject slime;
    public EnemyMovement em;
    public MiniBossHealthSystem hs;
    public float slimeSpeed = 50.0f;
    public float bulletForce = 50;
    public GameObject clone;
    public bool phase2 = false;

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

            //PHASE 1 ATTACKS
            if(phase2 == false)
            {
                if(rand == 0)
                {
                    yield return StartCoroutine(ShootLaser());
                    yield return new WaitForSeconds(1f); // delay between attacks
                }
                else if(rand == 1 && transform.position.x > 0)
                {
                    yield return StartCoroutine(ShootSlimeLeft());
                    yield return new WaitForSeconds(1f); // delay between attacks
                }
                else if(rand == 1 && transform.position.x < 0)
                {
                    yield return StartCoroutine(ShootSlimeRight());
                    yield return new WaitForSeconds(1f); // delay between attacks
                }
                else if(rand == 2)
                {
                    yield return StartCoroutine(SeekerShot());
                    yield return new WaitForSeconds(1f); // delay between attacks
                }
            }

            //PHASE 2 ATTACKS
            if(phase2 == true)
            {
                if(rand == 0)
                {
                    yield return StartCoroutine(ShootFourLasers());
                    yield return new WaitForSeconds(1f); // delay between attacks
                }
                else if(rand == 1 && transform.position.x > 0)
                {
                    yield return StartCoroutine(TripleShotSlimesLeft());
                    yield return new WaitForSeconds(1f); // delay between attacks
                }
                else if(rand == 1 && transform.position.x < 0)
                {
                    yield return StartCoroutine(TripleShotSlimesRight());
                    yield return new WaitForSeconds(1f); // delay between attacks
                }
                else if(rand == 2)
                {
                    yield return StartCoroutine(SeekerShots());
                    yield return new WaitForSeconds(1f); // delay between attacks
                }
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
            animator.SetTrigger("ChargeLeft");
            Vector3 laserPos = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);
            clone = Instantiate(quadLaser, laserPos, transform.rotation);
            
            BoxCollider2D laserHitbox;
            BoxCollider2D laserHitbox2;
            laserHitbox = clone.GetComponent<BoxCollider2D>();
            laserHitbox2 = clone.transform.GetChild(0).GetComponent<BoxCollider2D>();

            yield return new WaitForSeconds(0.6f);  
            laserHitbox.enabled = true;
            laserHitbox2.enabled = true;
            yield return new WaitForSeconds(5.0f);

            if(hs.enemyHealth > 0)
            {
                em.enabled = true;
                laserHitbox.enabled = false;
                laserHitbox2.enabled = false;
                Destroy(clone);
            }

        }
    }

    private IEnumerator ShootSlimeLeft()
    {
         if(firingEnabled)
        {
            animator.SetTrigger("ShootLeft");
            yield return new WaitForSeconds(0.25f);
            GameObject slimeClone = Instantiate(slime, transform.position + new Vector3(-2.0f, 0.75f, 0), Quaternion.identity);
            slimeClone.GetComponent<Rigidbody2D>().AddForce(-transform.right * slimeSpeed);

        }
        yield return null;
    }

    private IEnumerator ShootSlimeRight()
    {
        if(firingEnabled)
        {
            animator.SetTrigger("ShootRight");
            yield return new WaitForSeconds(0.25f);
            GameObject slimeClone = Instantiate(slime, transform.position + new Vector3(2.0f, 0.75f, 0), Quaternion.identity);
            slimeClone.GetComponent<Rigidbody2D>().AddForce(transform.right * slimeSpeed);

        }
        yield return null;
    }

    private IEnumerator TripleShotSlimesLeft()
    {
        
        for(int i = 1 ; i <= 3; i++)
        {
            if(firingEnabled)
            {
                animator.SetTrigger("ShootLeft");
                yield return new WaitForSeconds(0.25f);
                GameObject slimeClone = Instantiate(slime, transform.position + new Vector3(-2.0f, 0.75f, 0), Quaternion.identity);
                slimeClone.GetComponent<Rigidbody2D>().AddForce(-transform.right * slimeSpeed);
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    private IEnumerator TripleShotSlimesRight()
    {
        for(int i = 1 ; i <= 3; i++)
        {
            if(firingEnabled)
            {
                animator.SetTrigger("ShootRight");
                yield return new WaitForSeconds(0.25f);
                GameObject slimeClone = Instantiate(slime, transform.position + new Vector3(2.0f, 0.75f, 0), Quaternion.identity);
                slimeClone.GetComponent<Rigidbody2D>().AddForce(transform.right * slimeSpeed);
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    public IEnumerator SeekerShot()
    {
        //Find closest player
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        List<GameObject> alivePlayers = new List<GameObject>();
        GameObject target = null;

        for(int i = 0; i < players.Length; i++)
        {
            healthSystem hs = players[i].GetComponent<healthSystem>();
            if(hs != null)
            {
                if(hs.life > 0)
                    alivePlayers.Add(players[i]);
            }
        }
        
        if(alivePlayers.Count > 1)
        {
            float distance1 = Vector3.Distance(gameObject.transform.position, alivePlayers[0].transform.position);
            float distance2 = Vector3.Distance(gameObject.transform.position, alivePlayers[1].transform.position);
            if(distance2 < distance1)
                target = alivePlayers[1];
            else
                target = alivePlayers[0];
        }
        else if(alivePlayers.Count > 0)
            target = alivePlayers[0];

        //Disable boss movement
        em.enabled = false;

        yield return new WaitForSeconds(0.25f);

        //Spawn bullets aiming at the closest player
        int shots = 3;
        GameObject[] bullets = new GameObject[shots];
        for(int i = 0; i < shots; i++)
        {
            Vector3 direction = target.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x ) * Mathf.Rad2Deg + (90 / (shots - 1)) * i - 45; 
            Vector3 position = Quaternion.Euler(0, 0, angle) * transform.right;

            bullets[i] = Instantiate(bossBulletPrefab, transform.position + position * 2.5f, Quaternion.Euler(0, 0, angle));
            yield return new WaitForSeconds(0.075f);
        }
        
        yield return new WaitForSeconds(0.5f);
        
        //Shoot bullets
        for(int i = 0; i < shots; i++)
        {
            if(bullets[i] != null)
                bullets[i].GetComponent<SeekerBullet>().allowMovement = true;
        }
        
        //Enable boss movement
        em.enabled = false;
    }

    public IEnumerator SeekerShots()
    {
        //Disable boss movement
        em.enabled = false;

        yield return new WaitForSeconds(0.25f);

        //Spawn bullets
        int shots = 10;
        GameObject[] bullets = new GameObject[shots];
        for(int i = 0; i < shots; i++)
        {
            float bulDirX = Mathf.Sin(((360 / shots * i + 180f) * Mathf.PI) / 180f);
            float bulDirY = Mathf.Cos(((360 / shots * i + 180f) * Mathf.PI) / 180f);
            bullets[i] = Instantiate(bossBulletPrefab, new Vector3(transform.position.x + bulDirX * 2.5f, transform.position.y + bulDirY * 2.5f, 0), Quaternion.Euler(0, 0, -90 + (-360 / shots * i)));
            yield return new WaitForSeconds(0.075f);
        }
        
        yield return new WaitForSeconds(0.5f);
        
        //Shoot bullets
        for(int i = 0; i < shots; i++)
        {
            if(bullets[i] != null)
                bullets[i].GetComponent<SeekerBullet>().allowMovement = true;
        }

        //Enable boss movement
        em.enabled = false;
    }

    void Update()
    {
        if(clone != null)
        {
            clone.transform.Rotate(new Vector3(0,0,50*Time.deltaTime));
        }

        if (hs.enemyHealth < hs.healthAmount * 0.5f)
        {
            phase2 = true;
        }

        if(hs.enemyHealth <= 0)
        {
            Destroy(clone);
        }
       
    }
}


    

