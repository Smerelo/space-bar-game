using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class AlertArrow : MonoBehaviour
{
    private AudioSource source;

    public bool PlayingSound { get; private set; }

    private void Awake()
    {
        source = GetComponent<AudioSource>();

    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PlaySound()
    {
        if (!PlayingSound)
        {
            PlayingSound = true;
            source.Play();
        }
    }

    public void StopSound()
    {
        PlayingSound = false;
        source.Stop();

    }


}
