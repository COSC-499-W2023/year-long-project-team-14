using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class uiTest
{
    private GameObject player;
    private PlayerController playerController;
    private GameObject bullet;
    private bulletUI bulletUI;
    private GameObject heart;
    private healthSystem healthSystem;
    private GameObject template;

    [SetUp]
    public void Setup()
    {
        //spawn and set up player 1
        UnityEngine.Object playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Players/Mage_player1.prefab");
        player = GameObject.Instantiate(playerPrefab) as GameObject;
        playerController = player.GetComponent<PlayerController>();
        playerController.unitTest = true; //this prevents some code from running in PlayerController that requires user input
        playerController.attackCharge = 3;
        playerController.attackChargeSpeed = 10;
        healthSystem = player.GetComponent<healthSystem>();
        bulletUI = player.GetComponent<bulletUI>();

        //Spawn in the level template 
        template = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Levels/TestLevel.prefab")) as GameObject;
    }

    [UnityTest]
    public IEnumerator uiHeart()
    {
        //check if player is alive
        Assert.IsFalse(healthSystem.dead);
        //call the takeDamage function and assert that the ui heart image is deactivated accordingly
        healthSystem.takeDamage();
        yield return new WaitForSeconds(0.1f);
        heart = healthSystem.hearts[healthSystem.hearts.Length-1];
        Assert.IsFalse(heart.activeSelf);
        //take damage a 2nd time
        yield return new WaitForSeconds(1.05f);
        healthSystem.takeDamage();
        yield return new WaitForSeconds(0.1f);
        heart = healthSystem.hearts[healthSystem.hearts.Length - 2];
        Assert.IsFalse(heart.activeSelf);
        //take damage a 3rd time
        yield return new WaitForSeconds(1.05f);
        healthSystem.takeDamage();
        yield return new WaitForSeconds(0.1f);
        heart = healthSystem.hearts[healthSystem.hearts.Length - 3];
        Assert.IsFalse(heart.activeSelf);
        //check if player is dead after losing 3 lives (life < 1)
        Assert.IsTrue(healthSystem.dead);
    }

    [UnityTest]
    public IEnumerator uiBullet()
    {
        //shoot a bullet and assert that the ui bullet image is deactivated so that it reflects attack charge
        playerController.Shoot();
        yield return new WaitForSeconds(0.01f);
        bullet = bulletUI.bullets[(int)playerController.attackCharge];
        Assert.IsFalse(bullet.activeSelf);

        //wait and check if the image is reactivated once attack charge goes back up
        yield return new WaitForSeconds(0.5f);
        Assert.IsTrue(bullet.activeSelf);
    }

    [TearDown]
    public void Teardown()
    {
        //destroy all gameobjects
        GameObject.Destroy(player);

        /* Had to add the if below, heart was trying to tear down playerController when it was already torn down.
        */
        if(playerController != null){
            Object.Destroy(playerController.gameObject);
        }
        GameObject.Destroy(bullet);
        GameObject.Destroy(heart);
        GameObject.Destroy(template);
    }
}
