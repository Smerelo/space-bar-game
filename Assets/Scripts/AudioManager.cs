using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] List<AudioClip> backgroundMusic;
    private bool stopped;

    public int CurrentSong { get; private set; }
    public int TotalSongs { get; private set; }

    private void Awake()
    {
        SoundManager.MusicVolume = 0.7f;
    }
    void Start()
    {
        TotalSongs = backgroundMusic.Count;
        SoundManager.MusicSource.loop = false;
        if (TotalSongs == 0) stopped = true;
    }

    void Update()
    {
        if (!stopped && !SoundManager.MusicSource.isPlaying)
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
        SoundManager.MusicSource.clip = backgroundMusic[CurrentSong];
        SoundManager.MusicSource.Play();
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
        SoundManager.MusicSource.Stop();
        CurrentSong = 0;
        stopped = true;
    }


}
