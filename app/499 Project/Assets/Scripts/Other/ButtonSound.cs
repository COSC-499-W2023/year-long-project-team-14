using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    [SerializeField] private AudioSource buttonSound;
    void Start()
    {
        // Find all buttons in the scene and register the function to be called when clicked
        Button[] buttons = FindObjectsOfType<Button>();
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(PlayButtonPressSound);
        }
    }

    void PlayButtonPressSound()
    {
        // Play the assigned sound effect
        if (buttonSound != null)
        {
            buttonSound.Play();
        }
    }
}