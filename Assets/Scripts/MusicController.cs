using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioSource[] audioSources; // Array to hold multiple AudioSources
    private bool isMuted = false;
    private int currentTrack = -1; // Keep track of the currently playing track

    void Start()
    {
        StopAllMusic();
        // Optionally initialize audioSources if not set in the Inspector
        if (audioSources == null || audioSources.Length == 0)
        {
            audioSources = GetComponents<AudioSource>();
        }

        // Start by playing the menu music
        PlayMenuMusic();
    }

    // Method to mute/unmute all audio sources
    public void ToggleMute()
    {
        isMuted = !isMuted;
        foreach (AudioSource source in audioSources)
        {
            source.mute = isMuted;
        }
    }

    // Method to play the menu music (element 0)
    public void PlayMenuMusic()
    {
        if (currentTrack != 0)
        {
            StopCurrentMusic();
            audioSources[0].Play();
            currentTrack = 0;
        }
    }

    // Method to play the in-game music (element 1)
    public void PlayInGameMusic()
    {
        if (currentTrack != 1)
        {
            StopCurrentMusic();
            audioSources[1].Play();
            currentTrack = 1;
        }
    }

    // Method to stop the currently playing music
    private void StopCurrentMusic()
    {
        if (currentTrack >= 0 && currentTrack < audioSources.Length)
        {
            audioSources[currentTrack].Stop();
        }
    }
    private void StopAllMusic()
    {
        foreach (AudioSource source in audioSources)
        {
            source.Stop();
        }
        currentTrack = -1; // Reset the current track
    }


}
