using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor; 
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInvincible : MonoBehaviour
{


    private GameObject orc;
    private EnemyAttack enemyAttack;


    private GameObject orc2;
    private EnemyAttack enemyAttack2;

    private GameObject path;
    private GameObject cam;

    GameObject player1;
    healthSystem healthSystem1;
    

    // Start is called before the first frame update
   
    [UnitySetUp]
    public IEnumerator Setup()
    {
        //Load in test scene because previous unit test puts you in the main menu which covers up what is happening during the unit test
        SceneManager.LoadScene("Test");
        yield return null;

        //This allows the orc to move and avoid obstacles
        path = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Other/Pathfinder.prefab")) as GameObject;

        //Spawn the orc 
        orc = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemies/Orc_cyan.prefab"), new Vector3(-5, 0, 0), Quaternion.identity) as GameObject;
        enemyAttack = orc.GetComponent<EnemyAttack>();
        enemyAttack.bulletSpeed = 35;
        enemyAttack.shootInterval = 10f;

        //Restrict the orc from moving
        orc.GetComponent<EnemyMovement>().movementSpeed = 0;

        //Spawn the orc 
        orc2 = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemies/Orc_cyan.prefab"), new Vector3(5, 0, 0), Quaternion.identity) as GameObject;
        enemyAttack2 = orc2.GetComponent<EnemyAttack>();
        enemyAttack2.bulletSpeed = 25;
        enemyAttack2.shootInterval = 10f;

        //Restrict the orc from moving
        orc2.GetComponent<EnemyMovement>().movementSpeed = 0;

        //Spawn player 1 on the right side of the wall
        player1 = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Players/Mage_player1.prefab"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        healthSystem1 = player1.GetComponent<healthSystem>();
    }

    [UnityTest]
    public IEnumerator invincibleTest()
    {

        //Allow orc to shoot
        enemyAttack.lastShootTime = -10;
        enemyAttack2.lastShootTime = -10;

        //Get starting health
        int health = healthSystem1.life;

        //Wait for orc's to shoot at player
        yield return new WaitForSeconds(0.5f);
        

        //Check that the player only lost 1 life
        Assert.IsTrue(healthSystem1.life == health - 1);

    }

    [TearDown]
    public void Teardown()
    {
        //Clean up any objects created during the tests
        GameObject.Destroy(orc);
        GameObject.Destroy(path);
        GameObject.Destroy(orc2);
        GameObject.Destroy(player1);
    }
}
