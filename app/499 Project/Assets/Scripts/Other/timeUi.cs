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
            float t = gameMaster.gameTime;
            double time = ((double)t);
            double ms = Math.Round((time % 1), 2);
            double s = Math.Round((time - ms) % 60);
            double m = Math.Round((time - s - ms) / 60);

            string score = "";

            if (m < 10)
                score += "0" + m;
            else
                score += "" + m;

            if (s < 10)
                score += ":0" + s;
            else
                score += ":" + s;

            ms *= 100;
            if (ms < 10)
                score += ".0" + ms;
            else
                score += "." + ms;
            timeText.text = "TIME: " + score;
        }
    }
}
