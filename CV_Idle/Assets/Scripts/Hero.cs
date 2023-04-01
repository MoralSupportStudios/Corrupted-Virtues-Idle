using UnityEngine;

public class Hero : MonoBehaviour
{
    public HeroData heroData;

    // Other variables specific to the hero instance
    // ...

    void Start()
    {
        // Initialize the hero with the data from the HeroData ScriptableObject
        InitializeHero();
    }

    void InitializeHero()
    {
        // Set hero stats from the HeroData ScriptableObject
        // Example: GetComponent<SpriteRenderer>().sprite = heroData.heroSprite;
        // ...
    }

    // Other functions specific to the hero instance
    // ...
}
