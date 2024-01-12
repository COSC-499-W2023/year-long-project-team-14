using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using LootLocker.Requests; 

public class LeaderboardManager : MonoBehaviour 
{
    public bool connected = false;

    public TMP_Text[] playerRanks = new TMP_Text[10];
    public TMP_Text[] playerNames = new TMP_Text[10];
    public TMP_Text[] playerScores = new TMP_Text[10];

    public int players = 1;
    public int difficulty = 1;

    void Start()
    {
        StartCoroutine(LoginRoutine());
    }

    IEnumerator LoginRoutine()
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
                StartCoroutine(LoginRoutine());
            }
        });
        yield return new WaitWhile(() => done == false);
    }

    public void SubmitScore(int score, string leaderboardID)
    {   
        if(connected)
        {
            bool done = false;

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

    public void FetchHighscores(string leaderboardID)
    {   
        if(connected)
        {
            bool done = false;
            
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

    public void DisplayHighscore(int iteration, int rank, string name, int memberID, int score)
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
    }

    public string FormatTime(int t)
    {
        print(t);
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


}
