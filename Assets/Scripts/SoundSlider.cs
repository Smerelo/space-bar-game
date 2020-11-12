using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class SoundSlider : MonoBehaviour
{
    private Slider slider;
    private bool unlockSlider;
    private enum SoundType
    {
        Music,
        SoundEffects,
        Master
    }
    [SerializeField] private SoundType soundType;
    private void Start()
    {
        slider = GetComponent<Slider>();
        slider.minValue = 0.0001f;
        slider.maxValue = 1;
        switch (soundType)
        {
            case SoundType.Music:
                slider.value = SoundManager.MusicVolume;
                break;
            case SoundType.SoundEffects:
                slider.value = SoundManager.SfxVolume;
                break;
            case SoundType.Master:
                slider.value = SoundManager.MasterVolume;
                break;
            default:
                Debug.LogError("No sound type selected for slider.");
                break;
        }
    }

    private void Update()
    {

    }

    public void ChangeMusicVolume(float vol)
    {
        switch (soundType)
        {
            case SoundType.Music:
                SoundManager.MusicVolume = vol;
                break;
            case SoundType.SoundEffects:
                SoundManager.SfxVolume = vol;
                break;
            case SoundType.Master:
                SoundManager.MasterVolume = vol;
                break;
            default:
                break;
        }
    }
}
