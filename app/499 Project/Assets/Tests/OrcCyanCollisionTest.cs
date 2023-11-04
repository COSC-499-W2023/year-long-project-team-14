using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor; 
using UnityEngine.InputSystem;

public class OrcCyanCollisionTest 
{
    private UnityEngine.Object orcPreFab;
    private GameObject orc;
    private PlayerController orcController;

    private UnityEngine.Object templatePrefab;
    private GameObject template;

    [SetUp]
    public void Setup()
    {
        //spawn and set up the player
        orcPreFab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Orc_cyan.prefab"); 
        orc = GameObject.Instantiate(orcPreFab) as GameObject;
       
       //Spawn and set up the level template
        templatePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/LevelTemplate.prefab"); 
        template = GameObject.Instantiate(templatePrefab) as GameObject;

        // Set up the player controller
        orcController = orc.GetComponent<PlayerController>();
        orcController.unitTest = true; 

    }

    [UnityTest]
    public IEnumerator OrcCollisionTestTop()
    {
        
         // Make the character walk up 
        orcController.SetMoveDirection(Vector2.up);

        // Wait for 5 seconds to ensure the player hits the top wall
        yield return new WaitForSeconds(5.0f); 

        // Check if the the player is within the level template 
        Assert.IsTrue(orc.transform.position.y < 6.5); 
    }

    [UnityTest]
    public IEnumerator OrcCollisionTestBottom()
    {
        
         // Make the character walk down 
        orcController.SetMoveDirection(Vector2.down);

        // Wait for 5 seconds to ensure the player hits the top wall
        yield return new WaitForSeconds(5.0f);

        // Check if the the player is within the level template 
        Assert.IsTrue(orc.transform.position.y > -7.5); 
    }

    [UnityTest]
    public IEnumerator OrcCollisionTestLeft()
    {
        
         // Make the character walk left 
        orcController.SetMoveDirection(Vector2.left);

        // Wait for 5 seconds to ensure the player hits the top wall
        yield return new WaitForSeconds(5.0f); 

        // Check if the the player is within the level template 
        Assert.IsTrue(orc.transform.position.x > -11.5);  
    }

    [UnityTest]
    public IEnumerator OrcCollisionTestRight()
    {
        
         // Make the character walk right 
        orcController.SetMoveDirection(Vector2.right);

        // Wait for 5 seconds to ensure the player hits the top wall
        yield return new WaitForSeconds(5.0f);

        // Check if the the player is within the level template 
        Assert.IsTrue(orc.transform.position.x < 11.5); 
    }

    [TearDown]
    public void Teardown()
    {
        // Clean up any objects created during the tests.
        GameObject.Destroy(orc);
        Object.Destroy(orcController.gameObject);

        GameObject.Destroy(template);
    }
}

