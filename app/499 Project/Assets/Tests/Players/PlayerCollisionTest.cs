using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor; 
using UnityEngine.InputSystem;

public class PlayerCollisionTest 
{
    private UnityEngine.Object playerPrefab;
    GameObject player;
    PlayerController playerController;

    private UnityEngine.Object templatePrefab;
    GameObject template;

    [SetUp]
    public void Setup()
    {
        //spawn and set up the player
        playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Players/Mage_player1.prefab"); 
        player = GameObject.Instantiate(playerPrefab) as GameObject;
       
       //Spawn and set up the level template
        templatePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Levels/TestLevel.prefab"); 
        template = GameObject.Instantiate(templatePrefab) as GameObject;

        // Set up the player controller
        playerController = player.GetComponent<PlayerController>();
        playerController.unitTest = true; 

        playerController.moveSpeed = 30;

    }

    [UnityTest]
    public IEnumerator PlayerCollisionTestTop()
    {
        
         // Make the character walk up 
        playerController.SetMoveDirection(Vector2.up);

        // Wait for 1 seconds to ensure the player hits the top wall
        yield return new WaitForSeconds(1f); 

        // Check if the the player is within the level template 
        Assert.IsTrue(player.transform.position.y < 6.5); 
    }

    [UnityTest]
    public IEnumerator PlayerCollisionTestBottom()
    {
        
         // Make the character walk down 
        playerController.SetMoveDirection(Vector2.down);

        // Wait for 1 seconds to ensure the player hits the bottom wall
        yield return new WaitForSeconds(1f);

        // Check if the the player is within the level template 
        Assert.IsTrue(player.transform.position.y > -7.5); 
    }

    [UnityTest]
    public IEnumerator PlayerCollisionTestLeft()
    {
        
         // Make the character walk left 
        playerController.SetMoveDirection(Vector2.left);

        // Wait for 1 seconds to ensure the player hits the left wall
        yield return new WaitForSeconds(1f); 

        // Check if the the player is within the level template 
        Assert.IsTrue(player.transform.position.x > -11.5);  
    }

    [UnityTest]
    public IEnumerator PlayerCollisionTestRight()
    {
        
         // Make the character walk right 
        playerController.SetMoveDirection(Vector2.right);

        // Wait for 1 seconds to ensure the player hits the right wall
        yield return new WaitForSeconds(1f);

        // Check if the the player is within the level template 
        Assert.IsTrue(player.transform.position.x < 11.5); 
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
