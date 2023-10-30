using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.TestTools;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class PauseMenuTest
{
    private PauseMenu pauseMenuScript;

    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("Menu");
        pauseMenuScript = Object.FindObjectOfType<PauseMenu>();
    }

    [UnityTest]
    public IEnumerator PauseMenu_PauseAndResume()
    {
        pauseMenuScript.Pause(new InputAction.CallbackContext());
        yield return new WaitForSeconds(1);
        Assert.AreEqual(true, pauseMenuScript.pauseMenu);
        Assert.AreEqual(true, pauseMenuScript.pauseMenuUI.activeSelf);
        Assert.AreEqual(0f, Time.timeScale);

        pauseMenuScript.Pause(new InputAction.CallbackContext());
        yield return new WaitForSeconds(1);
        Assert.AreEqual(false, pauseMenuScript.pauseMenu);
        Assert.AreEqual(false, pauseMenuScript.pauseMenuUI.activeSelf);
        Assert.AreEqual(1f, Time.timeScale);
        Assert.AreEqual(null, EventSystem.current.currentSelectedGameObject);
    }

    [TearDown]
    public void Teardown()
    {
        SceneManager.UnloadSceneAsync("Menu");
    }
}