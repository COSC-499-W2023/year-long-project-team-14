using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseMenuTests
{

    [UnityTest]
    public IEnumerator PauseMenu_ResumeGame()
    {
        // Arrange
        SceneManager.LoadScene(0);
        yield return null;
        var pauseMenu = new GameObject().AddComponent<PauseMenu>();
        pauseMenu.pauseMenuUI = new GameObject();
        pauseMenu.resumeButton = new GameObject();

        // Act
        pauseMenu.Pause();
        pauseMenu.Resume();

        // Assert
        Assert.IsFalse(PauseMenu.GameIsPaused);
        Assert.IsFalse(pauseMenu.pauseMenuUI.activeSelf);
        Assert.AreEqual(1f, Time.timeScale);

        yield return null;
    }

    [UnityTest]
    public IEnumerator PauseMenu_LoadMenu()
    {
        // Arrange
        SceneManager.LoadScene(0);
        yield return null;
        var pauseMenu = new GameObject().AddComponent<PauseMenu>();

        // Act
        pauseMenu.LoadMenu();
        yield return new WaitForSeconds(0.6f);

        // Assert
        Assert.AreEqual(1f, Time.timeScale);
        Assert.AreEqual(0, SceneManager.GetActiveScene().buildIndex);

        yield return null;
    }

    [UnityTest]
    public IEnumerator PauseMenu_Restart()
    {
        // Arrange
        SceneManager.LoadScene(1);
        yield return null;
        var pauseMenu = new GameObject().AddComponent<PauseMenu>();

        // Act
        pauseMenu.Restart();
        yield return new WaitForSeconds(0.6f);

        // Assert
        Assert.AreEqual(1f, Time.timeScale);
        Assert.AreEqual(1, SceneManager.GetActiveScene().buildIndex);

        yield return null;
    }

    [TearDown]
    public void Teardown()
    {
        SceneManager.LoadScene("Test");
    }
}
