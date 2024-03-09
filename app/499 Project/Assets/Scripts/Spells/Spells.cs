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
    public float yOffset = 11.5f;
    private List<GameObject> chads = new List<GameObject>();

    [SerializeField] private AudioSource fireballSound;
    [SerializeField] private AudioSource lightningSound;
    [SerializeField] private AudioSource seekingOrbSound;
    [SerializeField] private AudioSource chadSound;

    void Update()
    {
        cooldownTimer += Time.deltaTime;
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
        }
    }

    public void FireballSpell()
    {
        fireballSound.Play();
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
        // GameObject orb = Instantiate(seekingOrbPrefab, playerController.gunFollow.position, playerController.playerCenter.transform.rotation);
        // Rigidbody2D orbRB = orb.GetComponent<Rigidbody2D>();
        // orbRB.AddForce(-playerController.gunFollow.up * 200 * playerController.bulletForce);
        seekingOrbSound.Play();
        Quaternion rotation = playerController.playerCenter.transform.rotation * Quaternion.Euler(0, 0, 180);

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

}
