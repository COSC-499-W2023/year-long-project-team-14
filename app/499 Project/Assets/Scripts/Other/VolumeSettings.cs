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
playerPrefs.GetFloat("Volume", 1);
  Load();}
 else
 {
Load(); }
public void ChangeVolume()
{
    AudioListener.volume = volumeSlider.value;
    Save();
}

private void Load()
{
 volumeSlider.value = PlayerPrefs.GetFloat("Volume");
}

private void Save()
{
 PlayerPrefs.SetFloat("Volume", volumeSlider.value);
}
}

