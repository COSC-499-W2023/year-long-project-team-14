using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor; 
using UnityEngine.InputSystem;

public class PlayerAnimationTest
{
    private UnityEngine.Object playerPrefab;
    GameObject player;
    private GameObject template;
    PlayerController playerController;
    private Animator animator;

    [SetUp]
    public void Setup()
    {
        //Spawn in the level template 
        template = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Levels/TestLevel.prefab")) as GameObject;

        //spawn and set up the player
        playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Players/Mage_player1.prefab"); 
        player = GameObject.Instantiate(playerPrefab) as GameObject;
        playerController = player.GetComponent<PlayerController>();
        playerController.unitTest = true; //this prevents some code from running in PlayerController that requires user input

        // Assign the player controller and animator components.
        playerController = player.GetComponent<PlayerController>();
        animator = player.GetComponent<Animator>();
    }

    [UnityTest]
    public IEnumerator PlayerAnimationTestRight()
    {
        
        // Simulate move input walking right.
        playerController.SetMoveDirection(Vector2.right);

        // Wait for a few frames or a condition to ensure the animation has played.
        yield return new WaitForSeconds(0.1f); // Adjust the time as needed.

        // Check if the correct animation x float is stored for playing the corresponding animation from Animator tree and check if Boolean IsWalking is true while input is set to walk right
        Assert.IsTrue(animator.GetFloat("X") > 0 && animator.GetBool("IsWalking") == true); 
    }

    [UnityTest]
    public IEnumerator PlayerAnimationTestLeft()
    {
        
        // Simulate move input walking right.
        playerController.SetMoveDirection(Vector2.left);

        // Wait for a few frames or a condition to ensure the animation has played.
        yield return new WaitForSeconds(0.1f); // Adjust the time as needed.

        // Check if the correct animation x float is stored for playing the corresponding animation from Animator tree and check if Boolean IsWalking is true while input is set to walk right
        Assert.IsTrue(animator.GetFloat("X") < 0 && animator.GetBool("IsWalking") == true); 
    }

    [UnityTest]
    public IEnumerator PlayerAnimationTestUp()
    {
        
        // Simulate move input walking right.
        playerController.SetMoveDirection(Vector2.up);

        // Wait for a few frames or a condition to ensure the animation has played.
        yield return new WaitForSeconds(0.1f); // Adjust the time as needed.

        // Check if the correct animation x float is stored for playing the corresponding animation from Animator tree and check if Boolean IsWalking is true while input is set to walk right
        Assert.IsTrue(animator.GetFloat("Y") > 0 && animator.GetBool("IsWalking") == true); 
    }

    [UnityTest]
    public IEnumerator PlayerAnimationTestDown()
    {
        
        // Simulate move input walking right.
        playerController.SetMoveDirection(Vector2.down);

        // Wait for a few frames or a condition to ensure the animation has played.
        yield return new WaitForSeconds(0.1f); // Adjust the time as needed.

        // Check if the correct animation x float is stored for playing the corresponding animation from Animator tree and check if Boolean IsWalking is true while input is set to walk right
        Assert.IsTrue(animator.GetFloat("Y") < 0 && animator.GetBool("IsWalking") == true); 
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