using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class DashTest : MonoBehaviour
{
    private PlayerController playerController;
    private GameObject player;
    private healthSystem healthSystem;

    private GameObject orc;
    private EnemyAttack enemyAttack;
    private GameObject path;

    private GameObject cam;




    [UnitySetUp]
    public IEnumerator Setup()
    {
        //Load in test scene because previous unit test puts you in the main menu which covers up what is happening during the unit test
        SceneManager.LoadScene("Test");
        yield return null;

        //spawn and set up the player
        UnityEngine.Object playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Players/Mage_player1.prefab"); 
        player = GameObject.Instantiate(playerPrefab) as GameObject;
        playerController = player.GetComponent<PlayerController>();

        playerController.unitTest = true; //this prevents some code from running in PlayerController that requires user input

        // set up the health system
        healthSystem = player.GetComponent<healthSystem>();

        //Set up the orc
        orc = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemies/Orc_cyan.prefab"), new Vector3(-10, 0, 0), Quaternion.identity) as GameObject;
        enemyAttack = orc.GetComponent<EnemyAttack>();
        enemyAttack.bulletSpeed = 35;
        enemyAttack.shootInterval = 10f;

        
        //This is here to avoid errors
        path = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Other/Pathfinder.prefab")) as GameObject;

          //Restrict the orc from moving
        orc.GetComponent<EnemyMovement>().movementSpeed = 0;
        
    }

    [UnityTest]
    public IEnumerator dashAvoidTest()
    {
        //wait for dash to charge
        yield return new WaitForSeconds(1f);

        //Allow orc to shoot
        enemyAttack.lastShootTime = -10;
        //wait to allow the orc to shoot at a more standarized time
        yield return new WaitForSeconds(0.1f);

        // get the player moving left for dash direction
        playerController.SetMoveDirection(Vector2.left);
        // wait for the bullet to get closer to the player
        yield return new WaitForSeconds(0.1f);


        // Get the player to dash
        playerController.Dash();

        // Move the player right so they dont walk into the orc
        playerController.SetMoveDirection(Vector2.right);

        //Wait then ensure health is three / player did not take damage
        yield return new WaitForSeconds(1f);

        //ensure player did not take damage 
        Assert.IsTrue(healthSystem.life == 3);

        yield return null;
    }

    [UnityTest]
    public IEnumerator dashAnimationEffectTest()
    {
        //Wait for dash to charge
        yield return new WaitForSeconds(1f);

        //Allow orc to shoot
        enemyAttack.lastShootTime = -10;
        
        //Wait to allow the orc to shoot at a more standarized time
        yield return new WaitForSeconds(0.1f);

        // Set player move direction to left for dash direction
        playerController.SetMoveDirection(Vector2.left);
        
        // Wait for the bullet to get closer to the player
        yield return new WaitForSeconds(0.1f);

        // Get the player to dash
        playerController.Dash();

        // After player has dashed, check that Dash effect Game Object has spawned properly within the Game Scene
        Assert.IsTrue(GameObject.FindGameObjectsWithTag("dashEffect").Length > 0);

        yield return null;
    }


    [TearDown]
    public void Teardown()
    {
        // Clean up any objects created during the tests.
        Object.Destroy(playerController.gameObject);
        GameObject.Destroy(player);

        Object.Destroy(enemyAttack);
        GameObject.Destroy(orc);

        GameObject.Destroy(path);

        
    }
}
