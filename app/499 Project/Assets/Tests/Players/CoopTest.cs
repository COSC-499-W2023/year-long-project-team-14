using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

public class CoopTest
{
    private MainMenu mainMenuScript;
    private GameObject playerMenuObject;
    private GameObject playButton;
    private GameObject player1Button;
    private GameObject player2Button;
    private GameObject easyButton;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        //Load the main menu
        SceneManager.LoadScene("Menu");

        yield return null;

        //Get access to some of the menu objects
        mainMenuScript = Object.FindObjectOfType<MainMenu>();
        playerMenuObject = mainMenuScript.playerMenuObject;
        player1Button = mainMenuScript.player1Button;
        player2Button = mainMenuScript.player2Button;
        easyButton = mainMenuScript.easyButton;
    }

    [UnityTest]
    public IEnumerator OnePlayerTest()
    {
        //Set the player selection menu active and call the 1 player function
        playerMenuObject.SetActive(true);
        ExecuteEvents.Execute(player1Button, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
        ExecuteEvents.Execute(easyButton, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);

        //Wait and check that the game scene is loaded and check that only 1 player spawned in
        yield return new WaitForSeconds(0.6f);
        Assert.AreEqual("GameScene", SceneManager.GetActiveScene().name);
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Assert.IsTrue(players.Length == 1);
    }

    [UnityTest]
    public IEnumerator TwoPlayerTest()
    {
        //Set the player selection menu active and call the 2 player function
        playerMenuObject.SetActive(true);
        ExecuteEvents.Execute(player2Button, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
        ExecuteEvents.Execute(easyButton, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);

        //Wait and check that the game scene is loaded and check that 2 players spawned in
        yield return new WaitForSeconds(0.6f);
        Assert.AreEqual("GameScene", SceneManager.GetActiveScene().name);
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Assert.IsTrue(players.Length == 2);
    }
}
