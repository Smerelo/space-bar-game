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

    [SerializeField] private FoodTypeAsset[] foodPlateTypes;
    public FoodTypeAsset[] FoodPlateTypes { get { return foodPlateTypes; }}
    [System.Serializable]
    public class FoodTypeAsset
    {
        [SerializeField] private Order.FoodTypes type;
        [SerializeField] private Sprite foodSprite;
        [SerializeField] private float price;
        [SerializeField] private float preparationTime;

        public Order.FoodTypes Type { get { return type; } }
        public Sprite FoodSprite { get { return foodSprite; } }
        public float Price { get { return price; } }
        public float PreparationTime { get { return preparationTime; } }
    }
}
