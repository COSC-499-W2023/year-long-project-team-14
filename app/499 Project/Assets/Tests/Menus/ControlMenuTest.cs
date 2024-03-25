using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class ControlMenuTest
{

    [UnityTest]
    public IEnumerator ControlMenuBackTest()
    {
        SceneManager.LoadScene(1);
        yield return null;

        GameObject canvas = GameObject.FindWithTag("Canvas");
        ControlMenu controlMenu = canvas.GetComponent<ControlMenu>();
        controlMenu.optionsMenu = canvas.GetComponent<OptionsMenu>();
        controlMenu.controlMenuUI = new GameObject();
        controlMenu.optionsMenuUI = new GameObject();

        controlMenu.Back();
        Assert.IsFalse(controlMenu.controlMenuUI.activeSelf);
        Assert.IsTrue(controlMenu.optionsMenuUI.activeSelf);
    }

    [UnityTest]
    public IEnumerator ControlMenuButtonTest()
    {
        SceneManager.LoadScene(1);
        yield return null;

        GameObject canvas = GameObject.FindWithTag("Canvas");
        ControlMenu controlMenu = canvas.GetComponent<ControlMenu>();
        controlMenu.pauseMenu = canvas.GetComponent<PauseMenu>();
        controlMenu.controlMenuUI = new GameObject();
        controlMenu.pauseMenuUI = new GameObject();

        controlMenu.ControlMenuButton();
        Assert.IsTrue(controlMenu.controlMenuUI.activeSelf);
        Assert.IsFalse(controlMenu.pauseMenuUI.activeSelf);
    }

    [TearDown]
    public void Teardown()
    {
        SceneManager.LoadScene("Test");
    }
}
