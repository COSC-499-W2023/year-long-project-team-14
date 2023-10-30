using UnityEngine;
using UnityEngine.SceneManagement;
using NUnit.Framework;

public class PauseMenuTest
{
    private PauseMenu pauseMenuScript;

    [SetUp]
    public void Setup()
    {
        // Load the scene with the PauseMenu
        SceneManager.LoadScene("Menu");
        pauseMenuScript = Object.FindObjectOfType<PauseMenu>();
    }

    [Test]
    public void PauseMenu_PauseAndResume()
    {
        // Simulate the pause action
        var context = new InputAction.CallbackContext();
        context.performed = true;

        pauseMenuScript.Pause(context);

        // Pause and Resume assertions
        Assert.IsTrue(pauseMenuScript.pauseMenu);
        Assert.IsTrue(pauseMenuScript.pauseMenuUI.activeSelf);
        Assert.AreEqual(0f, Time.timeScale);

        // Simulate the resume action
        context.performed = false;
        pauseMenuScript.Pause(context);

        // Additional assertions after resuming
        Assert.IsFalse(pauseMenuScript.pauseMenu);
        Assert.IsFalse(pauseMenuScript.pauseMenuUI.activeSelf);
        Assert.AreEqual(1f, Time.timeScale);
        Assert.IsNull(EventSystem.current.currentSelectedGameObject);
    }

    [TearDown]
    public void Teardown()
    {
        // Unload the scene
        SceneManager.UnloadSceneAsync("Menu");
    }
}
