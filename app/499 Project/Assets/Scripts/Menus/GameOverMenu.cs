using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameOverMenu : MonoBehaviour
{
    public Animator fadeAnim;
    public bool GameIsOver = false;
    public GameObject gameOverMenuUI;
    public GameObject restartButton;
    public bool gameOverMenu = false;
    public int playercount; 

    public void Start()
    {
        StartCoroutine(SelectMenuButton());
        playercount = PlayerPrefs.GetInt("playerCount");
    }

    public void Update(){
        if (playercount == 0 && GameIsOver==false){
            StartCoroutine(ShowGameOverMenu());
        }

    }
    public IEnumerator ShowGameOverMenu()
    {
        GameIsOver = true;
        
        yield return new WaitForSecondsRealtime(1f);

         if (fadeAnim != null)
            fadeAnim.Play("ScreenFadeOut");

        yield return new WaitForSecondsRealtime(0.5f);

        gameOverMenuUI?.SetActive(true);

        if (Gamepad.all.Count > 0)
        {
            EventSystem.current?.SetSelectedGameObject(null);
            EventSystem.current?.SetSelectedGameObject(restartButton);
        }

        if (fadeAnim != null)
            fadeAnim.Play("ScreenFadeIn");
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
        GameIsOver = true;
        if (Gamepad.all.Count > 0)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(restartButton);
        }    
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
