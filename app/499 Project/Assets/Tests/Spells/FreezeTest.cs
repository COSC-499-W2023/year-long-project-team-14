using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor; 
using UnityEngine.SceneManagement;

public class FreezeTest
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
        player = GameObject.Instantiate(playerPrefab, new Vector3(0, 5, 0), Quaternion.identity) as GameObject;
        playerController = player.GetComponent<PlayerController>();
        spells = player.GetComponent<Spells>();
        spells.spellName = "Freeze";
        spells.cooldownTimer = 100;
        playerController.unitTest = true; //this prevents some code from running in PlayerController that requires user input

        //Spawn and set up the enemy
        enemy = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemies/Orc_bonk.prefab"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        enemyHealthSystem = enemy.GetComponent<EnemyHealthSystem>();
        enemyMovement = enemy.GetComponent<EnemyMovement>();
        enemyMovement.movementSpeed = 0;

        //Spawn an enemy bullet
        GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Bullets/EnemyBullet.prefab"), new Vector3(0, 0, 0), Quaternion.identity);
    }

    [UnityTest]
    public IEnumerator FreezeEnemiesTest()
    {
        //Cast the freeze spell
        spells.CastSpell();

        //Check that the enemy components are disabled
        Assert.IsTrue(!enemyMovement.enabled); 

        //Check that the ice cube exists
        GameObject ice = GameObject.Find("IceCube(Clone)");
        Assert.IsTrue(ice != null); 

        //Wait for fireball to hit enemy
        yield return new WaitForSeconds(3.1f);

        //Check that the enemy components are enabled
        Assert.IsTrue(enemyMovement.enabled); 

        //Check that enemy bullets get deleted
        GameObject bullet = GameObject.Find("EnemyBullet");
        Assert.IsTrue(bullet == null); 
    }

    [UnityTearDown]
    public IEnumerator Teardown()
    {
        //Load into test scene for the next test
        SceneManager.LoadScene("Test");
        yield return null;
    }
}
