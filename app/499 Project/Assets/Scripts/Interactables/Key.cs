using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public SpriteRenderer prompt;
    public Sprite xPrompt;
    public Sprite aPrompt;
    public Sprite ePrompt;

    private bool playerIsOverKey = false;
    GameMaster gameMaster;
    private void Start()
    {
        //get access to game master
        gameMaster = GameObject.FindWithTag("GameMaster").GetComponent<GameMaster>();
    }

    //player is within range
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.gameObject.GetComponent<healthSystem>().chad)
        {
            playerIsOverKey = true;
           
            prompt.gameObject.SetActive(true);
            if(other.gameObject.GetComponent<PlayerController>().player1)
            {
                if(gameMaster.player1Controls == "PS")
                {
                    prompt.sprite = xPrompt;
                }
                else if(gameMaster.player1Controls == "Xbox")
                {
                    prompt.sprite = aPrompt;
                }
                else
                    prompt.sprite = ePrompt;
            }
            else
            {
                if(gameMaster.player2Controls == "PS")
                {
                    prompt.sprite = xPrompt;
                }
                else if(gameMaster.player2Controls == "Xbox")
                {
                    prompt.sprite = aPrompt;
                }
                else
                    prompt.sprite = ePrompt;
            }
        }
    }

    //player is no longer within range
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsOverKey = false;
            prompt.gameObject.SetActive(false);
        }
    }

    public void Interact()
    {
        if(playerIsOverKey)
        {
            gameMaster.hasKey = true;
            Destroy(gameObject);
        }
    }
}
