using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    public SpriteRenderer prompt;
    public Sprite xPrompt;
    public Sprite aPrompt;
    public Sprite ePrompt;

    private bool playerIsOverExit = false;
    public bool exitUnlocked = false;
    GameMaster gameMaster;
    public SpriteRenderer spriteRenderer;
    public Sprite ladderSprite;
    public Sprite trapDoorSprite;
    public List<GameObject> allEnemies = new List<GameObject>();
    public GameObject ladderArrow;
    public int secretExit = 0;

    [SerializeField] private AudioSource levelCompleteSound;
    private void Start()
    {
        //get access to game master
        gameMaster = GameObject.FindWithTag("GameMaster").GetComponent<GameMaster>();

        //set exit inactive to start if not in shop
        if(!gameMaster.inShop && secretExit != 3 && secretExit != 5 && secretExit != 7  && secretExit != 8)
            SetLadderActive(false);
        else
            SetLadderActive(true);
    }


    //player is within range
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.gameObject.GetComponent<healthSystem>().chad)
        {
            playerIsOverExit = true;
            
            if(gameMaster.currentLevel == 1 && exitUnlocked && ladderArrow != null)
                ladderArrow.SetActive(false);
            
            if(exitUnlocked)
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
            playerIsOverExit = false;
            prompt.gameObject.SetActive(false);
        }
    }

    //go to next level if player interacts with exit and satisfies conditions
    public void Interact()
    {
        if (playerIsOverExit && exitUnlocked)
        {
            exitUnlocked = false;
            gameMaster.LevelComplete();

            if(secretExit != 0)
                gameMaster.secretExit = secretExit;
        }
    }

    //sets exit active or inactive
    public void SetLadderActive(bool active)
    {
        if(active)
        {
            spriteRenderer.sprite = ladderSprite;
            exitUnlocked = true;
            if(gameMaster.currentLevel == 1 && ladderArrow != null)
                ladderArrow.SetActive(true);

            if (!gameMaster.inShop && (secretExit == 0 || secretExit == 2))
            {
                levelCompleteSound.Play();
            }
        }
        else
        {
            spriteRenderer.sprite = trapDoorSprite;
            exitUnlocked = false;
        }

    }
}
