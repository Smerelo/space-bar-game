using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Audio;

public static class SoundManager
{
    private static float musicVolume;
    private static float sfxVolume;
    private static float masterVolume;
    private static AudioMixer mixer;
    private static AudioSource musicSource;
    private static AudioSource sfxSource;
    private static List<AudioSource> sfxSources;
    public static AudioMixer Mixer
    {
        get
        {
            if (mixer == null)
            {
                mixer = GameAssets.I.mixer;
            }
            return mixer;
        }
        private set
        {
            mixer = value;
        }
    }
    public static float MasterVolume
    {
        get
        {
            return masterVolume;
        }
        set
        {
            masterVolume = value;
            SetVolume();
        }
    }
    public static float MusicVolume
    {
        get
        {
            return musicVolume;
        }
        set
        {
            musicVolume = value;
            SetVolume();
        }
    }
    public static float SfxVolume
    {
        get
        {
            return sfxVolume;
        }
        set
        {
            sfxVolume = value;
            SetVolume();
        }
    }
    public static AudioSource MusicSource
    {
        get
        {
            CreateSources();
            return musicSource;
        }
        private set
        {
            musicSource = value;
        }
    } 
    
    public static AudioSource SfxSource
    {
        get
        {
            CreateSources();
            return sfxSource;
        }
        private set
        {
            sfxSource = value;
        }
    }

    private static GameObject soundAndMusic;
    private static void SetVolume()
    {
        CreateSources();
        Mixer.SetFloat("MusicVol", 20 * Mathf.Log(MusicVolume));
        Mixer.SetFloat("SfxVol", 20 * Mathf.Log(SfxVolume));
        Mixer.SetFloat("MasterVol", 20 * Mathf.Log(MasterVolume));
    }

    private static void CreateSources()
    {
        if (soundAndMusic == null)
        {
            soundAndMusic = new GameObject("SoundAndMusic");
        }
        if (musicSource == null)
        {
            GameObject music = new GameObject("Music");
            music.transform.SetParent(soundAndMusic.transform);
            musicSource = music.AddComponent<AudioSource>();
            musicSource.outputAudioMixerGroup = Mixer.FindMatchingGroups("Music")[0];
        }
        if (sfxSources == null)
        {
            GameObject sfx = new GameObject("Sfx");
            sfx.transform.SetParent(soundAndMusic.transform);
            sfxSource = sfx.AddComponent<AudioSource>();
            sfxSources = new List<AudioSource>();
            sfxSource.outputAudioMixerGroup = mixer.FindMatchingGroups("Sfx")[0];
        }
    }
    //form here:https://www.youtube.com/watch?v=QL29aTa7J5Q
}
