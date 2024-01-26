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
    public PauseMenu pauseMenu;

    void Start()
    {
        StartCoroutine(SelectMenuButon());
    }

    //Sets control menu to be active and deactivates pause menu
    public void ControlMenuButton()
    {
        controlMenu = true;
        pauseMenu.pauseMenu = false;
        controlMenuUI.SetActive(true); 
        pauseMenuUI.SetActive(false); 
        StartCoroutine(SelectMenuButon());
    }
    
    //Sets pause menu to be active and deactivates control menu
    public void Back()
    {
        controlMenu = false;
        pauseMenu.pauseMenu = true;
        controlMenuUI.SetActive(false); 
        pauseMenuUI.SetActive(true); 
        EventSystem.current.SetSelectedGameObject(null);
    }

    public IEnumerator SelectMenuButon() 
    {
        if(Gamepad.all.Count > 0) 
        {
            if(EventSystem.current.currentSelectedGameObject == null)
            {
                if(controlMenu)
                {
                    EventSystem.current.SetSelectedGameObject(backButton);
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


