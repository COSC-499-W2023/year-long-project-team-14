using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    // Define the next level scene name
    GameMaster gameMaster;
    private bool playerIsOverPortal = false;
    public static bool portalExists = false;
    public List<GameObject> allEnemies = new List<GameObject>();
    private void Start()
    {
        // Initially, the portal is inactive
        portalExists = true;
        SetPortalActive(false);
        gameMaster = GameObject.FindWithTag("GameMaster").GetComponent<GameMaster>();
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
        if (playerIsOverPortal && Input.GetKeyDown(KeyCode.E))
        {
            portalExists = false;
            gameMaster.LevelComplete();
        }
    }

    public void SetPortalActive(bool active)
    {
        gameObject.SetActive(active);
    }
}