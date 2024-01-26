using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spells : MonoBehaviour
{
    public string spellName = "Fireball";
    public float spellCooldown = 1;
    public float cooldownTimer = 0;
    public PlayerController playerController;
    public GameObject fireballPrefab;

    void Update()
    {
        //temporary way to get user input until controller support is added for this
        if(Input.GetKeyDown("q"))
        {
            CastSpell();
        }
        
        cooldownTimer += Time.deltaTime;
    }

    public void CastSpell()
    {
        //Only cast spell if the cooldown is complete
        if(cooldownTimer >= spellCooldown)
        {
            //Reset cooldown
            cooldownTimer = 0;

            //Cast the correct spell that the user has equipped
            if(spellName == "Fireball")
            {
                FireballSpell();
            }
            else if(spellName == "")
            {
                
            }
        }
    }

    public void FireballSpell()
    {
        GameObject fireball = Instantiate(fireballPrefab, playerController.gunFollow.position, playerController.playerCenter.rotation);
        Rigidbody2D fireballRB = fireball.GetComponent<Rigidbody2D>();
        fireballRB.AddForce(-playerController.gunFollow.up * 50 * playerController.bulletForce);
    }
}
