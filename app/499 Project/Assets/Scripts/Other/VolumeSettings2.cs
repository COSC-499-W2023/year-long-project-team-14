using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public AudioClip[] tracks;
    private AudioSource audioSource;
 [SerializeField] private Slider volumeSlider;

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

    void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("Volume");
    }

    void Save()
    {
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ShuffleTracks();
        PlayNextTrack();
    }

void ShuffleTracks()
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
    void PlayNextTrack()
    {
        if (tracks.Length > 0)
        {
            audioSource.clip = tracks[0];
            audioSource.Play();
            
            Invoke("PlayNextTrack", audioSource.clip.length);
        }
    }
}
