using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class spellUi : MonoBehaviour
{
    public bool player1 = true;
    public Image image;
    public Sprite fireSprite;
    public Sprite lightningSprite;
    public Sprite SeekingSprite;
    public Sprite chadSprite;
    public Sprite freezeSprite;
    public Sprite shieldSprite;
    Spells spells;

    public GameMaster gameMaster;
    public Image button;
    public Sprite ltIcon;
    public Sprite qIcon;

    // Start is called before the first frame update
    void Start()
    {
        image.enabled = false;
        // Get access to Spells
        spells = GetComponentInParent<Spells>();
        GameObject g = GameObject.FindWithTag("GameMaster");
        if(g != null)
            gameMaster = g.GetComponent<GameMaster>();
    }

    // Update is called once per frame
    void Update()
    {
        if (spells.spellName == "Fireball" || spells.spellName == "Lightning" || spells.spellName == "SeekingOrb" || spells.spellName == "SummonChad" || spells.spellName == "Freeze" || spells.spellName == "Shield")
        {
            image.enabled = true;

            // Set the spell icon based on the spell name
            if (spells.spellName == "Fireball")
                image.sprite = fireSprite;
            else if (spells.spellName == "Lightning")
                image.sprite = lightningSprite;
            else if (spells.spellName == "SeekingOrb")
                image.sprite = SeekingSprite;
            else if (spells.spellName == "SummonChad")
                image.sprite = chadSprite;
            else if (spells.spellName == "Freeze")
                image.sprite = freezeSprite;
            else if (spells.spellName == "Shield")
                image.sprite = shieldSprite;
                
            // Update the fill amount based on the remaining cooldown
            if (spells.cooldownTimer > 0)
            {
                image.fillAmount = (spells.cooldownTimer / spells.spellCooldown);
            }
            else
            {
                // Reset fill amount to fully visible when cooldown is complete
                image.fillAmount = 0f;
            }

            if(gameMaster != null && spells.cooldownTimer >= spells.spellCooldown)
            {
                //Change prompt icon depending on controls
                if(player1)
                {
                    if(gameMaster.player1Controls == "PS")
                    {
                        button.sprite = ltIcon;
                    }
                    else if(gameMaster.player1Controls == "Xbox")
                    {
                        button.sprite = ltIcon;
                    }
                    else
                        button.sprite = qIcon;
                }
                else
                {
                    if(gameMaster.player2Controls == "PS")
                    {
                        button.sprite = ltIcon;
                    }
                    else if(gameMaster.player2Controls == "Xbox")
                    {
                        button.sprite = ltIcon;
                    }
                    else
                        button.sprite = qIcon;
                }

                button.color = new Color(1, 1, 1, 1);
                image.color = new Color(1, 1, 1, 1);
            }
            else 
            {
                button.color = new Color(1, 1, 1, 0);
                image.color = new Color(1, 1, 1, 0.35f);
            }
        }
        else
        {
            image.enabled = false;
        }
    }
}
