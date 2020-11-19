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
    private CinemachineBrain cinemachine;
    private bool hiding = false;
    private float timer;

    void Start()
    {
        MobileUI = GameObject.Find("CrossPlatUI");
        cinemachine = GameObject.Find("Main Camera").GetComponent<CinemachineBrain>();
        UI = GameObject.Find("MainUI");
        camera = transform.GetChild(0).gameObject.GetComponent<CinemachineVirtualCamera>();
        camera.Follow = transform;
        canvas = transform.GetChild(1).gameObject;
        background = canvas.transform.GetChild(0).gameObject.GetComponent<Image>();
        cardMenu = canvas.transform.GetChild(1).gameObject;
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
        LeanTween.scale(cardMenu, new Vector3(0, 0, 0), 0.2f);
        isMenuActive = false;
        background.enabled = false;
        //cardMenu.SetActive(true);
    }

    public void ShowCardMenu()
    {
        hiding = false;
        UI.SetActive(false);
        MobileUI.SetActive(false);
        camera.gameObject.SetActive(true);
        isMenuActive = true;
        background.enabled = true;
        cardMenu.SetActive(true);
        LeanTween.scale(cardMenu, new Vector3(1, 1, 1), 0.2f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ShowCardMenu();

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    // Start is called before the first frame update



}
