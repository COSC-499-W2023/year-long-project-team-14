using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

[TestFixture]
public class VolumeSettingsTests
{
    [Test]
    public void ChangeVolume_SetsAudioListenerVolume()
    {
        GameObject go = new GameObject();
        VolumeSettings volumeSettings = go.AddComponent<VolumeSettings>();
        Slider slider = go.AddComponent<Slider>();
        volumeSettings.volumeSlider = slider;

        float expectedVolume = 0.0f; 

        volumeSettings.ChangeVolume();

        Assert.AreEqual(expectedVolume, AudioListener.volume);
    }

    [Test]
    public void Load_SetsVolumeSliderValue()
    {
        GameObject go = new GameObject();
        VolumeSettings volumeSettings = go.AddComponent<VolumeSettings>();
        Slider slider = go.AddComponent<Slider>();
        volumeSettings.volumeSlider = slider;

        float expectedSliderValue = 0.5f; 

        PlayerPrefs.SetFloat("Volume", expectedSliderValue);

        volumeSettings.Load();

        Assert.AreEqual(expectedSliderValue, volumeSettings.volumeSlider.value);
    }

    [Test]
    public void Save_SavesVolumeSliderValue()
    {
        GameObject go = new GameObject();
        VolumeSettings volumeSettings = go.AddComponent<VolumeSettings>();
        Slider slider = go.AddComponent<Slider>();
        volumeSettings.volumeSlider = slider;

        float expectedSliderValue = 0.8f; 

        volumeSettings.volumeSlider.value = expectedSliderValue;
        volumeSettings.Save();

        Assert.AreEqual(expectedSliderValue, PlayerPrefs.GetFloat("Volume"));
    }
    
}
