using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI; 
    public GameObject resumeButton;
    public bool pauseMenu = false;

    void Start()
    {
        StartCoroutine(SelectMenuButon());
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(GameIsPaused)
            {
                pauseMenu = false;
                pauseMenuUI.SetActive(false); 
                Time.timeScale = 1f;
                GameIsPaused = false;
                EventSystem.current.SetSelectedGameObject(null);
            }
            else
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
        }
    }

    public void LoadMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void Resume()
    {
        pauseMenu = false;
        pauseMenuUI.SetActive(false); 
        Time.timeScale = 1f;
        GameIsPaused = false;
        EventSystem.current.SetSelectedGameObject(null);
    }

    public IEnumerator SelectMenuButon() 
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
        StopCoroutine(SelectMenuButon());
        StartCoroutine(SelectMenuButon());
    }
}


