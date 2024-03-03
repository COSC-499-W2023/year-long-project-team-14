using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor; 
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class SpikesTest : MonoBehaviour
{
    GameObject player;
    PlayerController playerController;
    healthSystem healthSystem1;

    GameObject spikes;


    [UnitySetUp]
    public IEnumerator Setup()
    {

        //Load in test scene 
        SceneManager.LoadScene("Test");
        yield return null;

        //spawn and set up the player
        player = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Players/Mage_player1.prefab"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        healthSystem1 = player.GetComponent<healthSystem>();

        // Set up the player controller
        playerController = player.GetComponent<PlayerController>();
        playerController.moveSpeed = 30;

        //Spawn the spikes below the player.
        spikes = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Interactables/Spikes.prefab"), new Vector3(0, -5, 0), Quaternion.identity) as GameObject;


    }

    [UnityTest]
    public IEnumerator spikeDmgTest()
    {

        // Make the player walk down
        playerController.SetMoveDirection(Vector2.down);

        // Wait for 1 seconds to ensure the player hits the spikes
        yield return new WaitForSeconds(0.5f);

        // Check if the the player took damage from the spikes or not 
        Assert.IsTrue(healthSystem1.life == 2);
    }

   

    [TearDown]
    public void Teardown()
    {
        // Clean up any objects created during the tests.
        GameObject.Destroy(player);
        GameObject.Destroy(spikes);
        Object.Destroy(playerController);
        Object.Destroy(healthSystem1);
    }
}
