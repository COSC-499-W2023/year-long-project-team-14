using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor; 
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class bulletCollisionTest 
{
    private PlayerController playerController;
    private UnityEngine.Object playerPrefab;
    private GameObject player;
    private  GameObject[] bullets;

    private UnityEngine.Object templatePrefab;
    private GameObject template;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        SceneManager.LoadScene("Test");
        yield return null;

        //spawn and set up the player
        playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Mage_player1.prefab"); 
        player = GameObject.Instantiate(playerPrefab) as GameObject;
       
       //Spawn and set up the level template
        templatePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/TestLevel.prefab"); 
        template = GameObject.Instantiate(templatePrefab) as GameObject;

        // Set up the player controller
        playerController = player.GetComponent<PlayerController>();
        playerController.unitTest = true; 
        //Give the player a bullet to shoot
        playerController.attackCharge = 1;
        playerController.bulletForce = 35;

    }

    [UnityTest]
    public IEnumerator bulletCTest()
    {
        //Make the player shoot 
        playerController.Shoot();
       
        //Move the player so the bullet does not collide with it 
        playerController.SetMoveDirection(Vector2.left);

        //Store the bullet in a variable
        bullets = GameObject.FindGameObjectsWithTag("Player_bullet");

        //Let the bullet travel 
        yield return new WaitForSeconds(0.5f); 

        // Check if the bullet is within the level template.
        Assert.IsTrue(bullets[0].transform.position.y < 6.5 && bullets[0].transform.position.y > -7.5); 
    }

    [TearDown]
    public void Teardown()
    {
        // Clean up any objects created during the tests.
        GameObject.Destroy(player);
        Object.Destroy(playerController.gameObject);

        GameObject.Destroy(template);

        for(int i = 0; i < bullets.Length; i++)
            GameObject.Destroy(bullets[i]);
    }

}
