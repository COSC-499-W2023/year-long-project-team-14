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

    [UnitySetUp]
    public IEnumerator Setup()
    {
        SceneManager.LoadScene("Menu");

        yield return null;

        mainMenuScript = Object.FindObjectOfType<MainMenu>();
        playerMenuObject = mainMenuScript.playerMenuObject;
        player1Button = mainMenuScript.player1Button;
        player2Button = mainMenuScript.player2Button;
    }

    [UnityTest]
    public IEnumerator OnePlayerTest()
    {
        playerMenuObject.SetActive(true);
        ExecuteEvents.Execute(player1Button, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);

        yield return new WaitForSeconds(1);
        Assert.AreEqual("GameScene", SceneManager.GetActiveScene().name);
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Assert.IsTrue(players.Length == 1);
    }

    [UnityTest]
    public IEnumerator TwoPlayerTest()
    {
        playerMenuObject.SetActive(true);
        ExecuteEvents.Execute(player2Button, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);

        yield return new WaitForSeconds(1);
        Assert.AreEqual("GameScene", SceneManager.GetActiveScene().name);
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Assert.IsTrue(players.Length == 2);
    }

    [UnityTearDown]
    public IEnumerator Teardown()
    {
        // Wait for the scene to load
        yield return SceneManager.LoadSceneAsync("Menu");

        // Wait for a very short time to ensure that the scene is fully loaded
        yield return new WaitForSeconds(0.1f);

        // Unload the scene
        yield return SceneManager.UnloadSceneAsync("Menu");
    }

}
