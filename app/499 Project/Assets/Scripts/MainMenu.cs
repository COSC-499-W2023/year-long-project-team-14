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

    public GameObject playButton;
    public GameObject player1Button;

    public bool mainMenu = true;
    public bool playerMenu = false;

    void Start()
    {
        StartCoroutine(SelectMenuButon());
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
        PlayGame();
    }
    public void TwoPlayer()
    {
        PlayerPrefs.SetInt("playerCount", 2);
        PlayGame();
    }
    public void PlayerBackButton()
    {
        mainMenu = true;
        playerMenu = false;
        mainMenuObject.SetActive(true);
        playerMenuObject.SetActive(false);
        SelectButton(playButton);
    }


    public void PlayGame()
    {
        SceneManager.LoadScene(1);
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
    public IEnumerator SelectMenuButon() 
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
        else
            EventSystem.current.SetSelectedGameObject(null);
                    
        yield return new WaitForSecondsRealtime(1f);
        StopCoroutine(SelectMenuButon());
        StartCoroutine(SelectMenuButon());
    }
}


