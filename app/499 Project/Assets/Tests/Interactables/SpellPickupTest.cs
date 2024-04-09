using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SpellPickupTest
{
    private SeekingOrbPickup SeekingOrbPickup;

    private GameObject level;
    private GameObject path;
    private GameObject cam;
    private GameObject spellPickup;

    GameObject player;
    Spells spells;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        //Load in test scene because previous unit test puts you in the main menu which covers up what is happening during the unit test
        SceneManager.LoadScene("Test");
        yield return null;

        //Spawn in an empty level
        level = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Levels/TestLevel.prefab")) as GameObject;

        //spawn in a spell pickup
        spellPickup = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Interactables/SeekingOrb Pickup.prefab")) as GameObject;
        SeekingOrbPickup = spellPickup.GetComponent<SeekingOrbPickup>();

        //Spawn player below the Spell Pickup
        player = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Players/Mage_player1.prefab"), new Vector3(0, -10, 0), Quaternion.identity) as GameObject;
        spells = player.GetComponent<Spells>();

    }

    [UnityTest]
    public IEnumerator pickupTest()
    {
        //check that when player is over the pickup, the prompt shows up
        //assert the prompt is not showing as the player is not over the pickup
        Assert.IsTrue(!SeekingOrbPickup.prompt.isDisplayed);
        //move the player ontop of the pickup
        Vector3 newPosition = new Vector3(0, 0, 0);
        player.transform.position = newPosition;
        // wait a frame to allow for the position change
        yield return new WaitForSeconds(0.1f);
        //assert the prompt is showing
        Assert.IsTrue(SeekingOrbPickup.prompt.isDisplayed);

        yield return new WaitForSeconds(0.1f);
        //check that when a player interacts with the pickup, his spellname is switched to the corresponding pickup

        //assert the spellname is not equal to the pickup (Fireball is default)
        Assert.IsTrue(spells.spellName != "Fireball");
        //make the player interact with the pickup
        SeekingOrbPickup.Interact();
        // wait a frame to allow for the interaction
        yield return new WaitForSeconds(0.1f);
        //assert spellname is equal to the proper pickup
        Assert.IsTrue(spells.spellName == "SeekingOrb");
    }

    [TearDown]
    public void Teardown()
    {
        //Clean up any objects created during the tests
        GameObject.Destroy(spellPickup);
        GameObject.Destroy(level);
        GameObject.Destroy(path);
        GameObject.Destroy(player);

    }
}
