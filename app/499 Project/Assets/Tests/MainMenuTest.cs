using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

public class MainMenuTest
{
    private MainMenu mainMenuScript;
    private GameObject playerMenuObject;
    private GameObject playButton;
    private GameObject player1Button;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        SceneManager.LoadScene("Menu");

        // Wait for the scene to load
        yield return null;

        mainMenuScript = Object.FindObjectOfType<MainMenu>();
        if (mainMenuScript == null)
        {
            Debug.LogError("MainMenu script not found in the scene.");
            Assert.Fail("MainMenu script not found.");
        }

        playerMenuObject = mainMenuScript.playerMenuObject;
        if (playerMenuObject == null)
        {
            Debug.LogError("playerMenuObject not found in MainMenu script.");
            Assert.Fail("playerMenuObject not found.");
        }

        player1Button = mainMenuScript.player1Button;
        if (player1Button == null)
        {
            Debug.LogError("Player1 button not found in MainMenu script.");
            Assert.Fail("Play button not found.");
        }

        playButton = mainMenuScript.playButton;
        if (playButton == null)
        {
            Debug.LogError("Play button not found in MainMenu script.");
            Assert.Fail("Play button not found.");
        }
        mainMenuScript.mainMenu = true;
    }

    [UnityTest]
    public IEnumerator MainMenu_PlayButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
        ExecuteEvents.Execute(playButton, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);

        yield return new WaitForSeconds(1);
        Assert.IsTrue(playerMenuObject.activeSelf);
    }

    [UnityTest]
    public IEnumerator MainMenu_PlayerButton()
    {
        playerMenuObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        ExecuteEvents.Execute(player1Button, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);

        yield return new WaitForSeconds(1);
        Assert.AreEqual("GameScene", SceneManager.GetActiveScene().name);
    }

    [UnityTest]
    public IEnumerator MainMenu_PlayerBackButton()
    {
        playerMenuObject.SetActive(true);
        mainMenuScript.PlayerBackButton();
        yield return new WaitForSeconds(0.1f);
        Assert.IsFalse(playerMenuObject.activeSelf);
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
