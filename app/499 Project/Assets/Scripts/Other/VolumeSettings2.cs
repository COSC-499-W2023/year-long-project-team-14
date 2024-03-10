using UnityEngine;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] tracks;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ShuffleTracks();
        PlayNextTrack();
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
