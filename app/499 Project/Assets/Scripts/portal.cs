using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    // Define the next level scene name
    public GameObject nextLevelPrefab;
    private bool playerIsOverPortal = false;
    private bool portalUnlocked = false;
    GameMaster gameMaster;
    public SpriteRenderer spriteRenderer;
    public Sprite ladderSprite;
    public Sprite trapDoorSprite;
    public List<GameObject> allEnemies = new List<GameObject>();
    private void Start()
    {
        gameMaster = GameObject.FindWithTag("GameMaster").GetComponent<GameMaster>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        // Initially, the portal is inactive
        SetPortalActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsOverPortal = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsOverPortal = false;
        }
    }

    private void Update()
    {
        if (playerIsOverPortal && portalUnlocked && Input.GetKeyDown(KeyCode.E))
        {
            portalUnlocked = false;
            gameMaster.LevelComplete();
        }
    }

    // private void LoadNextLevel()
    // {
    //     // Load the next level scene when called
    //     Debug.Log("Loading next level: ");
    //     GameObject[] allObjects = FindObjectsOfType<GameObject>();

    //     // Destroy all objects except for essential ones 
    //     foreach (GameObject obj in allObjects)
    //     {
    //         /*if (obj.CompareTag("Player")) // if we want to keep the player or any other necessary objects
    //             continue;*/

    //         Destroy(obj);
    //     }

    //     // Instantiate the prefab for the next level
    //     Instantiate(nextLevelPrefab);
    // }

    public void SetPortalActive(bool active)
    {
        if(active)
        {
            spriteRenderer.sprite = ladderSprite;
            portalUnlocked = true;
        }
        else
        {
            spriteRenderer.sprite = trapDoorSprite;
            portalUnlocked = false;
        }

    }
}
