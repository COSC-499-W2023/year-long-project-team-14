using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

public class Spells : MonoBehaviour
{
    public string spellName = "Fireball";
    public float spellCooldown = 1;
    public float cooldownTimer = 0;
    public PlayerController playerController;
    public healthSystem hs;
    public GameObject fireballPrefab;
    public GameObject lightningPrefab;
    public GameObject seekingOrbPrefab;
    public GameObject chadPrefab;
    public GameObject iceCubePrefab;
    public GameObject shieldPrefab;
    public GameObject shield;
    public GameObject ScatterShotPrefab;

    //This is used to display the sprite for mage rage
     public GameObject mRAura;
     public GameObject aura;


    
    // Below is used to change the sprite during the duraiton of the mage rage spell 
    //This is not needed anymore
    //public SpriteRenderer spriteRenderer;


    //Used to indicate when the shield is active 
    public bool isShield = false;

    //Used to indicate when the player is in mage rage 
    public bool isRage = false;


    public float yOffset = 11.5f;
    private List<GameObject> chads = new List<GameObject>();

    [SerializeField] private AudioSource fireballSound;
    [SerializeField] private AudioSource lightningSound;
    [SerializeField] private AudioSource seekingOrbSound;
    [SerializeField] private AudioSource chadSound;
    [SerializeField] private AudioSource ScatterShotSound;

    public GameObject freezeFlash;
    public GameObject iceCubeBreak;

    public float bulletsAmount = 10; // Sets the total number of bullets in the spread
    public float startAngle = 0, endAngle = 360f; // Sets the start and end angle of the burst

    void Update()
    {
        cooldownTimer += Time.deltaTime;

        if(isShield == true){
            //If the shield is active 

            //Get the shield transform 
            Transform shieldT = shield.transform;

            //Make the shield position follow the players position
            shieldT.position = playerController.transform.position;
        }

        if(isRage == true){
            //If the shield is active 

            //Get the shield transform 
            Transform rageT = aura.transform;

            //Make the shield position follow the players position
            rageT.position = playerController.transform.position;
        }
    }

