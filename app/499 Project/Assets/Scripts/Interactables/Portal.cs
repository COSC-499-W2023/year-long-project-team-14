using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public SpriteRenderer prompt;
    public Sprite xPrompt;
    public Sprite aPrompt;
    public Sprite ePrompt;

    // Define the next level scene name
    GameMaster gameMaster;
    private bool playerIsOverPortal = false;
    public List<GameObject> allEnemies = new List<GameObject>();
    public GameObject port;
    public bool portalActive = false;

    [SerializeField] private AudioSource levelCompleteSound;
    private void Start()
    {
        // Initially, the portal is inactive
        SetPortalActive(false);
        gameMaster = GameObject.FindWithTag("GameMaster").GetComponent<GameMaster>();
    }

    //player is within range
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !other.gameObject.GetComponent<healthSystem>().chad)
        {
            playerIsOverPortal = true;

            if(portalActive)
            {
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
    }

    //player is no longer within range
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsOverPortal = false;
            prompt.gameObject.SetActive(false);
        }
    }

    //go to next level if player interacts with exit and satisfies conditions
    public void Interact()
    {
        if (playerIsOverPortal && portalActive)
        {
            portalActive = false;
            playerIsOverPortal = false;
            gameMaster.LevelComplete();
        }
    }

    //sets exit active or inactive
    public void SetPortalActive(bool active)
    {
        port.SetActive(active);

        if (active)
        {
            portalActive = true;
            if (!gameMaster.inShop)
            {
                levelCompleteSound.Play();
            }
        }
        else
            portalActive = false;
    }
}