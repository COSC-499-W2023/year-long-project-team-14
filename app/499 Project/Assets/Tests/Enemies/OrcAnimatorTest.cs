using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor; 
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class OrcAnimatorTest : MonoBehaviour
{
    private UnityEngine.Object orcPrefab;
    private GameObject orc;
    private EnemyMovement orcController;

    private UnityEngine.Object templatePrefab;
    private GameObject template;

    private GameObject path;
    private Animator animator;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        //Load in test scene because previous unit test puts you in the main menu which covers up what is happening during the unit test
        SceneManager.LoadScene("Test");
        yield return null;

        //Have to instantiate this based off of the implementation of the movement for the orc.
        path = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Other/Pathfinder.prefab")) as GameObject;

        //Spawn and set up the orc
        orcPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemies/Orc_cyan.prefab"); 
        orc = GameObject.Instantiate(orcPrefab) as GameObject;
        orcController = orc.GetComponent<EnemyMovement>();
        orcController.movementSpeed = 35;
       
       //Spawn and set up the emtpy level template so that the orc can freely walk in the room 
        templatePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Levels/TestLevel.prefab"); 
        template = GameObject.Instantiate(templatePrefab) as GameObject;
        
        //Setup Animator Component
        animator = orc.GetComponent<Animator>();
    }

    [UnityTest]
    public IEnumerator OrcAnimatorTestLeft()
    {
        // Make the orc walk left 
        orcController.NewTarget((float) -11.5,(float) 0);

        //Make the orc move immediately
        yield return null;
        orcController.waitTime = 0;

        // Wait for 1 second to ensure orc has started walking
        yield return new WaitForSeconds(0.5f); 

        // Check if the correct animation x float is stored for playing the corresponding animation from Animator tree
        Assert.IsTrue(animator.GetFloat("X") < 0); 
    }

    [UnityTest]
    public IEnumerator OrcAnimatorTestRight()
    {
        // Make the orc walk right 
        orcController.NewTarget((float) 11.5,(float) 0);

        //Make the orc move immediately
        yield return null;
        orcController.waitTime = 0;

        // Wait for 1 second to ensure orc has started walking
        yield return new WaitForSeconds(0.5f);

        // Check if the correct animation x float is stored for playing the corresponding animation from Animator tree
        Assert.IsTrue(animator.GetFloat("X") > 0); 
    }

    [UnityTest]
    public IEnumerator OrcAnimatorDeadTriggerTest()
    {
        // Check that while Orc is alive the Animator tree parameter "dead" is not active and assign boolean to check if the trigger is not active
        bool isDeadTriggerActive = animator.GetCurrentAnimatorStateInfo(0).IsName("Death");

        // Use NUnit Assert to check if the trigger is false
        Assert.IsFalse(isDeadTriggerActive, "The 'Death' trigger should not be active after Orc dies.");
        
        // End the coroutine
        yield break;
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