    public void Spell(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            CastSpell();
        }
    }

    public void CastSpell()
    {
        //Only cast spell if the cooldown is complete, player isn't dead, and game isn't paused
        if(cooldownTimer >= spellCooldown && !hs.dead && !PauseMenu.GameIsPaused)
        {
            //Reset cooldown
            cooldownTimer = 0;

            //Cast the correct spell that the user has equipped
            if(spellName == "Fireball")
            {
                FireballSpell();
            }
            else if(spellName == "Lightning")
            {   
                LightningSpell();
            }
            else if(spellName == "SeekingOrb")
            {   
                SeekingOrb();
            }
            else if (spellName == "SummonChad")
            {
                SummonChad();
            }
            else if (spellName == "Freeze")
            {
                StartCoroutine(Freeze());
            }
             else if (spellName == "Shield")
            {
               //If the user has the shield spell and presses q then call ShieldSpell()
               ShieldSpell();
            }
            else if (spellName == "mR")
            {
               //If the user has the mage rage spell and presses q then call ShieldSpell()
               mageRage();
            }
            else if (spellName == "ScatterShot")
            {
                scatterShot();
            }
        }
    }

    public void FireballSpell()
    {
        //Play shoot fireball sound effect
        fireballSound.Play();

        //Spawn fireball and launch it based on players aim direction
        GameObject fireball = Instantiate(fireballPrefab, playerController.gunFollow.position, playerController.playerCenter.transform.rotation);
        Rigidbody2D fireballRB = fireball.GetComponent<Rigidbody2D>();
        fireballRB.AddForce(-playerController.gunFollow.up * 25 * playerController.bulletForce);
    }

    public void LightningSpell()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for(int i = 0; i < enemies.Length; i++)
        {   EnemyHealthSystem enemyHealthSystem = enemies[i].GetComponent<EnemyHealthSystem>();
            if(enemyHealthSystem.enemyHealth > 0)
            {
                lightningSound.Play();
                Vector3 lightningPos = new Vector3(enemies[i].transform.position.x, enemies[i].transform.position.y + yOffset, enemies[i].transform.position.z);
                GameObject lightning = Instantiate(lightningPrefab, lightningPos, Quaternion.identity);
                enemyHealthSystem.takeDamage(1);
                Destroy(lightning, 0.25f);
            }
        }

        //Check if there is a boss in the level
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");
        if (boss != null)
        {
            MiniBossHealthSystem miniBossHealthSystem = boss.GetComponent<MiniBossHealthSystem>();
            if(miniBossHealthSystem.enemyHealth > 0)
            {
                    lightningSound.Play();
                    Vector3 lightningPos = new Vector3(boss.transform.position.x, boss.transform.position.y + yOffset, boss.transform.position.z);
                    GameObject lightning = Instantiate(lightningPrefab, lightningPos, Quaternion.identity);
                    miniBossHealthSystem.takeDamage(3);
                    Destroy(lightning, 0.25f);
            }
        }
    }

    public void SeekingOrb()
    {
        //Play orb shoot sound effect
        seekingOrbSound.Play();

        //Get player aim rotation
        Quaternion rotation = playerController.playerCenter.transform.rotation * Quaternion.Euler(0, 0, 180);

        //Spawn orbs and launch them based on rotation
        GameObject orb = Instantiate(seekingOrbPrefab, playerController.gunFollow.position, rotation);
        Rigidbody2D orbRB = orb.GetComponent<Rigidbody2D>();
        orbRB.AddForce(orb.transform.up * 80 * playerController.bulletForce);

        orb = Instantiate(seekingOrbPrefab, playerController.gunFollow.position, rotation * Quaternion.Euler(0, 0, -20));
        orbRB = orb.GetComponent<Rigidbody2D>();
        orbRB.AddForce(orb.transform.up * 80 * playerController.bulletForce);

        orb = Instantiate(seekingOrbPrefab, playerController.gunFollow.position, rotation * Quaternion.Euler(0, 0, 20));
        orbRB = orb.GetComponent<Rigidbody2D>();
        orbRB.AddForce(orb.transform.up * 80 * playerController.bulletForce);

       
    }

    public void SummonChad()
    {
        chadSound.Play();
        GameObject newChad = Instantiate(chadPrefab, playerController.gunFollow.position, Quaternion.identity);
        healthSystem chadHealth = newChad.GetComponent<healthSystem>();

        // Add the new Chad instance to the list
        chads.Add(newChad);

        // Start a coroutine to wait for a certain amount of time before calling Die() for this Chad instance
        StartCoroutine(DelayedDeath(newChad, chadHealth));
    }

    IEnumerator DelayedDeath(GameObject chad, healthSystem chadHealth)
    {
        yield return new WaitForSeconds(15f); //wait for timer before killing chad

        // Check if the chad still exists and has a health system
        if (chad != null && chadHealth != null)
        {
            chadHealth.Die();
            chads.Remove(chad); // Remove this Chad instance from the list
        }
    }

    public IEnumerator Freeze()
    {
        //Set freeze duration
        float freezeTime = 2;

        //Play flash animation
        GameObject flash = Instantiate(freezeFlash, new Vector3(0, 0, 0), Quaternion.identity);
        Destroy(flash, 1);

        //Destroy all enemy bullets
        GameObject[] enemyBullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
        for(int i = 0; i < enemyBullets.Length; i++) enemyBullets[i].SetActive(false);

        //Get array of enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for(int i = 0; i < enemies.Length; i++)
        {
            //Get enemy health system
            EnemyHealthSystem enemyHealthSystem = enemies[i].GetComponent<EnemyHealthSystem>();

            //Get enemy sprite renderer
            SpriteRenderer spriteRenderer = enemies[i].GetComponent<SpriteRenderer>();

            if(enemyHealthSystem != null)
            {
                if(enemyHealthSystem.enemyHealth > 0)
                {
                    //Disable movement and hit box
                    enemies[i].GetComponent<EnemyMovement>().enabled = false;
                    enemies[i].GetComponent<CircleCollider2D>().enabled = false;

                    //Spawn ice cube on enemy
                    GameObject ice = Instantiate(iceCubePrefab, enemies[i].transform.position, Quaternion.identity);
                    Destroy(ice, freezeTime);

                    //Make enemy invisible to avoid seeing them clip through the edge of the ice cube
                    spriteRenderer.color = new Color(0, 0, 0, 0);

                    //Disable orc attacks
                    EnemyAttack enemyAttack = enemies[i].GetComponent<EnemyAttack>();
                    if(enemyAttack != null)
                        enemyAttack.enabled = false;

                    //Disable triple shot attacks
                    EnemyTripleShot enemyTripleShot = enemies[i].GetComponent<EnemyTripleShot>();
                    if(enemyTripleShot != null)
                        enemyTripleShot.enabled = false;
                }
            }
        }

        //Get array of bosses
        GameObject[] bosses = GameObject.FindGameObjectsWithTag("Boss");
        for(int i = 0; i < bosses.Length; i++)
        {
            //Get boss health system
            MiniBossHealthSystem miniBossHealthSystem = bosses[i].GetComponent<MiniBossHealthSystem>();
                
            if(miniBossHealthSystem != null)
            {
                if(miniBossHealthSystem.enemyHealth > 0)
                {
                    //Play ice break effect on boss
                    GameObject effect = Instantiate(iceCubeBreak, bosses[i].transform.position, Quaternion.identity);
                    Destroy(effect, 1);
                }
            }
        }

        //Wait for duration of freeze
        yield return new WaitForSeconds(freezeTime);

        for(int i = 0; i < enemies.Length; i++)
        {
            //Get enemy health system
            EnemyHealthSystem enemyHealthSystem = enemies[i].GetComponent<EnemyHealthSystem>();
            
            //Get enemy sprite renderer
            SpriteRenderer spriteRenderer = enemies[i].GetComponent<SpriteRenderer>();

            if(enemyHealthSystem != null && enemyHealthSystem.enemyHealth > 0)
            {
                //Enable enemy movement and hit box
                enemies[i].GetComponent<EnemyMovement>().enabled = true;
                enemies[i].GetComponent<CircleCollider2D>().enabled = true;

                //Play ice break effect on enemy
                GameObject effect = Instantiate(iceCubeBreak, enemies[i].transform.position, Quaternion.identity);
                Destroy(effect, 1);

                //Make enemy visible again
                spriteRenderer.color = new Color(1, 1, 1, 1);

                //Enable orc attacks
                EnemyAttack enemyAttack = enemies[i].GetComponent<EnemyAttack>();
                if(enemyAttack != null)
                    enemyAttack.enabled = true;

                //Enable triple shot attacks
                EnemyTripleShot enemyTripleShot = enemies[i].GetComponent<EnemyTripleShot>();
                if(enemyTripleShot != null)
                    enemyTripleShot.enabled = false;
            }
        }
    }

    public void ShieldSpell(){
        //Instantiate the shield on the player
        shield = Instantiate(shieldPrefab, playerController.transform.position, Quaternion.identity);
        
        //Indicate that the shield is active 
        isShield = true;
        gameObject.layer = LayerMask.NameToLayer("ShieldPlayer");
        
        // Start a coroutine to wait for a certain amount of time before destroying the shield
        StartCoroutine(DestroyShield(shield));
    }

    IEnumerator DestroyShield(GameObject shield)
    {
        yield return new WaitForSeconds(5f); //wait for timer before destroying the shield
        Destroy(shield);
        //Mark that the shield is inactive.
        isShield = false;
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    //This will be used for the mage rage spell 
    public void mageRage(){
      
      
       //Increase the players damage
        // GameObject[] enemies2 = GameObject.FindGameObjectsWithTag("Enemy");
        // for(int i = 0; i < enemies2.Length; i++)
        // {  
        //     EnemyHealthSystem enemyHealthSystem = enemies2[i].GetComponent<EnemyHealthSystem>();
        //     enemyHealthSystem.mageRisOn = true;
        // }
        
        //Make the sprite show for the mage rage
        aura = Instantiate(mRAura, playerController.transform.position, Quaternion.identity);
        
        //Indicate that the shield is active 
        isRage = true;


        //Increase the recharge time of the bullets so the player can continously shoot
        playerController.attackChargeSpeed *= 1.75f;
        
        //Increase the speed of the bullets
        playerController.bulletForce *= 1.75f;

        //Increase the movement speed of the player
        playerController.moveSpeed *= 1.75f;


        StartCoroutine(timer()); //wait for timer before destroying the shield

       

    }

    IEnumerator timer()
    {

        yield return new WaitForSeconds(5f); //wait for timer to decrease stats back to normal

        

        //After 5 seconds return the stats to the original values.
        // GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        // for(int i = 0; i < enemies.Length; i++)
        // {  
        //     EnemyHealthSystem enemyHealthSystem = enemies[i].GetComponent<EnemyHealthSystem>();
        //     enemyHealthSystem.mageRisOn = false;
        // }

        // Color currentColor = spriteRenderer.color;


        // currentColor. = 255f;
        // currentColor.g = 11f;
        // currentColor.b = 11f;
        // currentColor.a= 255f;

        playerController.attackChargeSpeed /= 1.75f;
        
        playerController.bulletForce /= 1.75f;

        playerController.moveSpeed /= 1.75f;

         isRage = false;

        Destroy(aura);

    }

    public void scatterShot()
    {
        //Play shoot scatter shot sound effect
        ScatterShotSound.Play();
        GameObject Scatter = Instantiate(ScatterShotPrefab, playerController.gunFollow.position, Quaternion.identity);

        float angleStep = (endAngle - startAngle) / bulletsAmount;
        float angle = startAngle + 15;

            for (int i = 0; i < bulletsAmount + 1; i++)
            {
                GameObject bul = ScatterShot.Instance.GetBullet();

                float bulDirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
                float bulDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

                Vector3 bulMoveVector = new Vector3(bulDirX, bulDirY, 0f);
                Vector2 bulDir = (bulMoveVector - transform.position).normalized;

                bul.transform.position = transform.position;
                bul.transform.rotation = transform.rotation;
                bul.SetActive(true);
                bul.GetComponent<ScatterShotBullet>().SetMoveDirection(bulDir);

                angle += angleStep;
            }
    }

}
