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
    private Animator ligth1;  
    private Animator ligth2;
    private int zoneSelected = 0;


    void Start()
    {
        MobileUI = GameObject.Find("CrossPlatUI");
        UI = GameObject.Find("MainUI");
        camera = transform.GetChild(0).gameObject.GetComponent<CinemachineVirtualCamera>();
        camera.Follow = transform;
        canvas = transform.GetChild(1).gameObject;
        background = canvas.transform.GetChild(0).gameObject.GetComponent<Image>();
        cardMenu = canvas.transform.GetChild(1).gameObject;
        button1 = cardMenu.transform.GetChild(4).gameObject.GetComponent<Button>();
        button2 = cardMenu.transform.GetChild(5).gameObject.GetComponent<Button>();
        ligth1 = button1.transform.GetChild(1).gameObject.GetComponent<Animator>();
        ligth2 = button2.transform.GetChild(1).gameObject.GetComponent<Animator>();
        button1.onClick.AddListener(() => OnButtonClick(0));
        button2.onClick.AddListener(() => OnButtonClick(1));
    }

    public void OnButtonClick(int mod)
    {
        zoneSelected = mod;
        if (mod == 0)
        {
            ligth1.Play("Card_light_green");
            ligth2.Play("Card_light");

        }
        if (mod == 1)
        {
            ligth2.Play("Card_light_green");
            ligth1.Play("Card_light");

        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (!isMenuActive && !UI.activeSelf && timer > .5f)
        {
            UI.SetActive(true);
            MobileUI.SetActive(true);
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
        ShowCardMenu();

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

}
