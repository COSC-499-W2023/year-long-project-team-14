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
    public IEnumerator SubmitScoreTest()
    {
        //Load into game scene
        SceneManager.LoadScene("GameScene");
        yield return null;

        //Get LeaderboardManager
        leaderboardManager = GameObject.FindWithTag("LeaderboardManager").GetComponent<LeaderboardManager>();
        yield return null;


        //Connect to LootLocker
        leaderboardManager.StartSession();
        yield return new WaitForSeconds(1);

        //Check that their are no errors when connecting
        Assert.IsTrue(leaderboardManager.connected);


        //Upload score to leaderboard
        leaderboardManager.SubmitScore(100, "Test");
        yield return new WaitForSeconds(1);

        //Check that their are no errors when uploading a score
        Assert.IsTrue(leaderboardManager.connected);
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


        //Connect to LootLocker
        leaderboardManager.StartSession();
        yield return new WaitForSeconds(1);

        //Check that their are no errors when connecting
        Assert.IsTrue(leaderboardManager.connected);


        //Retrieve scores from leaderboard
        leaderboardManager.FetchHighscores("Test");
        yield return new WaitForSeconds(1);

        //Check that their are no errors when retrieving a score 
        Assert.IsTrue(leaderboardManager.connected);

        //Check that the scores are being formatted and displayed correctly
        Assert.IsTrue(leaderboardManager.playerScores[0].text == "00:01.00");
    }

    [UnityTearDown]
    public IEnumerator Teardown()
    {
        //Load into test scene for the next test
        SceneManager.LoadScene("Test");
        yield return null;
    }
}
