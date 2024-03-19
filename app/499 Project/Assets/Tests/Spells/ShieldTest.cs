using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor; 
using UnityEngine.SceneManagement;

public class ShieldTest
{
    private GameObject player;
    private PlayerController playerController;
     healthSystem healthSystem;
    private Spells spells;
    private GameObject enemy;
    private EnemyMovement enemyMovement;
    private EnemyHealthSystem enemyHealthSystem;
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
        
        //This allows the bonk to move and avoid obstacles
        path = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Other/Pathfinder.prefab")) as GameObject;

        //Spawn and set up the player
        UnityEngine.Object playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Players/Mage_player1.prefab"); 
        player = GameObject.Instantiate(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity) as GameObject;
        playerController = player.GetComponent<PlayerController>();
        spells = player.GetComponent<Spells>();
        healthSystem = player.GetComponent<healthSystem>();

        //Spawn and set up the enemy
        enemy = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemies/Orc_bonk.prefab"), new Vector3(0, -2, 0), Quaternion.identity) as GameObject;
        enemyHealthSystem = enemy.GetComponent<EnemyHealthSystem>();
        enemyMovement = enemy.GetComponent<EnemyMovement>();
        enemyMovement.movementSpeed = 3;
    }

    [UnityTest]
    public IEnumerator CastShieldTest()
    {
        //cast the shield spell
        spells.ShieldSpell();

        //Check that the shield exists
        GameObject shield = GameObject.Find("Shield(Clone)");
        Assert.IsTrue(shield != null); 

        //Wait for enemy to hit the player
        yield return new WaitForSeconds(2f);

        //Check that the player did not take damage from the enemy
        Assert.IsTrue(healthSystem.life == 3); 

        yield return new WaitForSeconds(4f);

        //Check that the shield got deleted after 10 s 
        Assert.IsTrue(shield == null); 
    }

    [UnityTearDown]
    public IEnumerator Teardown()
    {
         //Load into test scene for the next test
        SceneManager.LoadScene("Test");
        yield return null;
    }
}
