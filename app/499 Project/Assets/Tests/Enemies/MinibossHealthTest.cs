using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MinibossHealthTest : MonoBehaviour
{
    private GameObject miniboss;
    private MiniBossHealthSystem miniBossHealthSystem;
    private GameObject path;
    private GameObject template;


    [UnitySetUp]
    public IEnumerator Setup()
    {
         //Load in test scene because previous unit test puts you in the main menu which covers up what is happening during the unit test
        SceneManager.LoadScene("Test");
        yield return null;
         
        //Spawn in the level template 
        template = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Levels/TestLevel.prefab")) as GameObject;
        
        //Set up path so the miniboss can move and shoot without an error from the path finding algo
        path = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Other/Pathfinder.prefab")) as GameObject;
        
        //spawn and set up miniboss
        miniboss = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemies/mini_boss_viking.prefab"), new Vector3(-5, 0, 0), Quaternion.identity) as GameObject;

        miniBossHealthSystem = miniboss.GetComponent<MiniBossHealthSystem>();
        miniBossHealthSystem.enemyHealth = 5; //give the miniboss 2 lives
    }

    [UnityTest]
    public IEnumerator miniBossHealthDieTest()
    {

        //Test that miniboss health is properly set to 5 lives when instantiated
        yield return null;
        Assert.AreEqual(5, miniBossHealthSystem.enemyHealth);

        //Test that miniboss healthbar is also set to 5 when instantiated
        yield return null;
        Assert.AreEqual(5, miniBossHealthSystem.healthAmount);

        //Call miniboss health system Die function
        miniBossHealthSystem.Die();
        yield return null;
        
        //Assert that when Die function is called enemy health is set to 0
        Assert.AreEqual(0, miniBossHealthSystem.enemyHealth);
        yield return null;

        //Assert that when miniboss is dead it's attack and hitbox are both disabled 
        Assert.IsFalse(miniBossHealthSystem.shoot.firingEnabled);
        Assert.IsFalse(miniBossHealthSystem.enemyCollider.enabled);
    }

    [TearDown]
    public void Teardown()
    {
        //destroy all gameobjects
        GameObject.Destroy(miniboss);
        GameObject.Destroy(path);
        GameObject.Destroy(template);
    }
}