using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using LootLocker.Requests; 

public class LeaderboardManager : MonoBehaviour 
{
    public GameMaster gameMaster;
    public bool connected = false;
    public bool done = false;
    public bool inMenu = false;

    public TMP_Text[] playerRanks = new TMP_Text[10];
    public TMP_Text[] playerNames = new TMP_Text[10];
    public TMP_Text[] playerScores = new TMP_Text[10];

    public TMP_Text scoreButtonText;
    public TMP_Text playerButtonText;
    public TMP_Text difficultyButtonText;
     public TMP_Text displayNameText;

    public int scoreType = 1;
    public int players = 1;
    public int difficulty = 1;

    void Start()
    {
        StartCoroutine(StartSession());
    }
    
    public IEnumerator StartSession() //Connect to LootLocker
    {     
        if(!connected)   
        {
            done = false;

            LootLockerSDKManager.StartGuestSession((response) =>
            {
                if(response.success)
                {
                    PlayerPrefs.SetString("PlayerID", response.player_id.ToString());
                    done = true;
                    connected = true;    
                }
                else
                {
                    print("LootLocker failed to connect");
                    done = true;
                }
            });
            yield return new WaitWhile(() => done == false);

            if(inMenu)
            {
                if(PlayerPrefs.GetString("DisplayName") == "")
                    displayNameText.text = "Enter Name";
                else
                    displayNameText.text = "" +PlayerPrefs.GetString("DisplayName");
                
                StartCoroutine(UpdatePlayerName());
            }
        }

        yield return new WaitForSeconds(5);
        StartCoroutine(StartSession());
    }

    public void SetPlayerName(string name) //Change name locally
    {
        done = false;

        PlayerPrefs.SetString("DisplayName", name); 

        if(PlayerPrefs.GetString("DisplayName") == "")
            displayNameText.text = "Enter Name";
        else
            displayNameText.text = "" +PlayerPrefs.GetString("DisplayName");

        StartCoroutine(UpdatePlayerName());
    }

