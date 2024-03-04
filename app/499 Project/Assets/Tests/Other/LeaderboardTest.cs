using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor; 
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class LeaderboardTest : MonoBehaviour
{
    LeaderboardManager leaderboardManager;
    
    [UnityTest]
    public IEnumerator DisplayNameTest()
    {
        //Load into menu
        SceneManager.LoadScene("GameScene");
        yield return null;

        //Get LeaderboardManager
        leaderboardManager = GameObject.FindWithTag("LeaderboardManager").GetComponent<LeaderboardManager>();
        yield return null;


        //Wait while connecting to LootLocker
        yield return new WaitWhile(() => leaderboardManager.done == false);

        //Check that their are no errors when connecting
        Assert.IsTrue(leaderboardManager.connected);


        //Save current display name so it can be set again after the unit test
        string name = PlayerPrefs.GetString("DisplayName");

        //Set display name 
        leaderboardManager.SetPlayerName("Test");
        yield return new WaitWhile(() => leaderboardManager.done == false);

        //Check that display name has been changed
        Assert.IsTrue(PlayerPrefs.GetString("DisplayName") == "Test");

        //Check that their are no errors when updating display name on LootLocker
        Assert.IsTrue(leaderboardManager.connected);

        //Reset display name to what it was before
        leaderboardManager.SetPlayerName(name);
        yield return new WaitWhile(() => leaderboardManager.done == false);
    }

    [UnityTest]
    public IEnumerator SubmitScoreTest()
    {
        //Save previous name and set new name for test
        string name = PlayerPrefs.GetString("DisplayName");
        PlayerPrefs.SetString("DisplayName", "UnitTest");

        //Load into game scene
        SceneManager.LoadScene("GameScene");
        yield return null;

        //Get LeaderboardManager
        leaderboardManager = GameObject.FindWithTag("LeaderboardManager").GetComponent<LeaderboardManager>();
        leaderboardManager.unitTest = true;
        yield return null;

        //Wait while connecting to LootLocker
        yield return new WaitWhile(() => leaderboardManager.done == false);
        yield return new WaitForSeconds(2);

        //Check that their are no errors when connecting
        Assert.IsTrue(leaderboardManager.connected);
        
        //Upload score to leaderboard
        leaderboardManager.Submit();
        yield return new WaitWhile(() => leaderboardManager.done == false);

        //Check that the score was saved locally
        Assert.IsTrue(PlayerPrefs.GetInt("UnitTest") == 100);
        
        //Check that their are no errors when uploading score to LootLocker
        Assert.IsTrue(leaderboardManager.connected);

        //Reset score to 0
        PlayerPrefs.SetInt("UnitTest", 0);

        //Reset name
        PlayerPrefs.SetString("DisplayName", name);
    }

    [UnityTest]
    public IEnumerator FetchScoreTest()
    {
        //Load into menu
        SceneManager.LoadScene("Menu");
        yield return null;

        //Get LeaderboardManager
        leaderboardManager = GameObject.FindWithTag("LeaderboardManager").GetComponent<LeaderboardManager>();
        yield return null;


        //Wait while connecting to LootLocker
        yield return new WaitWhile(() => leaderboardManager.done == false);

        //Check that their are no errors when connecting
        Assert.IsTrue(leaderboardManager.connected);


        //Retrieve scores from leaderboard
        leaderboardManager.FetchHighscores("UnitTest");
        yield return new WaitWhile(() => leaderboardManager.done == false);

        //Check that their are no errors when retrieving a score 
        Assert.IsTrue(leaderboardManager.connected);

        //Check that the scores are being formatted and displayed correctly
        Assert.IsTrue(leaderboardManager.playerScores[0].text == "00:01.00");
    }

    [UnityTest]
    public IEnumerator LeaderboardMenuButtonsTest()
    {
        //Load into menu
        SceneManager.LoadScene("Menu");
        yield return null;

        //Get LeaderboardManager
        leaderboardManager = GameObject.FindWithTag("LeaderboardManager").GetComponent<LeaderboardManager>();
        yield return null;


        //Call each of the functions to change which leaderboard is displayed
        leaderboardManager.DifficultyButton();
        leaderboardManager.PlayerButton();
        leaderboardManager.ScoreDisplayType();

        //Check that each function modifies the leaderboard menu correctly
        Assert.IsTrue(leaderboardManager.difficulty == 2);
        Assert.IsTrue(leaderboardManager.difficultyButtonText.text == "MEDIUM");
        Assert.IsTrue(leaderboardManager.players == 2);
        Assert.IsTrue(leaderboardManager.playerButtonText.text == "2 PLAYER");
        Assert.IsTrue(leaderboardManager.scoreType == 2);
        Assert.IsTrue(leaderboardManager.scoreButtonText.text == "TOP SCORES");
    }

    [UnityTest]
    public IEnumerator LeaderboardButtonsTest()
    {
        //Load into menu
        SceneManager.LoadScene("Menu");
        yield return null;

        //Get MainMenu script
        MainMenu mainMenuScript = Object.FindObjectOfType<MainMenu>();
        yield return null;


        //Call LeaderboardMenu function and check if the leaderboard menu is now active
        mainMenuScript.LeaderboardMenu();
        Assert.IsTrue(mainMenuScript.leaderboardMenuObject.activeSelf);

        //Call LeaderboardBackButton function and check if the main menu is now active
        mainMenuScript.LeaderboardBackButton();
        Assert.IsTrue(mainMenuScript.mainMenuObject.activeSelf);
    }

    [UnityTearDown]
    public IEnumerator Teardown()
    {
        //Load into test scene for the next test
        SceneManager.LoadScene("Test");
        yield return null;
    }
}
