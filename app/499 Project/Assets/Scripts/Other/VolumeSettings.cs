using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;

    void Start()
    {
        if (PlayerPrefs.HasKey("Volume"))
        {
            AudioListener.volume = PlayerPrefs.GetFloat("Volume", 1); // Fix this line
            Load();
        }
        else
        {
            Load();
        }
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        Save();
    }

    void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("Volume");
    }

    void Save()
    {
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
    }
}
