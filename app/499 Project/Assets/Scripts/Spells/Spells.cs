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
    public float yOffset = 11.5f;

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
        }
    }

    public void FireballSpell()
    {
        GameObject fireball = Instantiate(fireballPrefab, playerController.gunFollow.position, playerController.playerCenter.transform.rotation);
        Rigidbody2D fireballRB = fireball.GetComponent<Rigidbody2D>();
        fireballRB.AddForce(-playerController.gunFollow.up * 50 * playerController.bulletForce);
    }

    public void LightningSpell()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for(int i = 0; i < enemies.Length; i++)
        {   EnemyHealthSystem enemyHealthSystem = enemies[i].GetComponent<EnemyHealthSystem>();
            if(enemyHealthSystem.enemyHealth > 0)
            {
                Vector3 lightningPos = new Vector3(enemies[i].transform.position.x, enemies[i].transform.position.y + yOffset, enemies[i].transform.position.z);
                GameObject lightning = Instantiate(lightningPrefab, lightningPos, Quaternion.identity);
                enemyHealthSystem.takeDamage(1);
                Destroy(lightning, 0.25f);
            }
                
        }

    }
}
