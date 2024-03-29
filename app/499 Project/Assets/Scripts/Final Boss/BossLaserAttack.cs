using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaserAttack : MonoBehaviour
{   
    public GameObject bossBulletPrefab;
    public Animator animator;
    public GameObject laserPrefab;
    public GameObject quadLaser;
    public GameObject slime;
    public GameObject bonk;
    public GameObject orc;
    public GameObject triple;
    public EnemyMovement em;
    public MiniBossHealthSystem hs;
    public float slimeSpeed = 50.0f;
    public float bulletForce = 50;
    public GameObject laser;
    public bool phase1 = true;
    public bool phase2 = false;
    public bool phase3 = false;
    public bool phase4 = false;
    public bool phase5 = false;
    public bool phase6 = false;

    private bool spinLaser = false;

    [SerializeField]
    private float laserSpinSpeed = 30; //How fast the laser spins
    [SerializeField]
    private int seekerShotAmount = 1; //Amount of shots for phase 1 seeker attack
    [SerializeField]
    private int seekerShotsAmount = 3; //Amount of shots for phase 2 seeker attack

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

        //Get difficulty
        diff = PlayerPrefs.GetInt("difficulty");
        
        if(diff == 1) 
        {   
            laserSpinSpeed *= 1;

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
            laserSpinSpeed *= 1.5f;

            rateOfFire /= 1.5f;
            waitTime /= 1.33f;
            bulletsAmount = Mathf.Round(bulletsAmount * 1.33f);

            spiralRateOfFire /= 1.78f;
            spiralAngleIncrease /= 1.33f;
            totalSpirals *= 1.33f;

            fireAtPlayerRateOfFire /= 2f;
            fireAtPlayerAmount *= 2f;
        }
        else if(diff == 3)
        {
            laserSpinSpeed *= 2f;

            rateOfFire /= 2f;
            waitTime /= 1.67f;
            bulletsAmount = Mathf.Round(bulletsAmount * 1.67f);

            spiralRateOfFire /= 2.78f;
            spiralAngleIncrease /= 1.67f;
            totalSpirals *= 1.67f;

            fireAtPlayerRateOfFire /= 3f;
            fireAtPlayerAmount *= 3f;
        }
        else if(diff == 4)
        {
            laserSpinSpeed *= 2.5f;

            rateOfFire /= 2.5f;
            waitTime /= 2f;
            bulletsAmount *= 2f;

            spiralRateOfFire /= 4f;
            spiralAngleIncrease /= 2f;
            totalSpirals *= 2f;

            fireAtPlayerRateOfFire /= 4f;
            fireAtPlayerAmount *= 4f;
        }

        seekerShotAmount *= diff;
        seekerShotsAmount *= diff;

        totalShotsPerBurst = 1 + diff;
    }

    private IEnumerator AlternatingShooting()
    {
        bool spawnEnemies1 = true;
        bool spawnEnemies2 = true;
        bool spawnEnemies3 = true;
        bool spawnEnemies4 = true;
        bool spawnEnemies5 = true;

        while (firingEnabled == true) 
        {
            yield return new WaitForSeconds(2f / (((diff - 1) / 2) + 1)); // delay between attacks
            int rand = Random.Range(0, 100);

            //PHASE 1 ATTACKS
            if(phase1 && spawnEnemies1)
            {
                if(transform.position.x > 0)
                    StartCoroutine(ShootEnemiesLeft(0));
                else
                    StartCoroutine(ShootEnemiesRight(0));
                spawnEnemies1 = false;
                yield return new WaitForSeconds(2f); // delay between attacks
            }
            else if(phase2 && spawnEnemies2)
            {
                if(transform.position.x > 0)
                    StartCoroutine(ShootEnemiesLeft(1));
                else
                    StartCoroutine(ShootEnemiesRight(1));
                spawnEnemies2 = false;
                yield return new WaitForSeconds(0.5f); // delay between attacks
            }
            else if(phase3 && spawnEnemies3)
            {
                if(transform.position.x > 0)
                    StartCoroutine(ShootEnemiesLeft(2));
                else
                    StartCoroutine(ShootEnemiesRight(2));
                spawnEnemies3 = false;
                yield return new WaitForSeconds(0.5f); // delay between attacks
            }
            else if(phase5 && spawnEnemies4)
            {
                if(transform.position.x > 0)
                    StartCoroutine(ShootEnemiesLeft(3));
                else
                    StartCoroutine(ShootEnemiesRight(3));
                spawnEnemies4 = false;
                yield return new WaitForSeconds(0.5f); // delay between attacks
            }
            else if(phase6 && spawnEnemies5)
            {
                if(transform.position.x > 0)
                    StartCoroutine(ShootEnemiesLeft(4));
                else
                    StartCoroutine(ShootEnemiesRight(4));
                spawnEnemies5 = false;
                yield return new WaitForSeconds(0.5f); // delay between attacks
            }
            else if(phase4 == false)
            {
                if(rand < 25)
                {
                    yield return StartCoroutine(ShootLaser());
                    yield return new WaitForSeconds(0.5f); // delay between attacks
                }
                else if(rand < 50 && transform.position.x > 0)
                {
                    yield return StartCoroutine(TripleShotSlimesLeft((int)Mathf.Ceil((diff + 0.9f) / 3)));
                    yield return new WaitForSeconds(0.5f); // delay between attacks
                }
                else if(rand < 50 && transform.position.x < 0)
                {
                    yield return StartCoroutine(TripleShotSlimesRight((int)Mathf.Ceil((diff + 0.9f) / 3)));
                    yield return new WaitForSeconds(0.5f); // delay between attacks
                }
                else if(rand < 75)
                {
                    yield return StartCoroutine(SeekerShot());
                    yield return new WaitForSeconds(1f); // delay between attacks
                }
                else if(rand < 100)
                {
                    yield return StartCoroutine(FireBursts());
                    yield return new WaitForSeconds(0.5f); // delay between attacks
                }
                
            }

            //PHASE 2 ATTACKS
            else if(phase4 == true)
            {
                if(rand < 25)
                {
                    yield return StartCoroutine(ShootFourLasers());
                    yield return new WaitForSeconds(0.5f); // delay between attacks
                }
                else if(rand < 50 && transform.position.x > 0)
                {
                    yield return StartCoroutine(TripleShotSlimesLeft((int)Mathf.Ceil((diff + 0.9f) / 3) + 1));
                    yield return new WaitForSeconds(0.5f); // delay between attacks
                }
                else if(rand < 50 && transform.position.x < 0)
                {
                    yield return StartCoroutine(TripleShotSlimesRight((int)Mathf.Ceil((diff + 0.9f) / 3) + 1));
                    yield return new WaitForSeconds(0.5f); // delay between attacks
                }
                else if(rand < 75)
                {
                    yield return StartCoroutine(SeekerShots());
                    yield return new WaitForSeconds(1f); // delay between attacks
                }
                else if(rand < 100)
                {
                    yield return StartCoroutine(FireDoubleSpiral());
                    yield return new WaitForSeconds(0.5f); // delay between attacks
                }
            }

            yield return new WaitForSeconds(2f / (((diff - 1) / 2) + 1)); // delay between attacks
        }
    }

    private IEnumerator FireBursts() 
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
                GameObject bul = BulletSprayPool.Instance.GetBossBullet();

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
                    GameObject bul = BulletSprayPool.Instance.GetBossBullet();

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
                GameObject bul = BulletSprayPool.Instance.GetBossBullet();

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

            if(target != null)
            {
                em.enabled = false;
                animator.SetTrigger("ChargeLeft");
                Vector3 laserPos = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);
                Vector3 direction = target.transform.position - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x ) * Mathf.Rad2Deg + 90; 
                laser = Instantiate(laserPrefab, laserPos, Quaternion.Euler(0, 0, angle));

                BoxCollider2D laserHitbox;
                laserHitbox = laser.GetComponent<BoxCollider2D>();
                
                yield return new WaitForSeconds(0.6f);  
                if(firingEnabled)
                {
                    laserHitbox.enabled = true;
                    spinLaser = true;
                }

                yield return new WaitForSeconds(180 / laserSpinSpeed);
                
                if(firingEnabled)
                {
                    em.enabled = true;
                    laserHitbox.enabled = false;
                    spinLaser = false;
                    Destroy(laser);
                }
            }
        }
    }

    private IEnumerator ShootFourLasers()
    {
        if(firingEnabled)
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

            if(target != null)
            {
                em.enabled = false;
                animator.SetTrigger("ChargeLeft");
                Vector3 laserPos = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);
                Vector3 direction = target.transform.position - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x ) * Mathf.Rad2Deg + 45; 
                laser = Instantiate(quadLaser, laserPos, Quaternion.Euler(0, 0, angle));
                
                BoxCollider2D laserHitbox;
                BoxCollider2D laserHitbox2;
                laserHitbox = laser.GetComponent<BoxCollider2D>();
                laserHitbox2 = laser.transform.GetChild(0).GetComponent<BoxCollider2D>();

                yield return new WaitForSeconds(0.6f);  
                if(firingEnabled)
                {
                    laserHitbox.enabled = true;
                    laserHitbox2.enabled = true;
                    spinLaser = true;
                }
                yield return new WaitForSeconds(120 / laserSpinSpeed);

                if(firingEnabled)
                {
                    em.enabled = true;
                    laserHitbox.enabled = false;
                    laserHitbox2.enabled = false;
                    spinLaser = false;
                    Destroy(laser);
                }
            }
        }
    }

    private IEnumerator ShootEnemiesLeft(int n)
    {
        if(firingEnabled)
        {
            GameObject enemyPrefab = null;
            int amount = 0;
            if(n == 1)
            {
                enemyPrefab = slime;
                amount = 2;
            }
            else if(n == 2)
            {
                enemyPrefab = orc;
                amount = 2;
            }
            else if(n == 3)
            {
                enemyPrefab = bonk;
                amount = 2;
            }
            else if(n == 4)
            {
                enemyPrefab = triple;
                amount = 2;
            }
            
            if(n == 0)
            {
                yield return new WaitForSeconds(0.25f);
                animator.SetTrigger("ShootLeft");
                yield return new WaitForSeconds(0.25f);

                GameObject enemy = Instantiate(slime, transform.position + new Vector3(-2.0f, 0.75f, 0), Quaternion.Euler(0, 0, 0));
                enemy.GetComponent<Rigidbody2D>().AddForce(-enemy.transform.right * slimeSpeed);
                enemy.transform.rotation = Quaternion.identity;

                yield return new WaitForSeconds(0.25f);
                animator.SetTrigger("ShootLeft");
                yield return new WaitForSeconds(0.25f);

                enemy = Instantiate(orc, transform.position + new Vector3(-2.0f, 0.75f, 0), Quaternion.Euler(0, 0, 0));
                enemy.GetComponent<Rigidbody2D>().AddForce(-enemy.transform.right * slimeSpeed);
                enemy.transform.rotation = Quaternion.identity;

                yield return new WaitForSeconds(0.25f);
                animator.SetTrigger("ShootLeft");
                yield return new WaitForSeconds(0.25f);

                enemy = Instantiate(bonk, transform.position + new Vector3(-2.0f, 0.75f, 0), Quaternion.Euler(0, 0, 0));
                enemy.GetComponent<Rigidbody2D>().AddForce(-enemy.transform.right * slimeSpeed);
                enemy.transform.rotation = Quaternion.identity;

                // yield return new WaitForSeconds(0.25f);
                // animator.SetTrigger("ShootLeft");
                // yield return new WaitForSeconds(0.25f);

                // enemy = Instantiate(triple, transform.position + new Vector3(-2.0f, 0.75f, 0), Quaternion.Euler(0, 0, 0));
                // enemy.GetComponent<Rigidbody2D>().AddForce(-enemy.transform.right * slimeSpeed);
                // enemy.transform.rotation = Quaternion.identity;
            }
            else
            {
                animator.SetTrigger("ShootLeft");
                yield return new WaitForSeconds(0.25f);

                for(int i = 0; i < amount; i++)
                {
                    GameObject enemy = Instantiate(enemyPrefab, transform.position + new Vector3(-2.0f, 0.75f, 0), Quaternion.Euler(0, 0, (40 / (amount + 1)) * (i + 1) - 20));
                    enemy.GetComponent<Rigidbody2D>().AddForce(-enemy.transform.right * slimeSpeed);
                    enemy.transform.rotation = Quaternion.identity;
                }
            }
        }
        yield return null;
    }

    private IEnumerator ShootEnemiesRight(int n)
    {
        if(firingEnabled)
        {
            GameObject enemyPrefab = null;
            int amount = 0;
            if(n == 1)
            {
                enemyPrefab = slime;
                amount = 2;
            }
            else if(n == 2)
            {
                enemyPrefab = orc;
                amount = 2;
            }
            else if(n == 3)
            {
                enemyPrefab = bonk;
                amount = 2;
            }
            else if(n == 4)
            {
                enemyPrefab = triple;
                amount = 2;
            }

            if(n == 0)
            {
                yield return new WaitForSeconds(0.25f);
                animator.SetTrigger("ShootRight");
                yield return new WaitForSeconds(0.25f);

                GameObject enemy = Instantiate(slime, transform.position + new Vector3(-2.0f, 0.75f, 0), Quaternion.Euler(0, 0, 0));
                enemy.GetComponent<Rigidbody2D>().AddForce(-enemy.transform.right * slimeSpeed);
                enemy.transform.rotation = Quaternion.identity;

                yield return new WaitForSeconds(0.25f);
                animator.SetTrigger("ShootRight");
                yield return new WaitForSeconds(0.25f);

                enemy = Instantiate(orc, transform.position + new Vector3(-2.0f, 0.75f, 0), Quaternion.Euler(0, 0, 0));
                enemy.GetComponent<Rigidbody2D>().AddForce(-enemy.transform.right * slimeSpeed);
                enemy.transform.rotation = Quaternion.identity;

                yield return new WaitForSeconds(0.25f);
                animator.SetTrigger("ShootRight");
                yield return new WaitForSeconds(0.25f);

                enemy = Instantiate(bonk, transform.position + new Vector3(-2.0f, 0.75f, 0), Quaternion.Euler(0, 0, 0));
                enemy.GetComponent<Rigidbody2D>().AddForce(-enemy.transform.right * slimeSpeed);
                enemy.transform.rotation = Quaternion.identity;

                // yield return new WaitForSeconds(0.25f);
                // animator.SetTrigger("ShootRight");
                // yield return new WaitForSeconds(0.25f);
                
                // enemy = Instantiate(triple, transform.position + new Vector3(-2.0f, 0.75f, 0), Quaternion.Euler(0, 0, 0));
                // enemy.GetComponent<Rigidbody2D>().AddForce(-enemy.transform.right * slimeSpeed);
                // enemy.transform.rotation = Quaternion.identity;
            }
            else
            {
                animator.SetTrigger("ShootRight");
                yield return new WaitForSeconds(0.25f);

                for(int i = 0; i < amount; i++)
                {
                    GameObject enemy = Instantiate(enemyPrefab, transform.position + new Vector3(2.0f, 0.75f, 0), Quaternion.Euler(0, 0, (40 / (amount + 1)) * (i + 1) - 20));
                    enemy.GetComponent<Rigidbody2D>().AddForce(enemy.transform.right * slimeSpeed);
                    enemy.transform.rotation = Quaternion.identity;
                }
            }
        }
        yield return null;
    }

    private IEnumerator TripleShotSlimesLeft(int n)
    {
        for(int i = 1 ; i <= n; i++)
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

    private IEnumerator TripleShotSlimesRight(int n)
    {
        for(int i = 1 ; i <= n; i++)
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
        if(firingEnabled == true)
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

            if(target != null)
            {
                //Disable boss movement
                em.enabled = false;

                yield return new WaitForSeconds(0.25f);

                //Spawn bullets aiming at the closest player
                GameObject[] bullets = new GameObject[seekerShotAmount];
                for(int i = 0; i < seekerShotAmount; i++)
                {
                    if(firingEnabled == true)
                    {
                        Vector3 direction = target.transform.position - transform.position;
                        float angle = Mathf.Atan2(direction.y, direction.x ) * Mathf.Rad2Deg + (180 / (seekerShotAmount + 1)) * (i + 1) - 90; 
                        Vector3 position = Quaternion.Euler(0, 0, angle) * transform.right;

                        bullets[i] = Instantiate(bossBulletPrefab, transform.position + position * 2.5f, Quaternion.Euler(0, 0, angle));
                        yield return new WaitForSeconds(0.1f);
                    }
                }
                
                yield return new WaitForSeconds(0.5f);
                
                //Shoot bullets
                for(int i = 0; i < seekerShotAmount; i++)
                {
                    if(firingEnabled == true)
                        if(bullets[i] != null)
                            bullets[i].GetComponent<SeekerBullet>().allowMovement = true;
                }
                
                //Enable boss movement
                em.enabled = true;
            }
        }
    }

    public IEnumerator SeekerShots()
    {
        if(firingEnabled == true)
        {
            //Disable boss movement
            em.enabled = false;

            yield return new WaitForSeconds(0.25f);

            //Spawn bullets
            GameObject[] bullets = new GameObject[seekerShotsAmount];
            for(int i = 0; i < seekerShotsAmount; i++)
            {
                if(firingEnabled == true)
                {
                    float bulDirX = Mathf.Sin(((360 / seekerShotsAmount * i + 180f) * Mathf.PI) / 180f);
                    float bulDirY = Mathf.Cos(((360 / seekerShotsAmount * i + 180f) * Mathf.PI) / 180f);
                    bullets[i] = Instantiate(bossBulletPrefab, new Vector3(transform.position.x + bulDirX * 2.5f, transform.position.y + bulDirY * 2.5f, 0), Quaternion.Euler(0, 0, (-360 / (seekerShotsAmount + 1) * (i + 1) - 90)));
                    yield return new WaitForSeconds(0.1f);
                }
            }
            
            yield return new WaitForSeconds(0.5f);
            
            //Shoot bullets
            for(int i = 0; i < seekerShotsAmount; i++)
            {
                if(firingEnabled == true)
                    if(bullets[i] != null)
                        bullets[i].GetComponent<SeekerBullet>().allowMovement = true;
            }

            //Enable boss movement
            em.enabled = true;
        }
    }

    void Update()
    {
        if(laser != null && spinLaser)
        {
            laser.transform.Rotate(new Vector3(0, 0, laserSpinSpeed * Time.deltaTime));
        }

        if (hs.enemyHealth < hs.healthAmount * 0.8f)
        {
            phase2 = true;
        }
        if (hs.enemyHealth < hs.healthAmount * 0.6f)
        {
            phase3 = true;
        }
        if (hs.enemyHealth < hs.healthAmount * 0.5f)
        {
            phase4 = true;
        }
        if (hs.enemyHealth < hs.healthAmount * 0.4f)
        {
            phase5 = true;
        }
        if (hs.enemyHealth < hs.healthAmount * 0.2f)
        {
            phase6 = true;
        }

        if(hs.enemyHealth <= 0)
        {
            Destroy(laser);
        }
    }
}


    

