using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    private bool playerIsOverExit = false;
    public bool exitUnlocked = false;
    GameMaster gameMaster;
    public SpriteRenderer spriteRenderer;
    public Sprite ladderSprite;
    public Sprite trapDoorSprite;
    public List<GameObject> allEnemies = new List<GameObject>();
    private void Start()
    {
        //get access to game master
        gameMaster = GameObject.FindWithTag("GameMaster").GetComponent<GameMaster>();

        //set exit inactive to start
        SetLadderActive(false);
    }

    //check if player is on exit
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsOverExit = true;
        }
    }

    //check if player left exit
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsOverExit = false;
        }
    }

    //go to next level if player interacts with exit and satisfies conditions
    private void Update()
    {
        if (playerIsOverExit && exitUnlocked && Input.GetKeyDown(KeyCode.E))
        {
            exitUnlocked = false;
            gameMaster.LevelComplete();
        }
    }

    //sets exit active or inactives
    public void SetLadderActive(bool active)
    {
        if(active)
        {
            spriteRenderer.sprite = ladderSprite;
            exitUnlocked = true;
        }
        else
        {
            spriteRenderer.sprite = trapDoorSprite;
            exitUnlocked = false;
        }

    }
}
