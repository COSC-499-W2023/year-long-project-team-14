using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverMenuTest
{
    [UnityTest]
    public IEnumerator ShouldShowGameOverMenu()
    {
       var gameOverMenu = new GameObject().AddComponent<GameOverMenu>();

        yield return gameOverMenu.ShowGameOverMenu();

        Assert.IsTrue(gameOverMenu.GameIsOver, "GameIsOver should be true");

    }

    [UnityTest]
    public IEnumerator GameOver_LoadMenu()
    {
        SceneManager.LoadScene(0);
        yield return null;
        var pauseMenu = new GameObject().AddComponent<GameOverMenu>();

        pauseMenu.LoadMenu();
        yield return new WaitForSeconds(0.6f);

        Assert.AreEqual(1f, Time.timeScale);
        Assert.AreEqual(0, SceneManager.GetActiveScene().buildIndex);

        yield return null;
    }

    [UnityTest]
    public IEnumerator PauseMenu_Restart()
    {
        SceneManager.LoadScene(1);
        yield return null;
        var pauseMenu = new GameObject().AddComponent<GameOverMenu>();

        
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
