using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public Animator fadeAnim;
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI; 
    public GameObject resumeButton;
    public bool pauseMenu = false;

    void Start()
    {
        StartCoroutine(SelectMenuButton());
    }

    //Load into menu scene
    public void LoadMenu() {
        StartCoroutine(GoToMenu());
    }

    public IEnumerator GoToMenu()
    {
        if(fadeAnim != null)
            fadeAnim.Play("ScreenFadeOut");
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.LoadScene(0);
    }

    //Restart the game
    public void Restart()
    {
        StartCoroutine(RestartGame());
    }

    public IEnumerator RestartGame()
    {
        if(fadeAnim != null)
            fadeAnim.Play("ScreenFadeOut");
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.LoadScene(1);
    }

    //Pause the game and display pause menu
    public void Pause()
    {
        pauseMenu = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        if(Gamepad.all.Count > 0)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(resumeButton);
        }
    }

    //Resume the game deactivate pause menu
    public void Resume()
    {
        pauseMenu = false;
        pauseMenuUI.SetActive(false); 
        Time.timeScale = 1f;
        GameIsPaused = false;
        EventSystem.current.SetSelectedGameObject(null);
    }

    public IEnumerator SelectMenuButton() //Selects a button depending on which menu you are in when using a controller and no buttons are already selected
    {
        if(Gamepad.all.Count > 0) 
        {
            if(EventSystem.current.currentSelectedGameObject == null)
            {
                if(pauseMenu)
                {
                    EventSystem.current.SetSelectedGameObject(resumeButton);
                }
            }
        }
        else
            EventSystem.current.SetSelectedGameObject(null);
                    
        yield return new WaitForSecondsRealtime(1f);
        StopCoroutine(SelectMenuButton());
        StartCoroutine(SelectMenuButton());
    }
}

