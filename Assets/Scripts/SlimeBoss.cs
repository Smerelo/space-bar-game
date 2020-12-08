using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class SlimeBoss : MonoBehaviour
{

    [SerializeField] private Transform landingPosition;
    [SerializeField] private GameObject mainUi;
    [SerializeField] private GameObject mobileUI;
    [SerializeField] private GameObject sideMenu;
    [SerializeField] private Slider healthBar;

    private string currentState;
    private Animator animator;
    private CinemachineVirtualCamera camera;
    private GameObject healthBarPosition;

    private const string BOSS_FALLING = "Falling";

    internal void Attack()
    {
        throw new NotImplementedException();
    }

    private const string BOSS_IDLE = "Idle";
    private const string BOSS_ANGRY = "Angry";
    private const string BOSS_ATTACK = "BossAttack";
    private const string BOSS_LANDING = "Landing";
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        camera = transform.GetChild(0).gameObject.GetComponent<CinemachineVirtualCamera>();
        camera.Follow = transform;
        healthBarPosition = healthBar.gameObject;
    }


    private void MoveHealthBar()
    {
        ChangeAnimationState(BOSS_IDLE);
        camera.gameObject.SetActive(false);
        mainUi.SetActive(true);
        mobileUI.SetActive(true);
        sideMenu.SetActive(true);
        LeanTween.moveLocalX(healthBarPosition, 7.41f, .7f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private void Land()
    {
        CameraShake.Instance.ShakeCamera(.3f, 3f);
        animator.Play("Landing");
        Invoke("LandingCompleted", .2f);
    }

    private void Scream()
    {
        ChangeAnimationState(BOSS_ATTACK);
        CameraShake.Instance.ShakeCamera(1f, 2f);
        Invoke("MoveHealthBar", 1.3f);
    }

    private void LandingCompleted()
    {
        ChangeAnimationState(BOSS_IDLE);
        Invoke("Scream", .5f);

    }
    internal void PlayCinematic()
    {
        camera.gameObject.SetActive(true);
        mainUi.SetActive(false);
        mobileUI.SetActive(false);
        sideMenu.SetActive(false);
        LeanTween.moveY(gameObject, landingPosition.transform.position.y, .5f).setOnComplete(Land);
    }

    private void ChangeAnimationState(string newState)
    {
        if (currentState == newState)
        {
            return;
        }
        animator.Play(newState);
        currentState = newState;
    }
}
