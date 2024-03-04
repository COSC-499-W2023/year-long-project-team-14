using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor; 
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class OrcTripleShotTest : MonoBehaviour
{
    private GameObject orc;
    private EnemyTripleShot enemyAttack;

    private GameObject level;
    private GameObject path;
    private GameObject cam;

    GameObject player1;
    healthSystem healthSystem1;

    [UnitySetUp]
    public IEnumerator Setup()
    {

        //Load in test scene because previous unit test puts you in the main menu which covers up what is happening during the unit test
        SceneManager.LoadScene("Test");
        yield return null;

        //Spawn in a level with a wall in the center
        level = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Levels/LevelTemplate.prefab")) as GameObject;

        path = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Other/Pathfinder.prefab")) as GameObject;

        //Spawn the orc on the level
        orc = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemies/Orc_soldier.prefab"), new Vector3(-5, 0, 0), Quaternion.identity) as GameObject;
        enemyAttack = orc.GetComponent<EnemyTripleShot>();
        enemyAttack.bulletSpeed = 20f;
        enemyAttack.shootInterval = 5;

        //Restrict the orc from moving
        orc.GetComponent<EnemyMovement>().movementSpeed = 0;

        //Spawn player 1 
        player1 = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Players/Mage_player1.prefab"), new Vector3(5, 0, 0), Quaternion.identity) as GameObject;
        healthSystem1 = player1.GetComponent<healthSystem>();
       
    }

    [UnityTest]
    public IEnumerator OrcShootTest()
    {
        //Allow orc to shoot
        enemyAttack.lastShootTime = 5;

        //Get starting health 
        int health = healthSystem1.life;

        //Wait for orc to shoot at player
        yield return new WaitForSeconds(1);

        //Check that the first and last bullet hit the player
        Assert.IsTrue(healthSystem1.life < health);
    }

    [TearDown]
    public void Teardown()
    {
        //Clean up any objects created during the tests
        GameObject.Destroy(orc);
        GameObject.Destroy(level);
        GameObject.Destroy(path);
        GameObject.Destroy(player1);
    }
}
