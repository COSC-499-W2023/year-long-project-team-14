using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
        if (gameMaster != null)
        {
            float time = gameMaster.gameTime;
            timetext.text = "Time: " + time;
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
