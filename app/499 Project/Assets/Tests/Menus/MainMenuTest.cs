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
    private GameObject difficultyMenuObject;
    private GameObject playButton;
    private GameObject player1Button;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        //Wait for the menu scene to load
        SceneManager.LoadScene("Menu");
        yield return null;

        //Get access to some of the menu objects
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

        difficultyMenuObject = mainMenuScript.difficultyMenuObject;
        if (difficultyMenuObject == null)
        {
            Debug.LogError("difficultyMenuObject not found in MainMenu script.");
            Assert.Fail("difficultyMenuObject not found.");
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
        //Execute the play button and check that the player menu is now active
        EventSystem.current.SetSelectedGameObject(null);
        ExecuteEvents.Execute(playButton, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);

        yield return new WaitForSeconds(0.1f);
        Assert.IsTrue(playerMenuObject.activeSelf);
    }

    [UnityTest]
    public IEnumerator MainMenu_PlayerButton()
    {
        //Execute the 1 player button and check that the game scene is loaded
        playerMenuObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        ExecuteEvents.Execute(player1Button, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);

        yield return new WaitForSeconds(0.1f);
        Assert.IsTrue(difficultyMenuObject.activeSelf);
    }

    [UnityTest]
    public IEnumerator MainMenu_PlayerBackButton()
    {
        //Execute the player menu back button and check that the player menu is no longer active
        playerMenuObject.SetActive(true);
        mainMenuScript.PlayerBackButton();
        yield return new WaitForSeconds(0.1f);
        Assert.IsFalse(playerMenuObject.activeSelf);
    }
}
