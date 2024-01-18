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

    public TMP_Text[] playerRanks = new TMP_Text[10];
    public TMP_Text[] playerNames = new TMP_Text[10];
    public TMP_Text[] playerScores = new TMP_Text[10];

    public TMP_Text scoreButtonText;
    public TMP_Text playerButtonText;
    public TMP_Text difficultyButtonText;

    public int scoreType = 1;
    public int players = 1;
    public int difficulty = 1;

    void Start()
    {
        StartSession();
    }

    public void StartSession() //Connect to LootLocker
    {
        bool done = false;

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
                StartSession();
            }
        });
    }

    public void SubmitScore(int score, string leaderboardID) //Save and upload score to correct leaderboard
    {   
        if(connected)
        {
            bool done = false;

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
    }

    public void FetchHighscores(string leaderboardID) //Retrieve leaderboard scores 
    {   
        if(connected)
        {
            bool done = false;
            
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
            
            LootLockerSDKManager.GetScoreList(leaderboardID, 10, 0, (response) =>
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

        if(playerNames[iteration].text == PlayerPrefs.GetString("PlayerID"))
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
            difficultyButtonText.text = "Medium";
        }
        if(difficulty == 2)
        {
            difficulty++;
            difficultyButtonText.text = "Hard";
        }
        if(difficulty == 3)
        {
            difficulty++;
            difficultyButtonText.text = "Extreme";
        }
        if(difficulty == 4)
        {
            difficulty = 1;
            difficultyButtonText.text = "Easy";
        }
        FetchHighscores(null);
    }

    public void PlayerButton() //Changes which player count leaderboard should be displayed
    {
        if(players == 1)
        {
            players = 2;
            playerButtonText.text = "2 Player";
        }
        else
        {
            players = 1;
            playerButtonText.text = "1 Player";
        }
        
        FetchHighscores(null);
    }

    public void ScoreDisplayType() //Work in progress: Changes the leaderboard menu to display your score or the top scores
    {
        if(scoreType == 1)
        {
            scoreType = 2;
            scoreButtonText.text = "Top Scores";
        }
        else
        {
            scoreType = 1;
            scoreButtonText.text = "Your Score";
        }
    }
}
