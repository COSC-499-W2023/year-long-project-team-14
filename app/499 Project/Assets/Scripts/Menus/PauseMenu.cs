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
    public GameMaster gameMaster;
    public MusicManager musicManager;

    public void OnEnable()
    {
        if (pauseMenuUI != null)
            EventSystem.current.SetSelectedGameObject(resumeButton);
    }

    public void Update()
    {
        // if (Keyboard.current.escapeKey.wasPressedThisFrame)
        // {
        //     if (GameIsPaused)
        //     {
        //         Resume();
        //     }
        //     else
        //     {
        //         Pause();
        //     }
        // }
    }

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
    public void Pause()
    {
        pauseMenu = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        if(gameMaster != null)
            gameMaster.SelectButton(resumeButton);
        
        if(musicManager != null)
            musicManager.audioSource.Pause();
    }

    public void Resume()
    {
        pauseMenu = false;
        pauseMenuUI.SetActive(false); 
        Time.timeScale = 1f;
        GameIsPaused = false;
        EventSystem.current.SetSelectedGameObject(null);

        if(musicManager != null)
            musicManager.audioSource.Play();
    }
}


