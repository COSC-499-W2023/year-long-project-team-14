using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor; 
using UnityEngine.InputSystem;

public class PlayerAttackTest
{
    private GameObject player;
    private GameObject template;
    private PlayerController playerController;
    private GameObject[] bullets;

    [SetUp]
    public void Setup()
    {
        //spawn and set up the player
        UnityEngine.Object playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Players/Mage_player1.prefab"); 
        player = GameObject.Instantiate(playerPrefab) as GameObject;
        playerController = player.GetComponent<PlayerController>();
        playerController.unitTest = true; //this prevents some code from running in PlayerController that requires user input
        playerController.attackCharge = 1;
        playerController.attackChargeSpeed = 2;

        //Spawn in the level template 
        template = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Levels/TestLevel.prefab")) as GameObject;
    }

    [UnityTest]
    public IEnumerator AttackTest()
    {
        //attempt to shoot 2 bullets, assert that only 1 bullet spawned since attackCharge is < 2
        playerController.Shoot();
        yield return new WaitForSeconds(0.1f);
        playerController.Shoot();
        bullets = GameObject.FindGameObjectsWithTag("Player_bullet");
        Assert.IsTrue(bullets.Length == 1);
  
        //wait and check the bullets position to make sure it is moving in the right direction
        float y = bullets[0].transform.position.y;
        yield return new WaitForSeconds(0.25f);
        Assert.IsTrue(bullets[0].transform.position.y < y);
 
        //wait and shoot another bullet to check that attackCharge increases over time correctly
        yield return new WaitForSeconds(0.25f);
        playerController.Shoot();
        bullets = GameObject.FindGameObjectsWithTag("Player_bullet");
        Assert.IsTrue(bullets.Length == 2);
    }

    [TearDown]
    public void Teardown()
    {
        //destroy all gameobjects
        Object.Destroy(playerController.gameObject);
        GameObject.Destroy(player);
        GameObject.Destroy(template);
        for(int i = 0; i < bullets.Length; i++)
            GameObject.Destroy(bullets[i]);
    }
}

