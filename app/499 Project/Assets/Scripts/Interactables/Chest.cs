using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    bool chestOpened = false;
    GameMaster gameMaster;
    public SpriteRenderer spriteRenderer;
    public Sprite openChest;
    private void Start()
    {
        //get access to game master
        gameMaster = GameObject.FindWithTag("GameMaster").GetComponent<GameMaster>();

        if(gameMaster.hasKey)
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
    public void Interact()
    {
        if(playerIsOverChest && gameMaster.hasKey)
        {
            if(!chestOpened)
            {
                chestOpened = true;
                spriteRenderer.sprite = openChest;
                Destroy(prompt.gameObject);
            }
        }
    }
}
