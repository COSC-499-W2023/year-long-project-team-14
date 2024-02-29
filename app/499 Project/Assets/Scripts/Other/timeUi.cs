using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timeUi : MonoBehaviour
{
    public GameMaster gameMaster;
    private Text timeText;

    void Start()
    {
        timeText = GetComponent<Text>();
    }

    void Update()
    {
        if (gameMaster != null && timeText != null)
        {
            double t = Math.Floor(gameMaster.gameTime * 100);
            double time = t / 100;
            double ms = time % 1;
            double s = Math.Floor((time - ms) % 60);
            double m = Math.Floor((time - s - ms) / 60);

            string score = "";

            if (m < 10)
                score += "0" + m;
            else
                score += "" + m;

            if (s < 10)
                score += ":0" + s;
            else
                score += ":" + s;

            ms = Math.Floor(ms * 100);
            if (ms < 10)
                score += ".0" + ms;
            else
                score += "." + ms;
            timeText.text = "TIME: " + score;
        }
    }
}
