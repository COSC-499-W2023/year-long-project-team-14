using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;

public class WinMenu : MonoBehaviour
{
    public Animator fadeAnim;
    public GameObject WinMenuUI;
    public GameObject restartButton;
    public int playercount;    
    public GameMaster gameMaster;
    public Text timetext;
    public bool winMenu = false;

    void Start()
    {
        gameMaster = GameObject.FindObjectOfType<GameMaster>();
    }

    void Update()
    {
        if (gameMaster != null && timetext != null)
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
            timetext.text = "TIME: " + score;
        }
    }
 public void LoadMenu()
    {
        StartCoroutine(GoToMenu());
    }

    public IEnumerator GoToMenu()
    {
        if (fadeAnim != null)
            fadeAnim.Play("ScreenFadeOut");
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void Restart()
    {
        StartCoroutine(RestartGame());
    }

    public IEnumerator RestartGame()
    {
        if (fadeAnim != null)
            fadeAnim.Play("ScreenFadeOut");
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

}
