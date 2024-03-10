using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;

public class VolumeSettings : MonoBehaviour
{
        [SerializeField] private Slider volumeSlider;

public void ChangeVolume()
{
    AudioListener.volume = volumeSlider.value;
}
}
