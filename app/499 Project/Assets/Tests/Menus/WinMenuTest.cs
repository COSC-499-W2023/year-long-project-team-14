using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class WinMenuTest
{
    [UnityTest]
    public IEnumerator WinMenu_BackToMainMenu()
    {
        SceneManager.LoadScene(0);
        yield return null;
        var winMenu = new GameObject().AddComponent<WinMenu>();

        winMenu.LoadMenu();
        yield return new WaitForSeconds(0.6f);

        Assert.AreEqual(1f, Time.timeScale);
        Assert.AreEqual(0, SceneManager.GetActiveScene().buildIndex);

        yield return null;
    }

    [UnityTest]
    public IEnumerator WinMenu_RestartTheGame()
    {
        SceneManager.LoadScene(0);
        yield return null;
        var winMenu = new GameObject().AddComponent<WinMenu>();

        winMenu.Restart();
        yield return new WaitForSeconds(0.6f);

        // Assert
        Assert.AreEqual(1f, Time.timeScale);
       // Assert.AreEqual(1, SceneManager.GetActiveScene().buildIndex);

    }

    [TearDown]
    public void Teardown()
    {
        SceneManager.LoadScene("Test");
    }
}
