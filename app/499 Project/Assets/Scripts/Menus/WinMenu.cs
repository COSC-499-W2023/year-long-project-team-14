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

        void Start()
    {
        StartCoroutine(SelectMenuButton());

        gameMaster = GameObject.FindObjectOfType<GameMaster>();
    }

    void Update()
    {
        if (gameMaster != null && timetext != null)
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

    public IEnumerator SelectMenuButton()
    {
        if (Gamepad.all.Count > 0 && EventSystem.current != null)
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                if (restartButton != null)
                {
                    EventSystem.current.SetSelectedGameObject(restartButton);
                }
            }
        }
        else
        {
            EventSystem.current?.SetSelectedGameObject(null);
        }

        yield return new WaitForSecondsRealtime(1f);

        StartCoroutine(SelectMenuButton());
    }
 
}
