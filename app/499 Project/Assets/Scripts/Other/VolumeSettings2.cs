using UnityEngine;
using UnityEngine.UI; 
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public AudioClip[] tracks;
    public AudioSource audioSource;
    [SerializeField] public Slider volumeSlider;

    private int currentTrackIndex = 0;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();

        ShuffleTracks();

        PlayNextTrack();

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
        float volume = volumeSlider.value; 
        AudioListener.volume = volume; 
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

    public void ShuffleTracks()
    {
        List<AudioClip> shuffledTracks = new List<AudioClip>(tracks);
        int n = shuffledTracks.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            AudioClip value = shuffledTracks[k];
            shuffledTracks[k] = shuffledTracks[n];
            shuffledTracks[n] = value;
        }
        tracks = shuffledTracks.ToArray();
    }

    public void PlayNextTrack()
    {
        if (tracks.Length > 0)
        {
            audioSource.clip = tracks[currentTrackIndex];
            audioSource.Play();

            currentTrackIndex++;
            if (currentTrackIndex >= tracks.Length)
            {
                currentTrackIndex = 0;
                ShuffleTracks(); 
            }

            audioSource.loop = false;
            audioSource.Play();

            Invoke("PlayNextTrack", audioSource.clip.length);
        }
    }
}
