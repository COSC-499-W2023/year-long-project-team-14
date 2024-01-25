using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class ControlMenuTest
{

    [UnityTest]
    public IEnumerator ControlMenu_ResumeGame()
    {
        SceneManager.LoadScene(0);
        yield return null;
        var controlMenu = new GameObject().AddComponent<ControlMenu>();
        controlMenu.controlMenuUI = new GameObject();
        controlMenu.resumeButton = new GameObject();

        controlMenu.Resume();

        Assert.IsFalse(controlMenu.controlMenuUI.activeSelf);
        Assert.AreEqual(1f, Time.timeScale);

        yield return null;
    }

    [UnityTest]
    public IEnumerator controlMenu_LoadMenu()
    {
        SceneManager.LoadScene(0);
        yield return null;
        var controlMenu = new GameObject().AddComponent<ControlMenu>();

        controlMenu.LoadMenu();
        yield return new WaitForSeconds(0.6f);

        Assert.AreEqual(1f, Time.timeScale);
        Assert.AreEqual(0, SceneManager.GetActiveScene().buildIndex);

        yield return null;
    }


    [TearDown]
    public void Teardown()
    {
        SceneManager.LoadScene("Test");
    }
}
