using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BreakableWallsTest : MonoBehaviour
{
    private PlayerController playerController;

    private GameObject level;
    private GameObject wall;
    private GameObject cam;

    GameObject player;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        //Load in test scene because previous unit test puts you in the main menu which covers up what is happening during the unit test
        SceneManager.LoadScene("Test");
        yield return null;

        //Spawn in an empty level
        level = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Levels/TestLevel.prefab")) as GameObject;

        //spawn a breakable wall below the player
        wall = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Interactables/BreakableWall.prefab"), new Vector3(0, -5, 0), Quaternion.identity) as GameObject;

        //Spawn player above the wall
        player = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Players/Mage_player1.prefab"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        // Set up the player controller
        playerController = player.GetComponent<PlayerController>();
        playerController.unitTest = true;
        //Give the player a bullet to shoot
        playerController.attackCharge = 0;
        playerController.bulletForce = 35;
        playerController.attackChargeSpeed = 0;
    }

    [UnityTest]
    public IEnumerator breakTest()
    {
        //Make the player shoot 
        playerController.attackCharge = 1;
        playerController.Shoot();

        //Wait for the bullet to hit the breakable wall
        yield return new WaitForSeconds(0.5f);

        //Check that the wall is destroyed
        Assert.IsTrue(wall == null);
    }

    [TearDown]
    public void Teardown()
    {
        //Clean up any objects created during the tests
        GameObject.Destroy(level);
        GameObject.Destroy(player);
        GameObject.Destroy(wall);
    }
}
