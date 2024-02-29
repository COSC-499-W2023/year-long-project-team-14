using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class OrcHealthTest
{
    private GameObject orc;
    private EnemyHealthSystem enemyHealthSystem;
    private GameObject path;
    private GameObject template;


    [SetUp]
    public void Setup()
    {
        //spawn and set up player 1
        UnityEngine.Object orcPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemies/Orc_cyan.prefab");
        orc = GameObject.Instantiate(orcPrefab) as GameObject;

        enemyHealthSystem = orc.GetComponent<EnemyHealthSystem>();
        enemyHealthSystem.enemyHealth = 2; //give the enemy 2 lives

        //Set up path so the orc can move and shoot without an error from the path finding algo
        path = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Other/Pathfinder.prefab")) as GameObject;


        //Spawn in the level template 
        template = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Levels/TestLevel.prefab")) as GameObject;
    }

    [UnityTest]
    public IEnumerator orcHealth()
    {
        

        enemyHealthSystem.takeDamage();
        // Assert the orc lost a life
        yield return null;
        Assert.AreEqual(1, enemyHealthSystem.enemyHealth);
        enemyHealthSystem.takeDamage();
        // Assert the orc lost another life
        yield return null;
        Assert.AreEqual(-1, enemyHealthSystem.enemyHealth);
        //check that  when the orc dies, he cannot move, attack, or be collided with
        enemyHealthSystem.Die();
        yield return null;
        Assert.IsFalse(enemyHealthSystem.enemyMovement.enabled);
        Assert.IsFalse(enemyHealthSystem.ea.enabled);
        Assert.IsFalse(enemyHealthSystem.enemyCollider.enabled);
        
    }


 /*   [UnityTest]
    public IEnumerator portalTest()
    {
        GameObject ladderObject = new GameObject();
        Ladder ladder = ladderObject.AddComponent<Ladder>();
        ladder.allEnemies.Add(orc);
        enemyHealthSystem.ladder = ladder;
        Assert.IsFalse(ladder.gameObject.activeSelf);
        enemyHealthSystem.Die();
        yield return new WaitForSeconds(1f);
        Assert.IsTrue(ladder.gameObject.activeSelf);

    }*/

    [TearDown]
    public void Teardown()
    {
        //destroy all gameobjects
        GameObject.Destroy(orc);
        GameObject.Destroy(path);
        GameObject.Destroy(template);
    }
}