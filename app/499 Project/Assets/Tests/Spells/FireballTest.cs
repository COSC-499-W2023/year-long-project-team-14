using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor; 
using UnityEngine.SceneManagement;

public class FireballTest
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
        spells.spellName = "Fireball";
        playerController.unitTest = true; //this prevents some code from running in PlayerController that requires user input

        //Spawn and set up the enemy
        enemy = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemies/Orc_bonk.prefab"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        enemyHealthSystem = enemy.GetComponent<EnemyHealthSystem>();
        enemyMovement = enemy.GetComponent<EnemyMovement>();
        enemyMovement.movementSpeed = 0;
    }

    [UnityTest]
    public IEnumerator ShootFireballTest()
    {
        //Get enemy health before getting hit by fireball
        int health = enemyHealthSystem.enemyHealth;

        //Shoot a fireball at enemy
        spells.FireballSpell();

        //Check that the fireball exists
        GameObject fireball = GameObject.FindWithTag("Fireball");
        Assert.IsTrue(fireball != null); 

        //Wait for fireball to hit enemy
        yield return new WaitForSeconds(0.5f);

        //Check that the enemy took damage from the fireball
        Assert.IsTrue(enemyHealthSystem.enemyHealth < health); 

        //Check that the fireball got deleted after colliding with something
        Assert.IsTrue(fireball == null); 
    }

    [UnityTearDown]
    public IEnumerator Teardown()
    {
        //Load into test scene for the next test
        SceneManager.LoadScene("Test");
        yield return null;
    }
}
