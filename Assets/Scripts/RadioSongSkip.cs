using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioSongSkip : MonoBehaviour
{
    [SerializeField] AudioManager audioManager;
    private Animator animator;
    private Color startcolor;
    private Vector3 originalScale;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("Mute", false);
    }

    void Update()
    {

    }

    private void OnMouseDown()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (hit.collider != null)
        {
            if (hit.transform == transform)
            {
                CycleSongs();
            }
        }
    }

    private void CycleSongs()
    {
        if (audioManager.CurrentSong == audioManager.TotalSongs)
        {
            animator.SetBool("Mute", true);
            audioManager.Stop();
        }
        else
        {
            animator.SetBool("Mute", false);
            audioManager.SkipSong();
        }
    }
    void OnMouseEnter()
    {
        originalScale = transform.localScale;
        transform.localScale = 1.1f * originalScale;

    }
    void OnMouseExit()
    {
        transform.localScale = originalScale;
    }
}
