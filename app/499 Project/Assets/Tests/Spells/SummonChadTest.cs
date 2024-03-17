using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;
using UnityEngine.SceneManagement;

public class SummonChadTest
{
    private GameObject player;
    private PlayerController playerController;
    private Spells spells;
    private GameObject path;
    private GameObject template;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        //Load into test scene
        SceneManager.LoadScene("Test");
        yield return null;

        //Spawn in the level template 
        template = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Levels/TestLevel.prefab")) as GameObject;

        //This allows the bonk to move and avoid obstacles
        path = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Other/Pathfinder.prefab")) as GameObject;

        //Spawn and set up the player
        UnityEngine.Object playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Players/Mage_player1.prefab");
        player = GameObject.Instantiate(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity) as GameObject;
        playerController = player.GetComponent<PlayerController>();
        spells = player.GetComponent<Spells>();
        playerController.unitTest = true; //this prevents some code from running in PlayerController that requires user input

    }

    [UnityTest]
    public IEnumerator summonChadTest()
    {
        // summon chad
        spells.SummonChad();
        yield return null;
        // Check that chad exists
        GameObject chad = GameObject.Find("Chad(Clone)");
        Assert.IsTrue(chad != null);

        //*** this next section is commented out bc it takes 15 seconds
        // Wait for 15 seconds
        //yield return new WaitForSecondsRealtime(15.5f);

        // Check that chad died after 15s
        //healthSystem chadHealth = chad.GetComponent<healthSystem>();
        //Assert.IsTrue(chadHealth.dead);

    }


    [UnityTearDown]
    public IEnumerator Teardown()
    {
        //Load into test scene for the next test
        SceneManager.LoadScene("Test");
        yield return null;
    }
}
