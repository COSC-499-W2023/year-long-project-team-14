using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;
using UnityEngine.InputSystem;

public class BrokenWallsTest
{
    private UnityEngine.Object playerPrefab;
    GameObject player;
    PlayerController playerController;

    private UnityEngine.Object templatePrefab;
    GameObject template;

    private GameObject[] bullets;

    [SetUp]
    public void Setup()
    {
        //spawn and set up the player
        playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Players/Mage_player1.prefab");
        player = GameObject.Instantiate(playerPrefab) as GameObject;

        //Spawn and set up the level template with broken walls below the player
        templatePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Levels/TestLevel3.prefab");
        template = GameObject.Instantiate(templatePrefab) as GameObject;

        // Set up the player controller
        playerController = player.GetComponent<PlayerController>();
        playerController.unitTest = true;

        playerController.moveSpeed = 30;
        //Give the player a bullet to shoot
        playerController.attackCharge = 0;
        playerController.bulletForce = 35;
        playerController.attackChargeSpeed = 0;

    }

    [UnityTest]
    public IEnumerator playerCollision()
    {

        // Make the player walk down
        playerController.SetMoveDirection(Vector2.down);

        // Wait for 1 seconds to ensure the player hits the top wall
        yield return new WaitForSeconds(0.5f);

        // Check if the the player is above the broken walls 
        Assert.IsTrue(player.transform.position.y > -6);
    }

    [UnityTest]
    public IEnumerator bulletCollision()
    {

        //Make the player shoot 
        playerController.attackCharge = 1;
        playerController.Shoot();

        //Store the bullet in a variable
        bullets = GameObject.FindGameObjectsWithTag("Player_bullet");

        //Let the bullet travel 
        yield return new WaitForSeconds(0.5f);

        // Check if the bullet is past the broken walls, ie. did not collide.
        Assert.IsTrue(bullets[0].transform.position.y > -6);
    }

    [TearDown]
    public void Teardown()
    {
        // Clean up any objects created during the tests.
        GameObject.Destroy(player);
        GameObject.Destroy(template);
        Object.Destroy(playerController.gameObject);
    }
}
