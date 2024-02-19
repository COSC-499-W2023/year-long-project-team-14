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
    public GameMaster gameMaster;

    public void Start()
    {
        playercount = PlayerPrefs.GetInt("playerCount");
    }

    //If both players are dead, end game
    public void Update(){
        if (playercount == 0 && GameIsOver==false){
            StartCoroutine(ShowGameOverMenu());
        }
    }

    //Display the game over menu
    public IEnumerator ShowGameOverMenu()
    {
        GameIsOver = true;
        gameOverMenu = true;

        if(gameMaster != null)
            gameMaster.stopTimer = true;

        yield return new WaitForSecondsRealtime(1f);

         if (fadeAnim != null)
            fadeAnim.Play("ScreenFadeOut");

        yield return new WaitForSecondsRealtime(0.5f);

        gameOverMenuUI?.SetActive(true);

        if(gameMaster != null)
            gameMaster.SelectButton(restartButton);

        if (fadeAnim != null)
            fadeAnim.Play("ScreenFadeIn");
    }

    //Load into menu scene
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

    //Restart game
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
}
