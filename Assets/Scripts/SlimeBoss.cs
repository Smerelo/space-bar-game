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
    private int zoneAttacked;
    private Animator animator;
    private CinemachineVirtualCamera camera;
    private GameObject healthBarPosition;
    private CentralTransactionLogic ctl;
    private List<ZoneManagment> zones;
    private GameObject employeeAttacked;
    private Animator tentacleAnimator;
    private GameObject tentacleObj;

    private const string BOSS_FALLING = "Falling";
    private const string BOSS_IDLE = "Idle";
    private const string BOSS_ANGRY = "Angry";
    private const string BOSS_ATTACK = "BossAttack";
    private const string BOSS_LANDING = "Landing";

    private const string TENTACLE_IDLE = "TentacleIdle";
    private const string SPAWN_TENTACLE = "SpawnTentacle";
    private const string TENTACLE_RETURN = "TentacleReturn";
    private const string ATTACK_BARMAN = "AttackBarman";

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        tentacleObj = transform.GetChild(1).gameObject;
        tentacleAnimator = tentacleObj.GetComponent<Animator>();
        camera = transform.GetChild(0).gameObject.GetComponent<CinemachineVirtualCamera>();
        camera.Follow = transform;
        healthBarPosition = healthBar.gameObject;
        ctl = GameObject.Find("SpaceCantina").GetComponent<CentralTransactionLogic>();
        zones = new List<ZoneManagment>();
        foreach (ZoneManagment zone in ctl.zones.Values)
        {
            if (zone.name != "Cleaning")
            {
                zones.Add(zone);

            }
        }
    }


    internal void Attack()
    {
        zoneAttacked = UnityEngine.Random.Range(0, 2);
        employeeAttacked = zones[zoneAttacked].GetRandomEmployee();
        if (employeeAttacked == null)
        {
            zoneAttacked = zoneAttacked == 0 ? 1 : 0;
            employeeAttacked = zones[zoneAttacked].GetRandomEmployee();
        }
        if (employeeAttacked != null)
        {
            tentacleObj.transform.position = new Vector3(employeeAttacked.transform.position.x + 0.6f, employeeAttacked.transform.position.y - 0.4f, 0) ;
            ChangeAnimationState(SPAWN_TENTACLE, tentacleAnimator);
            if (zoneAttacked == 0)
            {
                employeeAttacked.GetComponent<Barman>().Stop();

            }
            else
            {
                employeeAttacked.GetComponent<Waiter>().Stop();
            }
            Invoke("YeetEmployee", tentacleAnimator.GetCurrentAnimatorStateInfo(0).length);
        }
    }

    private void YeetEmployee()
    {
        ChangeAnimationState(ATTACK_BARMAN, tentacleAnimator);
        Invoke("DestroyEmployee", 0.03f);

        Invoke("TentacleReturn", .7f);
    }

    private void DestroyEmployee()
    {
        zones[zoneAttacked].RemoveEmployee(employeeAttacked);
    }


    private void TentacleReturn()
    {
        ChangeAnimationState(TENTACLE_RETURN, tentacleAnimator);
        Invoke("HideTentacle", tentacleAnimator.GetCurrentAnimatorStateInfo(0).length);

    }
    private void HideTentacle()
    {

    }
    private void MoveHealthBar()
    {
        ChangeAnimationState(BOSS_IDLE, animator);
        camera.gameObject.SetActive(false);
        mainUi.SetActive(true);
        mobileUI.SetActive(true);
        sideMenu.SetActive(true);
        LeanTween.moveLocalX(healthBarPosition, 7.41f, .7f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            Attack();
        }
    }



    private void Land()
    {
        CameraShake.Instance.ShakeCamera(.3f, 3f);
        animator.Play("Landing");
        Invoke("LandingCompleted", .2f);
    }

    private void Scream()
    {
        ChangeAnimationState(BOSS_ATTACK, animator);
        CameraShake.Instance.ShakeCamera(1f, 2f);
        Invoke("MoveHealthBar", 1.3f);
    }

    private void LandingCompleted()
    {
        ChangeAnimationState(BOSS_IDLE, animator);
        Invoke("Scream", .5f);

    }
    internal void PlayCinematic()
    {
        camera.gameObject.SetActive(true);
        mainUi.SetActive(false);
        mobileUI.SetActive(false);
        //sideMenu.SetActive(false);
        LeanTween.moveY(gameObject, landingPosition.transform.position.y, .5f).setOnComplete(Land);
    }


    
    private void ChangeAnimationState(string newState, Animator currentAnimator)
    {
        if (currentState == newState)
        {
            return;
        }
        currentAnimator.Play(newState);
        currentState = newState;
    }
}
