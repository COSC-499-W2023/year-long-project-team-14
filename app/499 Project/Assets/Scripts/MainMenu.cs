using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    public GameObject playButton;

    public bool mainMenu = true;

    void Start()
    {
        StartCoroutine(SelectMenuButon());

        if(Gamepad.all.Count > 0)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(playButton);
        }
    }

    public void PlayGame()
    {
     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
     Debug.Log("Quit");
     Application.Quit();
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
            }
        }
        else
            EventSystem.current.SetSelectedGameObject(null);
                    
        yield return new WaitForSecondsRealtime(1f);
        StopCoroutine(SelectMenuButon());
        StartCoroutine(SelectMenuButon());
    }
}


