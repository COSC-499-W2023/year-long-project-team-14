using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    public GameObject floatingText;

    private bool playerIsOverExit = false;
    public bool exitUnlocked = false;
    GameMaster gameMaster;
    public SpriteRenderer spriteRenderer;
    public Sprite ladderSprite;
    public Sprite trapDoorSprite;
    public List<GameObject> allEnemies = new List<GameObject>();
    public GameObject ladderArrow;
    private void Start()
    {
        //get access to game master
        gameMaster = GameObject.FindWithTag("GameMaster").GetComponent<GameMaster>();

        //set exit inactive to start if not in shop
        if(!gameMaster.inShop)
            SetLadderActive(false);
        else
            SetLadderActive(true);
    }

    //player is within range
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(gameMaster.currentLevel == 1 && ladderArrow != null)
                ladderArrow.SetActive(false);

            playerIsOverExit = true;
        }
        if (other.CompareTag("Player") && exitUnlocked)
        {
            ShowFloatingText();
        }
    }
    void ShowFloatingText()
    {
        Instantiate(floatingText, transform.position, Quaternion.identity, transform);
    }

    //player is no longer within range
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsOverExit = false;
        }
    }

    //go to next level if player interacts with exit and satisfies conditions
    public void Interact()
    {
        if (playerIsOverExit && exitUnlocked)
        {
            exitUnlocked = false;
            gameMaster.LevelComplete();
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
        }
        else
        {
            spriteRenderer.sprite = trapDoorSprite;
            exitUnlocked = false;
        }

    }
}
