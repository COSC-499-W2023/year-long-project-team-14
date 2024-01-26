using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ControlMenu : MonoBehaviour
{
   public Animator fadeAnim;
    public GameObject controlMenuUI; 
    public GameObject resumeButton;
    public bool controlMenu = false;

    void Start()
    {
        StartCoroutine(SelectMenuButon());
    }

    public void LoadMenu() {
        StartCoroutine(GoToMenu());
    }

    public IEnumerator GoToMenu()
    {
        if(fadeAnim != null)
            fadeAnim.Play("ScreenFadeOut");
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }


    public void Resume()
    {
        controlMenu = false;
        controlMenuUI.SetActive(false); 
        Time.timeScale = 1f;
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
                    EventSystem.current.SetSelectedGameObject(resumeButton);
                }
            }
        }
        else
            EventSystem.current.SetSelectedGameObject(null);
                    
        yield return new WaitForSecondsRealtime(1f);
        StopCoroutine(SelectMenuButon());
        StartCoroutine(SelectMenuButon());
    }

    public void ControlMenuButton()
    {
        controlMenu = true;
        controlMenuUI.SetActive(true); 
        Time.timeScale = 0f;
        StartCoroutine(SelectMenuButon());
    }
}


