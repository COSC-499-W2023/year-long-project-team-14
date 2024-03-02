using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class OptionsMenu : MonoBehaviour
{
    public GameObject eventSystem;
    public GameObject optionsMenuObject;

    public GameObject pauseMenuObject;


    void Start()
    {
        StartCoroutine(SetUpControls());
        StartCoroutine(SelectMenuButton());
        
    }

    public void OptionsMenu() {
        optionsMenu = true;
        mainMenu = false;
        optionsMenuObject.SetActive(true); 
        mainMenuObject.SetActive(false); 
        SelectButton(musicSlider);
        StartCoroutine(SelectMenuButton());
    }

   

}
