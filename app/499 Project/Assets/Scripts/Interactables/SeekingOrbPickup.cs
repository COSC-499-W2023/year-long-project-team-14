using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeekingOrbPickup : MonoBehaviour
{
    private bool playerIsOver = false;
    GameObject player;
    public float bobbingAmount = 0.1f; // Amount of bobbing movement
    public float bobbingSpeed = 0.5f; // Speed of bobbing movement
    private float startY; // Initial Y position of the sprite

    [SerializeField] public promptUi prompt;

    public GameMaster gameMaster;
    public Image promptIcon;
    public Sprite xPrompt;
    public Sprite aPrompt;
    public Sprite ePrompt;

    void Start()
    {
        startY = transform.position.y; // Store the initial Y position

        GameObject g = GameObject.FindWithTag("GameMaster");
        if(g != null)
            gameMaster = g.GetComponent<GameMaster>();
    }

    void Update()
    {
        // Calculate the new Y position using Mathf.PingPong
        float newY = startY + Mathf.PingPong(Time.time * bobbingSpeed, bobbingAmount * 2);

        // Update the sprite's position using Translate for smooth movement
        transform.Translate(0f, newY - transform.position.y, 0f);
    }

    //player is within range
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsOver = true;
            player = other.gameObject;
            if (!prompt.isDisplayed)
            {
                prompt.SetUp();
                promptIcon.gameObject.SetActive(true);
            }

            //Change prompt icon depending on controls
            if(gameMaster != null && other.gameObject.GetComponent<PlayerController>().player1)
            {
                if(gameMaster.player1Controls == "PS")
                {
                    promptIcon.sprite = xPrompt;
                }
                else if(gameMaster.player1Controls == "Xbox")
                {
                    promptIcon.sprite = aPrompt;
                }
                else
                    promptIcon.sprite = ePrompt;
            }
            else if(gameMaster != null)
            {
                if(gameMaster.player2Controls == "PS")
                {
                    promptIcon.sprite = xPrompt;
                }
                else if(gameMaster.player2Controls == "Xbox")
                {
                    promptIcon.sprite = aPrompt;
                }
                else
                    promptIcon.sprite = ePrompt;
            }
        }
    }

    //player is no longer within range
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsOver = false;
            if (prompt.isDisplayed)
            {
                prompt.Close();
                promptIcon.gameObject.SetActive(false);
            }
        }
    }

    //pick up if player interacts and satisfies conditions
    public void Interact()
    {
        if (playerIsOver)
        {
            playerIsOver = false;
            player.GetComponent<Spells>().spellName = "SeekingOrb";
            player.GetComponent<Spells>().spellCooldown = 15;
            Destroy(gameObject);
        }
    }

}