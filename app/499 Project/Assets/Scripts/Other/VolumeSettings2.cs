using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public AudioClip[] tracks;
    public AudioClip minibossTrack; 
    public AudioSource audioSource;
    [SerializeField] public Slider volumeSlider;

    private int currentTrackIndex = 0;
    private GameMaster gameMaster;
    private int previousLevel = -1;

    public void Start()
    {
        gameMaster = FindObjectOfType<GameMaster>();
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
            AudioClip nextClip;

            if (gameMaster.currentLevel == 6)
                nextClip = minibossTrack;
            else
            {
                nextClip = tracks[currentTrackIndex];
                currentTrackIndex++;
                if (currentTrackIndex >= tracks.Length)
                {
                    currentTrackIndex = 0;
                    ShuffleTracks(); 
                }
            }

            audioSource.clip = nextClip;
            audioSource.Play();

            Invoke("PlayNextTrack", nextClip.length);
        }
    }

    void Update()
    {
        if (gameMaster.currentLevel != previousLevel)
        {
            if (gameMaster.currentLevel == 6)
            {
                audioSource.Stop();
                audioSource.clip = minibossTrack;
                audioSource.Play();
            }
            else
            {
                PlayNextTrack();
            }

            previousLevel = gameMaster.currentLevel;
        }
    }
}
