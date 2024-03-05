using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class spellUi : MonoBehaviour
{
    public Image image;
    public Sprite fireSprite;
    public Sprite lightningSprite;
    public Sprite SeekingSprite;
    public Sprite chadSprite;
    Spells spells;

    // Start is called before the first frame update
    void Start()
    {
        image.enabled = false;
        // Get access to Spells
        spells = GetComponentInParent<Spells>();
    }

    // Update is called once per frame
    void Update()
    {
        if (spells.spellName == "Fireball" || spells.spellName == "Lightning" || spells.spellName == "SeekingOrb" || spells.spellName == "SummonChad")
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
        }
        else
        {
            image.enabled = false;
        }
    }
}
