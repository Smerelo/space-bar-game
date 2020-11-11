using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GameAssets : MonoBehaviour
{
    private static GameAssets instance;

    public static GameAssets I
    {
        get
        {
            if (instance == null)
            {
                instance = Instantiate(Resources.Load("GameAssets") as GameObject).GetComponent<GameAssets>();
            }
            return (instance);
        }
    }

    public AudioMixer mixer;
}
