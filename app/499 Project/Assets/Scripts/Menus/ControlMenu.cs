using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ControlMenu : MonoBehaviour
{
    public GameObject controlMenuUI; 
    public GameObject pauseMenuUI;
    public GameObject backButton;
    public GameObject optionsMenuUI;
    public bool controlMenu = false;
    public GameMaster gameMaster;
    public PauseMenu pauseMenu;
    public bool optionsMenu = false;

    public void ControlMenuButton()
    {
        controlMenu = true;
        pauseMenu.pauseMenu = false;
        controlMenuUI.SetActive(true); 
        pauseMenuUI.SetActive(false); 
        gameMaster.SelectButton(backButton);
    }
    public void Back()
    {
        controlMenu = false;
        controlMenuUI.SetActive(false); 
        optionsMenuUI.SetActive(true); 
        optionsMenu = true;

        StartCoroutine(WaitForBack());
    }


    public IEnumerator WaitForBack()
    {
        yield return null;
        pauseMenu.pauseMenu = true;
    }

    public void SetFirstSelected()
    {
        EventSystem.current.SetSelectedGameObject(backButton);
    }

    public void OnEnable()
    {
        SetFirstSelected();
    }

}

