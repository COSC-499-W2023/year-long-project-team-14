using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameOverMenu : MonoBehaviour
{
    public Animator fadeAnim;
    public static bool GameIsOver = false;
    public GameObject gameOverMenuUI;
    public GameObject restartButton;
    public bool gameOverMenu = false;
    

    void Start()
    {
        StartCoroutine(SelectMenuButton());
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
        GameIsOver = false;
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
        GameIsOver = false;
        SceneManager.LoadScene(1);
    }

    public void GameOver()
    {
        gameOverMenu = true;
        gameOverMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsOver = true;
        if (Gamepad.all.Count > 0)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(restartButton);
        }
    }

    public IEnumerator SelectMenuButton()
    {
        if (Gamepad.all.Count > 0)
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                if (gameOverMenu)
                {
                    EventSystem.current.SetSelectedGameObject(restartButton);
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
