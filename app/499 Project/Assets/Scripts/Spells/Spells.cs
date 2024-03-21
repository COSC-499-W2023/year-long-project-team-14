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

    //Used to store the prefabs of enemy's for the rainbow spell. 
    public GameObject e1;
    public GameObject e2;
    public GameObject e3;
    public GameObject e4;

    //Used to indicate when the shield is active 
    public bool isShield = false;

    public float yOffset = 11.5f;
    private List<GameObject> chads = new List<GameObject>();

    [SerializeField] private AudioSource fireballSound;
    [SerializeField] private AudioSource lightningSound;
    [SerializeField] private AudioSource seekingOrbSound;
    [SerializeField] private AudioSource chadSound;

    public GameObject freezeFlash;
    public GameObject iceCubeBreak;


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
            else if (spellName == "Rainbow")
            {
               //If the user has the shield spell and presses q then call ShieldSpell()
               RainbowSpell();
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
        float freezeTime = 3;

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
            if(enemyHealthSystem != null && enemyHealthSystem.enemyHealth > 0)
            {
                //Enable enemy movement and hit box
                enemies[i].GetComponent<EnemyMovement>().enabled = true;
                enemies[i].GetComponent<CircleCollider2D>().enabled = true;

                //Play ice break effect on enemy
                GameObject effect = Instantiate(iceCubeBreak, enemies[i].transform.position, Quaternion.identity);
                Destroy(effect, 1);

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
        
        
        // Start a coroutine to wait for a certain amount of time before destroying the shield
        StartCoroutine(DestroyShield(shield));
    }

    IEnumerator DestroyShield(GameObject shield)
    {
        yield return new WaitForSeconds(5f); //wait for timer before destroying the shield
        Destroy(shield);
        //Mark that the shield is inactive.
        isShield = false;
    }

    public void RainbowSpell(){
        int rng = Random.Range(1,8);

        //Cast the spell associated with the rng value 
        // TODO: increase the range with each addition of a new spell
            if(rng == 1)
            {
                FireballSpell();
            }
            else if(rng == 2)
            {   
                LightningSpell();
            }
            else if(rng == 3)
            {   
                SeekingOrb();
            }
            else if (rng == 4)
            {
                SummonChad();
            }
            else if (rng == 5)
            {
                StartCoroutine(Freeze());
            }
             else if (rng == 6)
            {
               //If the user has the shield spell and presses q then call ShieldSpell()
               ShieldSpell();
            }
            else if (rng >= 7)
            {
               //If you hit the unlucky 7 than get a second rng value and spawn that associated enemy 
               int rng2 = Random.Range(1,5);

               if(rng2 == 1){
                Instantiate(e1, playerController.transform.position, Quaternion.identity);
               }
               else if(rng2 == 2){
                Instantiate(e2, playerController.transform.position, Quaternion.identity);
               }
               else if(rng2 == 3){
                Instantiate(e3, playerController.transform.position, Quaternion.identity);
               }
               else if(rng2 == 4){
                Instantiate(e4, playerController.transform.position, Quaternion.identity);
               }
            }
            
    }

}
