using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private Popularity popularity;
    [SerializeField] private Animator[] animators;
    private float score;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowScore()
    {
        score = popularity.GetScore();
        if (score <= 9 && score >= 8)
        {
            animators[4].Play("Stars");
            StartCoroutine( PlayNextAnimation(animators[4], "StarVoid")); 
        }   
        if (score <= 7 && score >= 6)
        {
            animators[4].Play("Stars");
            StartCoroutine(PlayNextAnimation(animators[4], "StarVoid")); 
            animators[3].Play("Stars");
            StartCoroutine(PlayNextAnimation(animators[3], "StarVoid")); 
        }       
        if (score <= 5 && score >= 3)
        {
            animators[4].Play("Stars");
            StartCoroutine(PlayNextAnimation(animators[4], "StarVoid")); 
            animators[3].Play("Stars");
            StartCoroutine(PlayNextAnimation(animators[3], "StarVoid")); 
            animators[2].Play("Stars");
            StartCoroutine(PlayNextAnimation(animators[2], "StarVoid")); 
        }  
        if (score <= 2 && score >= 1)
        {
            animators[4].Play("Stars");
            StartCoroutine(PlayNextAnimation(animators[4], "StarVoid")); 
            animators[3].Play("Stars");
            StartCoroutine(PlayNextAnimation(animators[3], "StarVoid")); 
            animators[2].Play("Stars");
            StartCoroutine(PlayNextAnimation(animators[2], "StarVoid")); 
            animators[1].Play("Stars");
            StartCoroutine(PlayNextAnimation(animators[1], "StarVoid")); 
        }  
    }


    private void StarVoid()
    {
        Debug.Log("here");
        if (score <= 9 && score >= 8)
        {
            animators[4].Play("Starvoid");
        }
        if (score <= 7 && score >= 6)
        {
            animators[4].Play("Starvoid");
            animators[3].Play("Starvoid");
        }
        if (score <= 5 && score >= 3)
        {
            animators[4].Play("Starvoid");
            animators[3].Play("Starvoid");
            animators[2].Play("Starvoid");
        }
        if (score <= 2 && score >= 1)
        {
            animators[4].Play("Starvoid");
            animators[3].Play("Starvoid");
            animators[2].Play("Starvoid");
            animators[1].Play("Starvoid");
        }
    }

    IEnumerator PlayNextAnimation(Animator anim, string method)
    {
        yield return new WaitForEndOfFrame();
        Invoke(method, anim.GetCurrentAnimatorStateInfo(0).length);
    }

}
