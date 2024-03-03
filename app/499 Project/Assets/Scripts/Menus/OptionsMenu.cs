using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Audio;


public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider musicVolumeSlider;
    public Slider voiceVolumeSlider;
    public Button backButton;
    public GameObject firstSelected;
    public GameObject pauseMenu;

    private void Start()
    {
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0);
        voiceVolumeSlider.value = PlayerPrefs.GetFloat("VoiceVolume", 0);
        backButton.Select();
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetVoiceVolume(float volume)
    {
        audioMixer.SetFloat("VoiceVolume", volume);
        PlayerPrefs.SetFloat("VoiceVolume", volume);
    }

    public void SetFirstSelected()
    {
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }

    public void Back()
    {
        gameObject.SetActive(false);
        pauseMenu.SetActive(true);
    }
}
    
