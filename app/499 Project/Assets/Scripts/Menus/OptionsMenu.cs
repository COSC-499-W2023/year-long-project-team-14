using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;




public class OptionsMenu : MonoBehaviour
{
    public GameObject eventSystem;
    public GameObject optionsMenuObject;
    public GameObject pauseMenuObject;

    public GameObject mainMenuObject;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider voiceSlider;
    public Slider masterSlider;



    void Start()
    {
        eventSystem = GameObject.Find("EventSystem");
        optionsMenuObject = GameObject.Find("OptionsMenu");
        pauseMenuObject = GameObject.Find("PauseMenu");
        musicSlider = GameObject.Find("MusicSlider").GetComponent<Slider>();
        voiceSlider = GameObject.Find("VoiceSlider").GetComponent<Slider>();
    }

    public void OnEnable() {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        voiceSlider.value = PlayerPrefs.GetFloat("VoiceVolume", 0.5f);
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (optionsMenuObject.activeSelf) {
                CloseOptionsMenu();
            }
        }
    }

    public void CloseOptionsMenu() {
        optionsMenuObject.SetActive(false);
        pauseMenuObject.SetActive(true);
        eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(GameObject.Find("OptionsButton"));
    }

    public void SetMusicVolume(float volume) {
        PlayerPrefs.SetFloat("MusicVolume", volume);
       // AudioManager.instance.SetMusicVolume(volume);
    }

    public void SetVoiceVolume(float volume) {
        PlayerPrefs.SetFloat("VoiceVolume", volume);
     // AudioManager.instance.SetVoiceVolume(volume);
    }


}
