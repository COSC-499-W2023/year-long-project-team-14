using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuObject;
    public GameObject optionsMenuObject;
    public GameObject playerMenuObject;
    public GameObject leaderboardMenuObject;
    public GameObject controlMenuObject;

    public GameObject playButton;
    public GameObject player1Button;
    public GameObject player2Button;
    public GameObject musicSlider;
    public GameObject leaderboardBackButton;
    public GameObject controlsBackButton;

    public bool mainMenu = true;
    public bool playerMenu = false;
    public bool optionsMenu = false;
    public bool leaderboardMenu = false;
    public bool controlMenu = true;


    public AudioSource buttonClick;
    public Animator fadeAnim;

    public LeaderboardManager leaderboardManager;
    public GameObject inputField;
    public GameObject quitButton;

    void Start()
    {
        //If playing the browser version, disable quit button because it does nothing
        if(Application.platform == RuntimePlatform.WebGLPlayer)
            quitButton.SetActive(false);
            
        StartCoroutine(SelectMenuButton());
        SelectButton(playButton);
    }

    //Set player menu active and deactivate main menu
    public void Play()
    {
        mainMenu = false;
        playerMenu = true;
        mainMenuObject.SetActive(false);
        playerMenuObject.SetActive(true);
        SelectButton(player1Button);
    }

    //Start the game with 1 player
    public void OnePlayer()
    {
        PlayerPrefs.SetInt("playerCount", 1);
        StartCoroutine(PlayGame());
    }

    //Start the game with 2 players
    public void TwoPlayer()
    {
        PlayerPrefs.SetInt("playerCount", 2);
        StartCoroutine(PlayGame());
    }

    //Set main menu active and deactivate player menu
    public void PlayerBackButton()
    {
        mainMenu = true;
        playerMenu = false;
        mainMenuObject.SetActive(true);
        playerMenuObject.SetActive(false);
        SelectButton(playButton);
    }

    //Load into the game scene
    public IEnumerator PlayGame()
    {
        fadeAnim.Play("ScreenFadeOut");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(1);
    }

    //Set options menu active and deactivate main menu
    public void OptionsMenu()
    {
        optionsMenu = true;
        mainMenu = false;
        optionsMenuObject.SetActive(true); 
        mainMenuObject.SetActive(false); 
        SelectButton(musicSlider);
    }
    
    //Set main menu active and deactivate options menu
    public void OptionsMenuBackButton()
    {
        optionsMenu = false;
        mainMenu = true;
        optionsMenuObject.SetActive(false); 
        mainMenuObject.SetActive(true); 
        SelectButton(playButton);
    }

    //Set leaderboard menu active and deactivate main menu
    public void LeaderboardMenu()
    {
        mainMenu = false;
        leaderboardMenu = true;
        mainMenuObject.SetActive(false);
        leaderboardMenuObject.SetActive(true);
        SelectButton(leaderboardBackButton);
        leaderboardManager.FetchHighscores(null);
    }

    //Set main menu active and deactivate leaderboard menu
    public void LeaderboardBackButton()
    {
        mainMenu = true;
        leaderboardMenu = false;
        mainMenuObject.SetActive(true);
        leaderboardMenuObject.SetActive(false);
        SelectButton(playButton);
    }

    //Set control menu active and deactivate main menu
    public void ControlMenu()
    {
        controlMenu = true;
        mainMenu = false;
        controlMenuObject.SetActive(true); 
        mainMenuObject.SetActive(false); 
        SelectButton(controlsBackButton);
    }
    
    //Set main menu active and deactivate control menu
    public void ControlMenuBackButton()
    {
        controlMenu = false;
        mainMenu = true;
        controlMenuObject.SetActive(false); 
        mainMenuObject.SetActive(true); 
        SelectButton(playButton);
    }

    //Quit the game
    public void QuitGame()
    {
     Debug.Log("Quit");
     Application.Quit();
    }

    //Selects a button for controller navigation
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
                else if(optionsMenu)
                {
                    EventSystem.current.SetSelectedGameObject(musicSlider);
                }
                else if(leaderboardMenu)
                {
                    EventSystem.current.SetSelectedGameObject(leaderboardBackButton);
                }
                else if(controlMenu)
                {
                    EventSystem.current.SetSelectedGameObject(controlsBackButton);
                }
            }
        }
        else if(EventSystem.current.currentSelectedGameObject != inputField)
            EventSystem.current.SetSelectedGameObject(null);
                    
        yield return new WaitForSecondsRealtime(1f);
        StopCoroutine(SelectMenuButton());
        StartCoroutine(SelectMenuButton());
    }

    public void Back(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(playerMenu)
                PlayerBackButton();
            else if(optionsMenu)
                OptionsMenuBackButton();
            else if(leaderboardMenu)
                LeaderboardBackButton();
            else if(controlMenu)
                ControlMenuBackButton();
            else return;
            
            buttonClick.Play();
        }
    }
}


