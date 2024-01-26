using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor; 
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class bulletCollisionTest 
{
    private PlayerController playerController;
    private UnityEngine.Object playerPrefab;
    private GameObject player;
    private  GameObject[] bullets;
    private  GameObject[] bullets2;


    private UnityEngine.Object templatePrefab;
    private GameObject template;

    private GameObject path;

    private EnemyAttack orcShooter;
    private UnityEngine.Object orcPrefab;
    private GameObject orc;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        SceneManager.LoadScene("Test");
        yield return null;

        //spawn and set up the player
        playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Players/Mage_player1.prefab"); 
        player = GameObject.Instantiate(playerPrefab) as GameObject;
       
       //Spawn and set up the level template
        templatePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Levels/TestLevel.prefab"); 
        template = GameObject.Instantiate(templatePrefab) as GameObject;

        // Set up the player controller
        playerController = player.GetComponent<PlayerController>();
        playerController.unitTest = true; 
        //Give the player a bullet to shoot
        playerController.attackCharge = 0;
        playerController.bulletForce = 35;
        playerController.attackChargeSpeed = 0;


        //Set up path so the orc can move and shoot without an error from the path finding algo
        path = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Other/Pathfinder.prefab")) as GameObject;

        //set up and spawn the orc 
        orcPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemies/Orc_cyan.prefab"); 
        orc = GameObject.Instantiate(orcPrefab, new Vector3(0, -5, 0), Quaternion.identity) as GameObject;
        orcShooter = orc.GetComponent<EnemyAttack>();
        orcShooter.bulletSpeed = 35;

        //restrict orcs shooting
        orcShooter.shootInterval = 10f;

        //restrict the orc from moving 
        orc.GetComponent<EnemyMovement>().movementSpeed = 0;

    }

    [UnityTest]
    public IEnumerator bulletCTest()
    {
        playerController.SetMoveDirection(Vector2.left);
        yield return new WaitForSeconds(0.5f); 

        //Make the player shoot 
        playerController.attackCharge = 1;
        playerController.Shoot();
       
        //Move the player so the bullet does not collide with it 
        playerController.SetMoveDirection(Vector2.left);

        //Store the bullet in a variable
        bullets = GameObject.FindGameObjectsWithTag("Player_bullet");

        //Let the bullet travel 
        yield return new WaitForSeconds(0.5f); 

        // Check if the bullet is within the level template.
        Assert.IsTrue(bullets[0].transform.position.y < 6.5 && bullets[0].transform.position.y > -7.5); 
    }

//This unit test is to test friendly bullets colliding with enemy bullets
    [UnityTest]
    public IEnumerator bulletCbullet()
    {
        // Give the orc time to adjust its shoot path so it matches the players 
        yield return new WaitForSeconds(0.5f); 
        // Allow the orc to shoot 
        orcShooter.lastShootTime = -10;

        //Allow the player to shoot 
        playerController.attackCharge = 1;
        playerController.Shoot();

        //Allow the bullets to make contact 
        yield return new WaitForSeconds(0.5f); 

        /*store the amount of bullets in an array which should be empty because the bullets have already collided
          so both arrays would be empty.
        */
        bullets2 = GameObject.FindGameObjectsWithTag("EnemyBullet");

        bullets = GameObject.FindGameObjectsWithTag("Player_bullet");        

        // Check if the bullet is within the level template.
        Assert.IsTrue(bullets.Length == 0 && bullets2.Length == 0); 
    }

    [TearDown]
    public void Teardown()
    {
        // Clean up any objects created during the tests.
        GameObject.Destroy(player);
        Object.Destroy(playerController.gameObject);

        GameObject.Destroy(orc);
        GameObject.Destroy(orcShooter.gameObject);

        GameObject.Destroy(template);

        if(bullets.Length != 0){
            for(int i = 0; i < bullets.Length; i++)
                GameObject.Destroy(bullets[i]);
        }

        if(bullets2.Length != 0){
            for(int i = 0; i < bullets2.Length; i++)
                GameObject.Destroy(bullets2[i]);
        }
    }

}
