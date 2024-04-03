using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor; 
using UnityEngine.SceneManagement;

public class VoidBeamTest
{
    private GameObject player;
    private PlayerController playerController;
    private Spells spells;
    private GameObject enemy;
    private EnemyMovement enemyMovement;
    private EnemyHealthSystem enemyHealthSystem;
    private GameObject path;
    private GameObject template;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        //Load into test scene
        SceneManager.LoadScene("Test");
        yield return null;

        //Spawn in the level template 
        template = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Levels/TestLevel.prefab")) as GameObject;
        
        //This allows the bonk to move and avoid obstacles
        path = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Other/Pathfinder.prefab")) as GameObject;

        //Spawn and set up the player
        UnityEngine.Object playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Players/Mage_player1.prefab"); 
        player = GameObject.Instantiate(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity) as GameObject;
        playerController = player.GetComponent<PlayerController>();
        spells = player.GetComponent<Spells>();
        spells.spellName = "VoidBeam";
        playerController.unitTest = true; //this prevents some code from running in PlayerController that requires user input

        //Spawn and set up the enemy
        enemy = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemies/Orc_bonk.prefab"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        enemyHealthSystem = enemy.GetComponent<EnemyHealthSystem>();
        enemyMovement = enemy.GetComponent<EnemyMovement>();
        enemyMovement.movementSpeed = 0;
    }

    [UnityTest]
    public IEnumerator ShootVoidBeamTest()
    {
        //Get enemy health before getting hit by void beam
        int health = enemyHealthSystem.enemyHealth;

        //Shoot void beam at enemy
        spells.VoidBeam();

        //Check that the void beam exists
        GameObject voidBeam = GameObject.Find("VoidBeam(Clone)");
        Assert.IsTrue(voidBeam != null); 

        //Wait for void beam to hit enemy
        yield return new WaitForSeconds(0.1f);

        //Check that the enemy took damage from the void beam
        Assert.IsTrue(enemyHealthSystem.enemyHealth < health); 

        yield return new WaitForSeconds(0.3f);

        //Check that the void beam got deleted
        Assert.IsTrue(voidBeam == null); 
    }

    [UnityTearDown]
    public IEnumerator Teardown()
    {
        //Load into test scene for the next test
        SceneManager.LoadScene("Test");
        yield return null;
    }
}