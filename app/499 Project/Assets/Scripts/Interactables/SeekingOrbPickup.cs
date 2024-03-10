using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekingOrbPickup : MonoBehaviour
{
    private bool playerIsOver = false;
    GameObject player;
    public float bobbingAmount = 0.1f; // Amount of bobbing movement
    public float bobbingSpeed = 0.5f; // Speed of bobbing movement
    private float startY; // Initial Y position of the sprite

    [SerializeField] private promptUi prompt;

    void Start()
    {
        startY = transform.position.y; // Store the initial Y position
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
            if (!prompt.isDisplayed) prompt.SetUp();
        }
    }

    //player is no longer within range
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsOver = false;
            if (prompt.isDisplayed) prompt.Close();
        }
    }

    //pick up if player interacts and satisfies conditions
    public void Interact()
    {
        if (playerIsOver)
        {
            playerIsOver = false;
            player.GetComponent<Spells>().spellName = "SeekingOrb";
            Destroy(gameObject);
        }
    }

}