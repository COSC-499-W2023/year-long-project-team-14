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
    public bool controlMenu = false;
    public GameMaster gameMaster;
    public PauseMenu pauseMenu;

    //Sets control menu to be active and deactivates pause menu
    public void ControlMenuButton()
    {
        controlMenu = true;
        pauseMenu.pauseMenu = false;
        controlMenuUI.SetActive(true); 
        pauseMenuUI.SetActive(false); 
        gameMaster.SelectButton(backButton);
    }
    
    //Sets pause menu to be active and deactivates control menu
    public void Back()
    {
        controlMenu = false;
        controlMenuUI.SetActive(false); 
        pauseMenuUI.SetActive(true); 
        gameMaster.SelectButton(pauseMenu.resumeButton);
        StartCoroutine(WaitForBack());
    }

    public IEnumerator WaitForBack()
    {
        yield return null;
        pauseMenu.pauseMenu = true;
    }
}

