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
    private GameObject playButton;

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

        playButton = mainMenuScript.playButton;
        if (playButton == null)
        {
            Debug.LogError("Play button not found in MainMenu script.");
            Assert.Fail("Play button not found.");
        }
        mainMenuScript.mainMenu = true;
    }

    [UnityTest]
    public IEnumerator MainMenu_PlayGameButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
        ExecuteEvents.Execute(playButton, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);

        yield return new WaitForSeconds(1);
        Assert.AreEqual("GameScene", SceneManager.GetActiveScene().name);
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

