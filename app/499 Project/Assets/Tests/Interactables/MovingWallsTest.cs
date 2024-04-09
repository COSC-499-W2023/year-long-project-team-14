using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MovingWallsTest : MonoBehaviour
{
    private GameObject orc;
    private EnemyHealthSystem enemyHealthSystem;
    private movingWall movingWall;
    private Transform childWall;

    private GameObject level;
    private GameObject wall;
    private GameObject path;
    private GameObject cam;

    GameObject player;
    healthSystem healthSystem;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        //Load in test scene because previous unit test puts you in the main menu which covers up what is happening during the unit test
        SceneManager.LoadScene("Test");
        yield return null;

        //Spawn in an empty level
        level = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Levels/TestLevel.prefab")) as GameObject;

        wall = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Interactables/movingWall.prefab")) as GameObject;
        //restrict wall from moving
        childWall = wall.transform.Find("movingWall");
        childWall.GetComponent<movingWall>().speed = 0;

        //This allows the orc to move and avoid obstacles
        path = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Other/Pathfinder.prefab")) as GameObject;

        //Spawn the orc above the moving wall, between it and the outter upper wall
        orc = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemies/Orc_cyan.prefab"), new Vector3(0, 5, 0), Quaternion.identity) as GameObject;
        enemyHealthSystem = orc.GetComponent<EnemyHealthSystem>();

        //Restrict the orc from moving
        orc.GetComponent<EnemyMovement>().movementSpeed = 0;

        //Spawn player below the wall
        player = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Players/Mage_player1.prefab"), new Vector3(0, -5, 0), Quaternion.identity) as GameObject;
        healthSystem = player.GetComponent<healthSystem>();

    }

    [UnityTest]
    public IEnumerator movingWallHurtsEnemy()
    {
        //Allow the wall to move
        childWall.GetComponent<movingWall>().speed = 10f;

        //Get enemy health
        int health = enemyHealthSystem.enemyHealth;

        //Wait for the wall to squish the enemy against the upper wall
        yield return new WaitForSeconds(1.0f);

        //Check that the orc was damaged
        Assert.IsTrue(enemyHealthSystem.enemyHealth < health);
        //Destroy(orc);
    }

    [UnityTest]
    public IEnumerator movingWallHurtsPlayer()
    {
        Destroy(orc);
        //restrict wall from moving
        childWall.GetComponent<movingWall>().speed = 0;

        //move the player between the moving wall and upper wall
        Vector3 newPosition = new Vector3(0, 5, 0);
        player.transform.position = newPosition;

        // wait a frame to allow for the position change
        yield return null;

        //Allow the wall to move
        childWall.GetComponent<movingWall>().speed = 10f;

        //Wait for the wall to squish the player against the upper wall
        yield return new WaitForSeconds(1.0f);

        //Check that the player was damaged
        Assert.IsTrue(healthSystem.life < 3);
        //Destroy(player);
    }

    [TearDown]
    public void Teardown()
    {
        //Clean up any objects created during the tests
        GameObject.Destroy(orc);
        GameObject.Destroy(level);
        GameObject.Destroy(path);
        GameObject.Destroy(player);
        GameObject.Destroy(wall);
    }
}
