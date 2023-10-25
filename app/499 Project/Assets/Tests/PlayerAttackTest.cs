using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor; 
using UnityEngine.InputSystem;

public class PlayerAttackTest
{

    [UnityTest]
    public IEnumerator Player1AttackTest()
    {
        //spawn and set up the player
        UnityEngine.Object playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Mage_player1.prefab"); 
        GameObject player = GameObject.Instantiate(playerPrefab) as GameObject;
        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.unitTest = true; //this prevents some code from running in PlayerController that requires user input
        playerController.attackCharge = 1;
        playerController.attackChargeSpeed = 2;

        //attempt to shoot 2 bullets, assert that only 1 bullet spawned since attackCharge is < 2
        playerController.Shoot();
        yield return new WaitForSeconds(0.1f);
        playerController.Shoot();
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Player_bullet");
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
}

