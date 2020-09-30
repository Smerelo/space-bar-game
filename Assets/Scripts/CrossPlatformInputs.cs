using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossPlatformInputs : MonoBehaviour
{
    public static CrossPlatformInputs Instance { get; private set; }
    private FloatingJoystick joystick;
    private void Awake()
    {
        Instance = this;
        joystick = transform.GetChild(0).GetComponent<FloatingJoystick>();
    }

#if UNITY_EDITOR
    public float GetAxisHorizontal()
    {
        return Input.GetAxis("Horizontal");
    }
    public float GetAxisVertical()
    {
        return Input.GetAxis("Vertical");
    }
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
         
    public float GetAxisHorizontal()
    {
        return joystick.Horizontal;
    }
    public float GetAxisVertical()
    {
        return joystick.Vertical;
    }

#endif

}
