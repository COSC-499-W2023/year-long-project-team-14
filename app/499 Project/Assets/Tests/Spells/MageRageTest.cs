using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor; 
using UnityEngine.SceneManagement;

public class MageRageTest
{
    private GameObject player;
    private PlayerController playerController;
    private Spells spells;
    private GameObject template;
    private SpriteRenderer spriteRenderer;


    [UnitySetUp]
    public IEnumerator Setup()
    {
        //Load into test scene
        SceneManager.LoadScene("Test");
        yield return null;

        //Spawn in the level template 
        template = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Levels/TestLevel.prefab")) as GameObject;
    
        //Spawn and set up the player
        UnityEngine.Object playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Players/Mage_player1.prefab"); 
        player = GameObject.Instantiate(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity) as GameObject;
        playerController = player.GetComponent<PlayerController>();
        spells = player.GetComponent<Spells>();
        // Below will be used to check that the sprite changes.
        spriteRenderer = player.GetComponent<SpriteRenderer>();
        playerController.unitTest = true; //this prevents some code from running in PlayerController that requires user input

    }

    [UnityTest]
    public IEnumerator CastLightningTest()
    {

        //Cast the mageRage spell
        spells.mageRage();

        Color cT = new Color (255, 0, 0, 255);
        Color cT2 = new Color (255, 255, 255, 255);


        //Check that the mage's stats have been changed
        Assert.IsTrue(playerController.attackChargeSpeed == 5); 
        Assert.IsTrue(playerController.bulletForce == 25); 
        Assert.IsTrue(playerController.moveSpeed == 20); 
        Assert.IsTrue(spriteRenderer.color ==  cT);


        //Wait for buff from the spell to run out
        yield return new WaitForSeconds(5f);

        //Check that the mage's stats have returned back to the original before the buff.
        Assert.IsTrue(playerController.attackChargeSpeed == 2); 
        Assert.IsTrue(playerController.bulletForce == 12); 
        Assert.IsTrue(playerController.moveSpeed == 8); 
        Assert.IsTrue(spriteRenderer.color == cT2);

    }

    [UnityTearDown]
    public IEnumerator Teardown()
    {
        //Load into test scene for the next test
        SceneManager.LoadScene("Test");
        yield return null;
    }
}
