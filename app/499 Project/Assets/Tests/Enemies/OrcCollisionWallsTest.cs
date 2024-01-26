using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor; 
using UnityEngine.InputSystem;


public class OrcCollisionWalls : MonoBehaviour
{
    private UnityEngine.Object orcPrefab;
    private GameObject orc;
    private EnemyMovement orcController;

    private UnityEngine.Object templatePrefab;
    private GameObject template;

    //private UnityEngine.Object pathfinder;
    private GameObject path;

    [SetUp]
    public void Setup()
    {
        //Have to instantiate this based off of the implementation of the movement for the orc.
        path = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Other/Pathfinder.prefab")) as GameObject;

        //spawn and set up the orc
        orcPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemies/Orc_cyan.prefab"); 
        orc = GameObject.Instantiate(orcPrefab) as GameObject;
        orcController = orc.GetComponent<EnemyMovement>();
        orcController.movementSpeed = 35;
       
       //Spawn and set up the level template
        templatePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Levels/TestLevel.prefab"); 
        template = GameObject.Instantiate(templatePrefab) as GameObject;
    }


    [UnityTest]
    public IEnumerator OrcCollisionTestTop()
    {
        // Make the orc walk up 
        orcController.NewTarget((float) 0, (float)6.5);

        //Make the orc move immediately
        yield return null;
        orcController.waitTime = 0;

        // Wait for 1 seconds to ensure the orc hits the top wall
        yield return new WaitForSeconds(1f); 

        // Check if the the orc is within the level template 
        Assert.IsTrue(orc.transform.position.y < 6.5); 
    }

    [UnityTest]
    public IEnumerator OrcCollisionTestBottom()
    {
        
        // Make the orc walk down 
        orcController.NewTarget((float) 0,(float) -7.5);

        //Make the orc move immediately
        yield return null;
        orcController.waitTime = 0;

        // Wait for 1 seconds to ensure the orc hits the top wall
        yield return new WaitForSeconds(1f);

        // Check if the the orc is within the level template 
        Assert.IsTrue(orc.transform.position.y > -7.5); 
    }

    [UnityTest]
    public IEnumerator OrcCollisionTestLeft()
    {
        
        // Make the orc walk left 
        orcController.NewTarget((float) -11.5,(float) 0);

        //Make the orc move immediately
        yield return null;
        orcController.waitTime = 0;

        // Wait for 1 seconds to ensure the orc hits the top wall
        yield return new WaitForSeconds(1f); 

        // Check if the the orc is within the level template 
        Assert.IsTrue(orc.transform.position.x > -11.5);  
    }

    [UnityTest]
    public IEnumerator OrcCollisionTestRight()
    {
        
        // Make the orc walk right 
        orcController.NewTarget((float) 11.5,(float) 0);

        //Make the orc move immediately
        yield return null;
        orcController.waitTime = 0;

        // Wait for 1 seconds to ensure the orc hits the top wall
        yield return new WaitForSeconds(1f);

        // Check if the the orc is within the level template 
        Assert.IsTrue(orc.transform.position.x < 11.5); 
    }


    [TearDown]
    public void Teardown()
    {
        // Clean up any objects created during the tests.
        GameObject.Destroy(orc);
        GameObject.Destroy(template);
        GameObject.Destroy(path);
    }

}
