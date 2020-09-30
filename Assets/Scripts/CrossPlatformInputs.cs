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
        Vector2 movement = new Vector2(GetAxisHorizontal(), GetAxisVertical());
        
        if (movement.magnitude < 0.001f)
        {
            return (0);
        }
        return movement.normalized.x;
    }
    public float GetVertical()
    {
        Vector2 movement = new Vector2(GetAxisHorizontal(), GetAxisVertical());

        if (movement.magnitude < 0.001f)
        {
            return (0);
        }
        return movement.normalized.y;
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
