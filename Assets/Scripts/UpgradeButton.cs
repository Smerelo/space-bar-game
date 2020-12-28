using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UpgradeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Animator animator;
    public Animator animator2;
    public int buttonNb;
    public EndOfDay endOfDay;

    private const string IDLE = "idle";
    private const string WALKING = "walking";

    private const string BOTTLE_IDLE = "bottle_idle";
    private const string SERVING = "serving";

    private string currentState1;
    private string currentState2;
    private Player player;
    private float currentSpeed;
    private float currentTaskSpeed;
    private int price = 10;
    private int nbOfUpgrades = 0;
    private TextMeshProUGUI priceText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buttonNb == 0)
        {
            ChangeAnimationState(WALKING, animator, currentState1);
        }if (buttonNb == 1)
        {
            ChangeAnimationState(SERVING, animator2, currentState2);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
            ChangeAnimationState(IDLE, animator, currentState1);
            ChangeAnimationState(BOTTLE_IDLE, animator2, currentState2);
    }

    // Start is called before the first frame update
    void Start()
    {
        priceText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        priceText.text = "$" +  price.ToString();
        player = GameObject.Find("Player").GetComponent<Player>();
        currentSpeed = player.GetCurrentSpeed();
        currentTaskSpeed = player.GetCurrentTaskSpeed();
    }

    public void UpgradeSpeed()
    {
        if (price <= endOfDay.balance)
        {
            endOfDay.UpdateBalance(price);
            nbOfUpgrades++;
            price *= 2;
            priceText.text = "$" + price.ToString();
            currentSpeed += .5f;
            player.UpgradeSpeed(currentSpeed);
        }
    }

    public void UpgradeTaskSpeed()
    {
        if (price <= endOfDay.balance)
        {
            endOfDay.UpdateBalance(price);
            price *= 2;
            priceText.text = "$" + price.ToString();

            nbOfUpgrades++;
            currentSpeed += .2f;
            player.UpgradeTaskSpeed(currentSpeed);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void ChangeAnimationState(string newState, Animator currentAnimator, string currentState)
    {
        if (currentState == newState)
        {
            return;
        }
        currentAnimator.Play(newState);
        currentState = newState;
    }
}
