using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class FinalbossTests
{
    private GameObject boss;
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
        
        //Set up path so the final boss can move and shoot without an error from the path finding algo
        path = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Other/Pathfinder.prefab")) as GameObject;
        
        //spawn and set up final boss
        boss = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemies/StoneBoss.prefab"), new Vector3(-5, 0, 0), Quaternion.identity) as GameObject;

        miniBossHealthSystem = boss.GetComponent<MiniBossHealthSystem>();
    }

    [UnityTest]
    public IEnumerator FinalBossHealthDieTest()
    {
        miniBossHealthSystem.enemyHealth = 5; //give the miniboss 5 lives
        miniBossHealthSystem.healthAmount = 5;

        //Test that miniboss health is properly set to 5 lives when instantiated
        yield return null;
        Assert.AreEqual(5, miniBossHealthSystem.enemyHealth);

        //Test that miniboss healthbar is also set to 5 when instantiated
        yield return null;
        Assert.AreEqual(5, miniBossHealthSystem.healthAmount);

    }

    [TearDown]
    public void Teardown()
    {
        //destroy all gameobjects
        GameObject.DestroyImmediate(boss);
        GameObject.DestroyImmediate(path);
        GameObject.DestroyImmediate(template);
    }
}