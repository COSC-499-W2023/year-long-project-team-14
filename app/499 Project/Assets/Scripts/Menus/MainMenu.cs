using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuObject;
    public GameObject playerMenuObject;
    public GameObject leaderboardMenuObject;

    public GameObject playButton;
    public GameObject player1Button;
    public GameObject player2Button;
    public GameObject leaderboardBackButton;

    public bool mainMenu = true;
    public bool playerMenu = false;
    public bool leaderboardMenu = true;

    public Animator fadeAnim;

    public LeaderboardManager leaderboardManager;
    public GameObject inputField;
    public GameObject quitButton;

    void Start()
    {
        if(Application.platform == RuntimePlatform.WebGLPlayer)
            quitButton.SetActive(false);
            
        StartCoroutine(SelectMenuButton());
        SelectButton(playButton);
    }

    public void Play()
    {
        mainMenu = false;
        playerMenu = true;
        mainMenuObject.SetActive(false);
        playerMenuObject.SetActive(true);
        SelectButton(player1Button);
    }
    public void OnePlayer()
    {
        PlayerPrefs.SetInt("playerCount", 1);
        StartCoroutine(PlayGame());
    }
    public void TwoPlayer()
    {
        PlayerPrefs.SetInt("playerCount", 2);
        StartCoroutine(PlayGame());
    }
    public void PlayerBackButton()
    {
        mainMenu = true;
        playerMenu = false;
        mainMenuObject.SetActive(true);
        playerMenuObject.SetActive(false);
        SelectButton(playButton);
    }

    public IEnumerator PlayGame()
    {
        fadeAnim.Play("ScreenFadeOut");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(1);
    }

    public void LeaderboardMenu()
    {
        mainMenu = false;
        leaderboardMenu = true;
        mainMenuObject.SetActive(false);
        leaderboardMenuObject.SetActive(true);
        SelectButton(leaderboardBackButton);
        leaderboardManager.FetchHighscores(null);
    }

    public void LeaderboardBackButton()
    {
        mainMenu = true;
        leaderboardMenu = false;
        mainMenuObject.SetActive(true);
        leaderboardMenuObject.SetActive(false);
        SelectButton(playButton);
    }

    public void QuitGame()
    {
     Debug.Log("Quit");
     Application.Quit();
    }

    public void SelectButton(GameObject button)
    {
        if(Gamepad.all.Count > 0)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(button);
        }
    }
    public IEnumerator SelectMenuButton() //Selects a button depending on which menu you are in when using a controller and no buttons are already selected
    {
        if(Gamepad.all.Count > 0) 
        {
            if(EventSystem.current.currentSelectedGameObject == null)
            {
                if(mainMenu)
                {
                    EventSystem.current.SetSelectedGameObject(playButton);
                }
                else if(playerMenu)
                {
                    EventSystem.current.SetSelectedGameObject(player1Button);
                }
            }
        }
        else if(EventSystem.current.currentSelectedGameObject != inputField)
            EventSystem.current.SetSelectedGameObject(null);
                    
        yield return new WaitForSecondsRealtime(1f);
        StopCoroutine(SelectMenuButton());
        StartCoroutine(SelectMenuButton());
    }
}


