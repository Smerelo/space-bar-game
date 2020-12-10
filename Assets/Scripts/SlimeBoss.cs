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
    [SerializeField] private Popularity popularityBar;
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
    private Boss boss;

    private const string BOSS_FALLING = "Falling";
    private const string BOSS_IDLE = "Idle";
    private const string BOSS_ANGRY = "Angry";
    private const string BOSS_ATTACK = "BossAttack";
    private const string BOSS_LANDING = "Landing";
    private const string BOSS_EAT = "BossEat";

    private const string TENTACLE_IDLE = "TentacleIdle";
    private const string SPAWN_TENTACLE = "SpawnTentacle";
    private const string SPAWN_TENTACLE_KITCHEN = "SpawnTentacleKitchen";
    private const string TENTACLE_RETURN = "TentacleReturn";
    private const string TENTACLE_RETURN_KITCHEN = "TentacleReturnKitchen";
    private const string ATTACK_BARMAN = "AttackBarman";
    private const string ATTACK_WAITER = "AttackWaiter";
    private const string SHADOW_BD_SPAWN = "ShadowBDSpawn";
    private const string SHADOW_BD_RETURN = "ShadowBDReturn";
    private const string SHADOW_BW_SPAWN = "ShadowBWSpawn";
    private const string SHADOW_BW_RETURN = "ShadowBWReturn";
   

    // Start is called before the first frame update
    void Start()
    {
        boss = GetComponent<Boss>();
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

    internal void Eat()
    {
        ChangeAnimationState(BOSS_EAT, animator);
        StartCoroutine(PlayNextAnimation(animator, "BackToIdle"));
    }

    private void BackToIdle()
    {
        ChangeAnimationState(BOSS_IDLE, animator);
    }

    internal void Attack()
    {
        tentacleObj.SetActive(true);
        zoneAttacked = UnityEngine.Random.Range(0, 2);
        employeeAttacked = zones[zoneAttacked].GetRandomEmployee();
        ChangeAnimationState(BOSS_ATTACK, animator);
        if (employeeAttacked == null)
        {
            zoneAttacked = zoneAttacked == 0 ? 1 : 0;
            employeeAttacked = zones[zoneAttacked].GetRandomEmployee();
        }
        if (employeeAttacked != null)
        {
            tentacleObj.transform.position = new Vector3(employeeAttacked.transform.position.x + 0.8f, employeeAttacked.transform.position.y - 0.9f, 0) ;
            if (zoneAttacked == 0)
            {
                employeeAttacked.GetComponent<Barman>().Stop();

            }
            else
            {
                employeeAttacked.GetComponent<Waiter>().Stop();
            }
            if (zoneAttacked == 0)
            {
                ChangeAnimationState(SHADOW_BW_SPAWN, tentacleAnimator);
            }
            else
            {
                ChangeAnimationState(SHADOW_BD_SPAWN, tentacleAnimator);
            }
            StartCoroutine(PlayNextAnimation(tentacleAnimator, "SpawnTentacle"));
        }
    }

    private void SpawnTentacle()
    {
        if (zoneAttacked == 0)
        {
            ChangeAnimationState(SPAWN_TENTACLE_KITCHEN, tentacleAnimator);

        }
        else
            ChangeAnimationState(SPAWN_TENTACLE, tentacleAnimator);
        StartCoroutine(PlayNextAnimation(tentacleAnimator, "YeetEmployee"));
    }

    IEnumerator PlayNextAnimation(Animator anim, string method)
    {
        yield return new WaitForEndOfFrame();
        Invoke(method, anim.GetCurrentAnimatorStateInfo(0).length);
    }

    private void YeetEmployee()
    {
        if (zoneAttacked == 0)
        {
            ChangeAnimationState(ATTACK_BARMAN, tentacleAnimator);

        }
        else
            ChangeAnimationState(ATTACK_WAITER, tentacleAnimator);
        StartCoroutine(PlayNextAnimation(tentacleAnimator, "TentacleReturn"));
        Invoke("DestroyEmployee", 0.03f);
    }
    private void TentacleReturn()
    {
        if (zoneAttacked == 0)
        {
            ChangeAnimationState(TENTACLE_RETURN_KITCHEN, tentacleAnimator);

        }
        else
            ChangeAnimationState(TENTACLE_RETURN, tentacleAnimator);
        StartCoroutine(PlayNextAnimation(tentacleAnimator, "ReturnShadow"));
    }

    private void ReturnShadow()
    {
        if (zoneAttacked == 0)
        {
            ChangeAnimationState(SHADOW_BW_RETURN, tentacleAnimator);

        }
        else
            ChangeAnimationState(SHADOW_BD_RETURN, tentacleAnimator);
        StartCoroutine(PlayNextAnimation(tentacleAnimator, "HideTentacle"));
    }

    private void DestroyEmployee()
    {
        zones[zoneAttacked].RemoveEmployee(employeeAttacked);
    }


  
    private void HideTentacle()
    {
        tentacleObj.SetActive(false);
        ChangeAnimationState(BOSS_IDLE, animator);
        popularityBar.UpdatePopularity(-1);
        boss.attacking = false;
    }
    private void MoveHealthBar()
    {
        ChangeAnimationState(BOSS_IDLE, animator);
        camera.gameObject.SetActive(false);
        mainUi.SetActive(true);
        mobileUI.SetActive(true);
        sideMenu.SetActive(true);
        LeanTween.moveLocalX(healthBarPosition, 7.41f, .7f);
        boss.isInCinematic = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            GetAngry();
        }
    }

    public void GetAngry()
    {
        ChangeAnimationState(BOSS_ANGRY, animator);
        StartCoroutine( PlayNextAnimation(animator, "Attack"));
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
        tentacleObj.SetActive(false);
        camera.gameObject.SetActive(true);
        mainUi.SetActive(false);
        mobileUI.SetActive(false);
        sideMenu.SetActive(false);
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
