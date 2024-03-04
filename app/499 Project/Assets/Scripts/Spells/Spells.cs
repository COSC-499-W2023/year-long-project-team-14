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

    [SerializeField] private AudioSource fireballSound;
    [SerializeField] private AudioSource lightningSound;
    [SerializeField] private AudioSource seekingOrbSound;

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
        //chadSound.Play();
        GameObject chad = Instantiate(chadPrefab, playerController.gunFollow.position, Quaternion.identity);
        //kill Chad if he does not die after 20 seconds
        Invoke("KillChad", 20f);

    }

    void KillChad()
    {
        GameObject chad = GameObject.FindGameObjectWithTag("Chad");
        // Check if chad exists and has a health system
        if (chad != null)
        {
            healthSystem chadHealth = chad.GetComponent<healthSystem>();
            chadHealth.Die();
        }
    }

}
