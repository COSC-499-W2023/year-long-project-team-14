using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LaserTest : MonoBehaviour
{
    private GameObject level;
    private GameObject laser;
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

        laser = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Interactables/LaserBeam.prefab")) as GameObject;

        //Spawn player
        player = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Players/Mage_player1.prefab"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        healthSystem = player.GetComponent<healthSystem>();

    }

    [UnityTest]
    public IEnumerator LaserDamage()
    {
        //spawn laser beside the player
        laser = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Interactables/LaserBeam.prefab")) as GameObject;

        // Wait for 1 seconds to ensure the laser hits the player
        yield return new WaitForSeconds(0.5f);

        // Check if the the player took damage from the laser
        Assert.IsTrue(healthSystem.life == 2);

    }


    [TearDown]
    public void Teardown()
    {
        //Clean up any objects created during the tests
        GameObject.Destroy(level);
        GameObject.Destroy(player);
        GameObject.Destroy(laser);
    }
}
