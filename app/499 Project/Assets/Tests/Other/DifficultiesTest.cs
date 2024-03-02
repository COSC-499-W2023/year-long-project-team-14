using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

public class DifficultiesTest
{
    private MainMenu mainMenuScript;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        //Load the main menu
        SceneManager.LoadScene("Menu");

        yield return null;

        //Get access to mainMenuScript
        mainMenuScript = Object.FindObjectOfType<MainMenu>();        
    }

    [UnityTest]
    public IEnumerator EasyDifficultyTest()
    {
        //Start the game on easy difficulty
        mainMenuScript.Easy();

        //Wait and check that the game scene is loaded and check that difficulty is 1
        yield return new WaitForSeconds(0.6f);
        yield return null;
        Assert.AreEqual("GameScene", SceneManager.GetActiveScene().name);
        Assert.AreEqual(PlayerPrefs.GetInt("difficulty"), 1);      

        //Check that the difficulty modified enemy movement speed
        GameObject enemy = GameObject.FindWithTag("Enemy");
        Assert.IsTrue(enemy.GetComponent<EnemyMovement>().movementSpeed < 5);
    }

    [UnityTest]
    public IEnumerator MadnessDifficultyTest()
    {
        //Start the game on madness difficulty
        mainMenuScript.Madness();

        //Wait and check that the game scene is loaded and check that difficulty is 4
        yield return new WaitForSeconds(0.6f);
        yield return null;
        Assert.AreEqual("GameScene", SceneManager.GetActiveScene().name);
        Assert.AreEqual(PlayerPrefs.GetInt("difficulty"), 4);  

        //Check that the difficulty modified enemy movement speed
        GameObject enemy = GameObject.FindWithTag("Enemy");
        Assert.IsTrue(enemy.GetComponent<EnemyMovement>().movementSpeed > 5);
    }
}