    public IEnumerator UpdatePlayerName() //Update online leaderboard name
    {
        done = false;

        LootLockerSDKManager.SetPlayerName(PlayerPrefs.GetString("DisplayName"), (response) =>
        {
            if(response.success)
            {
                done = true;
                connected = true;
            }
            else
            {
                print("Failed to set player name");
                done = true;
                connected = false;
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    public void SubmitScore(int score, string leaderboardID) //Save and upload score to correct leaderboard
    {   
        done = false;

        if(leaderboardID == null)
        {
            if(gameMaster.difficulty == 1) leaderboardID = "Easy";
            else if(gameMaster.difficulty == 2) leaderboardID = "Medium";
            else if(gameMaster.difficulty == 3) leaderboardID = "Hard";
            else if(gameMaster.difficulty == 4) leaderboardID = "Extreme";

            if(gameMaster.playerCount == 1) leaderboardID += "1";
            else leaderboardID += "2";
            leaderboardID += "Player";
        }

        SaveScore(score, leaderboardID);

        LootLockerSDKManager.SubmitScore(PlayerPrefs.GetString("PlayerID"), score, leaderboardID, (response) =>
        {
            if(response.success)
            {
                done = true;
                connected = true;
            }
            else
            {
                print("Failed to upload score");
                done = true;
                connected = false;
            }
        });
    }

    public void SaveScore(int score, string leaderboardID) //Saves scores locally
    {
        if(leaderboardID == "Easy1Player")
            PlayerPrefs.SetInt("Easy1Player", score);
        else if(leaderboardID == "Medium1Player")
            PlayerPrefs.SetInt("Medium1Player", score);
        else if(leaderboardID == "Hard1Player")
            PlayerPrefs.SetInt("Hard1Player", score);
        else if(leaderboardID == "Extreme1Player")
            PlayerPrefs.SetInt("Extreme1Player", score);
        else if(leaderboardID == "Easy2Player")
            PlayerPrefs.SetInt("Easy2Player", score);
        else if(leaderboardID == "Medium2Player")
            PlayerPrefs.SetInt("Medium2Player", score);
        else if(leaderboardID == "Hard2Player")
            PlayerPrefs.SetInt("Hard2Player", score);
        else if(leaderboardID == "Extreme2Player")
            PlayerPrefs.SetInt("Extreme2Player", score);
        else if(leaderboardID == "UnitTest")
            PlayerPrefs.SetInt("UnitTest", score);
    }

    public void FetchHighscores(string leaderboardID) //Retrieve leaderboard scores 
    {       
        done = false;

        if(leaderboardID == null)
        {
            if(difficulty == 1) leaderboardID = "Easy";
            else if(difficulty == 2) leaderboardID = "Medium";
            else if(difficulty == 3) leaderboardID = "Hard";
            else if(difficulty == 4) leaderboardID = "Extreme";

            if(players == 1) leaderboardID += "1";
            else leaderboardID += "2";
            leaderboardID += "Player";
        }

        LootLockerSDKManager.GetMemberRank(leaderboardID, "0", (response) =>
        {
            if(response.success)
            {
                int start = 0;

                if(scoreType == 1)
                    start = response.rank < 6 ? 0 : response.rank - 5;

                LootLockerSDKManager.GetScoreList(leaderboardID, 10, start, (response) =>
                {
                    if(response.success)
                    {
                        LootLockerLeaderboardMember[] members = response.items;
                        string name = "";

                        for(int i = 0; i < members.Length; i++)
                        {
                            if(members[i].player.name != "")
                            {
                                name = members[i].player.name + "";
                            }
                            else
                            {
                                name = members[i].player.id + "";
                            }
                            DisplayHighscore(i, members[i].rank, name, members[i].player.id, members[i].score);
                        }
                        done = true;
                        connected = true;

                        int blankSpots = 10 - members.Length;
                        for(int i = 0; i < blankSpots; i++)
                        {
                            playerRanks[9 - i].text = " ";
                            playerNames[9 - i].text = " ";
                            playerScores[9 - i].text = " ";
                        }
                    }
                    else
                    {
                        print("Failed to fetch scores");
                        done = true;
                        connected = false;
                    }
                });
            }
            else
            {
                Debug.Log("Failed to get player rank");
                done = true;
                connected = false;
            }
        });
    }

    public void DisplayHighscore(int iteration, int rank, string name, int memberID, int score) //Display leaderboard scores on leaderboard menu
    {     
        if(rank != 0)
            playerRanks[iteration].text = rank + ".";
        else 
            playerRanks[iteration].text = " ";
        
        if(name != null)
            playerNames[iteration].text = name + "";
        else 
            playerNames[iteration].text = " ";
        
        if(score != 0)
            playerScores[iteration].text = FormatTime(score);
        else
            playerScores[iteration].text = " ";

        if(playerNames[iteration].text == PlayerPrefs.GetString("PlayerID") || playerNames[iteration].text == PlayerPrefs.GetString("DisplayName"))
        {
            playerNames[iteration].color = new Color32(255, 255, 0, 255);
            playerRanks[iteration].color = new Color32(255, 255, 0, 255);
            playerScores[iteration].color = new Color32(255, 255, 0, 255);
        }
        else
        {
            playerNames[iteration].color = new Color32(255, 255, 255, 255);
            playerRanks[iteration].color = new Color32(255, 255, 255, 255);
            playerScores[iteration].color = new Color32(255, 255, 255, 255);
        }
    }

    public string FormatTime(int t) //Properly format scores
    {
        double time = ((double)t) / 100;
        double ms = Math.Round((time % 1), 2);
        double s = Math.Round((time - ms) % 60);
        double m = Math.Round((time - s - ms) / 60);

        string score = "";

        if(m < 10)
            score += "0" +m;
        else 
            score += "" +m;

        if(s < 10)
            score += ":0" +s;
        else
            score += ":" +s;

        ms *= 100;
        if(ms < 10)
            score += ".0" +ms;
        else 
            score += "." +ms;

        return score;
    }

    public void DifficultyButton() //Changes which difficulty leaderboard should be displayed
    {
        if(difficulty == 1)
        {
            difficulty++;
            difficultyButtonText.text = "MEDIUM";
        }
        else if(difficulty == 2)
        {
            difficulty++;
            difficultyButtonText.text = "HARD";
        }
        else if(difficulty == 3)
        {
            difficulty++;
            difficultyButtonText.text = "EXTREME";
        }
        else if(difficulty == 4)
        {
            difficulty = 1;
            difficultyButtonText.text = "EASY";
        }
        
        FetchHighscores(null);
    }

    public void PlayerButton() //Changes which player count leaderboard should be displayed
    {
        if(players == 1)
        {
            players = 2;
            playerButtonText.text = "2 PLAYER";
        }
        else
        {
            players = 1;
            playerButtonText.text = "1 PLAYER";
        }
        
        FetchHighscores(null);
    }

    public void ScoreDisplayType() //Changes the leaderboard menu to display your score or the top scores
    {
        if(scoreType == 1)
        {
            scoreType = 2;
            scoreButtonText.text = "TOP SCORES";
        }
        else
        {
            scoreType = 1;
            scoreButtonText.text = "YOUR SCORE";
        }

        FetchHighscores(null);
    }
}