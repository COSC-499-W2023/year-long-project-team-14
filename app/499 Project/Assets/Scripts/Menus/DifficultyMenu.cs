using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class DifficultyMenu : MonoBehaviour
{

    public GameObject mainMenuObject;
    public GameObject playButton;
    public bool playerMenu = false;
    public bool difficultyMenu = false;
    public GameObject playerMenuObject;
    public GameObject difficultyMenuObject;
    public GameObject easyButton;
    public GameObject mediumButton;
    public GameObject hardButton;
    public GameObject backButton;


    void Start()
    {  
        StartCoroutine(SelectMenuButton());
        SelectButton(playButton);
    }

    public void BackButton()
    {
        difficultyMenu = false;
        playerMenu = true;
        playerMenuObject.SetActive(true);
        difficultyMenuObject.SetActive(false);
        SelectButton(playButton);
    }

    public void Easy()
    {
        PlayerPrefs.SetInt("difficulty", 0);
        SceneManager.LoadScene("Game");
    }

    public void Medium()
    {
        PlayerPrefs.SetInt("difficulty", 1);
        SceneManager.LoadScene("Game");
    }

    public void Hard()
    {
        PlayerPrefs.SetInt("difficulty", 2);
        SceneManager.LoadScene("Game");
    }

    public void Madness()
    {
        PlayerPrefs.SetInt("difficulty", 3);
        SceneManager.LoadScene("Game");
    }

    public void SelectButton(GameObject button)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(button);
    }

    public IEnumerator SelectMenuButton()
    {
        yield return new WaitForEndOfFrame();
        SelectButton(playButton);
    }

    public void OnBack(InputValue value)
    {
        if (playerMenu)
        {
            playerMenu = false;
            mainMenuObject.SetActive(true);
            playerMenuObject.SetActive(false);
            SelectButton(playButton);
        }
        else if (difficultyMenu)
        {
            difficultyMenu = false;
            playerMenu = true;
            playerMenuObject.SetActive(true);
            difficultyMenuObject.SetActive(false);
            SelectButton(backButton);
        }
    }

}
