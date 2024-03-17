using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CooldownUiTest : MonoBehaviour
{
    private GameObject level;

    GameObject player;
    spellUi ui;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        //Load in test scene because previous unit test puts you in the main menu which covers up what is happening during the unit test
        SceneManager.LoadScene("Test");
        yield return null;

        //Spawn in an empty level
        level = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Levels/TestLevel.prefab")) as GameObject;

        //Spawn and setup the player
        player = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Players/Mage_player1.prefab"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        ui = player.GetComponentInChildren<spellUi>();

    }

    [UnityTest]
    public IEnumerator displayProperIcon()
    {
        //Set the player's spell to lightning
        player.GetComponent<Spells>().spellName = "Lightning";
        yield return null;
        //Check that the proper Icon is displayed
        Assert.AreEqual(ui.image.sprite, ui.lightningSprite);

        //Set the player's spell to fireball
        player.GetComponent<Spells>().spellName = "Fireball";

        yield return null;

        //Check that the proper Icon is displayed
        Assert.AreEqual(ui.image.sprite, ui.fireSprite);
    }

    [UnityTest]
    public IEnumerator CooldownTest()
    {

        player.GetComponent<Spells>().CastSpell();
        // Assert fill is consistent with spell cooldown
        Assert.AreEqual(ui.image.fillAmount, (player.GetComponent<Spells>().cooldownTimer / player.GetComponent<Spells>().spellCooldown));

        yield return null;
    }



    [TearDown]
    public void Teardown()
    {
        //Clean up any objects created during the tests
        GameObject.Destroy(level);
        GameObject.Destroy(player);
    }
}
