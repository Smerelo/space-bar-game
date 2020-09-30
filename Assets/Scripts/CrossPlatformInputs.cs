using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossPlatformInputs : MonoBehaviour
{
    public static CrossPlatformInputs Instance { get; private set; }
    private FloatingJoystick joystick;
    [SerializeField] private float deadZone = 0.1f;

    private void Awake()
    {
        Instance = this;
        joystick = transform.GetChild(0).GetComponent<FloatingJoystick>();
    }

    public float GetHorizontal()
    {
        if (GetAxisHorizontal() > deadZone)
        {
            return 1f;
        }
        else if (GetAxisHorizontal() < -deadZone)
        {
            return -1f;
        }
        else
        {
            return 0f;
        }
    }
    public float GetVertical()
    {
        if (GetAxisVertical() > deadZone)
        {
            return 1f;
        }
        else if (GetAxisVertical() < -deadZone)
        {
            return -1f;
        }
        else
        {
            return 0f;
        }
    }

/*#if UNITY_EDITOR
    public float GetAxisHorizontal()
    {
        return Input.GetAxisRaw("Horizontal");
    }
    public float GetAxisVertical()
    {
        return Input.GetAxisRaw("Vertical");
    }
#endif

#if UNITY_ANDROID && !UNITY_EDITOR*/
         
    public float GetAxisHorizontal()
    {
        return joystick.Horizontal;
    }
    public float GetAxisVertical()
    {
        return joystick.Vertical;
    }


/*#endif*/

}
