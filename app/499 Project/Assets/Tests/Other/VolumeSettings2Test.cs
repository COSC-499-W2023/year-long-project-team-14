using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using System.Collections;

[TestFixture]
public class VolumeSettings2Tests
{
    [Test]
    public void ChangeVolume_SetsAudioListenerVolume()
    {
        GameObject go = new GameObject();
        MusicManager musicManager = go.AddComponent<MusicManager>();
        Slider slider = go.AddComponent<Slider>();
        musicManager.volumeSlider = slider;

        float expectedVolume = 0.0f; 

        musicManager.ChangeVolume();

        Assert.AreEqual(expectedVolume, AudioListener.volume);
    }

    [Test]
    public void Load_SetsVolumeSliderValue()
    {
        GameObject go = new GameObject();
        MusicManager musicManager = go.AddComponent<MusicManager>();
        Slider slider = go.AddComponent<Slider>();
        musicManager.volumeSlider = slider;

        float expectedSliderValue = 0.5f; 

        PlayerPrefs.SetFloat("Volume", expectedSliderValue);

        musicManager.Load();

        Assert.AreEqual(expectedSliderValue, musicManager.volumeSlider.value);
    }

    [Test]
    public void Save_SavesVolumeSliderValue()
    {
        GameObject go = new GameObject();
        MusicManager musicManager = go.AddComponent<MusicManager>();
        Slider slider = go.AddComponent<Slider>();
        musicManager.volumeSlider = slider;

        float expectedSliderValue = 0.8f; 

        musicManager.volumeSlider.value = expectedSliderValue;
        musicManager.Save();

        Assert.AreEqual(expectedSliderValue, PlayerPrefs.GetFloat("Volume"));
    }

    [Test]
    public void Shuffletrack()
    {  
        GameObject go = new GameObject();
        MusicManager musicManager = go.AddComponent<MusicManager>();
        AudioClip[] tracks = new AudioClip[3];
        musicManager.tracks = tracks;
        musicManager.ShuffleTracks();
        Assert.IsNotNull(musicManager.tracks);
    }

  
    [Test]
    public void PlayMinibossTrack_OnLevelChange()
    {
        GameObject go = new GameObject();
        MusicManager musicManager = go.AddComponent<MusicManager>();

        Slider slider = go.AddComponent<Slider>();
        musicManager.volumeSlider = slider;

        GameMaster gameMaster = go.AddComponent<GameMaster>();

        gameMaster.currentLevel = 10;
        musicManager.gameMaster = gameMaster;
        musicManager.previousLevel = -1; 

        AudioSource audioSource = go.AddComponent<AudioSource>();
        musicManager.audioSource = audioSource;

        AudioClip minibossTrack = AudioClip.Create("miniboss", 44100, 2, 44100, false);
        musicManager.minibossTrack = minibossTrack;

        musicManager.Update();

        Assert.IsTrue(audioSource.isPlaying); 
        Assert.AreEqual(minibossTrack, audioSource.clip);
        Assert.AreEqual(gameMaster.currentLevel, musicManager.previousLevel); 
    }
}



