using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor; 
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class OrcAttackTest : MonoBehaviour
{
    private GameObject orc;
    private EnemyAttack enemyAttack;

    private GameObject level;
    private GameObject path;
    private GameObject cam;

    GameObject player1;
    healthSystem healthSystem1;
    GameObject player2;
    healthSystem healthSystem2;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        //Load in test scene because previous unit test puts you in the main menu which covers up what is happening during the unit test
        SceneManager.LoadScene("Test");
        yield return null;

        //Spawn in a level with a wall in the center
        level = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Levels/TestLevel2.prefab")) as GameObject;

        //This allows the orc to move and avoid obstacles
        path = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Other/Pathfinder.prefab")) as GameObject;

        //Spawn the orc on the left side of the wall
        orc = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemies/Orc_cyan.prefab"), new Vector3(-5, 0, 0), Quaternion.identity) as GameObject;
        enemyAttack = orc.GetComponent<EnemyAttack>();
        enemyAttack.bulletSpeed = 20;
        enemyAttack.shootInterval = 10f;

        //Restrict the orc from moving
        orc.GetComponent<EnemyMovement>().movementSpeed = 0;

        //Spawn player 1 on the right side of the wall
        player1 = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Players/Mage_player1.prefab"), new Vector3(5, 2, 0), Quaternion.identity) as GameObject;
        healthSystem1 = player1.GetComponent<healthSystem>();
        
        //Spawn player 2 on the left side of the wall
        player2 = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Players/Mage_player2.prefab"), new Vector3(-3, -5, 0), Quaternion.identity) as GameObject;
        healthSystem2 = player2.GetComponent<healthSystem>();
       
    }

    [UnityTest]
    public IEnumerator OrcShootTest()
    {
        //Allow orc to shoot
        enemyAttack.lastShootTime = -10;

        //Get starting health
        int health1 = healthSystem1.life;
        int health2 = healthSystem2.life;

        //Wait for orc to shoot at player
        yield return new WaitForSeconds(0.5f);

        //Check that the orc shot at player 2 and not player 1
        Assert.IsTrue(healthSystem2.life < health2);
        Destroy(player2);

        //Allow orc to shoot
        enemyAttack.lastShootTime = -10;

        //Wait for orc to shoot at player
        yield return new WaitForSeconds(0.75f);

        //Check that the orc was able to shoot off the wall to hit player 1
        Assert.IsTrue(healthSystem1.life < health1);
        Destroy(player1);
    }

    [UnityTest]
    public IEnumerator OrcShootParticleTest()
    {

        //Allow orc to shoot
        enemyAttack.lastShootTime = -10;

        //Wait for orc to shoot at player and for particle animation to end
        yield return new WaitForSeconds(1.5f);

        //Search the scene for particles and check that enemy bullet particle game object plays fully and is destroyed from the game scene after animation has ended
        Assert.IsTrue(GameObject.FindGameObjectsWithTag("effect").Length == 0);
        Destroy(player2);

        //Allow orc to shoot
        enemyAttack.lastShootTime = -10;

        //Wait for orc to shoot at player and for particle animation to end
        yield return new WaitForSeconds(2f);

        //Search the scene for particles and check that enemy bullet particle game object plays fully and is destroyed from the game scene after animation has ended
        Assert.IsTrue(GameObject.FindGameObjectsWithTag("effect").Length == 0);
        Destroy(player1);
    }

    [UnityTest]
    public IEnumerator OrcMoveToPlayerTest()
    {
        // Let the orc be able to move
        orc.GetComponent<EnemyMovement>().movementSpeed = 35;

        // Make the orc walk to the second player 
        orc.GetComponent<EnemyMovement>().NewTarget((float) -3,(float) -5);

        //Make the orc move immediately
        yield return null;
        orc.GetComponent<EnemyMovement>().waitTime = 0;

        //wait until  that player would take damage
        yield return new WaitForSeconds(1.5f);

        //Check that the orc was able to damange player 2
        Assert.IsTrue(healthSystem2.life < 3);
        Destroy(player1);
    }

    [TearDown]
    public void Teardown()
    {
        //Clean up any objects created during the tests
        GameObject.Destroy(orc);
        GameObject.Destroy(level);
        GameObject.Destroy(path);
        GameObject.Destroy(player1);
        GameObject.Destroy(player2);
    }
}
