using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor; 
using UnityEngine.SceneManagement;

public class SeekingOrbTest
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

        //Spawn in a level with a wall that the orbs have to navigate around
        template = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Levels/TestLevel2.prefab")) as GameObject;

        //This allows the orbs to move and avoid obstacles
        path = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Other/Pathfinder.prefab")) as GameObject;

        //Spawn and set up the player
        UnityEngine.Object playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Players/Mage_player1.prefab"); 
        player = GameObject.Instantiate(playerPrefab, new Vector3(-10, 0, 0), Quaternion.identity) as GameObject;
        playerController = player.GetComponent<PlayerController>();
        spells = player.GetComponent<Spells>();
        spells.spellName = "SeekingOrb";
        playerController.unitTest = true; //this prevents some code from running in PlayerController that requires user input

        //Spawn and set up the enemy
        enemy = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemies/Orc_bonk.prefab"), new Vector3(10, 0, 0), Quaternion.identity) as GameObject;
        enemyHealthSystem = enemy.GetComponent<EnemyHealthSystem>();
        enemyHealthSystem.enemyHealth = 5;
        enemyMovement = enemy.GetComponent<EnemyMovement>();
        enemyMovement.movementSpeed = 0;
    }

    [UnityTest]
    public IEnumerator ShootOrbTest()
    {
        //Get enemy health before getting hit by the orbs
        int health = enemyHealthSystem.enemyHealth;

        //Cast the orb spell
        spells.SeekingOrb();

        //Check that the orbs exists
        GameObject[] orbs = GameObject.FindGameObjectsWithTag("SeekingOrb");
        Assert.IsTrue(orbs.Length > 0); 

        //Wait for the orbs to hit the enemy
        yield return new WaitUntil(() => enemyHealthSystem.enemyHealth <= 0);
        yield return new WaitForSeconds(0.1f);
    
        //Check that the enemy got hit by the orbs
        Assert.IsTrue(enemyHealthSystem.enemyHealth < health);

        //Check that the orbs got deleted after colliding with something
        orbs = GameObject.FindGameObjectsWithTag("SeekingOrb");
        Assert.IsTrue(orbs.Length == 0); 
    }

    [UnityTearDown]
    public IEnumerator Teardown()
    {
        //Load into test scene for the next test
        SceneManager.LoadScene("Test");
        yield return null;
    }
}
