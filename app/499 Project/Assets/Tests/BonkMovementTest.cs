using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor; 
using UnityEngine.SceneManagement;

public class BonkMovementTest : MonoBehaviour
{
    private GameObject player;
    private PlayerController playerController;
    private healthSystem healthScript;
    private GameObject bonk;
    private EnemyMovement enemyMovement;
    private GameObject path;
    private GameObject template;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        //Load into test scene
        SceneManager.LoadScene("Test");
        yield return null;

        //Spawn in a level with a wall that the enemy has to navigate around
        template = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/TestLevel2.prefab")) as GameObject;

        //This allows the bonk to move and avoid obstacles
        path = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Pathfinder.prefab")) as GameObject;

        //Spawn and set up the player
        UnityEngine.Object playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Mage_player1.prefab"); 
        player = GameObject.Instantiate(playerPrefab, new Vector3(10, 0, 0), Quaternion.identity) as GameObject;
        playerController = player.GetComponent<PlayerController>();
        healthScript = player.GetComponent<healthSystem>();
        playerController.unitTest = true; //this prevents some code from running in PlayerController that requires user input

        //Spawn and set up the bonk
        bonk = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Orc_bonk.prefab"), new Vector3(-10, 0, 0), Quaternion.identity) as GameObject;
        enemyMovement = bonk.GetComponent<EnemyMovement>();
        enemyMovement.movementSpeed = 35;
        enemyMovement.noWait = true;
    }

    [UnityTest]
    public IEnumerator BonkMoveTest()
    {
        //Wait for the bonk to move through the level to get to the player
        yield return new WaitUntil(() => bonk.transform.position.x >= 9.5);

        //Check if the the bonk reached the player and damaged them
        Assert.IsTrue(healthScript.life == 2); 
    }

    [UnityTearDown]
    public IEnumerator Teardown()
    {
        //Load into test scene for the next test
        SceneManager.LoadScene("Test");
        yield return null;
    }
}
