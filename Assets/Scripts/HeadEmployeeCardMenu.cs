using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HeadEmployeeCardMenu : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{

    private GameObject cardMenu;
    private Image background;
    private bool isMenuActive = false;
    private CinemachineVirtualCamera camera;
    private GameObject canvas;
    private GameObject UI;
    private GameObject MobileUI;
    private float timer;
    private Button button1;
    private Button button2;
    private HeadEmployee headEmployeeScript;
    private Animator ligth1;  
    private Animator ligth2;
    private int zoneSelected = -1;
    private ZoneManagment waitingZone;
    private ZoneManagment barZone;
    private Animator photoAnimator;
    private string currentState;
    private Cv curriculum;

    void Start()
    {
        MobileUI = GameObject.Find("CrossPlatUI").gameObject;
        UI = GameObject.Find("MainUI");
        camera = transform.GetChild(0).gameObject.GetComponent<CinemachineVirtualCamera>();
        camera.Follow = transform;
        canvas = transform.GetChild(1).gameObject;
        background = canvas.transform.GetChild(0).gameObject.GetComponent<Image>();
        cardMenu = canvas.transform.GetChild(1).gameObject;
        photoAnimator = cardMenu.transform.GetChild(0).GetComponent<Animator>();
        button1 = cardMenu.transform.GetChild(4).gameObject.GetComponent<Button>();
        button2 = cardMenu.transform.GetChild(5).gameObject.GetComponent<Button>();
        ligth1 = button1.transform.GetChild(1).gameObject.GetComponent<Animator>();
        ligth2 = button2.transform.GetChild(1).gameObject.GetComponent<Animator>();
        button1.onClick.AddListener(() => OnButtonClick(0));
        button2.onClick.AddListener(() => OnButtonClick(1));
        headEmployeeScript = this.GetComponent<HeadEmployee>();
        barZone = GameObject.Find("Preparing").GetComponent<ZoneManagment>();
        waitingZone = GameObject.Find("Serving").GetComponent<ZoneManagment>();
    }

    internal void FillInfo(Cv cv)
    {
        curriculum = cv;
    }

    public void OnButtonClick(int mod)
    {
        if (mod == 0 && zoneSelected != mod)
        {
            zoneSelected = mod;
            ligth1.Play("Card_light_green");
            ligth2.Play("Card_light");
            headEmployeeScript.GetNewZone(barZone);
        }
        else  if (mod == 1 && zoneSelected != mod)
        {
            zoneSelected = mod;
            ligth2.Play("Card_light_green");
            ligth1.Play("Card_light");
            headEmployeeScript.GetNewZone(waitingZone);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (curriculum != null)
        {
            ChangeAnimation("Employee_" + curriculum.employeeType);

        }
    }

    private void ChangeAnimation(string newState)
    {
        if (!photoAnimator.GetCurrentAnimatorStateInfo(0).IsName(newState))
        {
            photoAnimator.Play(newState);
            currentState = newState;
        }

    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (isMenuActive == false)
        {
            ShowCardMenu();
        }
        else if (isMenuActive == true && eventData.lastPress == background)
        {
            HideCardMenu();
        }
    }

    public void HideCardMenu()
    {
        timer = 0f;
        camera.gameObject.SetActive(false);
        LeanTween.scale(cardMenu, new Vector3(0, 0, 0), 0.4f);
        isMenuActive = false;
        background.enabled = false;
        Invoke("ShowUI", 0.5f);
    }

    private void ShowUI()
    {
        UI.SetActive(true);
        MobileUI.SetActive(true);
    }

    public void ShowCardMenu()
    {
        UI.SetActive(false);
        MobileUI.SetActive(false);
        camera.gameObject.SetActive(true);
        isMenuActive = true;
        background.enabled = true;
        cardMenu.SetActive(true);
        LeanTween.scale(gameObject, new Vector3(1f, 1f, 1), 0.1f);
        LeanTween.scale(cardMenu, new Vector3(1, 1, 1), 0.4f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isMenuActive == false)
        {
            ShowCardMenu();
        }
        else if (isMenuActive == true && eventData.lastPress == background)
        {
            HideCardMenu();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isMenuActive)
        {
            LeanTween.scale(gameObject, new Vector3(1.1f, 1.1f, 1), 0.1f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.scale(gameObject, new Vector3(1f, 1f, 1), 0.1f);
    }

    internal void EnableCamera()
    {
        camera.gameObject.SetActive(true);
    }

    internal void DisableCamera()
    {

        camera.gameObject.SetActive(false);
    }
}
