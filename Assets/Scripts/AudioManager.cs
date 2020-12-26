using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] List<AudioClip> backgroundMusic;
    [SerializeField] List<AudioClip> sfx;
    [SerializeField] AudioClip MenuMusic;
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

    public void PlayBossMusic()
    {
        SoundManager.MusicSource.loop = true;
        CurrentSong = 3;
        SoundManager.MusicSource.clip = backgroundMusic[3];
        SoundManager.MusicSource.Play();
    }
    
    public void ResumeNormalMusic()
    {
        int i = UnityEngine.Random.Range(0, 4);
        SoundManager.MusicSource.loop = false;
        SoundManager.MusicSource.clip = backgroundMusic[i];
        CurrentSong = i;
        SoundManager.MusicSource.Play();
    }

    public void PlaySfx(int i)
    {
        SoundManager.SfxSource.clip = sfx[i];
        SoundManager.SfxSource.Play();
    }

    public void PlayMenuMusic()
    {
        SoundManager.MusicSource.loop = true;
        SoundManager.MusicSource.clip = MenuMusic;
        SoundManager.MusicSource.Play();
    }

    public void Stop()
    {
        SoundManager.MusicSource.Stop();
        CurrentSong = 0;
        stopped = true;
    }


}
