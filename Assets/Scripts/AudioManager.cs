using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource musicSource;
    private AudioSource sfxSource;
    [SerializeField] List<AudioClip> backgroundMusic;
    [SerializeField] List<AudioClip> sfx;
    private bool stopped;

    public int CurrentSong { get; private set; }
    public int TotalSongs { get; private set; }
    
    void Start()
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();
        TotalSongs = backgroundMusic.Count;
        musicSource.loop = false;
        sfxSource.loop = false;
        if (TotalSongs == 0) stopped = true;
        musicSource.volume = 0.01f;
    }

    void Update()
    {
        if (!stopped && !musicSource.isPlaying)
        {
            SkipSong();
        }
        if (Input.GetKeyDown("f"))
        {
            Screen.fullScreen = !Screen.fullScreen;
        }
        if (Screen.fullScreen && Input.GetKeyDown("escape"))
        {
            Screen.fullScreen = false;
        }
    }

    public void SkipSong()
    {
        stopped = false;
        musicSource.clip = backgroundMusic[CurrentSong];
        musicSource.Play();
        if (CurrentSong < TotalSongs)
        {
            CurrentSong++;
        }
        else
        {
            CurrentSong = 0;
        }
    }

    public void Stop()
    {
        musicSource.Stop();
        CurrentSong = 0;
        stopped = true;
    }


}
