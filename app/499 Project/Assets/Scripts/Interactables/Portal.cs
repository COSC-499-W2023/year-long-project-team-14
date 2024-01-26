using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject floatingText;

    // Define the next level scene name
    GameMaster gameMaster;
    private bool playerIsOverPortal = false;
    public List<GameObject> allEnemies = new List<GameObject>();
    public GameObject port;
    public bool portalActive = false;
    private void Start()
    {
        // Initially, the portal is inactive
        SetPortalActive(false);
        gameMaster = GameObject.FindWithTag("GameMaster").GetComponent<GameMaster>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsOverPortal = true;
        }
        if (other.CompareTag("Player") && portalActive)
        {
            ShowFloatingText();
        }
    }

    void ShowFloatingText()
    {
        Instantiate(floatingText, transform.position, Quaternion.identity, transform);
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
        if (playerIsOverPortal && portalActive && Input.GetKeyDown(KeyCode.E))
        {
            playerIsOverPortal = false;
            gameMaster.LevelComplete();
        }
    }

    public void SetPortalActive(bool active)
    {
        port.SetActive(active);

        if(active)
            portalActive = true;
        else
            portalActive = false;
    }
}