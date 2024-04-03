using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Chest : MonoBehaviour
{
    public SpriteRenderer prompt;
    public SpriteRenderer button;
    public Sprite openPrompt;
    public Sprite lockedPrompt;
    public Sprite xPrompt;
    public Sprite aPrompt;
    public Sprite ePrompt;

    private bool playerIsOverChest = false;
    public bool chestOpened = false;
    public bool needsKey = true;
    GameMaster gameMaster;
    public SpriteRenderer spriteRenderer;
    public Sprite openChest;

    public Sprite redSprite;
    public Sprite goldSprite;
    public Sprite greenSprite;
    public Sprite blueSprite;
    public Sprite purpleSprite;
    public Sprite greySprite;
    public GameObject redBullet;
    public GameObject goldBullet;
    public GameObject greenBullet;
    public GameObject blueBullet;
    public GameObject purpleBullet;
    public GameObject greyBullet;

    public GameObject canvas;
    public Animator chestEffect;
    public Animator chestPopup;

    public TextMeshProUGUI popupText;
    public Image popupImage;

    private void Start()
    {
        //get access to game master
        gameMaster = GameObject.FindWithTag("GameMaster").GetComponent<GameMaster>();

        if(gameMaster.hasKey || !needsKey)
        {
            button.gameObject.SetActive(true);
            prompt.sprite = openPrompt;
        }
    }

    //player is within range
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.gameObject.GetComponent<healthSystem>().chad && !chestOpened)
        {
            playerIsOverChest = true;
            
            prompt.gameObject.SetActive(true);
            if(other.gameObject.GetComponent<PlayerController>().player1)
            {
                if(gameMaster.player1Controls == "PS")
                {
                    button.sprite = xPrompt;
                }
                else if(gameMaster.player1Controls == "Xbox")
                {
                    button.sprite = aPrompt;
                }
                else
                    button.sprite = ePrompt;
            }
            else
            {
                if(gameMaster.player2Controls == "PS")
                {
                    button.sprite = xPrompt;
                }
                else if(gameMaster.player2Controls == "Xbox")
                {
                    button.sprite = aPrompt;
                }
                else
                    button.sprite = ePrompt;
            }
        
        }
    }

    //player is no longer within range
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !chestOpened)
        {
            playerIsOverChest = false;
            prompt.gameObject.SetActive(false);
        }
    }

    //go to next level if player interacts with exit and satisfies conditions
    public void Interact(PlayerController playerController, healthSystem hs)
    {
        if(playerIsOverChest && (gameMaster.hasKey || !needsKey))
        {
            if(!chestOpened)
            {
                chestOpened = true;
                spriteRenderer.sprite = openChest;
                Destroy(prompt.gameObject);
                
                canvas.SetActive(true);
                chestPopup.Play("chestPopup");

                int rand = Random.Range(0, 6);

                if(rand == 0) //Blast
                {
                    playerController.aimOrb.sprite = redSprite;
                    playerController.bulletPrefab = redBullet;

                    chestEffect.Play("ChestEffectRed");

                    popupText.text = "Blast Orb";
                    popupImage.sprite = redSprite;
                    
                    playerController.attackChargeSpeed /= 4f;
                    playerController.attackCharge = 1;
                    playerController.attackChargeMax = 2;
                    playerController.blastBullet = true;
                }
                else if(rand == 1) //Super
                {
                    playerController.aimOrb.sprite = goldSprite;
                    playerController.bulletPrefab = goldBullet;

                    chestEffect.Play("ChestEffectGold");

                    popupText.text = "Super Orb";
                    popupImage.sprite = goldSprite;

                    playerController.attackChargeSpeed *= 1.2f;
                    playerController.bulletForce *= 1.2f;
                    playerController.attackCharge = 0;
                    playerController.attackChargeMax = 1;
                    playerController.bulletBounces = 2;
                }
                else if(rand == 2) //Speed
                {
                    playerController.aimOrb.sprite = greenSprite;
                    playerController.bulletPrefab = greenBullet;

                    chestEffect.Play("ChestEffectGreen");

                    popupText.text = "Speed Orb";
                    popupImage.sprite = greenSprite;

                    playerController.bulletForce *= 1.5f;
                    playerController.moveSpeed *= 1.25f;
                }
                else if(rand == 3) //Giant
                {
                    playerController.aimOrb.sprite = blueSprite;
                    playerController.bulletPrefab = blueBullet;

                    chestEffect.Play("ChestEffectBlue");

                    popupText.text = "Giant Orb";
                    popupImage.sprite = blueSprite;

                    playerController.bulletForce /= 1.33f;
                    playerController.moveSpeed /= 1.33f;
                    playerController.attackChargeSpeed /= 1.67f;
                    playerController.bulletBounces = 0;
                    playerController.attackCharge = 1;
                    playerController.attackChargeMax = 2;
                }
                else if(rand == 4) //Bounce
                {
                    playerController.aimOrb.sprite = purpleSprite;
                    playerController.bulletPrefab = purpleBullet;

                    chestEffect.Play("ChestEffectPurple");

                    popupText.text = "Bouncy Orb";
                    popupImage.sprite = purpleSprite;

                    playerController.bulletBounces = 5;
                }
                else if(rand == 5) //Death
                {
                    playerController.aimOrb.sprite = greySprite;
                    playerController.bulletPrefab = greyBullet;

                    chestEffect.Play("ChestEffectGrey");

                    popupText.text = "Death Orb";
                    popupImage.sprite = greySprite;

                    hs.life = 1;
                    hs.maxLife = 1;
                    hs.hearts[1].SetActive(false);
                    hs.hearts[2].SetActive(false);
                }
            }
        }
    }
}
