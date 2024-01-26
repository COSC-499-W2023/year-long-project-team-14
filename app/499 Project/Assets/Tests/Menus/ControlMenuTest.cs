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
        //Load into game scene
        SceneManager.LoadScene(1);
        yield return null;

        //Set up control menu
        GameObject canvas = GameObject.FindWithTag("Canvas");
        ControlMenu controlMenu = canvas.GetComponent<ControlMenu>();
        controlMenu.pauseMenu = canvas.GetComponent<PauseMenu>();
        controlMenu.controlMenuUI = new GameObject();
        controlMenu.pauseMenuUI = new GameObject();

        //Call Back function and check that the pause menu is active and control menu is not
        controlMenu.Back();
        Assert.IsFalse(controlMenu.controlMenuUI.activeSelf);
        Assert.IsTrue(controlMenu.pauseMenuUI.activeSelf);
    }

    [UnityTest]
    public IEnumerator ControlMenuButtonTest()
    {
        //Load into game scene
        SceneManager.LoadScene(1);
        yield return null;

        //Set up control menu
        GameObject canvas = GameObject.FindWithTag("Canvas");
        ControlMenu controlMenu = canvas.GetComponent<ControlMenu>();
        controlMenu.pauseMenu = canvas.GetComponent<PauseMenu>();
        controlMenu.controlMenuUI = new GameObject();
        controlMenu.pauseMenuUI = new GameObject();

        //Call ControlMenuButton function and check that the control menu is active and pause menu is not
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
