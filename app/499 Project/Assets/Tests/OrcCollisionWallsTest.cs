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
    GameObject orc;
    EnemyMovement1 orcController;

    private UnityEngine.Object templatePrefab;
    GameObject template;

    //private UnityEngine.Object pathfinder;
    GameObject path;

  

   

    [SetUp]
    public void Setup()
    {
        path = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Pathfinder.prefab")) as GameObject;

        //spawn and set up the player
        orcPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Orc_cyan.prefab"); 
        orc = GameObject.Instantiate(orcPrefab) as GameObject;
        orcController = orc.GetComponent<EnemyMovement1>();
       
       //Spawn and set up the level template
        templatePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/LevelTemplate.prefab"); 
        template = GameObject.Instantiate(templatePrefab) as GameObject;
    }




    [UnityTest]
    public IEnumerator OrcCollisionTestTop()
    {

        orcController.NewTarget((float) 0, (float)6.5);
        InvokeRepeating("UpdatePath", 0f, 0.25f);


        // Wait for 5 seconds to ensure the player hits the top wall
        yield return new WaitForSeconds(5.0f); 

        // Check if the the player is within the level template 
        Assert.IsTrue(orc.transform.position.y < 6.5); 
    }

    [UnityTest]
    public IEnumerator OrcCollisionTestBottom()
    {
        
         // Make the character walk down 
         orcController.NewTarget((float) 0,(float) -7.5);
        InvokeRepeating("UpdatePath", 0f, 0.25f);

        // Wait for 5 seconds to ensure the player hits the top wall
        yield return new WaitForSeconds(5.0f);

        // Check if the the player is within the level template 
        Assert.IsTrue(orc.transform.position.y > -7.5); 
    }

    [UnityTest]
    public IEnumerator OrcCollisionTestLeft()
    {
        
         // Make the character walk left 
         orcController.NewTarget((float) -11.5,(float) 0);
        InvokeRepeating("UpdatePath", 0f, 0.25f);

        // Wait for 5 seconds to ensure the player hits the top wall
        yield return new WaitForSeconds(5.0f); 

        // Check if the the player is within the level template 
        Assert.IsTrue(orc.transform.position.x > -11.5);  
    }

    [UnityTest]
    public IEnumerator OrcCollisionTestRight()
    {
        
         // Make the character walk right 
         orcController.NewTarget((float) 11.5,(float) 0);
        InvokeRepeating("UpdatePath", 0f, 0.25f);

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
        GameObject.Destroy(template);
    }

}
