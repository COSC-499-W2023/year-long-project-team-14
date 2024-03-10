using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] public Slider volumeSlider;

    void Start()
    {
        if (PlayerPrefs.HasKey("Volume"))
        {
            AudioListener.volume = PlayerPrefs.GetFloat("Volume", 1);
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

   public void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("Volume");
    }

   public void Save()
    {
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
    }
}
